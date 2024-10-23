using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSwag.Helpers;

public class OpenApiDocumentHelper
{
    public static async Task<OpenApiDocument> FromUrlAsync(string url)
    {
        using var httpClient = new HttpClient();
        var openApiDocument = await OpenApiDocument.FromJsonAsync(await httpClient.GetStringAsync(url));
        if (string.IsNullOrWhiteSpace(openApiDocument.BaseUrl))
        {
            return openApiDocument;
        }
        if (openApiDocument.BaseUrl.StartsWith("http"))
            return openApiDocument;
        var str = openApiDocument.BaseUrl;
        if (str.EndsWith("/"))
            str = str.Remove(str.Length - 1);
        var uri = new Uri(url);
        openApiDocument.Servers.Clear();
        openApiDocument.Servers.Add(new OpenApiServer()
        {
            Url = uri.Scheme + "://" + str
        });
        return openApiDocument;
    }

    public async Task<OpenApiDocument> FromPathAsync(string swaggerFilePath)
    {
        return await OpenApiDocument.FromFileAsync(swaggerFilePath);
    }
}