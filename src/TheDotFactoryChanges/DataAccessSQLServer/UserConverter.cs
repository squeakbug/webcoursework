namespace DataAccessSQLServer
{
    public static class UserConverter
    {
        public static DataAccessInterface.UserInfo MapToBusinessEntity(UserInfo info)
        {
            return new DataAccessInterface.UserInfo
            {
                Id = info.Id,
                Login = info.Usr_login,
                Password = info.Usr_passwd,
                Name = info.Usr_name,
                Loggined = info.Usr_Loggined,
            };
        }

        public static UserInfo MapFromBusinessEntity(DataAccessInterface.UserInfo info)
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
