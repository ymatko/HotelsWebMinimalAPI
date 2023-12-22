namespace HotelsWebMinimalAPI.Auth
{
    public interface IUserRepository
    {
        UserDto GetUser(UserModel userModel);
    }
}
