
using Note.IDAL;
using Note.Model;

using System;
using System.Collections.Generic;
using Note.Oracle.DAL;

namespace Note.BLL
{
    public class UserBll
    {

        //private UserDal _userDal = new UserDal(); 강한결합방식

        private readonly IUserDal _userDal; // 느슨한 결합 방식

        public UserBll(IUserDal userDal)
        {
            _userDal = userDal;       
        }
        public User GetUser(int userNo)
        {
            return _userDal.GetUser(userNo);
        }

        public List<User> GetUerList()
        {
            throw new NotImplementedException();
        }

     
    }
}
