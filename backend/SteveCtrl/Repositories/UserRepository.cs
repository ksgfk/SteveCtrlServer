using SteveCtrl.Models;

namespace SteveCtrl.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<UserModel> Users { get; }

        UserModel? GetUserById(int uid);

        UserModel? GetUserByUsernameAndPassword(string username, string password);
    }

    public class SimpleUserRepository : IUserRepository
    {
        private static readonly List<UserModel> _users =
        [
            new UserModel(0,"ksgfk","12345",UserRole.Administrator),
            new UserModel(1,"shit","qwert",UserRole.Default),
        ];

        public IEnumerable<UserModel> Users => _users;

        public UserModel? GetUserById(int uid)
        {
            return _users.Find(i => i.Id == uid);
        }

        public UserModel? GetUserByUsernameAndPassword(string username, string password)
        {
            return _users.Find(i => i.Name == username && i.Password == password);
        }
    }
}
