using System;
using System.ComponentModel.DataAnnotations;

namespace Note.Model
{
    /// <summary>
    /// 공지사항
    /// </summary>
    public class Notice
    {
        /// <summary>
        /// 공지사항 번호
        /// </summary>
        [Key]
        public int NoticeNo { get; set; }

        /// <summary>
        /// 공지사항 제목
        /// </summary>
        [Required]
        public string NoticeTitle { get; set; }

        /// <summary>
        /// 공지사항 내용
        /// </summary>
        [Required]
        public string NoticeContents { get; set; }

        /// <summary>
        /// 공지사항 작성 날짜
        /// </summary>
        [Required]
        public DateTime NoticeWriteDate { get; set; }

        /// <summary>
        /// 공지사항 조회수
        /// </summary>
        [Required]
        public int NoticeViewCount { get; set; }

    }
}
