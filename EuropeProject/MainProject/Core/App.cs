using MainProject.Entities;
using MainProject.Services.Gifter;
using ORM;
using System.Reflection;

namespace MainProject.Core
{
    public class App
    {
        private DbContext _dbContext;
        private IGifter _gifter;
        public DbContext DB
        {
            get
            {
                return _dbContext;
            }
        }

        public App(string TablePath)
        {
            _gifter = new GetRandomGift(this);
            _dbContext = new DbContext(TablePath,Assembly.GetExecutingAssembly());
        }

        public void Register(string name, string password)
        {
            if (DB.Exists<User>(u => u.Name == name))
            {
                Console.WriteLine("This user already exists");
                return;
            }
            DB.Add<User>(new User() { Name = name, Password = password });
        }

        public void Login(string name, string password)
        {
            if (DB.Exists<User>(u => u.Name == name && u.Password == password))
            {
                Console.WriteLine("Logged in");
                string GiftforUser = _gifter.GenerateGift(DB.GetTable<User>().Single(u => u.Name == name && u.Password == password));
                if(GiftforUser != null) Console.WriteLine($"You won {GiftforUser}");

            }
            else
            {
                Console.WriteLine("incorrect password or username");
            }
        }
    }
}
