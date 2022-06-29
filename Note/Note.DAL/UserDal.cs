using Note.IDAL;
using Note.Model;
using System;
using System.Collections.Generic;

namespace Note.DAL
{
    /*  인터페이스를 상속받아서 강한 결합 보다는 느슨한 결합을 유도한다. */
    public class UserDal : IUserDal
    {
        public List<User> GetUerList()
        {
            throw new NotImplementedException();
        }

        public User GetUser(int userNo)
        {
            throw new NotImplementedException();
        }
    }
}
