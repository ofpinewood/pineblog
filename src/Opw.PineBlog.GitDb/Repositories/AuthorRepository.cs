using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System;
using Opw.PineBlog.Repositories;
using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.Json;

namespace Opw.PineBlog.GitDb.Repositories
{
    public class AuthorRepository : RepositoryBase, IAuthorRepository
    {
        public AuthorRepository(IOptionsSnapshot<PineBlogGitDbOptions> options) : base(options) { }

        public async Task<Author> SingleOrDefaultAsync(Expression<Func<Author, bool>> predicate, CancellationToken cancellationToken)
        {
            var gitDbContext = await GetGitDbContextAsync(cancellationToken);

            IDictionary<string, byte[]> files;

            try
            {
                files = await gitDbContext.GetFilesAsync(new string[] { BuildPath(Options.Value.RootPath, "Authors.json") }, cancellationToken);
            }
            catch
            {
                return null;
            }

            var json = Encoding.UTF8.GetString(files.Values.Single());
            var authors = JsonSerializer.Deserialize<IEnumerable<Author>>(json, new JsonSerializerOptions { AllowTrailingCommas = true });

            return authors.SingleOrDefault(predicate.Compile());
        }
    }
}
