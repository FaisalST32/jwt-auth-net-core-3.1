using Auth.Domain.User;
using System.Threading.Tasks;

namespace Auth.Application.Repositories
{
    public interface IUsersRepository
    {
        Task<User> GetUserById(int userId);
        Task<User> GetUserByCredentials(string username, string passwordHash);
        Task<User> GetUserByUsername(string username);
        Task<UserProfile> GetUserProfile(int userId);
        Task<UserProfile> UpdateUser(int userId, User userToUpdate);
        Task<UserProfile> AddUser(User user);
        Task<User> GetUserByRefreshToken(string refreshToken);
    }
}
