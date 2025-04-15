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
    public class InterestContextTests
    {
        static InterestContext interestContext;
        static InterestContextTests()
        {
            interestContext = new InterestContext(TestManager.dbContext);
        }

        [Test]
        public void CreateInterest()
        {
            Field field = new Field("Data Science");
            Interest interest = new Interest("AI");
            interest.Field = field;
            int interestsBefore = TestManager.dbContext.Interests.Count();

            interestContext.Create(interest);

            int interestsAfter = TestManager.dbContext.Interests.Count();
            Interest lastInterest = TestManager.dbContext.Interests.Last();
            Assert.That(interestsBefore + 1 == interestsAfter, "Interest is not created!");
        }

        [Test]
        public void ReadInterest()
        {
            Field field = new Field("Software Engeneering");
            Interest newInterest = new Interest("Desktop Applications");
            newInterest.Field = field;

            interestContext.Create(newInterest);

            Interest interest = TestManager.dbContext.Interests.Last();

            Assert.That(interest.Name == "Desktop Applications", "Read() does not get Interest by id!");
        }

        [Test]
        public void ReadAllInterest()
        {
            int interestsBefore = TestManager.dbContext.Interests.Count();

            int interestsAfter = interestContext.ReadAll().Count;

            Assert.That(interestsBefore == interestsAfter, "ReadAll() does not return all of the Interest!");
        }


        [Test]
        public void UpdateInterest()
        {
            Field field = new Field("Data Science");
            Interest interest = new Interest("AI");
            interest.Field = field;
            interestContext.Create(interest);

            Interest lastInterest = interestContext.ReadAll().Last();
            lastInterest.Name = "Updated Interest";

            interestContext.Update(lastInterest, false);

            Assert.That(interestContext.Read(lastInterest.Id).Name == "Updated Interest", "Update() does not change the Interest's name!");
        }


        [Test]
        public void DeleteInterest()
        {
            Field field = new Field("Data Science");
            Interest newInterest = new Interest("AI");
            newInterest.Field = field;

            interestContext.Create(newInterest);

            Interest interest = interestContext.ReadAll().Last();
            int interestId = newInterest.Id;

            interestContext.Delete(interestId);

            ArgumentException ex = Assert.Throws<ArgumentException>(() => interestContext.Read(interestId));
            Assert.That(ex.Message, Is.EqualTo($"Interest with id = {interestId} does not exist!"));
        }
    }
}
