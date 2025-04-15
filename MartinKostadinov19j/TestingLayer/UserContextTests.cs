using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using DataLayer;
namespace TestingLayer
{
    [TestFixture]
    public class UserContextTests
    {
        static UserContext userContext;
        static UserContextTests()
        {
            userContext = new UserContext(TestManager.dbContext);
        }

        [Test]
        public void CreateUser()
        {
            User newUser = new User("Martin", "Kostadinov", 18, "mausa", "123456789", "gmail.com");
            int usersBefore = TestManager.dbContext.Users.Count();

            userContext.Create(newUser);

            int usersAfter = TestManager.dbContext.Users.Count();
            User lastUser = TestManager.dbContext.Users.Last();
            Assert.That(usersBefore + 1 == usersAfter, "User is not created!");
        }

        [Test]
        public void ReadUser()
        {
            User newUser = new User("Martin", "Kostadinov", 18, "mausa", "123456789", "gmail.com");

            userContext.Create(newUser);

            User user = TestManager.dbContext.Users.Last();

            Assert.That(user.FirstName == "Martin", "Read() does not get User by id!");
        }

        [Test]
        public void ReadAllUser()
        {
            int usersBefore = TestManager.dbContext.Users.Count();

            int usersAfter = userContext.ReadAll().Count;

            Assert.That(usersBefore == usersAfter, "ReadAll() does not return all of the User!");
        }


        [Test]
        public void UpdateUser()
        {
            User newUser = new User("Martin", "Kostadinov", 18, "mausa", "123456789", "gmail.com");

            userContext.Create(newUser);

            User lastUser = userContext.ReadAll().Last();
            lastUser.FirstName = "Sotka";

            userContext.Update(lastUser, false);

            Assert.That(userContext.Read(lastUser.Id).FirstName == "Sotka", "Update() does not change the User's name!");
        }


        [Test]
        public void DeleteUser()
        {
            User newUser = new User("Martin", "Kostadinov", 18, "mausa", "123456789", "gmail.com");

            userContext.Create(newUser);

            User user = userContext.ReadAll().Last();
            int userId = newUser.Id;

            userContext.Delete(userId);

            ArgumentException ex = Assert.Throws<ArgumentException>(() => userContext.Read(userId));
            Assert.That(ex.Message, Is.EqualTo($"User with id = {userId} does not exist!"));
        }
    }
}
