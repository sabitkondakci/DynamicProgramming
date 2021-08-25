using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDemo
{
    class Program
    {    
        static void Main()
        {
            var a = BenchmarkRunner.Run<EqualityBenchmark>();
        }   
    }

    [MemoryDiagnoser]
    public class EqualityBenchmark
    {
        public int[] arr1;
        public int[] arr2;

        [GlobalSetup]
        public void Setup()
        {
            arr1 = Enumerable.Range(0, 1_000).ToArray();
            arr2 = Enumerable.Range(0, 1_000).ToArray();
        }

        public static bool LinqEquality<T>(T[] firstArray, T[] secondArray)
        {
            return firstArray.SequenceEqual(secondArray);
        }

        public static bool BitwiseEquality(int[] x, int[] y)
        {                  
            if (x == null && y == null)          
                return true;
            
            if (x == null || y == null || x.Length != y.Length)          
                return false;          

            int diff = 0;

            for (int i = 0; i < x.Length; i++)
            {
                diff |= x[i] ^ y[i];
            }

            return diff == 0;
        }

        public static bool EqComparerEquality<T>(T[] firstArray, T[] secondArray)
        {
            if (ReferenceEquals(firstArray, secondArray))          
                return true;
            
            if ( firstArray == null || secondArray == null || firstArray.Length != secondArray.Length )          
                return false;
            
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            for (int i = 0; i < firstArray.Length; i++)
            {
                if (!comparer.Equals(firstArray[i], secondArray[i]))               
                    return false;              
            }

            return true;
        }

        [Benchmark]
        public void LinqArrEqual() // 948 ms
        {
            int conditionalCount = 0;

            for (int i = 0; i < 100_000; i++)
            {
                if (LinqEquality(arr1, arr2))
                    conditionalCount++;
            }
        }

        [Benchmark]
        public void BitwiseArrEqual() // 77 ms
        {
            int conditionalCount = 0;

            for (int i = 0; i < 100_000; i++)
            {
                if (BitwiseEquality(arr1, arr2))
                    conditionalCount++;
            }
        }

        [Benchmark]
        public void EqualityComparerCheck() // 243 ms
        {
            int conditionalCount = 0;

            for (int i = 0; i < 100_000; i++)
            {
                if (EqComparerEquality(arr1, arr2))
                    conditionalCount++;
            }
        }
    }

}
        

