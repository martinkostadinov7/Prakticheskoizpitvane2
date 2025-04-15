using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;
namespace DataLayer
{
    public class InterestContext : IDb<Interest, int>
    {
        private AppDbContext dbContext;

        public InterestContext(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Create(Interest item)
        {
            Field FieldFromDb = dbContext.Fields.Find(item.Field.Id);
            if (FieldFromDb != null) item.Field = FieldFromDb;

            List<User> Users = new List<User>(item.Users.Count);
            for (int i = 0; i < item.Users.Count; ++i)
            {
                User UserFromDb = dbContext.Users.Find(item.Users[i].Id);
                if (UserFromDb != null) Users.Add(UserFromDb);
                else Users.Add(item.Users[i]);
            }
            item.Users = Users;

            dbContext.Interests.Add(item);
            dbContext.SaveChanges();
        }


        public Interest Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Interest> query = dbContext.Interests;
            if (useNavigationalProperties) query = query
            .Include(b => b.Field)
            .Include(b => b.Users);

            if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

            Interest Interest = query.FirstOrDefault(b => b.Id == key);

            if (Interest == null) throw new ArgumentException($"Interest with id = {key} does not exist!");

            return Interest;
        }

        public List<Interest> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Interest> query = dbContext.Interests;
            if (useNavigationalProperties) query = query
            .Include(b => b.Field)
            .Include(b => b.Users);

            if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

            return query.ToList();
        }

        public void Update(Interest item, bool useNavigationalProperties = false)
        {
            Interest InterestFromDb = Read(item.Id, useNavigationalProperties);

            dbContext.Entry<Interest>(InterestFromDb).CurrentValues.SetValues(item);

            if (useNavigationalProperties)
            {
                Field FieldFromDb = dbContext.Fields.Find(item.Field.Id);
                if (FieldFromDb != null) InterestFromDb.Field = FieldFromDb;
                else InterestFromDb.Field = item.Field;

                List<User> Users = new List<User>(item.Users.Count);
                for (int i = 0; i < item.Users.Count; ++i)
                {
                    User UserFromDb = dbContext.Users.Find(item.Users[i].Id);
                    if (UserFromDb != null) Users.Add(UserFromDb);
                    else Users.Add(item.Users[i]);
                }
                InterestFromDb.Users = Users;
            }

            dbContext.SaveChanges();
        }

        public void Delete(int key)
        {
            Interest InterestFromDb = Read(key);
            dbContext.Interests.Remove(InterestFromDb);
            dbContext.SaveChanges();
        }
    }
}
