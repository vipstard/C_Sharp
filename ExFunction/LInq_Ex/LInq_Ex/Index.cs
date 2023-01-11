using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInq_Ex
{
    internal class Index
    {
        static void Main(string[] args)
        {
            List<string> strLi = new List<string>()
            {
                "Java", "C Sharp", "C++", "JavaScript", "React"
            };

            var linqMethodResult = strLi
                .Select((item, index) => new  // 첫 번째 호출. 인덱스와 요소를 가지는 익명 타입의 객체 생성
                {
                    index,
                    subject = item
                })
                .Where(item => item.subject.StartsWith("C")) // "C"로 시작하는 요소를 필터
                .Select(item => new                          // 필터된 항목을 추출
                {
                    index = item.index,
                    subject = item.subject
                });

            foreach (var obj in linqMethodResult)
                Console.WriteLine("index: " + obj.index + " / Subject: " + obj.subject);
        }
    }
}
