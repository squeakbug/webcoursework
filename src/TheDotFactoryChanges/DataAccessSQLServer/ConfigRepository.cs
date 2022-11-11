using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using DataAccessInterface;

namespace DataAccessSQLServer
{
    public class ConfigRepository : IConfigRepository
    {
        IDbContext _ctx;
        private bool _disposedValue;

        public ConfigRepository(IDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException("context");
        }

        public async Task<IEnumerable<DataAccessInterface.Configuration>> GetConfigurations()
        {
            var result = new List<DataAccessInterface.Configuration>();
            IQueryable<DataAccessSQLServer.UserConfig> configs = _ctx.GetUserConfigSet();
            foreach (var cfg in configs)
                result.Add(ConfigConverter.MapToBusinessEntity(cfg));
            return result;
        }
        public async Task<DataAccessInterface.Configuration> GetConfigurationById(int id)
        {
            var cfg = await _ctx.UserConfig.FindAsync(id);
            return cfg == null ? null : ConfigConverter.MapToBusinessEntity(cfg);
        }
        public DataAccessInterface.Configuration GetFirstOrDefaultConfig()
        {
            var cfg = (from c in _ctx.UserConfig select c).FirstOrDefault();
            return cfg == null ? null : ConfigConverter.MapToBusinessEntity(cfg);
        }
        public async Task<int> Create(DataAccessInterface.Configuration cfg)
        {
            var dbCfg = ConfigConverter.MapFromBusinessEntity(cfg);
            _ctx.UserConfig.Add(dbCfg);
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch
            {
                throw new ApplicationException("save changes");
            }
            return dbCfg.Id;
        }
        public async Task Update(DataAccessInterface.Configuration cfg)
        {
            _ctx.UserConfig.Update(ConfigConverter.MapFromBusinessEntity(cfg));
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
            var cfg = _ctx.UserConfig.Find(id);
            if (cfg == null)
                throw new Exception("No configuration with such id");

            _ctx.UserConfig.Remove(cfg);
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
