using System.Globalization;

namespace ForkAndSpoon.Application
{
    public static class NameFormatter
    {
        // Normalize name: lowercase and convert to title case
        // Example: "  green PEPPER " → "Green Pepper"
        public static string Normalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var trimmed = input.Trim();
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(trimmed.ToLower());
        }
    }
}
