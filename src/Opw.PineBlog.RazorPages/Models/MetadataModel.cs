using System;
using System.Collections.Generic;

namespace Opw.PineBlog.RazorPages.Models
{
    /// <summary>
    /// Model for the _Metadata partial view.
    /// </summary>
    public class MetadataModel
    {
        /// <summary>
        /// The title of your object as it should appear within the graph, e.g., "The Rock".
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description of the page.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The keywords for search engines.
        /// </summary>
        public IEnumerable<string> Keywords { get; set; } = new List<string>();

        /// <summary>
        /// The published date, when Type is "article".
        /// </summary>
        public DateTime? Published { get; set; }

        /// <summary>
        /// The author of the page.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The type of your object, e.g., "video.movie".
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// An image URL which should represent your object within the graph.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// The canonical URL of your object that will be used as its permanent ID in the graph, e.g., "http://www.imdb.com/title/tt0117500/".
        /// </summary>
        public string Url { get; set; }
    }
}
