namespace WebControllers.Models
{
    public class UserPresenter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public int Coins { get; set; }
    }
}
