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
        public string SaveFile(QuestionVM question);
        public void DeleteOld(string filePath);

    }
}
