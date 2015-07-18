using Newtonsoft.Json;
using System.IO;

namespace Iremmats.ServiceBus.LargeMessage
{
    public class SerializationHelper
    {
        public static Stream Serialize(object data)
        {
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            var jsonTextWriter = new JsonTextWriter(streamWriter);
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.Serialize(jsonTextWriter, data);
            jsonTextWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static string SerializeToJsonString(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static T DeserializeFromJsonString<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static T Deserialize<T>(Stream data)
        {
            var reader = new StreamReader(data);
            reader.ReadToEnd();

            data.Seek(0, SeekOrigin.Begin);
            var jsonTextReader = new JsonTextReader(reader);
            var jsonSerializer = new JsonSerializer();
            return jsonSerializer.Deserialize<T>(jsonTextReader);
        }
    }
}
