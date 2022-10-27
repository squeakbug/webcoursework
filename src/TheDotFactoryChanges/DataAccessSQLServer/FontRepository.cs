using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccessInterface;

namespace DataAccessSQLServer
{
    public class FontRepository : IFontRepository
    {
        IDbContext _ctx;

        public FontRepository(IDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException("context");
        }

        public IEnumerable<DataAccessInterface.Font> GetFonts()
        {
            var result = new List<DataAccessInterface.Font>();
            IQueryable<DataAccessSQLServer.Font> fonts = _ctx.GetFontSet();
            foreach (var font in fonts)
                result.Add(FontConverter.MapToBusinessEntity(font));
            return result;
        }
        public DataAccessInterface.Font GetFontById(int id)
        {
            var font = _ctx.Fonts.Find(id);
            return font == null ? null : FontConverter.MapToBusinessEntity(font);
        }
        public int Create(DataAccessInterface.Font font)
        {
            var dbFont = FontConverter.MapFromBusinessEntity(font);
            _ctx.Fonts.Add(dbFont);
            _ctx.SaveChanges();
            return dbFont.Id;
        }
        public void Update(DataAccessInterface.Font font)
        {
            _ctx.Fonts.Update(FontConverter.MapFromBusinessEntity(font));
            _ctx.SaveChanges();
        }
        public void Delete(int id)
        {
            var font = _ctx.Fonts.Find(id);
            if (font == null)
                throw new Exception("No font with such id");

            _ctx.Fonts.Remove(font);
            _ctx.SaveChanges();
        }
    }
}
