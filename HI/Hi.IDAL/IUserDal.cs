using Hi.Model;
using System.Collections.Generic;

namespace Hi.IDAL
{
    public interface IUserDal
    {
        List<User> GetUsersList();

        User GetUser(int userNo);

        bool SaveUser(User user);
    }
}
