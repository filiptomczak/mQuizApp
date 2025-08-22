using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IFileService
    {
        public Task<string> SaveFile(QuestionVM question);
        public Task DeleteOld(string filePath);
    }
}
