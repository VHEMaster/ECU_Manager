using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ECU_Manager.Tools
{
    public static class Serializator<T>
    {
        public static void Serialize(string path, T obj)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(path, FileMode.Create))
                ser.Serialize(fs, obj);
        }

        public static T Deserialize(string path)
        {
            T result;
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(path, FileMode.Open))
                result = (T)ser.Deserialize(fs);
            return result;
        }
    }
}
