using System;

namespace Domain.Repositories
{
    public interface IRepositoryFactory
    {
        IUserRepository CreateUserRepository();
        IConvertionRepository CreateConvertionRepository();
        IFontRepository CreateFontRepository();
    }
}
