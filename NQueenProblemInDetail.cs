using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgrammingAlgorithms
{
    class NQueenProblem
    {
        private int item = 1;

        private int[,] firstDiagonal;
        private int[,] secondDiagonal;
        private int[,] copyOfChestBoard;
        private int[,] Rotation90;
        private int[,] Rotation180;
        private int[,] Rotation270;
        private List<int[,]> checkList;

        //N is dimention of board where queens are positioned
        public NQueenProblem(int N)
        {
            firstDiagonal=new int[N,N];
            secondDiagonal=new int[N,N];
            Rotation90=new int[N,N];
            Rotation180=new int[N,N];
            Rotation270=new int[N,N];

            checkList =new List<int[,]>();
        }
        
        //Fundemantal Solutions, it returns double amount of desired result 
        //the reason that it doesn't include first and second reflections for every rotation
        public void FundamentalSolution(int[,] chestBoard)
        {
            if (FundamentalSolveTheProblem(chestBoard) == false)
            {
                Console.Write("No solution");
            }
            checkList.Clear();
        }

        public void FullSolution(int[,] chestBoard)
        {
            if (SolveTheProblem(chestBoard) == false)
            {
                Console.Write("No solution");
            }
        }

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
                

            // Check upper diagonal on left side 
            for (int i = row, j = column; i >= 0 && j >= 0; i--, j--)
            {
                if (chestBoard[i, j] == 1)
                    return false;
            }
                

            // Check lower diagonal on left side 
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
            int N = chestBoard.GetLength(0);
            //if it reaches to last index of column Display and return true
            if (column == N)
            {
                Display(chestBoard);
                return true;
            }


            for (int r = 0; r < N; r++)
            {
                //       \
                //    ----  IsProper checks three places! Up-Left ,Down-Left ,Left
                //       /
                if (IsProper(chestBoard, r, column))
                {
                    // put the value in position 
                    chestBoard[r, column] = 1;                                   
                    
                    // Make result true if any placement is possible            
                    // Recursion follows the row , till N                       
                    decision = SolveTheProblem(chestBoard, column + 1) || decision;

                    //this is placed in stack in every cycle, beware!
                    //this is also called as backtracking
                    //if column+1 can't reach to the value of N , 0 is placed back in place of 1
                    chestBoard[r, column] = 0; 
                }
            }

            //If queen can not be placed in any row in this column then return false 
            return decision;
        }

        //this method shows fundamental solutions of NQueen problem
        //fundemantal solution excludes rotations if they are somehow alike
        //this method does't inclue diagonal reflections fro every rotation
        //includes norotational reflexions
        private bool FundamentalSolveTheProblem(int[,] chestBoard, int column = 0)
        {
           
            bool decision = false;
            int N = chestBoard.GetLength(0);

            copyOfChestBoard = new int[N, N];
            //if it reaches to last index of column Display and return true
            if (column == N)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        firstDiagonal[i, j] = chestBoard[i, j];
                        secondDiagonal[i, j] = chestBoard[i, j];
                        Rotation90[i, j] = chestBoard[i, j];
                        Rotation180[i, j] = chestBoard[i, j];
                        Rotation270[i, j] = chestBoard[i, j];

                    }
                }

                FirstDiagonalReflectionSwap(firstDiagonal); 
                SecondDiagonalReflectionSwap(secondDiagonal);
                RotateMatrix90(N,Rotation90);
                RotateMatrix180(N, Rotation180);
                RotateMatrix270(N,Rotation270);

                bool outCheck = true;
                //check if rotations and diagonal reflections are similar
                outCheck = OutCheck(N, outCheck);

                //if outCheck returns as true it means that there is a similar matrix in the list 
                if (outCheck)
                {
                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            copyOfChestBoard[i, j] = chestBoard[i, j];
                        }
                    }
                    checkList.Add(copyOfChestBoard);
                    Display(copyOfChestBoard);
                }
                    
                return true;
            }

            for (int r = 0; r < N; r++)
            {

                if (IsProper(chestBoard, r, column))
                {
                    // put the value in position 
                    chestBoard[r, column] = 1;

                    // Make result true if any placement 
                    // is possible 
                    decision = FundamentalSolveTheProblem(chestBoard, column + 1) || decision;

                    //this is placed in stack in every cycle, beware!
                    //this is also called as backtracking
                    chestBoard[r, column] = 0;
                }
            }

            //If queen can not be placed in any row in this column then return false 
            return decision;
        }
        private void FirstDiagonalReflectionSwap(int[,] chestBoard)
        {
            int N = chestBoard.GetLength(0);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    int temp = chestBoard[i, j];
                    chestBoard[i, j] = chestBoard[j, i];
                    chestBoard[j, i] = temp;
                }
            }
        }
        private void SecondDiagonalReflectionSwap(int[,] chestBoard)
        {
            int N = chestBoard.GetLength(0);

            for (int i = 0, j = N - 1; i < N && j >= 0; i++, j--)
            {
                for (int k = 0; k < j; k++)
                {
                    int temp = chestBoard[i, k];
                    chestBoard[i, k] = chestBoard[(N - 1) - k, j];
                    chestBoard[(N - 1) - k, j] = temp;
                }
            }
        }
        private bool OutCheck(int N, bool outCheck)
        {
            // checking the firstDiogonal, if there is one it returns false so that no need for extra availablity check
            foreach (var arr in checkList)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        //if one element pops up as different, outCheck is true , break the loop
                        if (arr[i, j] == firstDiagonal[i, j])
                            outCheck = false;
                        else
                        {
                            outCheck = true;
                            break;
                        }
                    }
                    //if loop is cut off by break it will come out as true, so that get out of second inner loop
                    if (outCheck) break;
                }
                //if all elements are similar then outCheck gets false, which means there is a similar object in list
                //so that get out of foreach loop, we are done
                if (!outCheck) break;
            }
            //if first foreach traverse returns true , it means that there is no similar matrix in current list 
            //so that check for others, -secondDiagonal and rotations-
            if (outCheck)
            {
                foreach (var arr in checkList)
                {
                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            if (arr[i, j] == secondDiagonal[i, j])
                                outCheck = false;
                            else
                            {
                                outCheck = true;
                                break;
                            }
                        }

                        if (outCheck) break;
                    }

                    if (!outCheck) break;
                }
            }

            if (outCheck)
            {
                foreach (var arr in checkList)
                {
                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            if (arr[i, j] == Rotation90[i, j])
                                outCheck = false;
                            else
                            {
                                outCheck = true;
                                break;
                            }
                        }

                        if (outCheck) break;
                    }

                    if (!outCheck) break;
                }
            }

            if (outCheck)
            {
                foreach (var arr in checkList)
                {
                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            if (arr[i, j] == Rotation180[i, j])
                                outCheck = false;
                            else
                            {
                                outCheck = true;
                                break;
                            }
                        }

                        if (outCheck) break;
                    }

                    if (!outCheck) break;
                }
            }

            if (outCheck)
            {
                foreach (var arr in checkList)
                {
                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            if (arr[i, j] == Rotation270[i, j])
                                outCheck = false;
                            else
                            {
                                outCheck = true;
                                break;
                            }
                        }

                        if (outCheck) break;
                    }

                    if (!outCheck) break;
                }
            }

            return outCheck;
        }
        //Clockwise Rotation!
        private void RotateMatrix90(int N, int[,] rotation)
        {
            // Consider all squares one by one 
            for (int x = 0; x < N / 2; x++)
            {
                // Consider elements in group of 4 in current square 
                for (int y = x; y < N - x - 1; y++)
                {
                    // store current cell in temp variable 
                    int temp = rotation[x, y];

                    // move values from right to top 
                    rotation[x, y] = rotation[y, N - 1 - x];

                    // move values from bottom to right 
                    rotation[y, N - 1 - x] = rotation[N - 1 - x,
                        N - 1 - y];

                    // move values from left to bottom 
                    rotation[N - 1 - x,
                            N - 1 - y]
                        = rotation[N - 1 - y, x];

                    // assign temp to left 
                    rotation[N - 1 - y, x] = temp;
                }
            }
        }
        private void RotateMatrix180(int N, int[,] rotation)
        {
            RotateMatrix90(N,rotation);
            RotateMatrix90(N,rotation);
        }
        private void RotateMatrix270(int N, int[,] rotation)
        {
            RotateMatrix90(N, rotation);
            RotateMatrix90(N, rotation);
            RotateMatrix90(N, rotation);
        }
    }
}
