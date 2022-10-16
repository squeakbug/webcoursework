using System.Collections.Generic;

namespace DataAccessInterface
{
    public interface IConfigRepository
    {
        IEnumerable<Configuration> GetConfigurations();
        Configuration GetConfigurationById(int id);
        Configuration GetFirstOrDefaultConfig();
        int Create(Configuration cfg);
        void Update(Configuration cfg);
        void Delete(int id);
    }
}
