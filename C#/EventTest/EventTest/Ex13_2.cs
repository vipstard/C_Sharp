using System;

namespace EventTest
{
     class Ex13_2
    {
        delegate void MyDelegate(int a);

        class Market
        {
            public event MyDelegate CustomerEvent;

            public void BuySomething ( int CustomerNo)
            {
                if (CustomerNo == 30) CustomerEvent(CustomerNo);
            }
        }

        class MainApp
        {
            static public void MyHandler(int a)
            {
                Console.WriteLine($"축하합니다! {a}번째 고객 이벤트에 당첨되셨습니다.");
            }
            static void Main(string[] args)
            {
                Market market = new Market();
                market.CustomerEvent += new MyDelegate(MyHandler);

                for (int customerNo = 0; customerNo < 100; customerNo += 10) 
                    market.BuySomething(customerNo);
            }
        }
    }
}
