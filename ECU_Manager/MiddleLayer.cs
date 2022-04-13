using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECU_Manager.Protocol;
using ECU_Manager.Packets;
using System.Threading;
using System.Runtime.InteropServices;
using ECU_Manager.Structs;
using ECU_Manager.Tools;
using System.Diagnostics;

namespace ECU_Manager
{
    public class MiddleLayer
    {
        public ComponentStructure ComponentStructure { get; private set; }
        public PacketHandler PacketHandler { get; private set; }
        public bool IsCtrlConnected = true;
        public bool IsSynchronizing { get { return bSyncRequired && !bSyncFastSync; } }
        private ConfigStruct oldconfig = new ConfigStruct(0);

        ProtocolHandler protocolHandler;
        List<IEcuEventHandler> eventHandlers;
        Thread thread;

        object SyncMutex = new object();
        bool bSyncRequestDone = false;
        bool bSyncRequired = false;
        int iSyncStep = 0;
        int iSyncSize = 0;
        int iSyncNum = 0;
        int iSyncNumimit = 0;
        int iSyncStepSize = 0;
        int iSyncError = 0;
        int iSyncLeft = 0;
        int iSyncOffset = 0;
        bool bSyncFastSync = false;
        bool bSyncLoad = false;
        bool bSyncSave = false;
        bool bSyncFlash = false;
        byte[] bSyncArray = null;

        public MiddleLayer(string portname)
        {
            eventHandlers = new List<IEcuEventHandler>();
            protocolHandler = new ProtocolHandler(portname, ReceivedEvent, SentEvent, TimeoutEvent);
            PacketHandler = new PacketHandler(protocolHandler);
            ComponentStructure = new ComponentStructure();
            thread = new Thread(BackgroundThread);
            thread.Name = "MiddleLayer Thread";
            thread.IsBackground = true;
            thread.Start();

            UpdateForceParameters();
        }


        public bool SyncLoad(bool flashLoad)
        {
            lock (SyncMutex)
            {
                if (bSyncRequired)
                    return false;

                iSyncStep = 0;
                bSyncRequired = true;
                bSyncRequestDone = true;
                bSyncLoad = true;
                bSyncFlash = flashLoad;
                iSyncError = 0;
                return true;
            }
        }

        public void RegisterEventHandler(IEcuEventHandler eventHandler)
        {
            lock (eventHandlers)
            {
                eventHandlers.Add(eventHandler);
            }
        }

        public bool SyncSave(bool flashSave)
        {
            lock (SyncMutex)
            {
                if (bSyncRequired)
                    return false;

                iSyncStep = 0;
                bSyncRequired = true;
                bSyncRequestDone = true;
                bSyncSave = true;
                bSyncFlash = flashSave;
                iSyncError = 0;
                return true;
            }
        }

        public int SyncGetError()
        {
            int val = iSyncError;
            return val;
        }

        public bool SyncFast()
        {
            lock (SyncMutex)
            {
                if (bSyncRequired)
                    return false;

                iSyncStep = 0;
                bSyncRequired = true;
                bSyncRequestDone = true;
                bSyncFastSync = true;
                iSyncError = 0;
                return true;
            }
        }

        public void UpdateForceParameters()
        {
            PacketHandler.SendForceParametersData(ComponentStructure.ForceParameters);
        }

        public void UpdateConfig()
        {
            byte[] senddata;
            int size = 4;
            int offset;
            bool equals;
            StructCopy<ParamsTable> structCopy = new StructCopy<ParamsTable>();
            while (true)
            {
                equals = true;
                byte[] current = structCopy.GetBytes(ComponentStructure.ConfigStruct.parameters);
                byte[] previous = structCopy.GetBytes(oldconfig.parameters);
                for (int i = 0; i < Marshal.SizeOf(typeof(ParamsTable)); i++)
                {

                    if (current[i] != previous[i])
                    {
                        equals = false;
                        offset = i - (i % 4);
                        i += 4 - (i % 4);
                        senddata = new byte[size];
                        for (int j = 0; j < size; j++)
                        {
                            senddata[j] = current[j + offset];
                            previous[j + offset] = current[j + offset];
                        }
                        oldconfig.parameters = structCopy.FromBytes(previous);
                        PacketHandler.SendConfigData(Marshal.SizeOf(typeof(ParamsTable)), offset, size, senddata);

                    }
                }
                if (equals) break;
            }
        }

