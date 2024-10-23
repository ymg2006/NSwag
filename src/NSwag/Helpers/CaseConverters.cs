using System.Text;

namespace NSwag.Helpers
{
    public static class CaseConverters
    {
        // Convert a string to PascalCase
        public static string ToPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // Pre-allocate StringBuilder capacity to minimize resizing
            var result = new StringBuilder(input.Length);
            var capitalizeNext = true; // Track if the next character should be capitalized

            foreach (var c in input)
            {
                // Handle word separators: spaces, hyphens, underscores
                if (c == ' ' || c == '-' || c == '_')
                {
                    capitalizeNext = true; // Next character should be capitalized
                }
                else
                {
                    // If we need to capitalize, do so and append; otherwise, append in lower case
                    result.Append(capitalizeNext ? char.ToUpperInvariant(c) : char.ToLowerInvariant(c));
                    capitalizeNext = false; // Reset flag
                }
            }

            // If the first character is not capitalized, we need to ensure it is lowercase
            if (result.Length > 0)
            {
                result[0] = char.ToUpperInvariant(result[0]);
            }

            return result.ToString();
        }

        // Convert a string to camelCase
        public static string ToCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // Pre-allocate StringBuilder capacity to minimize resizing
            var result = new StringBuilder(input.Length);
            var capitalizeNext = false; // Track if the next character should be capitalized

            foreach (var c in input)
            {
                // Handle word separators (spaces, hyphens, underscores)
                if (char.IsWhiteSpace(c) || c == '-' || c == '_')
                {
                    capitalizeNext = true; // Next character should be capitalized
                    continue; // Skip to the next character
                }

                // Determine character case and append
                if (capitalizeNext)
                {
                    result.Append(char.ToUpperInvariant(c));
                    capitalizeNext = false; // Reset the flag
                }
                else
                {
                    // Append as lowercase, unless it's the first character
                    result.Append(result.Length == 0 ? char.ToLowerInvariant(c) : char.ToLower(c));
                }
            }

            return result.ToString();
        }

        // Convert a string to snake_case
        public static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // Pre-allocate StringBuilder capacity to minimize resizing
            var result = new StringBuilder(input.Length);
            var isLastCharSeparator = true; // Track if the last character was a separator

            foreach (var c in input)
            {
                // Check if the character is a separator (space, hyphen, or underscore)
                if (char.IsWhiteSpace(c) || c == '-' || c == '_')
                {
                    isLastCharSeparator = true; // Mark as separator
                    continue; // Skip to the next character
                }

                // Append an underscore if the last character was a separator
                if (!isLastCharSeparator && result.Length > 0)
                {
                    result.Append('_');
                }

                // Append the lowercase character
                result.Append(char.ToLowerInvariant(c));
                isLastCharSeparator = false; // Reset the separator flag
            }

            return result.ToString();
        }

        // Convert a string to kebab-case
        public static string ToKebabCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // Pre-allocate StringBuilder capacity to minimize resizing
            var result = new StringBuilder(input.Length);
            var isLastCharSeparator = true; // Track if the last character was a separator

            foreach (var c in input)
            {
                // Handle separators
                if (char.IsWhiteSpace(c) || c == '-' || c == '_')
                {
                    isLastCharSeparator = true; // Mark as separator
                    continue; // Skip to the next character
                }

                // Append hyphen if the previous character was a separator
                if (isLastCharSeparator && result.Length > 0)
                {
                    result.Append('-');
                }

                // Append the current character in lowercase
                result.Append(char.ToLowerInvariant(c));
                isLastCharSeparator = false; // Reset the flag
            }

            return result.ToString();
        }

        // Convert a string to Title Case (Title Case with spaces between words)
        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // Pre-allocate StringBuilder capacity
            var result = new StringBuilder(input.Length);
            var capitalizeNext = true; // Flag for capitalization

            foreach (var c in input)
            {
                // Check for word boundaries
                if (char.IsWhiteSpace(c) || c == '-' || c == '_')
                {
                    result.Append(c); // Append separator
                    capitalizeNext = true; // Prepare to capitalize next character
                }
                else
                {
                    // Process character based on capitalization flag
                    result.Append(capitalizeNext ? char.ToUpperInvariant(c) : char.ToLowerInvariant(c));
                    capitalizeNext = false; // Reset flag after processing a character
                }
            }

            return result.ToString();
        }

        // Convert a string to ALL_CAPS_SNAKE_CASE
        public static string ToAllCapsSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var estimatedSize = input.Length * 2; // Estimate more space for underscores
            var result = new StringBuilder(estimatedSize);

            var needsUnderscore = false; // To track if an underscore is needed before the next word

            foreach (var c in input)
            {
                // Handle separators and word boundaries
                if (char.IsWhiteSpace(c) || c == '-' || c == '_')
                {
                    needsUnderscore = true; // Set flag to insert underscore before the next valid char
                }
                else if (char.IsLower(c))
                {
                    if (needsUnderscore && result.Length > 0)
                    {
                        result.Append('_');
                    }
                    result.Append(char.ToUpperInvariant(c)); // Convert lowercase to uppercase
                    needsUnderscore = false; // Reset flag
                }
                else if (char.IsUpper(c))
                {
                    if (needsUnderscore && result.Length > 0)
                    {
                        result.Append('_');
                    }
                    result.Append(c); // Append uppercase as is
                    needsUnderscore = false; // Reset flag
                }
                else
                {
                    // Handle digits or other characters directly, append without modifying
                    if (needsUnderscore && result.Length > 0)
                    {
                        result.Append('_');
                    }
                    result.Append(c);
                    needsUnderscore = false; // Reset flag
                }
            }

            return result.ToString();
        }
    }

}
