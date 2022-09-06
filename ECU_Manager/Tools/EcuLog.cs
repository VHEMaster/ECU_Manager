using ECU_Manager.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Manager.Tools
{
    public class EcuLog
    {
        public static IEnumerable<PointData> ParseLog(FileInfo fileInfo)
        {
            IEnumerable<PointData> points;
            Stream stream = fileInfo.Open(FileMode.Open, FileAccess.Read);

            try
            {
                points = parseLog(stream);
            }
            finally
            {
                stream.Close();
            }

            return points;
        }
        public static IEnumerable<PointData> ParseLog(string filePath)
        {
            IEnumerable<PointData> points;
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

            try
            {
                points = parseLog(stream);
            }
            finally
            {
                stream.Close();
            }

            return points;
        }
        public static void SaveLog(string filePath, IEnumerable<PointData> points)
        {
            Stream stream = File.Open(filePath, FileMode.Create, FileAccess.Write);
            try
            {
                saveLog(stream, points);
            }
            finally
            {
                stream.Close();
            }

        }
        private static void saveLog(Stream stream, IEnumerable<PointData> points)
        {
            StreamWriter writer = new StreamWriter(stream);

            try
            {
                FieldInfo[] fields = typeof(EcuParameters).GetFields();

                writer.Write("TimePoint,");
                for (int i = 0; i < fields.Length; i++)
                {
                    writer.Write(fields[i].Name);
                    if (fields.Last().Name != fields[i].Name)
                        writer.Write(',');
                }
                writer.WriteLine();

                foreach (PointData point in points)
                {
                    writer.Write((point.Seconds * 1000000.0D).ToString("F0"));
                    writer.Write(',');
                    for (int i = 0; i < fields.Length; i++)
                    {
                        writer.Write(fields[i].GetValue(point.Parameters).ToString());
                        if (fields.Last().Name != fields[i].Name)
                            writer.Write(',');
                    }
                    writer.WriteLine();
                }
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        private static IEnumerable<PointData> parseLog(Stream stream)
        {
            FieldInfo[] fields = typeof(EcuParameters).GetFields();
            List<PointData> points = new List<PointData>();
            StreamReader reader = new StreamReader(stream);
            string[] header;
            string[] split;
            string str;

            try
            {

                str = reader.ReadLine();
                str = str.TrimEnd(',');
                split = str.Split(',');

                for (int i = 0; i < split.Length; i++)
                {
                    split[i] = split[i].Trim();
                    if (split.Where(s => s.Equals(split[i])).Count() > 1)
                        throw new Exception("Field " + split[i] + " met more than one time.");
                }

                header = split;
                
                while(!reader.EndOfStream)
                {
                    PointData data = new PointData();
                    str = reader.ReadLine();
                    split = str.Split(',');
                    if(header.Length > 0)
                    {
                        try
                        {
                            if (header.Length == split.Length)
                            {
                                for(int i = 0; i < split.Length;i++)
                                {
                                    if (header[i] == "TimePoint")
                                    {
                                        data.Seconds = Convert.ToDouble(split[i]) / 1000000.0D;
                                    }
                                    else
                                    {
                                        FieldInfo field = fields.Where(f => f.Name.Equals(header[i])).FirstOrDefault();
                                        if(field != null)
                                        {
                                            if (field.FieldType == typeof(string))
                                                field.SetValueDirect(__makeref(data.Parameters), split[i]);
                                            else if (field.FieldType == typeof(float))
                                                field.SetValueDirect(__makeref(data.Parameters), Convert.ToSingle(split[i]));
                                            else if (field.FieldType == typeof(double))
                                                field.SetValueDirect(__makeref(data.Parameters), Convert.ToDouble(split[i]));
                                            else if (field.FieldType == typeof(int))
                                                field.SetValueDirect(__makeref(data.Parameters), Convert.ToInt32(split[i]));
                                            else if (field.FieldType == typeof(uint))
                                                field.SetValueDirect(__makeref(data.Parameters), Convert.ToUInt32(split[i]));
                                            else if (field.FieldType == typeof(short))
                                                field.SetValueDirect(__makeref(data.Parameters), Convert.ToInt16(split[i]));
                                            else if (field.FieldType == typeof(ushort))
                                                field.SetValueDirect(__makeref(data.Parameters), Convert.ToUInt16(split[i]));
                                            else if (field.FieldType == typeof(long))
                                                field.SetValueDirect(__makeref(data.Parameters), Convert.ToInt64(split[i]));
                                            else if (field.FieldType == typeof(ulong))
                                                field.SetValueDirect(__makeref(data.Parameters), Convert.ToUInt64(split[i]));
                                        }
                                    }
                                }
                            }
                            points.Add(data);
                        }
                        catch
                        {

                        }
        }
                }

            }
            finally
            {
                reader.Close();
            }

            return points;
        }
    }
}
