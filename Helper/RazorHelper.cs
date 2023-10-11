using System;
using System.IO;
using System.Linq;
using Microsoft.JSInterop;

namespace CSA_AzureApp.Helper
{
    public static class RazorPageHelper
    {
        public static string DetermineImageFormat(string fileName)
        {
            string[] supportedFormats = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".ico", ".tiff", ".mpo" };

            string fileExtension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(fileExtension))
            {
                return "";
            }

            if (supportedFormats.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
            {
                return GetImageMimeType(fileExtension);
            }

            return "";
        }

        public static string GetImageMimeType(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".bmp":
                    return "image/bmp";
                case ".webp":
                    return "image/webp";
                case ".ico":
                    return "image/x-icon";
                case ".tiff":
                    return "image/tiff";
                case ".mpo":
                    return "image/mpo";
                default:
                    return "";
            }
        }

        public static async void ShowError(IJSRuntime jsRuntime, string errorMessage)
        {
            await jsRuntime.InvokeVoidAsync("alert", errorMessage);
        }
    }
}