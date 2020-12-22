using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;

namespace DynamicProgrammingAlgorithms
{
    //Magic Sum = n(n^2+1)/2
    class MagicSquareForOddSizes
    {
        // method returns magicSum according to size
        public int MagicSquare(int[,] magicArray)
        {
            int n = magicArray.GetLength(0);
            int magicSum = n * (n * n + 1) / 2;

            if (n % 2 == 0)
                throw new RuntimeBinderInternalCompilerException("n must be odd");

            int row = n - 1;
            int col = n / 2;
            magicArray[row,col] = 1;

            for (int i = 2; i <= n * n; i++)
            {
                if (magicArray[(row + 1) % n,(col + 1) % n] == 0)
                {
                    row = (row + 1) % n;
                    col = (col + 1) % n;
                }
                else
                    row = (row - 1) % n;
                
                magicArray[row,col] = i;
            }

            return magicSum;
        }
    }
}
