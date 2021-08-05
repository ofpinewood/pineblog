using System.Threading.Tasks;
using System.Threading;
using Opw.PineBlog.Repositories;

namespace Opw.PineBlog.Git
{
    public class BlogUnitOfWork : IBlogUnitOfWork
    {
        private readonly BlogEntityDbContext _dbContext;

        public IBlogSettingsRepository BlogSettings { get; }
        public IAuthorRepository Authors { get; }
        public IPostRepository Posts { get; }

        public BlogUnitOfWork(BlogEntityDbContext dbContext)
        {
            _dbContext = dbContext;

            BlogSettings = new BlogSettingsRepository(_dbContext);
            Authors = new AuthorRepository(_dbContext);
            Posts = new PostRepository(_dbContext);
        }

        public Result<int> SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _dbContext.SaveChangesAsync(true, cancellationToken);
        }
    }
}
