using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataAccessInterface;

namespace DataAccessSQLServer
{
    public class ConvertionRepository : IConvertionRepository
    {
        IDbContext _ctx;

        public ConvertionRepository(IDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException("context");
        }

        public IEnumerable<DataAccessInterface.Convertion> GetConvertions()
        {
            var result = new List<DataAccessInterface.Convertion>();
            IQueryable<DataAccessSQLServer.Convertion> convertions = _ctx.GetConvertionSet();
            foreach (var cvt in convertions)
                result.Add(ConvertionConverter.MapToBusinessEntity(cvt));
            return result;
        }
        public DataAccessInterface.Convertion GetConvertionById(int id)
        {
            var cvt = _ctx.Convertions.Find(id);
            return cvt == null ? null : ConvertionConverter.MapToBusinessEntity(cvt);
        }
        public int Create(DataAccessInterface.Convertion cvt)
        {
            var dbCvt = ConvertionConverter.MapFromBusinessEntity(cvt);
            _ctx.Convertions.Add(dbCvt);
            _ctx.SaveChanges();
            return dbCvt.Id;
        }
        public void Update(DataAccessInterface.Convertion cvt)
        {
            _ctx.Convertions.Update(ConvertionConverter.MapFromBusinessEntity(cvt));
            _ctx.SaveChanges();
        }
        public void Delete(int id)
        {
            var cvt = _ctx.Convertions.Find(id);
            if (cvt == null)
                throw new Exception("No convertion with such id");

            _ctx.Convertions.Remove(cvt);
            _ctx.SaveChanges();
        }
    }
}
