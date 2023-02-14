using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repositories;

namespace Infrastructure.DataAccessSQLServer
{
    public class FontRepository : IFontRepository
    {
        IDbContext _ctx;
        private bool _disposedValue;

        public FontRepository(IDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException("context");
        }

        public async Task<IEnumerable<Domain.Entities.Font>> GetFonts()
        {
            var result = new List<Domain.Entities.Font>();
            IQueryable<DataAccessSQLServer.Font> fonts = _ctx.GetFontSet();
            foreach (var font in fonts)
                result.Add(FontConverter.MapToBusinessEntity(font));
            return result;
        }
        public async Task<Domain.Entities.Font> GetFontById(int id)
        {
            var font = await _ctx.Fonts.FindAsync(id);
            return font == null ? null : FontConverter.MapToBusinessEntity(font);
        }
        public Domain.Entities.Font GetFirstOrDefaultFont()
        {
            var font = (from f in _ctx.Fonts select f).FirstOrDefault();
            return font == null ? null : FontConverter.MapToBusinessEntity(font);
        }
        public async Task<int> Create(Domain.Entities.Font font)
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
        public async Task Update(Domain.Entities.Font font)
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _ctx.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
