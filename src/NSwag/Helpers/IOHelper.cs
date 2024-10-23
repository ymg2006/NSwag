using System;
using System.IO;
using NSwag.Commands;

namespace NSwag.Helpers
{
    public static class IoHelper
    {
        public static void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public static string ReadOutputPath(NSwagDocument nSwagDocument, string? configFilePath)
        {
            var outputDirectory = nSwagDocument.CodeGenerators.OpenApiToTypeScriptClientCommand.OutputFilePath;
            var outputPath = outputDirectory.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            configFilePath = Path.GetDirectoryName(configFilePath)!;
            if (outputDirectory.StartsWith('.') || outputPath.IndexOf(":", StringComparison.OrdinalIgnoreCase) < 0)
            {
                outputPath = Path.GetFullPath(Path.Combine(configFilePath, outputPath));
            }

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            return outputPath;
        }
    }
}