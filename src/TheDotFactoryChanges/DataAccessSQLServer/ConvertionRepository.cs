using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repositories;

namespace Infrastructure.DataAccessSQLServer
{
    public class ConvertionRepository : IConvertionRepository
    {
        IDbContext _ctx;
        private bool _disposedValue;

        public ConvertionRepository(IDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException("context");
        }

        public async Task<IEnumerable<Domain.Entities.Convertion>> GetConvertions()
        {
            var result = new List<Domain.Entities.Convertion>();
            IQueryable<DataAccessSQLServer.Convertion> convertions = _ctx.GetConvertionSet();
            foreach (var cvt in convertions)
                result.Add(ConvertionConverter.MapToBusinessEntity(cvt));
            return result;
        }
        public async Task<Domain.Entities.Convertion> GetConvertionById(int id)
        {
            var cvt = await _ctx.Convertions.FindAsync(id);
            return cvt == null ? null : ConvertionConverter.MapToBusinessEntity(cvt);
        }
        public async Task<int> Create(Domain.Entities.Convertion cvt)
        {
            var dbCvt = ConvertionConverter.MapFromBusinessEntity(cvt);
            _ctx.Convertions.Add(dbCvt);
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch
            {
                throw new ApplicationException("save changes");
            }
            return dbCvt.Id;
        }
        public async Task Update(Domain.Entities.Convertion cvt)
        {
            _ctx.Convertions.Update(ConvertionConverter.MapFromBusinessEntity(cvt));
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
            var cvt = _ctx.Convertions.Find(id);
            if (cvt == null)
                throw new Exception("No convertion with such id");

            _ctx.Convertions.Remove(cvt);
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
