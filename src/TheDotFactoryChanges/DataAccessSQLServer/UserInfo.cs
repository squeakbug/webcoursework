namespace Infrastructure.DataAccessSQLServer
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Usr_login { get; set; }
        public string Usr_passwd { get; set; }
        public string Usr_name { get; set; }
        public bool Usr_Loggined { get; set; }
    }
}
