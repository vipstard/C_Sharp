using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCoreStudy.Entities
{
    /// <summary>
    /// 모델 클래스
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Birth { get; set; }
    }
}