
using System.Collections.Generic;

namespace NSwag.Constants
{
    public static class Constant
    {
        public static readonly List<string> IgnoreModules = ["jQuery"];

        public static readonly List<string> UtilitiesModules =
        [
            "throwException",
            "FileParameter",
            "FileResponse",
            "SwaggerException",
            "ServiceBase",
            "blobToText"
        ];

        public static readonly List<string> TsBaseType =
        [
            "string", "number", "Date", "undefined", "any", "boolean", "void", "{ [key: string]: any; }",
            "{ [key: string]: string; }"
        ];

        public const string UtilsName = "Utilities";
    }
}