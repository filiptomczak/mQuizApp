using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IQuizExportService
    {
        string ExportQuizAsText(Quiz quiz);
    }
}
