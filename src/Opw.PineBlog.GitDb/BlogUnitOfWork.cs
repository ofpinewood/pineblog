using System.Threading.Tasks;
using System.Threading;
using Opw.PineBlog.Repositories;
using Opw.PineBlog.GitDb.Repositories;
using System;
using Microsoft.Extensions.Options;

namespace Opw.PineBlog.GitDb
{
    public class BlogUnitOfWork : IBlogUnitOfWork
    {
        public IBlogSettingsRepository BlogSettings { get; }
        public IAuthorRepository Authors { get; }
        public IPostRepository Posts { get; }

        public BlogUnitOfWork(IOptionsSnapshot<PineBlogGitDbOptions> options)
        {
            BlogSettings = new BlogSettingsRepository(options);
            Authors = new AuthorRepository(options);
            Posts = new PostRepository(options);
        }

        public Result<int> SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
