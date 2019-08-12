using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Opw
{
    public static class DateTimeExtensions
    {
        public static string ToUtc(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
        }

        public static string ToFriendlyDateTimeString(this DateTime Date)
        {
            return FriendlyDate(Date) + " @ " + Date.ToString("t").ToLower();
        }

        public static string ToFriendlyShortDateString(this DateTime Date)
        {
            return $"{Date.ToString("MMM dd")}, {Date.Year}";
        }

        public static string ToFriendlyDateString(this DateTime Date)
        {
            return FriendlyDate(Date);
        }

        static string FriendlyDate(DateTime date)
        {
            string FormattedDate;
            if (date.Date == DateTime.Today)
            {
                FormattedDate = "Today";
            }
            else if (date.Date == DateTime.Today.AddDays(-1))
            {
                FormattedDate = "Yesterday";
            }
            else if (date.Date > DateTime.Today.AddDays(-6))
            {
                // *** Show the Day of the week
                FormattedDate = date.ToString("dddd", CultureInfo.InvariantCulture);
            }
            else
            {
                FormattedDate = date.ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture);
            }
            return FormattedDate;
        }
    }
}
