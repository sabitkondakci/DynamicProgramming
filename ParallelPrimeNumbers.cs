using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelPrimeNumbers
{
    class Program
    {
        public static void Main()
        {
            IEnumerable<int> numbers = Enumerable.Range(3, 100000000 - 3);

            Stopwatch timer = new Stopwatch();

            timer.Start();
            var parallelQuery =
                from n in numbers.AsParallel()
                where IsNPrime(n)
                select n;
            timer.Stop();
            //less than 3 secs
            Console.WriteLine(timer.Elapsed.TotalMilliseconds);
        }

        
        private static bool IsNPrime(int n)
        {
            if (n == 1) return false;
            if (n == 2) return true;
            if (n > 2 && n % 2 == 0) return false;

            int divisor = (int)Math.Floor(Math.Sqrt(n));
            for (int i = 3; i <= divisor; i+=2)
                if (n % i == 0) return false;
            
            return true;
        }
    }
}
