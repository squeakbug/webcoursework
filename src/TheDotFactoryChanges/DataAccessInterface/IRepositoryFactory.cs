namespace DataAccessInterface
{
    public interface IRepositoryFactory
    {
        IUserRepository CreateUserRepository();
        IConfigRepository CreateConfigRepository();
        IConvertionRepository CreateConvertionRepository();
        IFontRepository CreateFontRepository();
    }
}
