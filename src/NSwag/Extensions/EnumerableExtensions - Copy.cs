using System;
using NSwag.Constants;

namespace NSwag.Extensions
{
    public static class Extensions
    {
        public static bool IsTypeScriptBaseType(this string typeToCheck)
        {
            if (string.IsNullOrWhiteSpace(typeToCheck))
                return false;

            // Remove all whitespace for easier comparison
            var normalizedType = typeToCheck.Replace(" ", "");

            // Check direct match
            if (Constant.TsBaseType.Contains(normalizedType))
                return true;

            // Check for nullable or undefined variants (e.g., "type|null" or "type|undefined")
            var typeParts = normalizedType.Split(['|'], StringSplitOptions.RemoveEmptyEntries);

            if (typeParts.Length == 2)
            {
                var mainType = typeParts[0];
                var modifier = typeParts[1];

                return Constant.TsBaseType.Contains(mainType) &&
                       modifier is "null" or "undefined";
            }

            return false;
        }
    }
}