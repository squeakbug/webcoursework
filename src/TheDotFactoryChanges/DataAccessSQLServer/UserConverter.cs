namespace Infrastructure.DataAccessSQLServer
{
    public static class UserConverter
    {
        public static Domain.Entities.UserInfo MapToBusinessEntity(UserInfo info)
        {
            return new Domain.Entities.UserInfo
            {
                Id = info.Id,
                Login = info.Usr_login,
                Password = info.Usr_passwd,
                Name = info.Usr_name,
                Loggined = info.Usr_Loggined,
            };
        }

        public static UserInfo MapFromBusinessEntity(Domain.Entities.UserInfo info)
        {
            return new UserInfo
            {
                Id = info.Id,
                Usr_login = info.Login,
                Usr_passwd = info.Password,
                Usr_name = info.Name,
                Usr_Loggined = info.Loggined,
            };
        }
    }
}
