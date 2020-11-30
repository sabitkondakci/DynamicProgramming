using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgrammingAlgorithms
{
    class NQueenProblem
    {
        private static int item = 1;

        //Display method to print possible solutions
        private void Display(int[,] chestBoard)
        {
            Console.Write($"{item++}");
            Console.WriteLine();
            for (int i = 0; i < chestBoard.GetLength(0); i++)
            {
                for (int j = 0; j < chestBoard.GetLength(1); j++)
                    Console.Write( $"{chestBoard[i, j]} ");

                Console.Write("\n");
            }

            Console.WriteLine();
        }

        //Method checks the place , if it's proper for certain row and column then place it
        private bool IsProper(int[,] chestBoard, int row, int column)
        {
            
            // check the left side of this column
            for (int i = 0; i < column; i++)
            {
                if (chestBoard[row, i] == 1)
                    return false;
            }
                

            // Check upper diagonal on LEFT side 
            for (int i = row, j = column; i >= 0 && j >= 0; i--, j--)
            {
                if (chestBoard[i, j] == 1)
                    return false;
            }
                

            // Check lower diagonal on LEFT side 
            for (int i = row, j = column; j >= 0 && i < chestBoard.GetLength(0); i++, j--)
            {
                if (chestBoard[i, j] == 1)
                    return false;
            }

            return true;
        }

        //A recursive function to solve the problem
        private bool SolveTheProblem(int[,] chestBoard, int column=0)
        {
            bool decision = false;

            //if it reaches to last index of column Display and return true
            if (column == chestBoard.GetLength(1))
            {
                Display(chestBoard);
                return true;
            }


            for (int r = 0; r < chestBoard.GetLength(0); r++)
            {
               
                if (IsProper(chestBoard, r, column))
                {
                    // put the value in position 
                    chestBoard[r, column] = 1;

                    // Make result true if any placement 
                    // is possible 
                    decision = SolveTheProblem(chestBoard, column + 1) || decision;

                    //this is placed in stack in every cycle, beware!
                    //this is also called as backtracking
                    chestBoard[r, column] = 0; 
                }
            }

            //If queen can not be placed in any row in this column then return false 
            return decision;
        }

    }
}
