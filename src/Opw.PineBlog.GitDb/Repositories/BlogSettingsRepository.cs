using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using Opw.PineBlog.Repositories;
using System;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Opw.PineBlog.GitDb.LibGit2;

namespace Opw.PineBlog.GitDb.Repositories
{
    public class BlogSettingsRepository : RepositoryBase, IBlogSettingsRepository
    {
        public BlogSettingsRepository(GitDbContext gitDbContext, IOptions<PineBlogGitDbOptions> options) : base(gitDbContext, options) { }

        public async Task<BlogSettings> SingleOrDefaultAsync(CancellationToken cancellationToken)
        {
            IDictionary<string, byte[]> files;

            try
            {
                files = await GitDbContext.GetFilesAsync(new string[] { PathHelper.Build(Options.Value.RootPath, GitDbConstants.BlogSettingsFile) }, cancellationToken);
            }
            catch
            {
                return null;
            }

            var json = Encoding.UTF8.GetString(files.Values.Single());
            var blogSettings = JsonSerializer.Deserialize<BlogSettings>(json, new JsonSerializerOptions { AllowTrailingCommas = true });            
            return blogSettings;
        }

        public BlogSettings Add([NotNull] BlogSettings blogSettings)
        {
            throw new NotImplementedException();
        }

        public BlogSettings Update([NotNull] BlogSettings blogSettings)
        {
            throw new NotImplementedException();
        }
    }
}
