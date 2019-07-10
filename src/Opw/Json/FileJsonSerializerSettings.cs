using Newtonsoft.Json;

namespace Opw.Json
{
    /// <summary>
    /// Serialization settings for reading/writing files
    /// </summary>
    public class FileJsonSerializerSettings : JsonSerializerSettings
    {
        public FileJsonSerializerSettings()
        {
            this.AddDefaultJsonSerializerSettings();
            Formatting = Formatting.Indented;
        }
    }
}
