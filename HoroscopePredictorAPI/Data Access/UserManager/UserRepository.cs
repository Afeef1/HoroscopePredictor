using HoroscopePredictorAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HoroscopePredictorAPI.Data_Access.UserRepository
{
    public class UserRepository : IUserRepository

    {
        private readonly ApiDbContext _apiDbContext;

        public UserRepository(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }

        public async Task AddUser(RegisterUser user)
        {
            await _apiDbContext.Users.AddAsync(user);
            await _apiDbContext.SaveChangesAsync();
        }

        public RegisterUser? GetCurrentUser(LoginUser user, string hashedInputPassword)
        {
            return _apiDbContext.Users
                            .FirstOrDefault(record => EF.Functions.Collate(record.Email, "Latin1_General_CI_AI") == user.Email
                          && record.Password.Equals(hashedInputPassword));
          

        }
        public bool DoesUserExist(string email)
        {
            return _apiDbContext.Users.Any(user => EF.Functions.Collate(user.Email,"Latin1_General_CI_AI")==email);
        }
    }
}
