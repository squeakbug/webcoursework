using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;

namespace Domain.Repositories
{
    public interface IFontRepository : IDisposable
    {
        Task<IEnumerable<Font>> GetFonts();
        Task<Font> GetFontById(int id);
        Font GetFirstOrDefaultFont();
        Task<int> Create(Font cfg);
        Task Update(Font cfg);
        Task Delete(int id);
    }
}
