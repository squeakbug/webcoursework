using WebControllers.Models;

namespace WebControllers
{
    public class UserConverter
    {
        public static Domain.NewUser MapToBusinessEntity(UserPayload user)
        {
            return new Domain.NewUser
            {
                Birthday = user.Birthday,
                Name = user.Name,
                Coins = user.Coins,
            };
        }

        public static UserPresenter MapFromBusinessEntity(Domain.User user)
        {
            return new UserPresenter
            {
                Id = user.Id,
                Birthday = user.Birthday,
                Name = user.Name,
                Coins = user.Coins,
            };
        }

        public static Domain.User MapToBusinessEntity(UserPresenter user)
        {
            return new Domain.User
            {
                Id = user.Id,
                Birthday = user.Birthday,
                Name = user.Name,
                Coins = user.Coins,
            };
        }
    }
}
