using Hi.IDAL;
using Hi.Model;
using System;
using System.Collections.Generic;

namespace Hi.Bll
{
    public class UserBll 
    {
        private readonly IUserDal _userDal;

        public UserBll(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public List<User> GetUsersList()
        {
            return _userDal.GetUsersList();
        }

        public User GetUser(int userNo)
        {
            if (userNo <= 0)
            {
                throw new ArgumentException();
            }

            return _userDal.GetUser(userNo);
        }

        public bool SaveUser(User user)
        {
            if (user == null) throw new ArgumentNullException();
            return _userDal.SaveUser(user);

        }
    }
}
