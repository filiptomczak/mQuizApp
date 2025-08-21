using Microsoft.AspNetCore.Hosting;
using Models.ViewModels;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task DeleteOld(string filePath)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var oldImgPath = Path.Combine(
                            wwwRootPath, filePath.TrimStart('/'));

            if (File.Exists(oldImgPath))
            {
                File.Delete(oldImgPath);
            }
        }

        public async Task<string> SaveFile(QuestionVM question)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (question.UploadedFile != null)
            {
                string fileName = DateTime.Now.Year + "_" +
                    DateTime.Now.Month + "_" +
                    DateTime.Now.Day + "_" +
                    DateTime.Now.Hour + "_" +
                    DateTime.Now.Minute + "_" +
                    DateTime.Now.Second + "_" +
                    question.UploadedFile.FileName;

                string questionPath = Path.Combine(wwwRootPath, @"images\question");

                if (!Directory.Exists(questionPath))
                {
                    Directory.CreateDirectory(questionPath);
                }


                try
                {
                    using (var fileStream = new FileStream(Path.Combine(questionPath, fileName), FileMode.Create))
                    {
                        await question.UploadedFile.CopyToAsync(fileStream);
                    }
                    return $"/images/question/{fileName}";
                }
                catch (Exception ex)
                {
                    // logowanie błędu np. _logger.LogError(ex, "Błąd przy zapisie pliku");
                    return string.Empty;
                }
            }
            return string.Empty;
        }
    }
}
