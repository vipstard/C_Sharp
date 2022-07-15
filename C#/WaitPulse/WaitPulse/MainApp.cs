using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WaitPulse
{
    
        class Counter
        {
            const int LOOP_COUNT = 1000;

            readonly object thislock;
            bool lockedCount = false;

            private int count;
            public int Count
            {
                get { return count;  }
            }

            public Counter()
            {
                thislock = new object();
                count = 0;
            }

            public void Increase()
            {
                int loopCount = LOOP_COUNT;

                while (loopCount-- > 0)
                {
                    lock (thislock)
                    {
                        while (count > 0 || lockedCount == true)
                            Monitor.Wait(thislock);

                        lockedCount = true;
                        count++;
                        lockedCount = false;

                        Monitor.Pulse(thislock);
                    }
                }
            }

            public void Decrease()
            {
                int loopCount = LOOP_COUNT;

                while(loopCount-- > 0)
                {
                    lock (thislock)
                    {
                        while (count < 0 || lockedCount == true)
                            Monitor.Wait(thislock);

                        lockedCount = true;
                        count--;
                        lockedCount = false;

                        Monitor.Pulse(thislock);
                    }
                }
            }
        }

    class MainApp
    {
        static void Main(string[] args)
        {
            Counter counter = new Counter();

            Thread incThread = new Thread(new ThreadStart(counter.Increase));
            Thread decThread = new Thread(new ThreadStart(counter.Decrease));

            incThread.Start();
            decThread.Start();

            incThread.Join();
            decThread.Join();

            Console.WriteLine(counter.Count);
        }
    }
}
