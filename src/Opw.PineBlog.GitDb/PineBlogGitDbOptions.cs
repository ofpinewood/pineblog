namespace Opw.PineBlog.GitDb
{
    public class PineBlogGitDbOptions
    {
        /// <summary>
        /// The url of the git repository.
        /// </summary>
        public string RepositoryUrl { get; set; }

        /// <summary>
        /// The name of the branch checkout.
        /// </summary>
        public string Branch { get; set; } = "main";

        ///// <summary>
        ///// The access token is only needed when the repository is private.
        ///// </summary>
        //public string AccessToken { get; set; }

        ///// <summary>
        ///// Git signature user name.
        ///// </summary>
        //public string UserName { get; set; }

        ///// <summary>
        ///// Git signature user email.
        ///// </summary>
        //public string UserEmail { get; set; }

        //public string Password { get; set; }

        public string LocalRepositoryBasePath { get; set; } = "pineblog-gitdb";
        
    }
}
