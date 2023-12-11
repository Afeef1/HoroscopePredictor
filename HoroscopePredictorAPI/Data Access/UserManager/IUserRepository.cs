using HoroscopePredictorAPI.Models;

namespace HoroscopePredictorAPI.Data_Access.UserRepository
{
    public interface IUserRepository
    {
        public Task AddUser(RegisterUser user);

        public RegisterUser? GetCurrentUser(LoginUser user,string hashedInput);

        public bool DoesUserExist(string email);
    }
}
