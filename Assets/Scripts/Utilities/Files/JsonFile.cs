using Newtonsoft.Json;
using System.IO;
namespace Plutono.Util
{
    public static class JsonFile<T>
    {
        /// <summary>
        /// Read a JSON file from a path and deserialize it into an object
        /// </summary>
        /// <param name="jsonPath">the path of this JSON</param>
        /// <returns>A deserialized object</returns>
        public static T FromPath(string jsonPath)
        {
            //deserialize JSON containing $ref keys
            var settings = new JsonSerializerSettings()
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore
            };
            var sr = new StreamReader(jsonPath);
            var json = sr.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        /// Serialize an object into a JSON string
        /// </summary>
        /// <param name="contents">the object to be serialized</param>
        /// <returns></returns>
        public static string ToJson(T contents)
        {
            var settings = new JsonSerializerSettings()
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore
            };
            var json = JsonConvert.SerializeObject(contents, settings);
            return json;
        }
    }
}