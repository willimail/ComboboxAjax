using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    class Program
    {
        

        static void Main(string[] args)
        {
            Task t = Task.Run(() =>
            {
                for(int i=0; i<10; i++)
                {
                    Console.Write("*");
                }
            });
            t.Wait();

            Console.Read();

        }
    }
}
