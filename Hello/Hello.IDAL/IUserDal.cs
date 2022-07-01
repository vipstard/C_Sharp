using Hello.Model;
using System;
using System.Collections.Generic;

namespace Hello.IDAL
{
    public interface IUserDal
    {
        List<User> GetUserLitst();

        User GetUser(int userNo);

        bool SaveUser(User user);
    }
}
