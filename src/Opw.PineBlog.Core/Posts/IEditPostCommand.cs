using System;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Interface for commands that edit posts.
    /// </summary>
    public interface IEditPostCommand
    {
        /// <summary>
        /// The name of the user adding the post.
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// The post title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The slug for this post.
        /// </summary>
        string Slug { get; set; }

        /// <summary>
        /// A short description for the post.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The post content in markdown format.
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// A comma separated list of categories.
        /// </summary>
        string Categories { get; set; }

        /// <summary>
        /// The date the post was published or NULL for unpublished posts.
        /// </summary>
        DateTime? Published { get; set; }

        /// <summary>
        /// Cover UURLrl.
        /// </summary>
        string CoverUrl { get; set; }

        /// <summary>
        /// Cover caption.
        /// </summary>
        string CoverCaption { get; set; }

        /// <summary>
        /// Cover link.
        /// </summary>
        string CoverLink { get; set; }
    }
}