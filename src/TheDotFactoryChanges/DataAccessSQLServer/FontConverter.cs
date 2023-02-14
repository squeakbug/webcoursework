using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccessSQLServer
{
    public class FontConverter
    {
        public static Domain.Entities.Font MapToBusinessEntity(Font font)
        {
            return new Domain.Entities.Font
            {
                Id = font.Id,
                Name = font.Name,
            };
        }

        public static Font MapFromBusinessEntity(Domain.Entities.Font font)
        {
            return new Font
            {
                Id = font.Id,
                Name = font.Name,
            };
        }
    }
}
