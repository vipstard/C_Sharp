using Hello.IDAL;
using Hello.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hello.BLL
{
    public class UserBLL 
    {
        private readonly IUserDal _userDal;

        // 오라클이 들어올지 MSSQL이 들어올지 모르니 인터페이스를 주입시켜준다.
        public UserBLL(IUserDal userDal) 
        {
            _userDal = userDal;
        }

        public List<User> GetUserLitst()
        {
            return _userDal.GetUserLitst();
        }

        public User GetUser(int userNo)
        {
            if(userNo <= 0)
            {
                throw new ArgumentException();
            }
            return _userDal.GetUser(userNo);
        }

    

        public bool SaveUser(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException();
            }
            return _userDal.SaveUser(user);
        }
    }
}
