        public static int[] FrequencyOfLetters(StringBuilder stringBuilder)
        {
            string text = stringBuilder.ToString();
            int[] result =
                text.AsParallel().Aggregate(
                    () => new int[26],       // new localFrequencies      

                    (localFrequencies, c) =>       
                    {
                        int index = char.ToUpper(c) - 'A';
                        if (index >= 0 && index <= 26) localFrequencies[index]++;
                        return localFrequencies;
                    },
                   
                    (mainFreq, localFreq) =>
                        mainFreq.Zip(localFreq, (f1, f2) => f1 + f2).ToArray(),

                    finalResult => finalResult     
                ); 
            return result;
        }
