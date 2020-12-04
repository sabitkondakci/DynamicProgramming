using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgrammingAlgorithms
{
    class RabinKarpStringMatchingHashTechnique
    {
        // the method made use of hashing technique
        public bool RabinCarpMatchingCheck(string pattern, StringBuilder sampleString, bool caseInsensitive = false)
        {
            // if case insensitiveness is desired or not
            if (caseInsensitive)
            {
                pattern = pattern.ToLower();
                sampleString = sampleString.Replace(sampleString.ToString(), sampleString.ToString().ToLower());
            }

            int N = pattern.Length;
            //traverse the sampleString
            for (int i = 0; i < sampleString.Length; i++)
            {
                //if i+N reaches the max limit then break the loop, it simlpy means there is no match
                if (i + N > sampleString.Length)
                    break;
                //compare hash codes of substrings which are size of N 
                if (pattern.GetHashCode() == sampleString.ToString().Substring(i, N).GetHashCode())
                    return true;
            }

            return false;
        }
    }
}
