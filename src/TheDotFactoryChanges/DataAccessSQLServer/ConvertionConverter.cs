using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DataAccessSQLServer
{
    public class ConvertionConverter
    {
        public static Domain.Entities.Convertion MapToBusinessEntity(Convertion cvt)
        {
            return new Domain.Entities.Convertion
            {
                Id = cvt.Id,
                Body = cvt.Body,
                Name = cvt.Name,
            };
        }

        public static Convertion MapFromBusinessEntity(Domain.Entities.Convertion cvt)
        {
            return new Convertion
            {
                Id = cvt.Id,
                Body = cvt.Body,
                Name = cvt.Name,
            };
        }
    }
}
