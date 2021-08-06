namespace Opw.PineBlog.GitDb
{
    public class GitSettings
    {
        public string AccessToken { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string LocalRepositoryBasePath { get; set; }
        public string SourceUrl { get; set; }
        public string Workdirpath { get; set; }
        public int CommitFrequency { get; set; }
        public int SyncFrequency { get; set; }
        public string Branch { get; set; }
    }
}
