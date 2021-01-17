using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelAggregation
{
    class Program
    {

        public static void Main()
        {
            Stopwatch timer = new Stopwatch();
            IEnumerable<int> nums = ParallelEnumerable.Range(0, int.MaxValue);
            long sum = 0;

            timer.Start();
            sum = nums.AsParallel().Aggregate<int,long,long>(
                () => 0,                                            // seedFactory: first value of localTotal
                (localTotal, n) => localTotal + n,                  // updateAccumulatorFunc:adding values to local total
                (mainTotal, localTotal) => mainTotal + localTotal,  // combineAccumulatorFunc:interlock add at mainTotal
                finalResult => finalResult);                        // resultSelector:last return value, sum
            timer.Stop();
            
            //less than 9 secs
            Console.WriteLine(timer.Elapsed.TotalMilliseconds);
            Console.WriteLine(sum);
        }
    }
}
