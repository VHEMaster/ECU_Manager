using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECU_Manager.Protocol;
using ECU_Manager.Packets;
using System.Threading;
using System.Runtime.InteropServices;
using ECU_Manager.Tables;
using ECU_Manager.Tools;

namespace ECU_Manager
{
    class MiddleLayer
    {
        public PacketHandler PacketHandler { get; private set; }
        public bool IsCtrlConnected = true;
        public bool IsSynchronizing { get { return bSyncRequired; } }
        public ConfigStruct Config = new ConfigStruct(0);
        private ConfigStruct oldconfig = new ConfigStruct(0);

        ProtocolHandler protocolHandler;
        MainForm mainForm;
        Thread thread;

        object SyncMutex = new object();
        bool bSyncRequestDone = false;
        bool bSyncRequired = false;
        int iSyncStep = 0;
        int iSyncSize = 0;
        int iSyncNum = 0;
        int iSyncNumimit = 0;
        int iSyncStepSize = 0;
        int iSyncLeft = 0;
        int iSyncOffset = 0;
        bool bSyncLoad = false;
        bool bSyncSave = false;
        bool bSyncFlash = false;
        byte[] bSyncArray = null;

        public MiddleLayer(MainForm mainForm, string portname)
        {
            this.mainForm = mainForm;
            protocolHandler = new ProtocolHandler(portname, ReceivedEvent, SentEvent, TimeoutEvent);
            PacketHandler = new PacketHandler(protocolHandler);
            thread = new Thread(BackgroundThread);
            thread.IsBackground = true;
            thread.Start();
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
                return true;
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
                return true;
            }
        }

