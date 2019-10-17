using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Opw
{
    public static class StringExtensions
    {
        public static string ToUrlSafeFileName(this string fileName)
        {
            var extension = fileName.Substring(fileName.LastIndexOf('.')).Trim('.');
            var name = fileName.Substring(0, fileName.LastIndexOf('.'));

            name = name.ToSlug();
            return $"{name}.{extension}";
        }

        public static string ToSlug(this string s)
        {
            string str = s.RemoveDiacritics();
            // add a space before every uppercase char
            str = Regex.Replace(str, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1");
            str = str.ToLower();

            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // trim 
            str = str.Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        /// <summary>
        /// Remove diacritics (accents) from a string
        /// </summary>
        /// <see cref="https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net"/>
        public static string RemoveDiacritics(this string s)
        {
            var normalizedString = s.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
