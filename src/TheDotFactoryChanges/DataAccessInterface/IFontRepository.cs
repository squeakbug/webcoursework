using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterface
{
    public interface IFontRepository : IDisposable
    {
        Task<IEnumerable<Font>> GetFonts();
        Task<Font> GetFontById(int id);
        Task<int> Create(Font cfg);
        Task Update(Font cfg);
        Task Delete(int id);
    }
}
