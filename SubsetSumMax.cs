using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgrammingAlgorithms
{
    class SubsetSumMax
    {
        // Method shows the proper values which add up to maxSum 
        // it returns a list where arrays are stored
        //  maxSum  : desired value for maximum sum
        //  valueSet: an array of values
        public List<long[]> SubsetListWithMaxSum(long maxSum, params long[] valueSet)
        {
            List<long[]> tempListSet=new List<long[]>();//a list to store arrays
            List<long> tempList=new List<long>();//a list to store values

            int count = 0;
            bool[,] controlBoolArray = Bool2DArrray(maxSum, valueSet);//2DBoolArray is created

            for (int i = valueSet.Length-1; i >=0; i--)
            {
                count = i;
                tempList.Clear();//in every cycle , clear the tempList

                if (controlBoolArray[i, maxSum])//if value is true , jump in
                {
                    tempList.Add(valueSet[i]);//add the value at row

                    for (long j = maxSum - valueSet[count]; j >= 0; j-=valueSet[count])
                    {
                        if (j == 0)
                        {
                            //Because of the fact that array is a collection of references,
                            //when I change,clear,sort it affects its refence copies
                            //so that I created another list copy with a different reference
                            long[] tempListCopy = new long[tempList.Count];
                            tempList.CopyTo(tempListCopy);//copying values to newly created list
                            tempListSet.Add(tempListCopy);//add an array to list
                            break;
                        }

                        while (count>=0)// count starts from current location of i which follows [i,maxSum]
                        {
                            //check upper location, if it's true then count--
                            if (count!=0 && controlBoolArray[count - 1, j])
                                count--;
                            else if(count - 1 == 0 || controlBoolArray[count - 1 , j]==false)
                            {
                                //if process bumps into a false label or count==1 then insert the value
                                tempList.Add(valueSet[count]); 
                                break;
                            }

                        }
                    }
                }//if statement's false , break the traverse 
                else
                    break;
            }

            return tempListSet;
        }

        //this method creates a bool array, which is the essence of table technique
        private bool[,] Bool2DArrray(long maxSum, params long[] valueSet)
        {
            Array.Sort(valueSet);//system only works with sorted sets

            bool[,] myBoolTable=new bool[valueSet.Length,maxSum+1];//all values are false by default
            for (int i = 0; i < valueSet.Length; i++)
                myBoolTable[i, 0] = true;//0.column is set to true

            for (int i = 0; i < valueSet.Length; i++)//traverse whole table
            {
                for (int j = 1; j < maxSum + 1; j++)
                {
                    if (i == 0)
                    {
                        if (j == valueSet[0])
                            myBoolTable[0, j] = true;
                    }
                    else
                    {
                        if (j < valueSet[i])
                        {
                            myBoolTable[i, j] = myBoolTable[i - 1, j];
                        }
                        else if(j==valueSet[i])
                        {
                            myBoolTable[i, j] = true;
                        }
                        else
                        {
                            if (myBoolTable[i-1, j])
                                myBoolTable[i, j] = true;
                            else
                            {
                                if (myBoolTable[i - 1, j - valueSet[i]])
                                    myBoolTable[i, j] = true;
                                else
                                    myBoolTable[i, j] = false;
                            }
                        }
                    }

                }
            }

            return myBoolTable;
        }
    }
}
