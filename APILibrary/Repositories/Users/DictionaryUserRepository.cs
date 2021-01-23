using System.Collections.Generic;
using APILibrary.Models.Users;

namespace APILibrary.Repositories.Users
{
    public class DictionaryUserRepository : IUserRepository
    {
        private readonly Dictionary<int, User> _users = new Dictionary<int, User>();

        public DictionaryUserRepository()
        {
            var user = new User
            {
                Id = 1,
                Username = "Peter",
                Email = "foo@mail.com"
            };
            var user1 = new User
            {
                Id = 2,
                Username = "Tim",
                Email = "bar@mail.com"
            };
            _users.Add(1, user);
            _users.Add(2, user1);
        }

        public User GetUser(int id)
        {
            return _users[id];
        }

        public IEnumerable<User> GetUsers()
        {
            return _users.Values;
        }

        public User GetUserWithId(int id)
        {
            _users.TryGetValue(id, out User result);
            return result;
        }

        public void Add(User user)
        {
            _users.Add(user.Id, user);
        }
        public void Delete(User user)
        {
            _users.Remove(user.Id);
        }

    }
}