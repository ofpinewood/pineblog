using System.Threading.Tasks;
using System.Threading;
using Opw.PineBlog.Repositories;
using Opw.PineBlog.GitDb.LibGit2;
using Opw.PineBlog.GitDb.Repositories;
using System;

namespace Opw.PineBlog.GitDb
{
    public class BlogUnitOfWork : IBlogUnitOfWork
    {
        private readonly GitDbContext _gitDbContext;

        public IBlogSettingsRepository BlogSettings { get; }
        public IAuthorRepository Authors { get; }
        public IPostRepository Posts { get; }

        public BlogUnitOfWork(GitDbContext gitDbContext)
        {
            _gitDbContext = gitDbContext;

            BlogSettings = new BlogSettingsRepository(_gitDbContext);
            Authors = new AuthorRepository(_gitDbContext);
            Posts = new PostRepository(_gitDbContext);
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