        public void UpdateConfig()
        {
            byte[] senddata;
            int size = 4;
            int offset;
            bool equals;
            StructCopy<ParamsTable> structCopy = new StructCopy<ParamsTable>();
            while(true)
            {
                equals = true;
                byte[] current = structCopy.GetBytes(Config.parameters);
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
            StructCopy<IgnitionTable> structCopy = new StructCopy<IgnitionTable>();
            while(true)
            {
                equals = true;
                byte[] current = structCopy.GetBytes(Config.tables[table]);
                byte[] previous = structCopy.GetBytes(oldconfig.tables[table]);
                for (int i = 0; i < Marshal.SizeOf(typeof(IgnitionTable)); i++)
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
                        PacketHandler.SendTableData(table, Marshal.SizeOf(typeof(IgnitionTable)), offset, size, senddata);

                    }
                }
                if (equals) break;
            }
        }

        private void BackgroundThread()
        {
            Action action = null;
            while (true)
            {
                if (IsCtrlConnected)
                {
                    //PacketHandler.SendPcConnected();
                }
                lock (SyncMutex)
                {
                    if (bSyncRequired && bSyncRequestDone)
                    {
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

                            if (iSyncStep == 0)
                            {
                                iSyncStep++;
                                iSyncSize = Marshal.SizeOf(typeof(ParamsTable));
                                if (bSyncLoad)
                                {
                                    bSyncArray = new byte[iSyncSize];
                                }
                                else if (bSyncSave)
                                {
                                    StructCopy<ParamsTable> structParamsSaveCopy = new StructCopy<ParamsTable>();
                                    bSyncArray = structParamsSaveCopy.GetBytes(Config.parameters);
                                }
                                iSyncLeft = iSyncSize;
                                iSyncOffset = 0;
                                iSyncNum = 0;
                            }
                            if (iSyncStep == 1)
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
                            if (iSyncStep == 2)
                            {
                                if (bSyncLoad)
                                {
                                    StructCopy<ParamsTable> structParamsCopy = new StructCopy<ParamsTable>();
                                    Config.parameters = structParamsCopy.FromBytes(bSyncArray);
                                }
                                iSyncStep++;
                                iSyncSize = Marshal.SizeOf(typeof(IgnitionTable));
                                if (bSyncLoad)
                                {
                                    bSyncArray = new byte[iSyncSize];
                                }
                                else if (bSyncSave)
                                {
                                    StructCopy<IgnitionTable> structTableSaveCopy = new StructCopy<IgnitionTable>();
                                    bSyncArray = structTableSaveCopy.GetBytes(Config.tables[iSyncNum]);
                                }
                                iSyncLeft = iSyncSize;
                                iSyncOffset = 0;
                                iSyncNumimit = Config.parameters.tables_count;
                            }
                            if (iSyncStep == 3)
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
                                    if (bSyncLoad)
                                    {
                                        StructCopy<IgnitionTable> structTablesCopy = new StructCopy<IgnitionTable>();
                                        Config.tables[iSyncNum] = structTablesCopy.FromBytes(bSyncArray);
                                    }

                                    if (++iSyncNum < iSyncNumimit)
                                    {
                                        iSyncLeft = iSyncSize;
                                        iSyncOffset = 0;

                                        if (bSyncSave)
                                        {
                                            StructCopy<IgnitionTable> structTableSaveCopy = new StructCopy<IgnitionTable>();
                                            bSyncArray = structTableSaveCopy.GetBytes(Config.tables[iSyncNum]);
                                        }
                                    }
                                    else
                                    {
                                        if (bSyncLoad)
                                        {
                                            StructCopy<ConfigStruct> structCopy = new StructCopy<ConfigStruct>();
                                            byte[] config = structCopy.GetBytes(Config);
                                            oldconfig = structCopy.FromBytes(config);
                                        }
                                        iSyncStep = 0;
                                        bSyncLoad = false;
                                        bSyncSave = false;
                                        bSyncFlash = false;
                                        bSyncRequired = false;
                                        action = new Action(() => mainForm.SynchronizedEvent());
                                        if (mainForm.InvokeRequired)
                                            mainForm.BeginInvoke(action);
                                        else action.Invoke();
                                    }
                                }

                            }
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        private void TimeoutEvent(Packet packet)
        {
            object packetobj = PacketHandler.GetPacket(packet.Message);
            if(packetobj != null)
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
            Action action = null;
            ushort crc;
            int errorcode = 0;
            if(packet != null)
            {
                object packetobj = PacketHandler.GetPacket(packet.Message);
                if(packetobj != null)
                {
                    if (packetobj.GetType() == typeof(PK_Ping))
                    {
                        PK_Ping ping = (PK_Ping)packetobj;
                        PacketHandler.SendPong(packet.Source, ping.RandomPing);
                    }
                    else if (packetobj.GetType() == typeof(PK_RestoreConfigAcknowledge))
                    {
                        PK_RestoreConfigAcknowledge rca = (PK_RestoreConfigAcknowledge)packetobj;
                        if (rca.ErrorCode == 0)
                        {
                            lock (SyncMutex)
                            {
                                if (bSyncFlash)
                                {
                                    bSyncRequestDone = true;
                                    bSyncFlash = false;
                                }
                            }
                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_SaveConfigAcknowledge))
                    {
                        PK_SaveConfigAcknowledge sca = (PK_SaveConfigAcknowledge)packetobj;
                        if (sca.ErrorCode == 0)
                        {
                            lock (SyncMutex)
                            {
                                if (bSyncFlash)
                                {
                                    bSyncRequestDone = true;
                                    bSyncFlash = false;
                                }
                            }
                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_GeneralStatusResponse))
                    {
                        PK_GeneralStatusResponse gsr = (PK_GeneralStatusResponse)packetobj;
                        action = new Action(() => mainForm.UpdateGeneralStatus(gsr));
                        if (mainForm.InvokeRequired)
                            mainForm.BeginInvoke(action);
                        else action.Invoke();
                    }
                    else if (packetobj.GetType() == typeof(PK_DragUpdateResponse))
                    {
                        PK_DragUpdateResponse dur = (PK_DragUpdateResponse)packetobj;
                        action = new Action(() => mainForm.UpdateDragStatus(dur));
                        if (mainForm.InvokeRequired)
                            mainForm.BeginInvoke(action);
                        else action.Invoke();
                    }
                    else if (packetobj.GetType() == typeof(PK_DragPointResponse))
                    {
                        PK_DragPointResponse dpr = (PK_DragPointResponse)packetobj;
                        action = new Action(() => mainForm.UpdateDragPoint(dpr));
                        if (mainForm.InvokeRequired)
                            mainForm.BeginInvoke(action);
                        else action.Invoke();
                    }
                    else if (packetobj.GetType() == typeof(PK_DragStartAcknowledge))
                    {
                        PK_DragStartAcknowledge dsaa = (PK_DragStartAcknowledge)packetobj;
                        action = new Action(() => mainForm.DragStartAck(dsaa));
                        if (mainForm.InvokeRequired)
                            mainForm.BeginInvoke(action);
                        else action.Invoke();
                    }
                    else if (packetobj.GetType() == typeof(PK_DragStopAcknowledge))
                    {
                        PK_DragStopAcknowledge dsta = (PK_DragStopAcknowledge)packetobj;
                        action = new Action(() => mainForm.DragStopAck(dsta));
                        if (mainForm.InvokeRequired)
                            mainForm.BeginInvoke(action);
                        else action.Invoke();
                    }
                    else if (packetobj.GetType() == typeof(PK_TableMemoryAcknowledge))
                    {
                        PK_TableMemoryAcknowledge tma = (PK_TableMemoryAcknowledge)packetobj;
                        lock (SyncMutex)
                        {
                            if (bSyncSave)
                            {
                                if (tma.ErrorCode == 0)
                                {
                                    iSyncOffset += iSyncStepSize;
                                    iSyncLeft -= iSyncStepSize;
                                    bSyncRequestDone = true;
                                }
                            }
                        }
                    }
                    else if (packetobj.GetType() == typeof(PK_ConfigMemoryAcknowledge))
                    {
                        PK_ConfigMemoryAcknowledge cma = (PK_ConfigMemoryAcknowledge)packetobj;
                        lock (SyncMutex)
                        {
                            if (bSyncSave)
                            {
                                if (cma.ErrorCode == 0)
                                {
                                    iSyncOffset += iSyncStepSize;
                                    iSyncLeft -= iSyncStepSize;
                                    bSyncRequestDone = true;
                                }
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
                                    crc = Crc.crc_16(cmd.Data, cmd.Data.Length);
                                    if (cmd.crc == crc)
                                    {
                                        for (int i = iSyncOffset, j = 0; j < iSyncStepSize; i++, j++)
                                            bSyncArray[i] = cmd.Data[j];
                                        iSyncOffset += iSyncStepSize;
                                        iSyncLeft -= iSyncStepSize;
                                        bSyncRequestDone = true;
                                    }
                                    else errorcode = 2;
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
                                    crc = Crc.crc_16(cmd.Data, cmd.Data.Length);
                                    if (cmd.crc == crc)
                                    {
                                        for (int i = iSyncOffset, j = 0; j < iSyncStepSize; i++, j++)
                                            bSyncArray[i] = cmd.Data[j];
                                        iSyncOffset += iSyncStepSize;
                                        iSyncLeft -= iSyncStepSize;
                                        bSyncRequestDone = true;
                                    }
                                    else errorcode = 2;
                                }
                                else errorcode = 1;
                            }
                            else errorcode = (int)cmd.ErrorCode + 50;
                        }
                        PacketHandler.SendTableAcknowledge((int)cmd.Table, (int)cmd.TableSize, (int)cmd.Offset, (int)cmd.Size, errorcode);
                    }
                }
            }
        }
    }
}
