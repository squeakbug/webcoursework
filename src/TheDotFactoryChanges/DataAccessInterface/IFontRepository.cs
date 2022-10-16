using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterface
{
    public interface IFontRepository
    {
        IEnumerable<Font> GetFonts();
        Font GetFontById(int id);
        int Create(Font cfg);
        void Update(Font cfg);
        void Delete(int id);
    }
}
