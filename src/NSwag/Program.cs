﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using NSwag.Constants;
using NSwag.Generators;
using NSwag.Helpers;
using Serilog;

namespace NSwag
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            // resolve the settings file
            var config = ArgumentsHelper.ReadArgs(args);

            if (string.IsNullOrWhiteSpace(config.ConfigPath))
            {
                throw new FileNotFoundException("Please specify *.nswag file.");
            }
            if (!File.Exists(config.ConfigPath))
            {
                throw new FileNotFoundException($"Not found config file from :{config.ConfigPath}");
            }

            if (string.IsNullOrWhiteSpace(config.DtoPath)) config.DtoPath = "../dtos";

            Func<string, string> caseConverter;
            switch (config.FileCase)
            {
                case "kebab-case":
                    caseConverter = CaseConverters.ToKebabCase;
                    break;
                case "camelCase":
                    caseConverter = CaseConverters.ToCamelCase;
                    break;
                case "snake_case":
                    caseConverter = CaseConverters.ToSnakeCase;
                    break;
                case "Title Case":
                    caseConverter = CaseConverters.ToTitleCase;
                    break;
                case "ALL_CAPS_SNAKE_CASE":
                    caseConverter = CaseConverters.ToAllCapsSnakeCase;
                    break;
                case "PascalCase":
                    caseConverter = x => x;
                    break;
                default:
                    config.FileCase = "No conversion";
                    caseConverter = x => x;
                    break;
            }
            Log.Information("Using {0} for generating file names", config.FileCase);

            Log.Information("Read config files:[{0}]", config.ConfigPath);
            var stopwatch = Stopwatch.StartNew();
            var configFilePath = Path.GetFullPath(config.ConfigPath);
            Log.Information("Use config file:[{0}]", configFilePath);
            var nSwagDocument = await NsWagDocumentHelper.LoadDocumentFromFileAsync(configFilePath);
            stopwatch.Stop();
            Log.Information("NSwag config file loaded, used time:{0}ms", stopwatch.Elapsed.TotalMilliseconds);
            var outputDirectory = IoHelper.ReadOutputPath(nSwagDocument, configFilePath);
            Log.Information("Output directory is :[{0}]", outputDirectory);
            stopwatch.Restart();
            // Fetch swagger
            var swaggerDocument = await OpenApiDocumentHelper.FromUrlAsync(nSwagDocument.SwaggerGenerators.FromDocumentCommand.Url);
            stopwatch.Stop();
            Log.Information("Swagger content loaded, used time:{0}ms", stopwatch.Elapsed.TotalMilliseconds);
            stopwatch.Restart();
            var settings = nSwagDocument.CodeGenerators.OpenApiToTypeScriptClientCommand.Settings;
            settings.ExcludedParameterNames ??= [];
            Constant.TsBaseType.AddRange(settings.ExcludedParameterNames);
            // Utilities
            var utilitiesScriptGenerator = new UtilitiesScriptGenerator(settings, swaggerDocument, caseConverter);
            await utilitiesScriptGenerator.GenerateUtilitiesFilesAsync(outputDirectory);
            stopwatch.Stop();
            Log.Information("Generate utilities complete, used time:{0}ms", stopwatch.Elapsed.TotalMilliseconds);
            stopwatch.Restart();
            // Dto classes
            var modelsScriptGenerator = new ModelsScriptGenerator(settings, swaggerDocument, caseConverter);
            modelsScriptGenerator.SetDtoPath(config.DtoPath);
            await modelsScriptGenerator.GenerateDtoFilesAsync(outputDirectory);
            stopwatch.Stop();
            Log.Information("Generate dto files over, used time:{0}ms", stopwatch.Elapsed.TotalMilliseconds);
            // Service proxies
            stopwatch.Restart();
            var clientsScriptGenerator = new ClientsScriptGenerator(settings, swaggerDocument, caseConverter);
            clientsScriptGenerator.SetDtoPath(config.DtoPath);
            await clientsScriptGenerator.GenerateClientClassFilesAsync(outputDirectory);
            stopwatch.Stop();
            Log.Information("Generate client files over, used time:{0}ms", stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}