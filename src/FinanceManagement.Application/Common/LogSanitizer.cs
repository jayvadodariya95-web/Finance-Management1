using System.Text.RegularExpressions;

namespace FinanceManagement.Application.Common
{
    public static class LogSanitizer
    {
        // Masks sensitive values like passwords, tokens, secrets, etc.
        private static readonly Regex SensitiveDataRegex = new Regex(
            @"(password|pwd|token|secret|apikey|api_key|authorization)\s*[:=]\s*([^,\s]+)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled
        );

        /// <summary>
        /// Removes or masks sensitive information from log messages
        /// </summary>
        public static string Sanitize(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return "No error message provided";

            // Mask sensitive key=value pairs
            var sanitized = SensitiveDataRegex.Replace(
                message,
                "$1=****"
            );

            // Prevent log flooding
            if (sanitized.Length > 500)
                sanitized = sanitized.Substring(0, 500) + "...";

            return sanitized;
        }
    }
}
