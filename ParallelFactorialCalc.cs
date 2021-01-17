using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelFactorialCalculaion
{
    class Program
    {
        public static double Factorial(int num)
        {
            IEnumerable<int> numbers = ParallelEnumerable.Range(1, num);

            double sum = numbers.AsParallel().Aggregate<int, double, double>(
                () => 1,
                (localTotal, n) => localTotal * n,
                (mainTotal, localTotal) => mainTotal * localTotal,
                finalResult => finalResult);

            return sum;
        } 
    }
}
