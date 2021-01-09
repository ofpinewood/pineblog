using System;

namespace Opw.PineBlog.Entities
{
    public class Post : IEntity<Guid>, IEntityCreated, IEntityModified
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public Guid AuthorId { get; set; }
        public virtual Author Author { get; set; }

        /// <summary>
        /// The post title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short description for the post.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The post content in markdown format.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// A comma separated list of categories.
        /// </summary>
        public string Categories { get; set; }

        /// <summary>
        /// The date the post was published or NULL for unpublished posts.
        /// </summary>
        public DateTime? Published { get; set; }

        public string Slug { get; set; }

        /// <summary>
        /// Cover URL.
        /// </summary>
        public string CoverUrl { get; set; }

        /// <summary>
        /// Cover caption text.
        /// </summary>
        public string CoverCaption { get; set; }

        /// <summary>
        /// Cover link.
        /// </summary>
        public string CoverLink { get; set; }
    }
}
