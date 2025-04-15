using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;
namespace DataLayer
{
    public class FieldContext : IDb<Field, int>
    {
        private AppDbContext dbContext;

        public FieldContext(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Create(Field item)
        {

            List<User> Users = new List<User>(item.Users.Count);
            for (int i = 0; i < item.Users.Count; ++i)
            {
                User UserFromDb = dbContext.Users.Find(item.Users[i].Id);
                if (UserFromDb != null) Users.Add(UserFromDb);
                else Users.Add(item.Users[i]);
            }
            item.Users = Users;

            dbContext.Fields.Add(item);
            dbContext.SaveChanges();
        }


        public Field Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Field> query = dbContext.Fields;
            if (useNavigationalProperties) query = query
            .Include(b => b.Users);

            if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

            Field Field = query.FirstOrDefault(b => b.Id == key);

            if (Field == null) throw new ArgumentException($"Field with id = {key} does not exist!");

            return Field;
        }

        public List<Field> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Field> query = dbContext.Fields;
            if (useNavigationalProperties) query = query
            .Include(b => b.Users);

            if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

            return query.ToList();
        }

        public void Update(Field item, bool useNavigationalProperties = false)
        {
            Field FieldFromDb = Read(item.Id, useNavigationalProperties);

            dbContext.Entry<Field>(FieldFromDb).CurrentValues.SetValues(item);

            if (useNavigationalProperties)
            {
                List<User> Users = new List<User>(item.Users.Count);
                for (int i = 0; i < item.Users.Count; ++i)
                {
                    User UserFromDb = dbContext.Users.Find(item.Users[i].Id);
                    if (UserFromDb != null) Users.Add(UserFromDb);
                    else Users.Add(item.Users[i]);
                }
                FieldFromDb.Users = Users;
            }

            dbContext.SaveChanges();
        }

        public void Delete(int key)
        {
            Field FieldFromDb = Read(key);
            dbContext.Fields.Remove(FieldFromDb);
            dbContext.SaveChanges();
        }
    }
}
