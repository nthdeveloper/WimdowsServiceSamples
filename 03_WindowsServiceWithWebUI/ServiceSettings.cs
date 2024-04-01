using System;
using System.IO;
using System.Xml.Serialization;

namespace WindowsServiceWithWebUI
{
    public class ServiceSettings
    {
        public string ApiAddress { get; set; }
        public int ApiPort { get; set; }

        public static ServiceSettings LoadFromXmFile(string filePath)
        {
            using(var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ServiceSettings));
                return  (ServiceSettings)serializer.Deserialize(stream);
            }
        }
    }
}
