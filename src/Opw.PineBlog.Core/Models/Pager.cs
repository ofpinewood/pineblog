
namespace Opw.PineBlog.Models
{
    /// <summary>
    /// Pager.
    /// </summary>
    public class Pager
    {
        public int CurrentPage { get; set; } = 1;
        public int ItemsPerPage { get; set; }
        public int Total { get; set; }
        public bool NotFound { get; set; }

        public int Newer { get; set; }
        public bool ShowNewer { get; set; }
        public string LinkToNewer { get; set; }

        public int Older { get; set; }
        public bool ShowOlder { get; set; }
        public string LinkToOlder { get; set; }

        public int LastPage { get; set; } = 1;

        /// <summary>
        /// Implementation of Pager.
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        public Pager(int currentPage, int itemsPerPage = 0)
        {
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;

            if (ItemsPerPage == 0)
                ItemsPerPage = 10;

            Newer = CurrentPage - 1;
            ShowNewer = CurrentPage > 1 ? true : false;

            Older = currentPage + 1;
        }

        /// <summary>
        /// Configure the pager.
        /// </summary>
        /// <param name="total">The total number of results.</param>
        /// <param name="pagingUrlPartFormat">A url part format for the paging querystring params.</param>
        public void Configure(int total, string pagingUrlPartFormat)
        {
            if (total == 0)
                return;

            if (ItemsPerPage == 0)
                ItemsPerPage = 10;

            Total = total;
            var lastItem = CurrentPage * ItemsPerPage;
            ShowOlder = total > lastItem ? true : false;
            if (CurrentPage < 1 || lastItem > total + ItemsPerPage)
            {
                NotFound = true;
            }
            LastPage = (total % ItemsPerPage) == 0 ? total / ItemsPerPage : (total / ItemsPerPage) + 1;
            if (LastPage == 0) LastPage = 1;

            if (ShowOlder) LinkToOlder = string.Format(pagingUrlPartFormat, Older);
            if (ShowNewer) LinkToNewer = string.Format(pagingUrlPartFormat, Newer);
        }
    }
}
