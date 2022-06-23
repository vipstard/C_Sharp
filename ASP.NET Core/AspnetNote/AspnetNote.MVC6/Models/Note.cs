using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetNote.MVC6.Models
{
    public class Note
    {
        /// <summary>
        /// 게시글 번호
        /// </summary>
        [Key] // PK 설정
        public int NoteNo { get; set; }

        /// <summary>
        /// 게시물 제목
        /// </summary>
        [Required] // Not Null 설정
        public string NoteTitle { get; set; }

        /// <summary>
        /// 게시물 내용
        /// </summary>
        [Required] // Not Null 설정
        public string Noteontents { get; set; }

        /// <summary>
        /// 작성자 번호
        /// </summary>
        [Required] // Not Null 설정
        public int UserNo { get; set; }

        [ForeignKey("UserNo")]
        public virtual User User { get; set; }
    }
}
