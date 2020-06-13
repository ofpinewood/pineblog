using Opw.PineBlog.Entities;
using Opw.PineBlog.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog
{
    public interface IRepository
    {
        Task<Result<int>> AddAuthorAsync(Author author, CancellationToken cancellationToken);
        Task<Result<int>> AddPostAsync(Post post, CancellationToken cancellationToken);
        Task<Author> GetAuthorByUsernameAsync(string username);
        Task<Result<int>> DeletePostAsync(Post post, CancellationToken cancellationToken);
        Task<Result<int>> DeleteBlogSettingsAsync(BlogSettings settings, CancellationToken cancellationToken);
        Task<BlogSettings> GetBlogSettingsAsync(CancellationToken cancellationToken);
        Task<IList<Post>> GetPostListAsync(bool includeUnpublished, Pager pager, string category, PineBlogOptions options, CancellationToken cancellationToken);
        Task<Post> GetPostByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Post> GetPostBySlugAsync(string slug, CancellationToken cancellationToken);
        Task<Post> GetNextPostAsync(Post currentPost, CancellationToken cancellationToken);
        Task<Post> GetPreviousPostAsync(Post currentPost, CancellationToken cancellationToken);
        Task<List<Post>> GetSyndicationPostsAsync(CancellationToken cancellationToken);
        Task<Result<Post>> PublishPostAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<Post>> UnpublishPostAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<int>> UpdateBlogSettingsAsync(BlogSettings settings, CancellationToken cancellationToken);
        Task<Result<int>> UpdatePostAsync(Post post, CancellationToken cancellationToken);
    }
}
