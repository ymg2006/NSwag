using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NSwag.Commands;

namespace NSwag.Helpers
{
    public static class IoHelper
    {
        public static async Task<bool> HandleFileAsync(string path, string content)
        {
            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                var fileSHA256 = CalculateSHA256(fileInfo);
                var contentCalculateSHA256 = CalculateSHA256(content);

                if (fileSHA256 == contentCalculateSHA256) return false;
                fileInfo.Delete();
            }
            await File.WriteAllTextAsync(path, NormalizeNewlines(content), Encoding.UTF8);
            return true;
        }

        public static bool TryDeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        static string NormalizeNewlines(string input)
        {
            return input.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        static string CalculateSHA256(FileInfo fileInfo)
        {
            var fileContent = File.ReadAllText(fileInfo.FullName);
            fileContent = NormalizeNewlines(fileContent);
            using var sha256 = SHA256.Create();
            var inputBytes = Encoding.UTF8.GetBytes(fileContent);
            var hashBytes = sha256.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        static string CalculateSHA256(string input)
        {
            using var sha256 = SHA256.Create();
            var inputBytes = Encoding.UTF8.GetBytes(NormalizeNewlines(input));
            var hashBytes = sha256.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
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