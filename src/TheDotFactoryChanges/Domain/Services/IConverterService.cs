using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Domain.Entities;

namespace Domain.Services
{
    public interface IConverterService : IService
    {
        Task<string> ConvertFont(int fontId, string inputText);

        Task<IEnumerable<string>> GetFontNames();
        Task<IEnumerable<Convertion>> GetConvertions();
        Task<Convertion> GetConvertionById(int id);
        Task<int> AddConvertion(Convertion cvt);

        Task<IEnumerable<Domain.Entities.Font>> GetFonts();
        Task<Domain.Entities.Font> GetFontById(int id);
        Task UpdateFont(Domain.Entities.Font font);
        Task DeleteFont(int id);
    }
}
