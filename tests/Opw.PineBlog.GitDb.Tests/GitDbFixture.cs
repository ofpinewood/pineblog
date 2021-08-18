using Microsoft.Extensions.Configuration;
using Opw.PineBlog.GitDb.LibGit2;
using System;

namespace Opw.PineBlog.GitDb
{
    public sealed class GitDbFixture : IDisposable
    {
        private GitDbContext _gitDbContext;

        public GitDbFixture()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

            var options = configuration.GetSection(nameof(PineBlogGitDbOptions)).Get<PineBlogGitDbOptions>();

            _gitDbContext = GitDbContext.Create(options);
            _gitDbContext.CheckoutBranch(options.Branch);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing) { }

            // always try to dispose, even if disposing=false
            if (_gitDbContext != null)
            {
                try { _gitDbContext?.Dispose(); } catch { }
                _gitDbContext = null;
                GC.Collect();
            }
        }
    }
}
