using System;

namespace ReadOnlyStruct
{
     /* 구조체는 readonly를 사용하여 모든 필드와 프로퍼티의 값을 수정할 수 없는 변경 불가능 구조체로 선언할 수 있다. 
      * (클래스는 변경불가능으로 선언 불가함)*/

    readonly struct RGBColor
    {
        public readonly byte R;
        public readonly byte G;
        public readonly byte B;

        public RGBColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
    class MainApp
    {
        static void Main(string[] args)
        {
            RGBColor Red = new RGBColor(255, 0, 0);
            // Red.G = 100; // 불변 객체의 상태를 건드리고 있다. (컴파일 에러 뜸)
            /* Red 객체 나머지 값 유지하면서 G값만 100인 객체를 얻으려면 '새로운' 객체를 만들면 된다.
             * ex -> RGBColor myColor = new RGBColor(Red.R, 100, Red.B); */
        }
    }
}
