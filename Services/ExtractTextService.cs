using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CSA_AzureApp.Models;
namespace CSA_AzureApp.Services
{
    public interface IExtractTextService
    {
        Task<string> ExtractTextAsync(MemoryStream memoryStream, string imageFormat);
    }

    public class ExtractTextService : IExtractTextService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ExtractTextService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _configuration["Azure:CognitiveServices:ComputerVisionApiKey"]);
        }

        public async Task<string> ExtractTextAsync(MemoryStream memoryStream, string imageFormat)
        {
            string extractedText = string.Empty;
            string computerVisionEndpoint = _configuration["Azure:CognitiveServices:ComputerVisionEndpoint"];
            var uri = $"{computerVisionEndpoint}/vision/v3.1/ocr";

            byte[] byteData = memoryStream.ToArray();

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                HttpResponseMessage response = await _httpClient.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var textAnalysis = JsonSerializer.Deserialize<TextAnalysis>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    extractedText = ProcessTextAnalysis(textAnalysis);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }

            return extractedText;
        }

        private string ProcessTextAnalysis(TextAnalysis textAnalysis)
        {
            var extractedText = new StringBuilder();

            foreach (Region region in textAnalysis.Regions)
            {
                foreach (Line line in region.Lines)
                {
                    foreach (Word word in line.Words)
                    {
                        extractedText.Append(" ").Append(word.Text);
                    }
                }
            }

            return extractedText.ToString().Trim();
        }
    }
}