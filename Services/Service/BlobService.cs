using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Models.ViewModels;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class BlobService : IFileService
    {
        private readonly string? _connectionString;
        private readonly string? _containerName;

        public BlobService(IConfiguration configuration)
        {
            foreach (var kv in configuration.AsEnumerable())
            {
                Console.WriteLine($"{kv.Key} = {kv.Value}");
            }

            _connectionString = configuration["BlobStorage:ConnectionString"];
            _containerName = configuration["BlobStorage:ContainerName"];
        }
        public async Task DeleteOld(string blobName)
        {
            var containerClient = new BlobContainerClient(_connectionString, _containerName);

            try
            {
                // Pobierz referencję do blobu
                var blobClient = containerClient.GetBlobClient(blobName);

                // Usuń blob, jeśli istnieje
                await blobClient.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                // logowanie błędu
                Console.WriteLine($"Błąd przy usuwaniu pliku: {ex.Message}");
            }
        }

        public async Task<string> SaveFile(QuestionVM question)
        {
            var file=question.UploadedFile;

            if (file == null || file.Length == 0)
                return string.Empty;

            var blobClient = new BlobContainerClient(_connectionString, _containerName);
            await blobClient.CreateIfNotExistsAsync();

            string fileName = $"{DateTime.Now:yyyy_MM_dd_HH_mm_ss}_{file.FileName}";
            var blob = blobClient.GetBlobClient(fileName);

            using (var stream = file.OpenReadStream())
            {
                await blob.UploadAsync(stream, overwrite: true);
            }

            // Zwracamy publiczny URL do pliku
            return blob.Uri.ToString();
        }
    }
}
