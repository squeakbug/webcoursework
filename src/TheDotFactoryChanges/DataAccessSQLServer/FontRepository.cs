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

        public async Task<IEnumerable<DataAccessInterface.Font>> GetFonts()
        {
            var result = new List<DataAccessInterface.Font>();
            IQueryable<DataAccessSQLServer.Font> fonts = _ctx.GetFontSet();
            foreach (var font in fonts)
                result.Add(FontConverter.MapToBusinessEntity(font));
            return result;
        }
        public async Task<DataAccessInterface.Font> GetFontById(int id)
        {
            var font = await _ctx.Fonts.FindAsync(id);
            return font == null ? null : FontConverter.MapToBusinessEntity(font);
        }
        public async Task<int> Create(DataAccessInterface.Font font)
        {
            var dbFont = FontConverter.MapFromBusinessEntity(font);
            _ctx.Fonts.Add(dbFont);
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch
            {
                throw new ApplicationException("save changes");
            }
            return dbFont.Id;
        }
        public async Task Update(DataAccessInterface.Font font)
        {
            _ctx.Fonts.Update(FontConverter.MapFromBusinessEntity(font));
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch
            {
                throw new ApplicationException("save changes");
            }
        }
        public async Task Delete(int id)
        {
            var font = _ctx.Fonts.Find(id);
            if (font == null)
                throw new Exception("No font with such id");

            _ctx.Fonts.Remove(font);
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch
            {
                throw new ApplicationException("save changes");
            }
        }
    }
}
