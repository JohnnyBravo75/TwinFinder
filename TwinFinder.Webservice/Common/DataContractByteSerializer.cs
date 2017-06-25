namespace TwinFinder.Webservice
{
    using System.IO;
    using System.Runtime.Serialization;

    public static class DataContractByteSerializer
    {
        public static byte[] Serialize<T>(this T obj)
        {
            var serializer = new DataContractSerializer(typeof(T));
            byte[] result = null;

            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                result = ms.ToArray();
            }

            return result;
        }

        public static T Deserialize<T>(this byte[] serializedByteArray)
        {
            T deserializedObject;

            var serializer = new DataContractSerializer(typeof(T));
            using (var ms = new MemoryStream(serializedByteArray))
            {
                deserializedObject = (T)serializer.ReadObject(ms);
            }

            return deserializedObject;
        }
    }
}