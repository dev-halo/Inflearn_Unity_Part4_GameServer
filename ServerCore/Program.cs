using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {
        static int number = 0;
        static object obj = new object();

        static void Thread_1()
        {
            for (int i = 0; i < 1000000; ++i)
            {
                // 상호배제 Mutual Exclusive

                lock (obj)
                {
                    number++;
                }
            }
        }

        // 데드락 DeadLock

        static void Thread_2()
        {
            for (int i = 0; i < 1000000; ++i)
            {
                lock (obj)
                {
                    number--;
                }
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(number);
        }
    }
}
