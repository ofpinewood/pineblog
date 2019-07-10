
namespace Opw
{
    public class ApplicationOptions
    {
        public string Name { get; set; }
        public Environment Environment { get; set; }
        public bool CreateAndSeedDatabases { get; set; }
        public bool WarmUpApplication { get; set; }
        public string Version { get; set; }

        public bool IsDevelopment
        {
            get { return (Environment < Environment.Production); }
        }
    }
}
