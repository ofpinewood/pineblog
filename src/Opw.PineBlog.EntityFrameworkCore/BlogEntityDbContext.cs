using Microsoft.EntityFrameworkCore;
using Opw.PineBlog.Entities;
using Opw.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using Opw.PineBlog.Models;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Opw.HttpExceptions;

namespace Opw.PineBlog.EntityFrameworkCore
{
    public class BlogEntityDbContext : EntityDbContext, IRepository
    {
        public DbSet<BlogSettings> BlogSettings { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Author> Authors { get; set; }

        public BlogEntityDbContext(DbContextOptions<BlogEntityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName($"PineBlog_{entityType.GetTableName()}");
            }
        }

        public Task<Result<int>> AddPostAsync(Post post, CancellationToken cancellationToken)
        {
            Posts.Add(post);
            return SaveChangesAsync(cancellationToken);
        }

        public Task<Author> GetAuthorByUsernameAsync(string username)
        {
            return Authors.SingleOrDefaultAsync(a => a.UserName.Equals(username));
        }

        public Task<Result<int>> DeletePostAsync(Post post, CancellationToken cancellationToken)
        {
            Posts.Remove(post);
            return SaveChangesAsync(true, cancellationToken);
        }

        public async Task<IList<Post>> GetPostListAsync(bool includeUnpublished, Pager pager, string category, PineBlogOptions options, CancellationToken cancellationToken)
        {
            var pagingUrlPartFormat = options.PagingUrlPartFormat;
            var predicates = new List<Expression<Func<Post, bool>>>();
            if (!includeUnpublished)
                predicates.Add(p => p.Published != null);
            if (!string.IsNullOrWhiteSpace(category))
            {
                predicates.Add(p => p.Categories.Contains(category));
                pagingUrlPartFormat += "&" + string.Format(options.CategoryUrlPartFormat, category);
            }
            var posts = await GetPagedListAsync(predicates, pager, pagingUrlPartFormat, cancellationToken);
            return posts;
        }

        private async Task<IList<Post>> GetPagedListAsync(IList<Expression<Func<Post, bool>>> predicates, Pager pager, string pagingUrlPartFormat, CancellationToken cancellationToken)
        {
            var skip = (pager.CurrentPage - 1) * pager.ItemsPerPage;

            var countQuery = Posts.Where(_ => true);
            foreach (var predicate in predicates)
                countQuery = countQuery.Where(predicate);

            var count = await countQuery.CountAsync(cancellationToken);

            pager.Configure(count, pagingUrlPartFormat);

            var query = Posts.Where(_ => true);

            foreach (var predicate in predicates)
                query = query.Where(predicate);

            query = query.Include(p => p.Author)
                .OrderByDescending(p => p.Published)
                .Skip(skip)
                .Take(pager.ItemsPerPage);

            return await query.ToListAsync(cancellationToken);
        }

        public Task<Post> GetPostByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Posts.SingleOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
        }

        public Task<Post> GetPostBySlugAsync(string slug, CancellationToken cancellationToken)
        {
            return Posts
                .Include(p => p.Author)
                .Where(p => p.Published != null)
                .Where(p => p.Slug.Equals(slug))
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<Result<Post>> PublishPostAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await GetPostByIdAsync(id, cancellationToken);
            if (entity == null)
                return Result<Post>.Fail(new NotFoundException<Post>($"Could not find post, id: \"{id}\""));

            entity.Published = DateTime.UtcNow;

            Posts.Update(entity);
            await SaveChangesAsync(true, cancellationToken);
            return Result<Post>.Success(entity);
        }

        public async Task<Result<Post>> UnpublishPostAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await GetPostByIdAsync(id, cancellationToken);
            if (entity == null)
                return Result<Post>.Fail(new NotFoundException<Post>($"Could not find post, id: \"{id}\""));

            entity.Published = null;

            Posts.Update(entity);
            await SaveChangesAsync(true, cancellationToken);
            return Result<Post>.Success(entity);
        }

        public async Task<Result<int>> UpdatePostAsync(Post post, CancellationToken cancellationToken)
        {
            Posts.Update(post);
            return await SaveChangesAsync(true, cancellationToken);
        }

        public Task<BlogSettings> GetBlogSettingsAsync(CancellationToken cancellationToken)
        {
            return BlogSettings.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<Result<int>> UpdateBlogSettingsAsync(BlogSettings settings, CancellationToken cancellationToken)
        {
            if (settings.Created == default)
                BlogSettings.Add(settings);
            else
                BlogSettings.Update(settings);
            return await SaveChangesAsync(true, cancellationToken);
        }

        public Task<Post> GetPreviousPostAsync(Post currentPost, CancellationToken cancellationToken)
        {
            return Posts
                .Where(p => p.Published < currentPost.Published)
                .OrderByDescending(p => p.Published)
                .Take(1)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public Task<Post> GetNextPostAsync(Post currentPost, CancellationToken cancellationToken)
        {
            return Posts
                .Where(p => p.Published > currentPost.Published)
                .OrderBy(p => p.Published)
                .Take(1)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public Task<List<Post>> GetSyndicationPostsAsync(CancellationToken cancellationToken)
        {
            return Posts
                .Include(p => p.Author)
                .Where(p => p.Published != null)
                .OrderByDescending(p => p.Published)
                .Take(25)
                .ToListAsync(cancellationToken);
        }

        public async Task<Result<int>> AddAuthorAsync(Author author, CancellationToken cancellationToken)
        {
            Authors.Add(author);
            return await SaveChangesAsync(true, cancellationToken);
        }

        public Task<Result<int>> DeleteBlogSettingsAsync(BlogSettings settings, CancellationToken cancellationToken)
        {
            BlogSettings.Remove(settings);
            return SaveChangesAsync(true, cancellationToken);
        }
    }
}