        public void UpdateTable(int table)
        {
            byte[] senddata;
            int size = 4;
            int offset;
            bool equals;
            StructCopy<EcuTable> structCopy = new StructCopy<EcuTable>();
            while (true)
            {
                equals = true;
                byte[] current = structCopy.GetBytes(ComponentStructure.ConfigStruct.tables[table]);
                byte[] previous = structCopy.GetBytes(oldconfig.tables[table]);
                for (int i = 0; i < Marshal.SizeOf(typeof(EcuTable)); i++)
                {
                    if (current[i] != previous[i])
                    {
                        equals = false;
                        offset = i - (i % 4);
                        i += 4 - (i % 4);
                        senddata = new byte[size];
                        for (int j = 0; j < size; j++)
                        {
                            senddata[j] = current[j + offset];
                            previous[j + offset] = current[j + offset];
                        }
                        oldconfig.tables[table] = structCopy.FromBytes(previous);
                        PacketHandler.SendTableData(table, Marshal.SizeOf(typeof(EcuTable)), offset, size, senddata);

                    }
                }
                if (equals) break;
            }
        }

        private void BackgroundThread()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Reset();
            while (true)
            {
                if (IsCtrlConnected)
                {
                    //PacketHandler.SendPcConnected();
                }
                lock (SyncMutex)
                {
                    if (bSyncRequired)
                    {
                        if (bSyncRequestDone)
                        {
                            stopwatch.Restart();
                            if (bSyncFlash)
                            {
                                bSyncRequestDone = false;
                                if (bSyncLoad)
                                {
                                    PacketHandler.SendRestoreRequest();
                                }
                                else if (bSyncSave)
                                {
                                    PacketHandler.SendSaveRequest();
                                }
                            }
                            else
                            {

                                if (iSyncError > 0)
                                {
                                    iSyncStep = 0;
                                    bSyncLoad = false;
                                    bSyncSave = false;
                                    bSyncFastSync = false;
                                    bSyncFlash = false;
                                    bSyncRequired = false;
                                    stopwatch.Reset();

                                    lock (eventHandlers)
                                    {
                                        foreach (IEcuEventHandler handler in eventHandlers)
                                            handler.SynchronizedEvent(iSyncError);
                                    }
                                }
                                else
                                {
                                    if (iSyncStep == 0)
                                    {
                                        if (bSyncFastSync)
                                        {
                                            iSyncStep++;
                                        }
                                        else
                                        {
                                            iSyncSize = Marshal.SizeOf(typeof(ParamsTable));
                                            if (bSyncLoad)
                                            {
                                                bSyncArray = new byte[iSyncSize];
                                            }
                                            else if (bSyncSave)
                                            {
                                                StructCopy<ParamsTable> structParamsSaveCopy = new StructCopy<ParamsTable>();
                                                bSyncArray = structParamsSaveCopy.GetBytes(ComponentStructure.ConfigStruct.parameters);
                                            }
                                            iSyncLeft = iSyncSize;
                                            iSyncOffset = 0;
                                            iSyncNum = 0;
                                            iSyncStep++;
                                        }
                                    }
                                    if (iSyncStep == 1)
                                    {
                                        if (bSyncFastSync)
                                        {
                                            iSyncStep++;
                                        }
                                        else
                                        {
                                            if (iSyncLeft > 0)
                                            {
                                                bSyncRequestDone = false;
                                                iSyncStepSize = iSyncLeft > Consts.PACKET_CONFIG_MAX_SIZE ? Consts.PACKET_CONFIG_MAX_SIZE : iSyncLeft;
                                                if (bSyncLoad)
                                                {
                                                    PacketHandler.SendConfigRequest(iSyncSize, iSyncOffset, iSyncStepSize);
                                                }
                                                else if (bSyncSave)
                                                {
                                                    byte[] saveparamsdata = new byte[iSyncStepSize];
                                                    for (int i = 0; i < iSyncStepSize; i++)
                                                        saveparamsdata[i] = bSyncArray[i + iSyncOffset];
                                                    PacketHandler.SendConfigData(iSyncSize, iSyncOffset, iSyncStepSize, saveparamsdata);
                                                }
                                            }
                                            else iSyncStep++;
                                        }
                                    }
                                    if (iSyncStep == 2)
                                    {
                                        if (!bSyncFastSync)
                                        {
                                            if (bSyncLoad)
                                            {
                                                StructCopy<ParamsTable> structParamsCopy = new StructCopy<ParamsTable>();
                                                ComponentStructure.ConfigStruct.parameters = structParamsCopy.FromBytes(bSyncArray);
                                            }
                                        }
                                        iSyncStep++;
                                        iSyncSize = Marshal.SizeOf(typeof(EcuCriticalBackup));
                                        if (bSyncLoad || bSyncFastSync)
                                        {
                                            bSyncArray = new byte[iSyncSize];
                                        }
                                        else if (bSyncSave)
                                        {
                                            StructCopy<EcuCriticalBackup> structParamsSaveCopy = new StructCopy<EcuCriticalBackup>();
                                            bSyncArray = structParamsSaveCopy.GetBytes(ComponentStructure.ConfigStruct.critical);
                                        }
                                        iSyncLeft = iSyncSize;
                                        iSyncOffset = 0;
                                        iSyncNum = 0;
                                    }
                                    if (iSyncStep == 3)
                                    {
                                        if (iSyncLeft > 0)
                                        {
                                            bSyncRequestDone = false;
                                            iSyncStepSize = iSyncLeft > Consts.PACKET_CRITICAL_MAX_SIZE ? Consts.PACKET_CRITICAL_MAX_SIZE : iSyncLeft;
                                            if (bSyncLoad || bSyncFastSync)
                                            {
                                                PacketHandler.SendCriticalRequest(iSyncSize, iSyncOffset, iSyncStepSize);
                                            }
                                            else if (bSyncSave)
                                            {
                                                byte[] savecriticaldata = new byte[iSyncStepSize];
                                                for (int i = 0; i < iSyncStepSize; i++)
                                                    savecriticaldata[i] = bSyncArray[i + iSyncOffset];
                                                PacketHandler.SendCriticalData(iSyncSize, iSyncOffset, iSyncStepSize, savecriticaldata);
                                            }
                                        }
                                        else iSyncStep++;
                                    }
                                    if (iSyncStep == 4)
                                    {
                                        if (bSyncLoad || bSyncFastSync)
                                        {
                                            StructCopy<EcuCriticalBackup> structCriticalCopy = new StructCopy<EcuCriticalBackup>();
                                            ComponentStructure.ConfigStruct.critical = structCriticalCopy.FromBytes(bSyncArray);

                                            StructCopy<EcuCriticalBackup> structCopy = new StructCopy<EcuCriticalBackup>();
                                            byte[] config = structCopy.GetBytes(ComponentStructure.ConfigStruct.critical);
                                            oldconfig.critical = structCopy.FromBytes(config);
                                        }
                                        iSyncStep++;
                                        iSyncSize = Marshal.SizeOf(typeof(EcuCorrections));
                                        if (bSyncLoad || bSyncFastSync)
                                        {
                                            bSyncArray = new byte[iSyncSize];
                                        }
                                        else if (bSyncSave)
                                        {
                                            StructCopy<EcuCorrections> structCriticalSaveCopy = new StructCopy<EcuCorrections>();
                                            bSyncArray = structCriticalSaveCopy.GetBytes(ComponentStructure.ConfigStruct.corrections);
                                        }
                                        iSyncLeft = iSyncSize;
                                        iSyncOffset = 0;
                                        iSyncNum = 0;
                                    }
                                    if (iSyncStep == 5)
                                    {
                                        if (iSyncLeft > 0)
                                        {
                                            bSyncRequestDone = false;
                                            iSyncStepSize = iSyncLeft > Consts.PACKET_CORRECTION_MAX_SIZE ? Consts.PACKET_CORRECTION_MAX_SIZE : iSyncLeft;
                                            if (bSyncLoad || bSyncFastSync)
                                            {
                                                PacketHandler.SendCorrectionsRequest(iSyncSize, iSyncOffset, iSyncStepSize);
                                            }
                                            else if (bSyncSave)
                                            {
                                                byte[] saveCorrectionsdata = new byte[iSyncStepSize];
                                                for (int i = 0; i < iSyncStepSize; i++)
                                                    saveCorrectionsdata[i] = bSyncArray[i + iSyncOffset];
                                                PacketHandler.SendCorrectionsData(iSyncSize, iSyncOffset, iSyncStepSize, saveCorrectionsdata);
                                            }
                                        }
                                        else iSyncStep++;
                                    }
                                    if (iSyncStep == 6)
                                    {
                                        if (bSyncLoad || bSyncFastSync)
                                        {
                                            StructCopy<EcuCorrections> structCorrectionsCopy = new StructCopy<EcuCorrections>();
                                            ComponentStructure.ConfigStruct.corrections = structCorrectionsCopy.FromBytes(bSyncArray);

                                            StructCopy<EcuCorrections> structCopy = new StructCopy<EcuCorrections>();
                                            byte[] config = structCopy.GetBytes(ComponentStructure.ConfigStruct.corrections);
                                            oldconfig.corrections = structCopy.FromBytes(config);
                                        }
                                        if (bSyncFastSync)
                                        {
                                            iSyncStep++;
                                        }
                                        else
                                        {
                                            iSyncStep++;
                                            iSyncSize = Marshal.SizeOf(typeof(EcuTable));
                                            if (bSyncLoad)
                                            {
                                                bSyncArray = new byte[iSyncSize];
                                            }
                                            else if (bSyncSave)
                                            {
                                                StructCopy<EcuTable> structTableSaveCopy = new StructCopy<EcuTable>();
                                                bSyncArray = structTableSaveCopy.GetBytes(ComponentStructure.ConfigStruct.tables[iSyncNum]);
                                            }
                                            iSyncLeft = iSyncSize;
                                            iSyncOffset = 0;
                                            iSyncNumimit = Consts.TABLE_SETUPS_MAX;
                                        }
                                    }
                                    if (iSyncStep == 7)
                                    {
                                        if (iSyncLeft > 0)
                                        {
                                            bSyncRequestDone = false;
                                            iSyncStepSize = iSyncLeft > Consts.PACKET_TABLE_MAX_SIZE ? Consts.PACKET_TABLE_MAX_SIZE : iSyncLeft;
                                            if (bSyncLoad)
                                            {
                                                PacketHandler.SendTableRequest(iSyncNum, iSyncSize, iSyncOffset, iSyncStepSize);
                                            }
                                            else if (bSyncSave)
                                            {
                                                byte[] savetablesdata = new byte[iSyncStepSize];
                                                for (int i = 0; i < iSyncStepSize; i++)
                                                    savetablesdata[i] = bSyncArray[i + iSyncOffset];
                                                PacketHandler.SendTableData(iSyncNum, iSyncSize, iSyncOffset, iSyncStepSize, savetablesdata);
                                            }
                                        }
                                        else
                                        {
                                            iSyncStep++;
                                        }

                                    }
                                    if (iSyncStep == 8)
                                    {
                                        if (bSyncFastSync)
                                        {
                                            iSyncStep++;
                                        }

                                        if (bSyncLoad)
                                        {
                                            StructCopy<EcuTable> structTablesCopy = new StructCopy<EcuTable>();
                                            ComponentStructure.ConfigStruct.tables[iSyncNum] = structTablesCopy.FromBytes(bSyncArray);
                                        }


                                        if (++iSyncNum < iSyncNumimit)
                                        {
                                            iSyncLeft = iSyncSize;
                                            iSyncOffset = 0;

                                            if (bSyncSave)
                                            {
                                                StructCopy<EcuTable> structTableSaveCopy = new StructCopy<EcuTable>();
                                                bSyncArray = structTableSaveCopy.GetBytes(ComponentStructure.ConfigStruct.tables[iSyncNum]);
                                            }
                                        }
                                        else
                                        {
                                            iSyncStep++;
                                        }
                                    }
                                    if (iSyncStep == 9)
                                    {
                                        if (!bSyncFastSync)
                                        {
                                            if (bSyncLoad)
                                            {
                                                StructCopy<ConfigStruct> structCopy = new StructCopy<ConfigStruct>();
                                                byte[] config = structCopy.GetBytes(ComponentStructure.ConfigStruct);
                                                oldconfig = structCopy.FromBytes(config);
                                            }
                                        }
                                        iSyncStep = 0;
                                        bSyncLoad = false;
                                        bSyncSave = false;
                                        bSyncFastSync = false;
                                        bSyncFlash = false;
                                        bSyncRequired = false;
                                        stopwatch.Reset();

                                        lock (eventHandlers)
                                        {
                                            foreach (IEcuEventHandler handler in eventHandlers)
                                                handler.SynchronizedEvent(iSyncError);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (bSyncFastSync)
                            {
                                if (!stopwatch.IsRunning)
                                    stopwatch.Restart();
                                if ((bSyncFlash && stopwatch.Elapsed.TotalSeconds > 10) || (!bSyncFlash && stopwatch.Elapsed.TotalSeconds > 3))
                                {
                                    iSyncStep = 0;
                                    bSyncLoad = false;
                                    bSyncSave = false;
                                    bSyncFastSync = false;
                                    bSyncFlash = false;
                                    bSyncRequired = false;
                                    iSyncError = -1;
                                    stopwatch.Reset();

                                    lock (eventHandlers)
                                    {
                                        foreach (IEcuEventHandler handler in eventHandlers)
                                            handler.SynchronizedEvent(iSyncError);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        stopwatch.Reset();
                    }
                }
                Thread.Sleep(1);
            }
        }

        private void TimeoutEvent(Packet packet)
        {
            object packetobj = PacketHandler.GetPacket(packet.Message);
            if (packetobj != null)
            {
                if (packetobj.GetType() == typeof(PK_PcConnected))
                    return;

            }
        }

        private void SentEvent(Packet packet)
        {

        }

        private void ReceivedEvent(Packet packet)
        {
            int errorcode = 0;
            if (packet != null)
            {
                object packetobj = PacketHandler.GetPacket(packet.Message);
                if (packetobj != null)
                {
                    if (packetobj.GetType() == typeof(PK_Ping))
                    {
                        PK_Ping ping = (PK_Ping)packetobj;
                        PacketHandler.SendPong(packet.Source, ping.RandomPing);
                    }
                    else if (packetobj.GetType() == typeof(PK_RestoreConfigAcknowledge))
                    {
                        PK_RestoreConfigAcknowledge rca = (PK_RestoreConfigAcknowledge)packetobj;
                        lock (SyncMutex)
                        {
                            if (bSyncFlash)
                            {
                                iSyncError = (int)rca.ErrorCode;
                                bSyncRequestDone = true;
                                bSyncFlash = false;
                            }
                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_SaveConfigAcknowledge))
                    {
                        PK_SaveConfigAcknowledge sca = (PK_SaveConfigAcknowledge)packetobj;
                        lock (SyncMutex)
                        {
                            if (bSyncFlash)
                            {
                                iSyncError = (int)sca.ErrorCode;
                                bSyncRequestDone = true;
                                bSyncFlash = false;
                            }
                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_ParametersResponse))
                    {
                        PK_ParametersResponse gsr = (PK_ParametersResponse)packetobj;
                        ComponentStructure.EcuParameters = gsr.Parameters;
                        lock (eventHandlers)
                        {
                            foreach (IEcuEventHandler handler in eventHandlers)
                                handler.UpdateParametersEvent(gsr.Parameters);
                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_DragUpdateResponse))
                    {
                        PK_DragUpdateResponse dur = (PK_DragUpdateResponse)packetobj;
                        lock (eventHandlers)
                        {
                            foreach (IEcuEventHandler handler in eventHandlers)
                                handler.UpdateDragStatusEvent(dur);
                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_DragPointResponse))
                    {
                        PK_DragPointResponse dpr = (PK_DragPointResponse)packetobj;
                        lock (eventHandlers)
                        {
                            foreach (IEcuEventHandler handler in eventHandlers)
                                handler.UpdateDragPointEvent(dpr);
                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_DragStartAcknowledge))
                    {
                        PK_DragStartAcknowledge dsaa = (PK_DragStartAcknowledge)packetobj;
                        lock (eventHandlers)
                        {
                            foreach (IEcuEventHandler handler in eventHandlers)
                                handler.DragStartAckEvent(dsaa);
                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_DragStopAcknowledge))
                    {
                        PK_DragStopAcknowledge dsta = (PK_DragStopAcknowledge)packetobj;
                        lock (eventHandlers)
                        {
                            foreach (IEcuEventHandler handler in eventHandlers)
                                handler.DragStopAckEvent(dsta);
                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_TableMemoryAcknowledge))
                    {
                        PK_TableMemoryAcknowledge tma = (PK_TableMemoryAcknowledge)packetobj;
                        lock (SyncMutex)
                        {
                            if (bSyncSave)
                            {
                                iSyncError = (int)tma.ErrorCode;
                                iSyncOffset += iSyncStepSize;
                                iSyncLeft -= iSyncStepSize;
                                bSyncRequestDone = true;
                            }
                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_ConfigMemoryAcknowledge))
                    {
                        PK_ConfigMemoryAcknowledge cma1 = (PK_ConfigMemoryAcknowledge)packetobj;
                        lock (SyncMutex)
                        {
                            if (bSyncSave)
                            {
                                iSyncError = (int)cma1.ErrorCode;
                                iSyncOffset += iSyncStepSize;
                                iSyncLeft -= iSyncStepSize;
                                bSyncRequestDone = true;
                            }

                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_CriticalMemoryAcknowledge))
                    {
                        PK_CriticalMemoryAcknowledge cma2 = (PK_CriticalMemoryAcknowledge)packetobj;
                        lock (SyncMutex)
                        {
                            if (bSyncSave)
                            {
                                iSyncError = (int)cma2.ErrorCode;
                                iSyncOffset += iSyncStepSize;
                                iSyncLeft -= iSyncStepSize;
                                bSyncRequestDone = true;
                            }

                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_CorrectionsMemoryAcknowledge))
                    {
                        PK_CorrectionsMemoryAcknowledge cma3 = (PK_CorrectionsMemoryAcknowledge)packetobj;
                        lock (SyncMutex)
                        {
                            if (bSyncSave)
                            {
                                iSyncError = (int)cma3.ErrorCode;
                                iSyncOffset += iSyncStepSize;
                                iSyncLeft -= iSyncStepSize;
                                bSyncRequestDone = true;
                            }
                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_ConfigMemoryData))
                    {
                        PK_ConfigMemoryData cmd = (PK_ConfigMemoryData)packetobj;
                        lock (SyncMutex)
                        {
                            if (cmd.ErrorCode == 0)
                            {
                                if (cmd.Offset == iSyncOffset && cmd.Size == iSyncStepSize && cmd.ConfigSize == iSyncSize)
                                {
                                    for (int i = iSyncOffset, j = 0; j < iSyncStepSize; i++, j++)
                                        bSyncArray[i] = cmd.Data[j];
                                    iSyncOffset += iSyncStepSize;
                                    iSyncLeft -= iSyncStepSize;
                                    bSyncRequestDone = true;
                                }
                                else errorcode = 1;
                            }
                            else errorcode = (int)cmd.ErrorCode + 50;
                        }
                        PacketHandler.SendConfigAcknowledge((int)cmd.ConfigSize, (int)cmd.Offset, (int)cmd.Size, errorcode);
                    }
                    else if (packetobj.GetType() == typeof(PK_TableMemoryData))
                    {
                        PK_TableMemoryData cmd = (PK_TableMemoryData)packetobj;
                        lock (SyncMutex)
                        {
                            if (cmd.ErrorCode == 0)
                            {
                                if (cmd.Offset == iSyncOffset && cmd.Size == iSyncStepSize && cmd.TableSize == iSyncSize && cmd.Table == iSyncNum)
                                {
                                    for (int i = iSyncOffset, j = 0; j < iSyncStepSize; i++, j++)
                                        bSyncArray[i] = cmd.Data[j];
                                    iSyncOffset += iSyncStepSize;
                                    iSyncLeft -= iSyncStepSize;
                                    bSyncRequestDone = true;
                                }
                                else errorcode = 1;
                            }
                            else errorcode = (int)cmd.ErrorCode + 50;
                        }
                        PacketHandler.SendTableAcknowledge((int)cmd.Table, (int)cmd.TableSize, (int)cmd.Offset, (int)cmd.Size, errorcode);
                    }
                    else if (packetobj.GetType() == typeof(PK_CriticalMemoryData))
                    {
                        PK_CriticalMemoryData cmd = (PK_CriticalMemoryData)packetobj;
                        lock (SyncMutex)
                        {
                            if (cmd.ErrorCode == 0)
                            {
                                if (cmd.Offset == iSyncOffset && cmd.Size == iSyncStepSize && cmd.CriticalSize == iSyncSize)
                                {
                                    for (int i = iSyncOffset, j = 0; j < iSyncStepSize; i++, j++)
                                        bSyncArray[i] = cmd.Data[j];
                                    iSyncOffset += iSyncStepSize;
                                    iSyncLeft -= iSyncStepSize;
                                    bSyncRequestDone = true;
                                }
                                else errorcode = 1;
                            }
                            else errorcode = (int)cmd.ErrorCode + 50;
                        }
                        PacketHandler.SendCriticalAcknowledge((int)cmd.CriticalSize, (int)cmd.Offset, (int)cmd.Size, errorcode);
                    }
                    else if (packetobj.GetType() == typeof(PK_CorrectionsMemoryData))
                    {
                        PK_CorrectionsMemoryData cmd = (PK_CorrectionsMemoryData)packetobj;
                        lock (SyncMutex)
                        {
                            if (cmd.ErrorCode == 0)
                            {
                                if (cmd.Offset == iSyncOffset && cmd.Size == iSyncStepSize && cmd.CorrectionsSize == iSyncSize)
                                {
                                    for (int i = iSyncOffset, j = 0; j < iSyncStepSize; i++, j++)
                                        bSyncArray[i] = cmd.Data[j];
                                    iSyncOffset += iSyncStepSize;
                                    iSyncLeft -= iSyncStepSize;
                                    bSyncRequestDone = true;
                                }
                                else errorcode = 1;
                            }
                            else errorcode = (int)cmd.ErrorCode + 50;
                        }
                        PacketHandler.SendCorrectionsAcknowledge((int)cmd.CorrectionsSize, (int)cmd.Offset, (int)cmd.Size, errorcode);
                    }
                    else if (packetobj.GetType() == typeof(PK_ForceParametersDataAcknowledge))
                    {
                        PK_ForceParametersDataAcknowledge fpda = (PK_ForceParametersDataAcknowledge)packetobj;
                        if (fpda.ErrorCode != 0)
                        {

                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_ResetStatusResponse))
                    {
                        PK_ResetStatusResponse rsr = (PK_ResetStatusResponse)packetobj;
                        if (rsr.ErrorCode != 0)
                        {

                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_StatusResponse))
                    {
                        PK_StatusResponse sr = (PK_StatusResponse)packetobj;
                        lock (eventHandlers)
                        {
                            List<CheckDataItem> list = CheckData.GenerateFromBitmap(sr.CheckBitmap.bytes, sr.CheckBitmapRecorded.bytes);
                            foreach (IEcuEventHandler handler in eventHandlers)
                                handler.UpdateStatusEvent(list);
                        }
                    }
                }
            }
        }
    }
}
