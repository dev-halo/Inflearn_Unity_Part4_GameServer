using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class SpinLock
    {
        volatile int locked = 0;

        public void Acquire()
        {
            while (true)
            {
                //int original = Interlocked.Exchange(ref locked, 1);
                //if (original == 0)
                //    break;

                // CAS Compare-And-Swap
                int expected = 0; // 예상하는 값.
                int desired = 1; // 대입 원하는 값.
                if (Interlocked.CompareExchange(ref locked, desired, expected) == expected)
                    break;
            }
        }

        public void Release()
        {
            locked = 0;
        }
    }

    class Program
    {
        static int num = 0;
        static SpinLock spinLock = new SpinLock();

        static void Thread_1()
        {
            for (int i = 0; i < 1000000; ++i)
            {
                spinLock.Acquire();
                num++;
                spinLock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 1000000; ++i)
            {
                spinLock.Acquire();
                num--;
                spinLock.Release();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(num);
        }
    }
}
