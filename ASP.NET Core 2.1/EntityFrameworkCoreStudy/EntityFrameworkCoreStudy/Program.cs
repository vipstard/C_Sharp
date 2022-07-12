using EntityFrameworkCoreStudy.Data;
using System;
using System.Linq;

namespace EntityFrameworkCoreStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new EfStudyDbContext())
            {

                var list = db.Users.ToList();
                                

                foreach (var user in list)
                {
                    Console.WriteLine($"{user.UserId}.{user.UserName}({user.Birth})");
                }

                #region + Linq의 분류 (2가지)
                // # Linq 분류(2가지)
                // 1. 쿼리구문
                // from user in users
                // where ....
                // select new ...

                // 2. 메서드 구문  직관적, 간단하게 쓸 수있음
                // db.Users.Where().ToList();
                #endregion
                // .Where(), OrderBy()
                // 1. Where() -> 조건절 -> 리스트가 가능
                // SELECT * FROM Users WHERE UserId =1

                // var list = db.Users.ToList(); 전체출력
                // var list = db.Users.Where(u => u.UserName == "뚱이"); 

                // foreach(var user in list)
                // {
                //    Console.WriteLine($"{user.UserId}.{user.UserName}, {user.Birth}");
                // }

                // 게시물 1개 수정 -> 데이터 가져옴
                // 특정 데이터 1개 가져오기
                // .First(), .FirstOrdDefault(), SIngle(0, .SIngleOrDefault()
                // var user = db.Users.First();
                // SELECT TOP 1 * FROM  Users

                // var user = db.Users.First(u => u.UserId == 1);
                // SELECT TOP 1 * FROM Users WHERE UserNAme = "임길동"

                // SingleOrDefulat() vs FirstOrDefault()
                // SingleOrDefault() 오늘부터 권장

                // # OrderBy()
                // 1,2,3,4,5,6

                // # DecendingBy()
                // 6, 5, 4, 3, 2, 1


            }

        }
    }

 

    
}

// 1. SELECT 쿼리

// var selectList = db.Users; 
// 1) DbSet<User> selectList = db.Users; 
// 2) List<User> selectList = db.Users.ToList();
// 3) IEnumerable<User> selectList = db.Users.AsEnumerable();
// IEnumerable<User> selectList = db.Users.AsEnumerable().Where(u => u.UserId == 1);

// IQueryable<User> selectList = from user in db.Users select user; // Linq to Sql 


// IEnumerable vs IQueryable
// Extension Query => 작성이 가능
// 1. IEnumerable => 쿼리 => 데이터(10명) => Client => 느리다.
// 2. IQueryable => 쿼리 =>데이터(10명) => Server => 빠르다.


// 2. INSER 쿼리
// db.Users.Add(User);
// db.SaveChanges();

//db.Users.Add(new User
//{

//    UserId = 3,
//    UserName = "임길동",
//    Birth = "909090"
//  });

//db.SaveChanges(); // Commit 이라 생각하면 된다.

/* 3. Update 쿼리
var user = new User { UserId = 1, UserName = "장길동" }; // 되도록 분리 합치면 코드가 너무 길어진다.
db.Entry(user).State = EntityState.Modified;
db.SaveChanges();*/

// 4. Delete 쿼리 
// DELETE FROM User WHERE UserId =2; 일반적
// var user = new User { UserId = 1  };
// db.Users.Remove(user);
// db.SaveChanges();


//IEnumerable<User> selectList = db.Users.ToList();

//foreach (var item in selectList)
//{
//    Console.WriteLine(item.UserName);
//}
