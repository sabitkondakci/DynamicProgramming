using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgrammingAlgorithms
{
    class NQueenProblem
    {
        // this method finds all possible solutions of NQueenProblem
        public bool IsProper(int[,] chestBoard, int row, int column)
        {
            
            for (int i = 0; i < column; i++)
            {
                if (chestBoard[row, i] == 1)
                    return false;
            }

            for (int i= row ,j=column; i>=0 && j>=0;i--,j--)
            {
                if (chestBoard[i, j] == 1)
                    return false;
            }

            for (int i = row,j=column; j >= 0 && i < chestBoard.GetLength(0); i++, j--)
            {
                if (chestBoard[i, j] == 1)
                    return false;
            }

            return true;
        }

        //This method starts from 0. column by default and chestBoard is the designed array for chest board
        public bool SolveTheProblem(int[,] chestBoard, int column=0 )
        {
            List<List<int>> solutionList=new List<List<int>>();
            List<int> results=new List<int>();
            // if column reaches to the end successfully, you're done
            if (column >= chestBoard.GetLength(1))
            {
                foreach (var i in chestBoard)
                {
                   results.Add(i); 
                }
                
                //Mirror image of matrix is also a possible solution
                for (int i = 0; i < chestBoard.GetLength(0); i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        chestBoard[i, j] = chestBoard[i, j] + chestBoard[j, i] 
                                           - (chestBoard[j, i] = chestBoard[i, j]);
                    }
                }
                
                foreach (var k in chestBoard)
                {
                    results.Add(k);
                }

                solutionList.Add(results);
                PrintToConsole(solutionList, chestBoard.GetLength(0));
                return true;
            }
            
            //increase the row number , till it recurses
            for (int i = 0; i < chestBoard.GetLength(0); i++)
            {
                //check all diagonal lines , upper rows 
                if (IsProper(chestBoard, i, column))
                {
                    // if there is no problem , simply place it
                    chestBoard[i, column] = 1;
                    // recursion for next column
                    if (SolveTheProblem(chestBoard, column+1))
                        return true;

                    chestBoard[i, column] = 0; // this is backtracking
                }
            }

            return false;
        }
        private void PrintToConsole(List<List<int>> fullList,int N)
        {
            int count = 0;

            foreach (var items in fullList)
            {
                foreach (var element in items)
                {
                    Console.Write(element + " ");
                    count++;
                    if (count % N == 0)
                        Console.WriteLine();

                    if (count % (N*N) == 0)
                        Console.WriteLine();
                }

            }
        }
    }
}
