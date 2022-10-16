using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessSQLServer
{
    public class FontConverter
    {
        public static DataAccessInterface.Font MapToBusinessEntity(Font font)
        {
            return new DataAccessInterface.Font
            {
                Id = font.Id,
                Name = font.Name,
                Size = font.Size,
            };
        }

        public static Font MapFromBusinessEntity(DataAccessInterface.Font font)
        {
            return new Font
            {
                Id = font.Id,
                Name = font.Name,
                Size = font.Size,
            };
        }
    }
}
