using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TwinFinder.Base.Utils
{
    /// <summary>
    /// Serialisiert und Deserialisiert beliebige .NET-Objekte
    /// </summary>
    public class XmlSerializerHelper<T>
    {
        private XmlSerializer xmlSerializer;

        /// <summary>
        /// Gets or sets den Dateinamen der "XML"-Datei
        /// </summary>
        public string FileName { get; set; }

        public XmlSerializerHelper()
        {
            Type[] knownTypes = KnownTypesProvider.GetKnownTypes(null, excludeNameSpacePrefixes: new string[] { "Telerik", "log4net", "Newtonsoft" }).ToArray();
            this.xmlSerializer = new XmlSerializer(typeof(T), knownTypes);
        }

        /// <summary>
        /// Deserialisiert ein beliebiges zuvor serialisiertes .NET-Objekt
        /// </summary>
        /// <returns>.NET-Objekt</returns>
        public T Load()
        {
            TextReader reader = null;
            string path = this.FileName;
            try
            {
                reader = new StreamReader(path);
                T obj = (T)this.xmlSerializer.Deserialize(reader);
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Es ist ein Fehler beim Deserialisieren eines Objektes aufgetreten", ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
        }

        /// <summary>
        /// Serialisiert ein belibiges .NET-Objekt
        /// </summary>
        /// <param name="obj">zu serialisierendes Objekt</param>
        public void Save(T obj)
        {
            TextWriter writer = null;
            string path = this.FileName;

            try
            {
                writer = new StreamWriter(path);
                this.xmlSerializer.Serialize(writer, obj);
            }
            catch (Exception ex)
            {
                throw new Exception("Es ist ein Fehler beim Serialisieren des Objektes '" + obj.ToString() + "' aufgetreten", ex);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }
        }

        public string SerializeToString(T objectInstance)
        {
            var xml = new StringBuilder();

            using (TextWriter writer = new StringWriter(xml))
            {
                this.xmlSerializer.Serialize(writer, objectInstance);
            }

            return xml.ToString();
        }

        public T DeserializeFromString(string xml)
        {
            return (T)this.DeserializeFromString(xml, typeof(T));
        }

        public object DeserializeFromString(string xml, Type type)
        {
            object result;

            using (TextReader reader = new StringReader(xml))
            {
                result = this.xmlSerializer.Deserialize(reader);
            }

            return result;
        }
    }
}