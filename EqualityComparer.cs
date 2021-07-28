using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShallowDeepCopy
{

    public class PrimaryColorComparer : EqualityComparer<PrimaryColor>
    {
        public override bool Equals(PrimaryColor x, PrimaryColor y)
        {
            return Default.Equals(x, y); // direct comparison "==" is of same execution time in .Net 5 +
        }

        public override int GetHashCode([DisallowNull] PrimaryColor obj)
        {
            return Default.GetHashCode(obj); // obj.GetHashCode() is of same execution time in .Net 5 +
        }
    }

    public enum PrimaryColor { Red, Yellow, Blue }


    [MemoryDiagnoser]
    public class CompareEnum
    {     
        public PrimaryColor firstPrimaryColor => PrimaryColor.Blue;
        public PrimaryColor secondPrimaryColor => PrimaryColor.Blue;

        public bool GenericBoxing<T>(T firstEnum, T secondEnum) where T : Enum 
        {
            return firstEnum.Equals(secondEnum);
        }

        public bool GenericBoxingEnumEquals<T>(T firstEnum, T secondEnum) where T : Enum
        {
            return Enum.Equals(firstEnum,secondEnum);        
        }

        public bool GenericEqualityComparer<T>(T firstEnum, T secondEnum) where T : Enum
        {
            return EqualityComparer<T>.Default.Equals(firstEnum,secondEnum);
        }

        [Benchmark]
        public void GenericBoxingTest()
        {
            for (int i = 0; i < 10_000; i++)
            {
                bool arePrimaryColorsEqualBoxing = GenericBoxing<PrimaryColor>(firstPrimaryColor, secondPrimaryColor); // ~ 160 micro seconds
            }            
        }

        [Benchmark]
        public void GenericBoxingEnumEqualsTest()
        {
            for (int i = 0; i < 10_000; i++)
            {
                bool arePrimaryColorsEqualEnum = GenericBoxingEnumEquals<PrimaryColor>(firstPrimaryColor, secondPrimaryColor); // ~ 180 micro seconds
            }
        }

        [Benchmark]
        public void GenericEqualityComparerTest()
        {
         
            for (int i = 0; i < 10_000; i++)
            {
                bool arePrimaryColorsEqComparer = GenericEqualityComparer<PrimaryColor>(firstPrimaryColor, secondPrimaryColor); // ~ 3 micro seconds
            }
        }

        [Benchmark]
        public void PrimaryColorComparerEqualsTest()
        {
            PrimaryColorComparer comparer = new();
            for (int i = 0; i < 10_000; i++)
            {
                bool arePrimaryColorsEqualDerived = comparer.Equals(firstPrimaryColor, secondPrimaryColor); // ~ 3 micro seconds
            }
        }
    }

    class Program
    {
        
        static void Main(string[] args)
        {

            var a = BenchmarkRunner.Run<CompareEnum>();
        
        }
    }
}
