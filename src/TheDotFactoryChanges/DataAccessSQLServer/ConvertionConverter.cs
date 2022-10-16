using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessSQLServer
{
    public class ConvertionConverter
    {
        public static DataAccessInterface.Convertion MapToBusinessEntity(Convertion cvt)
        {
            return new DataAccessInterface.Convertion
            {
                Id = cvt.Id,
                Body = cvt.Body,
                Head = cvt.Head,
                Name = cvt.Name,
            };
        }

        public static Convertion MapFromBusinessEntity(DataAccessInterface.Convertion cvt)
        {
            return new Convertion
            {
                Id = cvt.Id,
                Body = cvt.Body,
                Head = cvt.Head,
                Name = cvt.Name,
            };
        }
    }
}
