using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECU_Manager.Tools
{
    public class InternalQueue<T>
    {
        T[] array;
        int read;
        int write;
        Mutex lockmutex;
        Semaphore waitsem;
        int waitcount;

        public InternalQueue(int size)
        {
            lockmutex = new Mutex();
            waitsem = new Semaphore(0, 1);
            waitcount = 0;
            array = new T[size];
            read = 0;
            write = 0;
        }

        public void Enqueue(T element)
        {
            int newindex;

            lockmutex.WaitOne();

            array[write] = element;
            newindex = write + 1;
            if (newindex >= array.Length)
                newindex = 0;

            if (read == newindex)
                throw new Exception("Overflow");

            write = newindex;

            if (waitcount > 0)
            {
                if (waitcount >= Count)
                {
                    waitsem.Release();
                    waitcount = 0;
                }
            }
            lockmutex.ReleaseMutex();

        }

        public void Dequeue(int amount = 1)
        {
            int newindex;

            if (amount > Count)
                throw new Exception("Underflow!");

            newindex = read + amount;
            if (newindex >= array.Length)
                newindex -= array.Length;

            read = newindex;
        }

        public T ElementAt(int index)
        {
            int newindex = read;
            T element;

            lockmutex.WaitOne();
            if (index >= Count)
            {
                waitcount = index + 1;
                lockmutex.ReleaseMutex();
                waitsem.WaitOne();
            }
            else
            {
                lockmutex.ReleaseMutex();
            }

            newindex = read;

            newindex += index;
            if (newindex >= array.Length)
                newindex -= array.Length;

            element = array[newindex];

            return element;
        }

        public int Count
        {
            get
            {
                int rd, wr;

                rd = read;
                wr = write;

                if (wr >= rd) return wr - rd;
                else return (array.Length - rd + wr);
            }
        }

    }
}
