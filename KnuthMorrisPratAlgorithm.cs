using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DynamicProgrammingAlgorithms
{
    class KnuthMorrisPratAlgorithm
    {
        //this is a form of pattern matching algorithm
        public bool StringPatternMatchController(string pattern, StringBuilder sampleString,bool caseInsensitive=false)
        {
            // if case insensitive search is desired or not
            if (caseInsensitive)
            {
                pattern = pattern.ToLower();
                sampleString = sampleString.Replace(sampleString.ToString(),sampleString.ToString().ToLower());
            }
            
            //a list to add and check chars 
            List<char> indexPatternChars = new List<char>();
            //for the sake of simplicity a word-space char is added at zero index
            indexPatternChars.Insert(0, ' ');

            //to prevent a constant index call from pattern[] , a patternChar variable is created
            char[] patternChar = pattern.ToCharArray();

            //char indexes which repeat itself at specific locations [ ,a,b,a,b,a,d] => IndexPatternChars
            //                                     storedIndexes =>  [0,0,0,1,2,3,0]
            int[] storedIndexes = new int[patternChar.Length+1];

            //a loop to create stored indexes and indexPatterChars
            for (int i = 0; i < patternChar.Length; i++)
            {
                if (indexPatternChars.Contains(patternChar[i]))
                {
                    // for a pattern of       [a,b,a,b,c]
                    // indexPatternChars => [ ,a,b,a,b,c]
                    // storedIndexes     => [0,0,0,1,2,0]
                    
                    int lastIndex = indexPatternChars.LastIndexOf(patternChar[i]);
                    //the key part! take the lastIndex of repating char into consideration.
                    storedIndexes[i+1] = lastIndex;
                }
                
                indexPatternChars.Add(patternChar[i]);
            }

            int k = 0, j = 0;

            //traverse whole sampleString 
            while (k<sampleString.Length)
            {
                //if chars are similar then j++ and k++
                if (sampleString[k] == indexPatternChars[j+1])
                {
                    //whether storedIndexes.Length or indexPatternChars.Count
                    //they're same
                    if (j + 1 == indexPatternChars.Count - 1)
                        return true;

                    j++; k++;
                }
                else
                {
                    do
                    {   // go to the index stored in storedIndexes and repeat till j > 0 condition is provided
                        j = storedIndexes[j];

                        //if a char matches, then j++ ,break the loop, k++
                        if (sampleString[k] == indexPatternChars[j+1])
                        {
                            j++;
                            break;
                        }
                        

                    } while ((sampleString[k] != indexPatternChars[j+1] && j > 0));
                    
                    k++;
                }
            }

            //if k reaches to end with no return true interruption, return false
            return false;
        }
    }
}
