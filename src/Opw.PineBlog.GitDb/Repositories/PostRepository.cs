using Opw.PineBlog.Entities;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using Opw.PineBlog.Repositories;
using Microsoft.Extensions.Options;
using System.Text;
using System.Linq;
using System.Text.Json;
using Opw.PineBlog.GitDb.Entities;

namespace Opw.PineBlog.GitDb.Repositories
{
    public class PostRepository : RepositoryBase, IPostRepository
    {
        private readonly AuthorRepository _authorRepository;

        public PostRepository(IOptionsSnapshot<PineBlogGitDbOptions> options)
            : base(options)
        {
            _authorRepository = new AuthorRepository(options);
        }

        public async Task<Post> SingleOrDefaultAsync(Expression<Func<Post, bool>> predicate, CancellationToken cancellationToken)
        {
            var posts = await GetAllAsync(cancellationToken);

            return posts.SingleOrDefault(predicate.Compile());
        }

        public async Task<Post> GetNextAsync(DateTime published, CancellationToken cancellationToken)
        {
            var posts = await GetAllAsync(cancellationToken);

            var post = posts
              .Where(p => p.Published > published)
              .OrderBy(p => p.Published)
              .Take(1)
              .SingleOrDefault();

            if (post != null)
                post.Author = null;

            return post;
        }

        public async Task<Post> GetPreviousAsync(DateTime published, CancellationToken cancellationToken)
        {
            var posts = await GetAllAsync(cancellationToken);

            var post = posts
              .Where(p => p.Published < published)
              .OrderByDescending(p => p.Published)
              .Take(1)
              .SingleOrDefault();

            if (post != null)
                post.Author = null;

            return post;
        }

        public async Task<int> CountAsync(IEnumerable<Expression<Func<Post, bool>>> predicates, CancellationToken cancellationToken)
        {
            var posts = await GetAllAsync(cancellationToken);

            var query = posts.Where(_ => true);
            foreach (var predicate in predicates)
                query = query.Where(predicate.Compile());

            return query.Count();
        }

        public async Task<IEnumerable<Post>> GetAsync(IEnumerable<Expression<Func<Post, bool>>> predicates, int skip, int take, CancellationToken cancellationToken)
        {
            var posts = await GetAllAsync(cancellationToken);

            var query = posts.Where(_ => true);

            foreach (var predicate in predicates)
                query = query.Where(predicate.Compile());

            query = query
                .OrderByDescending(p => p.Published)
                .Skip(skip)
                .Take(take);

            return query.ToList();
        }

        public Post Add([NotNull] Post post)
        {
            throw new NotImplementedException();
        }

        public Post Update([NotNull] Post post)
        {
            throw new NotImplementedException();
        }

        public Post Remove([NotNull] Post post)
        {
            throw new NotImplementedException();
        }

        // TODO: add caching for get all posts
        protected async Task<IEnumerable<Post>> GetAllAsync(CancellationToken cancellationToken)
        {
            var gitDbContext = await GetGitDbContextAsync(cancellationToken);

            IDictionary<string, byte[]> files;
            var posts = new List<Post>();

            try
            {
                files = await gitDbContext.GetFilesAsync(BuildPath(Options.Value.RootPath, "posts"), cancellationToken);
            }
            catch
            {
                return posts;
            }

            var postFiles = files.Values.Select(b => Encoding.UTF8.GetString(b));
            foreach (var postFile in postFiles)
            {
                var json = postFile.Substring(0, postFile.IndexOf("<<< END METADATA"));
                var gitDbPost = JsonSerializer.Deserialize<GitDbPost>(json, new JsonSerializerOptions { AllowTrailingCommas = true });

                var post = new Post
                {
                    Title = gitDbPost.Title,
                    Description = gitDbPost.Description,
                    Categories = gitDbPost.Categories,
                    Published = gitDbPost.Published,
                    Slug = gitDbPost.Slug,
                    CoverUrl = gitDbPost.CoverUrl,
                    CoverCaption = gitDbPost.CoverCaption,
                    CoverLink = gitDbPost.CoverLink,
                };

                var content = postFile.Substring(postFile.IndexOf("<<< END METADATA") + "<<< END METADATA".Length);
                post.Content = content.Trim();

                // TODO: add caching for authors
                var author = await _authorRepository.SingleOrDefaultAsync(a => a.UserName == gitDbPost.AuthorId, cancellationToken);
                post.Author = author;

                posts.Add(post);
            }

            return posts;
        }
    }
}
