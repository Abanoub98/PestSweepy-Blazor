namespace Dashboard.Blazor.Extensions
{
    public static class LocalizationExtensions
    {
        /// <summary>
        /// Value format: "EN~AR"
        /// lang: "en" or "ar" (any case), can accept "en-US", "ar-EG", etc.
        /// </summary>
        public static string SplitLocalized(this string? value, string? lang, char separator = '~')
        {
            if (string.IsNullOrWhiteSpace(value))
                return "-";

            var isArabic = !string.IsNullOrWhiteSpace(lang) &&
                           lang.StartsWith("ar", StringComparison.OrdinalIgnoreCase);

            var parts = value.Split(separator);

            // If no separator found, return as-is
            if (parts.Length == 1)
                return value.Trim();

            var en = parts[0].Trim();
            var ar = (parts.Length > 1 ? parts[1] : "").Trim();

            return isArabic
                ? (!string.IsNullOrWhiteSpace(ar) ? ar : en)
                : (!string.IsNullOrWhiteSpace(en) ? en : ar);
        }
    }
}