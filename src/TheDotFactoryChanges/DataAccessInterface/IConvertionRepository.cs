using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessInterface
{
    public interface IConvertionRepository
    {
        IEnumerable<Convertion> GetConvertions();
        Convertion GetConvertionById(int id);
        int Create(Convertion cvt);
        void Update(Convertion cvt);
        void Delete(int id);
    }
}
