using System.Text;

namespace NSwag.Helpers
{
    public static class CaseConverters
    {
        // Convert a string to camelCase
        public static string ToCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            // Check if the first character is uppercase, if so, convert it to lowercase.
            if (char.IsUpper(input[0]))
            {
                var result = new StringBuilder(input);
                result[0] = char.ToLower(result[0]);
                return result.ToString();
            }

            return input; // If it's already camelCase, return as is.
        }

        // Convert a string to snake_case
        public static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var result = new StringBuilder(input.Length + 5); // Adding buffer to avoid frequent resizing

            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];

                if (char.IsUpper(c))
                {
                    if (i > 0)
                    {
                        result.Append('_');
                    }
                    result.Append(char.ToLower(c));
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        // Convert a string to kebab-case
        public static string ToKebabCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var result = new StringBuilder(input.Length + 5); // Adding some buffer

            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];

                if (char.IsUpper(c))
                {
                    if (i > 0)
                    {
                        result.Append('-');
                    }
                    result.Append(char.ToLower(c));
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        // Convert a string to Title Case (Title Case with spaces between words)
        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var result = new StringBuilder(input.Length + 5); // Adding some buffer

            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];

                if (char.IsUpper(c))
                {
                    if (i > 0)
                    {
                        result.Append(' ');
                    }
                }
                result.Append(c);
            }

            return result.ToString();
        }

        // Convert a string to ALL_CAPS_SNAKE_CASE
        public static string ToAllCapsSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var result = new StringBuilder(input.Length + 5); // Adding some buffer for underscores

            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];

                if (char.IsUpper(c))
                {
                    if (i > 0)
                    {
                        result.Append('_');
                    }
                    result.Append(c); // Already uppercase, no need to call ToUpper()
                }
                else
                {
                    result.Append(char.ToUpper(c)); // Convert lowercase letters to uppercase
                }
            }

            return result.ToString();
        }
    }

}
