﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NSwag.Commands;
using NSwag.Generators;
using NSwag.Helpers;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace NSwag.Tests
{
    public class TypeScriptGenerateTests
    {
        private readonly OpenApiDocumentHelper _swaggerDocumentHelper;
        private readonly ClientsScriptGenerator _selfTypeScriptGenerator;
        private readonly UtilitiesScriptGenerator _utilsScriptGenerator;
        private readonly ModelsScriptGenerator _modelsScriptGenerator;
        private readonly ITestOutputHelper _outputHelper;
        private readonly OpenApiDocument _openApiDocument;

        public TypeScriptGenerateTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _swaggerDocumentHelper = new OpenApiDocumentHelper();
            var nSwagDocument = LoadSettings().Result;
            _openApiDocument = LoadOpenApi().Result;
            var settings = nSwagDocument.CodeGenerators
                .OpenApiToTypeScriptClientCommand.Settings;
            _selfTypeScriptGenerator = new ClientsScriptGenerator(settings, _openApiDocument, x => x);
            _utilsScriptGenerator = new UtilitiesScriptGenerator(settings, _openApiDocument, x => x);
            _modelsScriptGenerator = new ModelsScriptGenerator(settings, _openApiDocument, x => x);
        }

        protected async Task<NSwagDocument> LoadSettings()
        {
            var configFilePath = Path.Combine(AppContext.BaseDirectory, "./Config/nswag.nswag");
            var nSwagDocument = await NsWagDocumentHelper.LoadDocumentFromFileAsync(configFilePath);

            nSwagDocument.ShouldNotBeNull();
            nSwagDocument.CodeGenerators.ShouldNotBeNull();
            nSwagDocument.CodeGenerators.OpenApiToTypeScriptClientCommand.ShouldNotBeNull();
            nSwagDocument.CodeGenerators.OpenApiToTypeScriptClientCommand.Settings.ShouldNotBeNull();
            return nSwagDocument;
        }

        protected async Task<OpenApiDocument> LoadOpenApi()
        {
            var swaggerFilePath = Path.Combine(AppContext.BaseDirectory, "./Config/swagger.json");
            var openApiDocument = await _swaggerDocumentHelper.FromPathAsync(swaggerFilePath);
            return openApiDocument;
        }

        #region ClientClass

        [Fact]
        public void GenerateClientClass_Test()
        {
            _selfTypeScriptGenerator.SetDtoPath("Dto");
            var classCode = _selfTypeScriptGenerator.GenerateClientClass("Account");
            classCode.ShouldNotBeNullOrWhiteSpace();
            classCode.ShouldContain("IAccountServiceProxy");
            _outputHelper.WriteLine(classCode);
        }
        [Fact]
        public void GenerateClientClasses_Test()
        {
            var clientClasses = _selfTypeScriptGenerator.GenerateClientClasses();
            clientClasses.ToList().Count.ShouldBeGreaterThan(0);
        }

        #endregion

        #region UtilitiesModule

        [Fact]
        public void GenerateUtilities_Test()
        {
            var utilitiesCode = _utilsScriptGenerator.GenerateUtilities();
            _outputHelper.WriteLine(utilitiesCode);
            utilitiesCode.ShouldNotBeNullOrWhiteSpace();
        }

        #endregion

        #region DtoClass

        [Fact]
        public void GenerateDtoClasses_Test()
        {
            var result = _modelsScriptGenerator.GenerateDtoClasses();
            result.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public void GenerateDtoClasses_ShouldBeImportParentType_Test()
        {
            var result = _modelsScriptGenerator.GenerateDtoClasses();
            var tags = result.Where(c => c.Key.StartsWith("Tag"));
            var tag = tags.FirstOrDefault(c => c.Key.Equals("TagDto"));
            var contains = tag.Value
                .Contains("import { ITagBasicDto,TagBasicDto } from './TagBasicDto';");
            contains.ShouldBe(true);
        }

        [Fact]
        public void GenerateDtoClasses_ShouldBeImportParentType_With_Signal_Test()
        {
            var result = _modelsScriptGenerator.GenerateDtoClasses();
            var tags = result.Where(c => c.Key.StartsWith("UserLogin"));
            var tag = tags.FirstOrDefault(c => c.Key.Equals("UserLoginAttemptDtoListResultDto"));
            tag.Value.Split("\r")[1].Replace("\n", "").ShouldBe("/* tslint:disable */");
        }

        [Fact]
        public void GenerateDtoClass_Test()
        {
            var schema = _openApiDocument.Definitions["ActivityDto"];
            var code = _modelsScriptGenerator.GenerateDtoClass(schema, "ActivityDto", out _);
            _outputHelper.WriteLine(code);
            code.ShouldNotBeNullOrWhiteSpace();
        }

        #endregion

        [Fact]
        public void GetAllOperations_Test()
        {
            var operations = _selfTypeScriptGenerator.GetAllOperationModels();
            operations.Count().ShouldBeGreaterThan(0);
        }
    }
}