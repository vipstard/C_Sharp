using System;
using System.Threading;

class MainApp
{
    static void DoSomething()
    {
        for (int i=0; i < 5; i++)
        {
            Console.WriteLine($"DoSomething : {i}");
            Thread.Sleep(10);
        }
    }

    static void Main(string[] args)
    {
        Thread t1 = new Thread(new ThreadStart(DoSomething));

        Console.WriteLine("Starting thread...");
        t1.Start();

        for (int i=0; i<5; i++)
        {
            Console.WriteLine($"Main : {i}");
            Thread.Sleep(10);
        }

        Console.WriteLine("Watring untill thread stops...");
        t1.Join();
        Console.WriteLine("Finished");

    }
}