using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Opw.PineBlog.Posts.Search
{
    public static class SearchQueryExtensions
    {
        public static IEnumerable<string> ParseTerms(this string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Array.Empty<string>();
            }

            // convert multiple spaces into one space   
            query = Regex.Replace(query, @"\s+", " ").Trim();
            return query.ToLower().Split(' ').ToList();
        }
    }
}
