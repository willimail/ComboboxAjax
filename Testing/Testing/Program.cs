using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Testing
{
    class Program
    {
        [ThreadStatic]
        static int _field;

        static void Main(string[] args)
        {
            new Thread(() =>
            {
                for(int i =0; i<10; i++)
                {
                    Console.WriteLine("Thread A: " + _field++);
                }
            }).Start();

            new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("Thread B: " + _field++);
                }
            }).Start();

            Console.Read();

        }
    }
}
