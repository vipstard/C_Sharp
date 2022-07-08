using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkCoreStudy.Entities
{
    /// <summary>
    /// 모델 클래스
    /// </summary>
    //[Table("s_users")] 1번 방법
    public class User
    {
        public int UserId { get; set; }

        [Column("s_userName")]
        public string UserName { get; set; }
        public string Birth { get; set; }
    }
}