using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;
namespace DataLayer
{
    public class UserContext : IDb<User, int>
    {
        private AppDbContext dbContext;

        public UserContext(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Create(User item)
        {
            List<User> Friends = new List<User>(item.Friends.Count);
            for (int i = 0; i < item.Friends.Count; ++i)
            {
                User UserFromDb = dbContext.Users.Find(item.Friends[i].Id);
                if (UserFromDb != null) Friends.Add(UserFromDb);
                else Friends.Add(item.Friends[i]);
            }
            item.Friends = Friends;

            List<Interest> Interests = new List<Interest>(item.Interests.Count);
            for (int i = 0; i < item.Interests.Count; ++i)
            {
                Interest InterestFromDb = dbContext.Interests.Find(item.Interests[i].Id);
                if (InterestFromDb != null) Interests.Add(InterestFromDb);
                else Interests.Add(item.Interests[i]);
            }
            item.Interests = Interests;

            dbContext.Users.Add(item);
            dbContext.SaveChanges();
        }


        public User Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<User> query = dbContext.Users;
            if (useNavigationalProperties) query = query
            .Include(b => b.Friends)
            .Include(b => b.Interests);

            if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

            User User = query.FirstOrDefault(b => b.Id == key);

            if (User == null) throw new ArgumentException($"User with id = {key} does not exist!");

            return User;
        }

        public List<User> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<User> query = dbContext.Users;
            if (useNavigationalProperties) query = query
            .Include(b => b.Friends)
            .Include(b => b.Interests);

            if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

            return query.ToList();
        }

        public void Update(User item, bool useNavigationalProperties = false)
        {
            User UserFromDb = Read(item.Id, useNavigationalProperties);

            dbContext.Entry<User>(UserFromDb).CurrentValues.SetValues(item);

            if (useNavigationalProperties)
            {
                List<User> Friends = new List<User>(item.Friends.Count);
                for (int i = 0; i < item.Friends.Count; ++i)
                {
                    User friendFromDb = dbContext.Users.Find(item.Friends[i].Id);
                    if (friendFromDb != null) Friends.Add(friendFromDb);
                    else Friends.Add(item.Friends[i]);
                }
                UserFromDb.Friends = Friends;

                List<Interest> Interests = new List<Interest>(item.Interests.Count);
                for (int i = 0; i < item.Interests.Count; ++i)
                {
                    Interest InterestFromDb = dbContext.Interests.Find(item.Interests[i].Id);
                    if (InterestFromDb != null) Interests.Add(InterestFromDb);
                    else Interests.Add(item.Interests[i]);
                }
                UserFromDb.Interests = Interests;
            }

            dbContext.SaveChanges();
        }

        public void Delete(int key)
        {
            User UserFromDb = Read(key);
            dbContext.Users.Remove(UserFromDb);
            dbContext.SaveChanges();
        }
    }
}
