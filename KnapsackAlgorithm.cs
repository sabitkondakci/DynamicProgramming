using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgrammingAlgorithms
{
    class KnapsackAlgorithm
    {
        // Formulation V[i,w] = Max{ V[ i-1 , w] , V[ i-1 , w - W[i] ] + P[i] }
        // W[] {} weight matrix and P[] {} profit matrix
        // Linearly it has O(2^n) time complexity , with table method it has  O(n*m) time and space complexity
        // This is the algorithm for undivisable objects, if you pick a phone as an example you can't split it physically

        /// <summary>
        /// Method returns a  List&lt;KeyValuePair&lt;int,double&gt;&gt;&gt; , which contains weight,profit data.
        /// </summary>
        /// <param name="m">Maximum weight to be desired </param>
        /// <param name="profitWeight"> List&lt;KeyValuePair&lt;int,double&gt;&gt;&gt; a List which contains weight,profit pair data </param>
        /// <returns></returns>
        public List<KeyValuePair<int, double>> ZeroOneKnapsacMethod(int m, List<KeyValuePair<int,double>> profitWeight)
        {
            //For the sake of algorithmic convenience a weight-profit of [0 , 0] object is inserted into profitWeight list.
            profitWeight.Insert(0,new KeyValuePair<int, double>(0,0));
            List<KeyValuePair<int, double>> tempList=new List<KeyValuePair<int, double>>(); // A temporaryList in which proper values are inserted
            int n = profitWeight.Count(); //row
            double[,] valMatrix=new double[n,m+1];//the table [nxm]
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= m; j++)
                {
                    if (i==0||j==0)
                    {
                        valMatrix[i,j] = 0;
                    }
                    else if(profitWeight[i].Key<=j)
                    {
                        valMatrix[i, j] =
                            Math.Max(profitWeight[i].Value + valMatrix[i - 1, j - profitWeight[i].Key],valMatrix[i-1,j]);
                    }
                    else
                    {
                        valMatrix[i, j] = valMatrix[i - 1, j];
                    }
                }
            }

            int k = n-1, l = m; // k is "row" and l is "column"

            while (k>0 && l>0)
            {
                if (valMatrix[k, l] == valMatrix[k - 1, l] ) //Check the object above current index, if the both are same , move up.
                {
                    k--;
                }
                else
                {
                    tempList.Add(profitWeight[k]);
                    l = l - profitWeight[k].Key; //Go to the remaining weight's column index
                    k--; // move up on the table
                }
            }
            
            return tempList;
        }

        public List<KeyValuePair<double, double>> KnapsackGreedyMethod(double m,List<KeyValuePair<double, double>> profitWeight)
        {
            List<KeyValuePair<double, double>> tempList = new List<KeyValuePair<double, double>>();
            List<KeyValuePair<double, double>> myContainer = profitWeight.OrderByDescending(x => x.Value / x.Key).ToList();

            foreach (var keyValuePair in myContainer)
            {
                if (m>=keyValuePair.Key)
                {
                    tempList.Add(keyValuePair);
                    m -= keyValuePair.Key;
                }
                else
                {
                    tempList.Add(new KeyValuePair<double, double>(m,(keyValuePair.Value)*(m/keyValuePair.Key)));
                    break;
                }
            }

            return tempList;
        }
    }
}
