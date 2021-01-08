using Opw.PineBlog.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Repositories
{
    public interface IPostRepository
    {
        Task<Post> SingleOrDefaultAsync(Expression<Func<Post, bool>> predicate, CancellationToken cancellationToken);

        /// <summary>
        /// Get the next post after the published date of the current post.
        /// </summary>
        /// <param name="published">The published date of the current post.</param>
        /// <param name="cancellationToken"></param>
        Task<Post> GetNextAsync(DateTime published, CancellationToken cancellationToken);

        /// <summary>
        /// Get the previous post before the published date of the current post.
        /// </summary>
        /// <param name="published">The published date of the current post.</param>
        /// <param name="cancellationToken"></param>
        Task<Post> GetPreviousAsync(DateTime published, CancellationToken cancellationToken);

        /// <summary>
        /// Returns the number of elements in a sequence based on one or more predicates.
        /// </summary>
        /// <param name="predicates">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<int> CountAsync(IEnumerable<Expression<Func<Post, bool>>> predicates, CancellationToken cancellationToken);

        /// <summary>
        /// Filters a sequence of values based on one or more predicates and returns it in descending order by published date.
        /// </summary>
        /// <param name="predicates">A function to test each element for a condition.</param>
        /// <param name="skip">The number of elements to skip before returning the remaining elements.</param>
        /// <param name="take">The number of elements to return.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<IEnumerable<Post>> GetAsync(IEnumerable<Expression<Func<Post, bool>>> predicates, int skip, int take, CancellationToken cancellationToken);

        Post Add([NotNull] Post post);

        Post Update([NotNull] Post post);

        Post Remove([NotNull] Post post);
    }
}