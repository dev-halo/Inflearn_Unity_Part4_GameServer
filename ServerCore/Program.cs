using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {
        static object lock1 = new object();
        static SpinLock lock2 = new SpinLock();

        class Reward
        {

        }

        // RWLock ReaderWriterLock
        static ReaderWriterLockSlim lock3 = new ReaderWriterLockSlim();

        static Reward GetRewardById(int id)
        {
            lock3.EnterReadLock();

            lock3.ExitReadLock();

            return null;
        }

        static void AddReward(Reward reward)
        {
            lock3.EnterWriteLock();

            lock3.ExitWriteLock();
        }

        static void Main(string[] args)
        {
            lock (lock1)
            {
            }
        }
    }
}
