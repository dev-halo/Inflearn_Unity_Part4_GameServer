using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    //class Lock
    //{
    //    // bool <- 커널
    //    AutoResetEvent available = new AutoResetEvent(true);
    //    //ManualResetEvent available = new ManualResetEvent(true);

    //    public void Acquire()
    //    {
    //        available.WaitOne(); // 입장 시도
    //    }

    //    public void Release()
    //    {
    //        available.Set(); // 문을 열어준다.
    //    }
    //}

    class Program
    {
        static int num = 0;
        //static Lock _lock = new Lock();

        // int ThreadId
        static Mutex _lock = new Mutex();

        static void Thread_1()
        {
            for (int i = 0; i < 100000; ++i)
            {
                //_lock.Acquire();
                //num++;
                //_lock.Release();

                _lock.WaitOne();
                num++;
                _lock.ReleaseMutex();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 100000; ++i)
            {
                //_lock.Acquire();
                //num--;
                //_lock.Release();

                _lock.WaitOne();
                num--;
                _lock.ReleaseMutex();
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
