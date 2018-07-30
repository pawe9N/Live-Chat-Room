using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Data.Entity;

namespace LiveChatRoom.Models
{
    public class EFUserRepository : IUserRepository
    {
        MyDatabaseEntities context = new MyDatabaseEntities();
        
        public IQueryable<User> Users
        {
            get { return context.Users; }
        }

        public User Save(User user)
        {
            if(user.UserID == 0)
            {
                context.Users.Add(user);
            }
            else
            {
                context.Entry(user).State = EntityState.Modified;
            }
            context.SaveChanges();
            return user;
        }

        public void Delete(User user)
        {
            context.Users.Remove(user);
            context.SaveChanges();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
        public void ValidateOnSaveEnabledFalse()
        {
            context.Configuration.ValidateOnSaveEnabled = false;
        }

    }
}