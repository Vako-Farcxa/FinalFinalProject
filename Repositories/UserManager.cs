using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using static Repositories.QuizAppData;

namespace Repositories
{
    public class UserManager
    {
        private List<User> _users;

        public UserManager()
        {
            FilePaths.EnsureFilesExist();
            _users = JsonDataHandler.ReadData<User>(FilePaths.UsersFile);
        }

        public void AddUser(User user)
        {
            _users.Add(user);
            SaveUsers();
        }

        public User GetUser(string username) => _users.FirstOrDefault(u => u.Username == username);

        public bool ValidateUser(string username, string password)
        {
            var user = GetUser(username);
            return user != null && user.Password == password;
        }

        private void SaveUsers()
        {
            JsonDataHandler.WriteData(FilePaths.UsersFile, _users);
        }
    }
}
