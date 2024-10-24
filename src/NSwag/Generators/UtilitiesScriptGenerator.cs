using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.TypeScript;
using NSwag.CodeGeneration.TypeScript;
using NSwag.CodeGeneration.TypeScript.Models;
using NSwag.Contants;
using NSwag.Helpers;

namespace NSwag.Generators;

public class UtilitiesScriptGenerator
{
    private readonly string _utilitiesModuleName;
    private readonly TypeScriptClientGeneratorSettings _clientGeneratorSettings;
    private readonly TypeScriptTypeResolver _resolver;
    private readonly TypeScriptExtensionCode _extensionCode;
    private readonly OpenApiDocument _openApiDocument;

    private Func<string, string> CaseConverter { get; }

    public UtilitiesScriptGenerator(TypeScriptClientGeneratorSettings clientGeneratorSettings,
        OpenApiDocument openApiDocument, Func<string, string> caseConverter)
    {
        _clientGeneratorSettings = clientGeneratorSettings;
        _resolver = new TypeScriptTypeResolver(clientGeneratorSettings.TypeScriptGeneratorSettings);
        _extensionCode = new TypeScriptExtensionCode(clientGeneratorSettings.TypeScriptGeneratorSettings.ExtensionCode,
            clientGeneratorSettings.TypeScriptGeneratorSettings.ExtendedClasses ?? new[]
            {
                clientGeneratorSettings.ConfigurationClass,
                clientGeneratorSettings.ClientBaseClass
            });
        _openApiDocument = openApiDocument;
        CaseConverter = caseConverter;
        _utilitiesModuleName = caseConverter.Invoke(Constant.UtilsName);
    }

    public async Task GenerateUtilitiesFilesAsync(string outputDirectory)
    {
        var utilities = GenerateUtilities();
        var path = Path.Combine(outputDirectory, _utilitiesModuleName + ".ts");
        await IoHelper.HandleFileAsync(path, utilities);
    }

    public string GenerateUtilities()
    {
        ////var tempClientCode = "Placeholder Code For SwaggerException!";
        var tempClientCode = new List<CodeArtifact>();
        tempClientCode.Add(new CodeArtifact("tsException", CodeArtifactType.Undefined,
            CodeArtifactLanguage.TypeScript, CodeArtifactCategory.Undefined,
            "Placeholder Code For SwaggerException!"));

        if (!string.IsNullOrEmpty(_clientGeneratorSettings.ClientBaseClass) && _clientGeneratorSettings.UseGetBaseUrlMethod)
        {
            tempClientCode.Add(new CodeArtifact("clientBaseClass", CodeArtifactType.Class,
                CodeArtifactLanguage.TypeScript, CodeArtifactCategory.Utility,
                $@"export class {_clientGeneratorSettings.ClientBaseClass} {{
                    public getBaseUrl(defaultUrl: string, fetchBaseUrl?:string) {{
                        return '{_openApiDocument.Servers?.FirstOrDefault()?.Url}' || defaultUrl || fetchBaseUrl || '';
                    }}
                }}"));
        }

        var model = new TypeScriptFileTemplateModel(tempClientCode, new List<CodeArtifact>(), _openApiDocument,
            _extensionCode, _clientGeneratorSettings, _resolver);
        var template =
            _clientGeneratorSettings.CodeGeneratorSettings.TemplateFactory.CreateTemplate("TypeScript", "File",
                model);
        var utilitiesCode = template.Render();
        utilitiesCode = utilitiesCode.Replace("function ", "export function ")
            .Replace("Placeholder Code For SwaggerException!", "");
        utilitiesCode = utilitiesCode.Replace("\n\n", "\n").Replace("\n\n", "\n").Replace("\n\n", "\n");
        return utilitiesCode;
    }
}