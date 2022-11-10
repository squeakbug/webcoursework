using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessInterface
{
    public interface IConfigRepository
    {
        Task<IEnumerable<Configuration>> GetConfigurations();
        Task<Configuration> GetConfigurationById(int id);
        Configuration GetFirstOrDefaultConfig();
        Task<int> Create(Configuration cfg);
        Task Update(Configuration cfg);
        Task Delete(int id);
    }
}
