using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Testing
{
    class Program
    {
        static ThreadLocal<int> _field = new ThreadLocal<int>(() => 
        {
            return Thread.CurrentThread.ManagedThreadId;
        });

        static void Main(string[] args)
        {
            new Thread(() =>
            {
                for(int i =0; i<_field.Value; i++)
                {
                    Console.WriteLine("Thread A: " + _field.Value);
                    Thread.Sleep(500);
                }
            }).Start();

            new Thread(() =>
            {
                for (int i = 0; i < _field.Value; i++)
                {
                    Console.WriteLine("Thread B: " + _field.Value);
                    Thread.Sleep(500);
                }
            }).Start();

            Console.Read();

        }
    }
}
