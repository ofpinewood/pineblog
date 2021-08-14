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

namespace Opw.PineBlog.GitDb.Repositories
{
    public class BlogSettingsRepository : RepositoryBase, IBlogSettingsRepository
    {
        public BlogSettingsRepository(IOptionsSnapshot<PineBlogGitDbOptions> options) : base(options) { }

        public async Task<BlogSettings> SingleOrDefaultAsync(CancellationToken cancellationToken)
        {
            var gitDbContext = await GetGitDbContextAsync(cancellationToken);

            IDictionary<string, byte[]> files;

            try
            {
                files = await gitDbContext.GetFilesAsync(new string[] { BuildPath(Options.Value.RootPath, "BlogSettings.json") }, cancellationToken);
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
