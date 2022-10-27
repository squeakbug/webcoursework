using System;
using System.Linq;
using System.Collections.Generic;

using DataAccessInterface;

namespace DataAccessSQLServer
{
    public class ConfigRepository : IConfigRepository
    {
        IDbContext _ctx;

        public ConfigRepository(IDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException("context");
        }

        public IEnumerable<DataAccessInterface.Configuration> GetConfigurations()
        {
            var result = new List<DataAccessInterface.Configuration>();
            IQueryable<DataAccessSQLServer.UserConfig> configs = _ctx.GetUserConfigSet();
            foreach (var cfg in configs)
                result.Add(ConfigConverter.MapToBusinessEntity(cfg));
            return result;
        }
        public DataAccessInterface.Configuration GetConfigurationById(int id)
        {
            var cfg = _ctx.UserConfig.Find(id);
            return cfg == null ? null : ConfigConverter.MapToBusinessEntity(cfg);
        }
        public DataAccessInterface.Configuration GetFirstOrDefaultConfig()
        {
            var cfg = (from c in _ctx.UserConfig select c).FirstOrDefault();
            return cfg == null ? null : ConfigConverter.MapToBusinessEntity(cfg);
        }
        public int Create(DataAccessInterface.Configuration cfg)
        {
            var dbCfg = ConfigConverter.MapFromBusinessEntity(cfg);
            _ctx.UserConfig.Add(dbCfg);
            _ctx.SaveChanges();
            return dbCfg.Id;
        }
        public void Update(DataAccessInterface.Configuration cfg)
        {
            _ctx.UserConfig.Update(ConfigConverter.MapFromBusinessEntity(cfg));
            _ctx.SaveChanges();
        }
        public void Delete(int id)
        {
            var cfg = _ctx.UserConfig.Find(id);
            if (cfg == null)
                throw new Exception("No configuration with such id");

            _ctx.UserConfig.Remove(cfg);
            _ctx.SaveChanges();
        }
    }
}
