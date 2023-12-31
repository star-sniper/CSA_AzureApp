﻿using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
namespace CSA_AzureApp.Services
{
    public interface IImageAnalysisService
    {
        Task<ImageAnalysis> AnalyzeImageAsync(MemoryStream memoryStream, string imageForma);
    }

    public class ImageAnalysisService : IImageAnalysisService
    {
        private readonly IConfiguration _configuration;

        public ImageAnalysisService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ImageAnalysis> AnalyzeImageAsync(MemoryStream memoryStream, string imageFormat)
        {
            var analysisModel = new ImageAnalysis();
            string computerVisionApiKey;
            string computerVisionEndpoint;
            computerVisionApiKey = _configuration["Azure:CognitiveServices:ComputerVisionApiKey"];
            computerVisionEndpoint = _configuration["Azure:CognitiveServices:ComputerVisionEndpoint"];
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", computerVisionApiKey);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Construct the request content
                var requestContent = new ByteArrayContent(memoryStream.ToArray());
                requestContent.Headers.ContentType = new MediaTypeHeaderValue(imageFormat);

                // Send the POST request to the Computer Vision API
                var response = await httpClient.PostAsync($"{computerVisionEndpoint}/vision/v3.0/analyze?visualFeatures=Tags,Description", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    // Read and process the JSON response
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    analysisModel = JsonSerializer.Deserialize<ImageAnalysis>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true, // Use this option to handle case-insensitive property names
                    });
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }

            return analysisModel;
        }
    }
}