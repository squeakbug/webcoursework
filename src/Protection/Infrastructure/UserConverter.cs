using Domain;

namespace Infrastructure
{
    public static class UserConverter
    {
        public static Domain.User MapToBusinessEntity(User user)
        {
            return new Domain.User
            {
                Id = user.Id,
                Birthday = user.Birthday,
                Name = user.Name,
                Coins = user.Coins,
            };
        }

        public static User MapFromBusinessEntity(Domain.User user)
        {
            return new User
            {
                Id = user.Id,
                Birthday = user.Birthday,
                Name = user.Name,
                Coins = user.Coins,
            };
        }

        public static User MapFromBusinessEntity(Domain.NewUser user)
        {
            return new User
            {
                Birthday = user.Birthday,
                Name = user.Name,
                Coins = user.Coins,
            };
        }
    }
}
