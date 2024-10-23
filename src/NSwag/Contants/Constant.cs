
using System.Collections.Generic;

namespace NSwag.Contants
{
    public static class Constant
    {
        public static readonly List<string> IgnoreModules = new()
        {
            "jQuery"
        };

        public static readonly List<string> UtilitiesModules = new()
        {
            "throwException",
            "FileParameter",
            "FileResponse",
            "SwaggerException",
            "ServiceBase",
            "blobToText"
        };

        public static List<string> TsBaseType = new()
        {
            "string","number","Date","undefined","any","boolean","void","{ [key: string]: any; }","{ [key: string]: string; }"
        };

        public static string UtilsName = "Utilities";
    }
}