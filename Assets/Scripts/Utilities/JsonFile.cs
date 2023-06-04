namespace Plutono.Util
{
    using Newtonsoft.Json;
    using System.IO;
    
    public class JsonFile<T>
    {
        public T FromPath(string jsonPath)
        {
            var settings = new JsonSerializerSettings()
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore
            };
            var r = new StreamReader(jsonPath);
            var json = r.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}