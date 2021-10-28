﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    // 재귀적 락을 허용할지 (No)
    // 재귀적 락을 허용할지 (Yes) WriteLock - > WirteLock OK, ReadLock -> ReadLock OK, ReadLock -> WriteLock NO
    // 스핀락 정책 (5000번 -> Yield)
    class Lock
    {
        const int EMPTY_FLAG = 0x00000000;
        const int WRITE_MASK = 0x7FFF0000;
        const int READ_MASK = 0x0000FFFF;
        const int MAX_SPIN_COUNT = 5000;

        // [Unused(1)] [WriteThreadId(15)] [ReadCount(16)]
        int flag = EMPTY_FLAG;
        int writeCount = 0;

        public void WriteLock()
        {
            // 동일 쓰레드가 WriteLock 을 이미 획득하고 있는지 확인.
            int lockThreadId = (flag & WRITE_MASK) >> 16;
            if (Thread.CurrentThread.ManagedThreadId == lockThreadId)
            {
                writeCount++;
                return;
            }

            // 아무도 WriteLock or ReadLock 을 획득하고 있지 않을 때, 경합해서 소유권을 얻는다.
            int desired = (Thread.CurrentThread.ManagedThreadId << 16) & WRITE_MASK;
            while (true)
            {
                for (int i = 0; i < MAX_SPIN_COUNT; ++i)
                {
                    if (Interlocked.CompareExchange(ref flag, desired, EMPTY_FLAG) == EMPTY_FLAG)
                    {
                        writeCount = 1;
                        return;
                    }
                }

                Thread.Yield();
            }
        }

        public void WriteUnlock()
        {
            int lockCount = --writeCount;
            if (lockCount == 0)
                Interlocked.Exchange(ref flag, EMPTY_FLAG);
        }

        public void ReadLock()
        {
            // 동일 쓰레드가 WriteLock 을 이미 획득하고 있는지 확인.
            int lockThreadId = (flag & WRITE_MASK) >> 16;
            if (Thread.CurrentThread.ManagedThreadId == lockThreadId)
            {
                Interlocked.Increment(ref flag);
                return;
            }

            // 아무도 WriteLock 을 획득하고 있지 않으면, ReadCount 를 1 늘린다.
            while (true)
            {
                for (int i = 0; i < MAX_SPIN_COUNT; ++i)
                {
                    int expected = (flag & READ_MASK);
                    if (Interlocked.CompareExchange(ref flag, expected + 1, expected) == expected)
                        return;
                }

                Thread.Yield();
            }
        }

        public void ReadUnlock()
        {
            Interlocked.Decrement(ref flag);
        }
    }
}
