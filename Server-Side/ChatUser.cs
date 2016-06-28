using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Server_Side
{
    public class ChatUser 
    {

        public  string Nome { get; set; }
        public string IP { get; set; }
        public string SocketIP { get; set; }
        public string MAC1 { get; set; }
        public string MAC2 { get; set; }
        public string MAC3 { get; set; }

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public void SerializeObject<T>(T serializableObject)
        {
            if (serializableObject == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(System.Configuration.ConfigurationManager.AppSettings["DataBase"]);
                    stream.Close();
                }
            }
            catch (System.Exception ex)
            {
                //Log exception here
            }
        }

        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T DeSerializeObject<T>()
        {
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["DataBase"])) { return default(T); }

            T objectOut = default(T);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(System.Configuration.ConfigurationManager.AppSettings["DataBase"]);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    System.Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (System.Exception ex)
            {
                //Log exception here
            }

            return objectOut;
        }
    }
}
