namespace Domain.Entities
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool Loggined { get; set; }
    }
}
