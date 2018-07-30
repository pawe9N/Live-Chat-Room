using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChatRoom.Models
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        User Save(User user);
        void Delete(User user);
        void SaveChanges();
        void ValidateOnSaveEnabledFalse();
    }
}
