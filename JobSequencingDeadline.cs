using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgrammingAlgorithms
{
    class JobSequencingDeadline
    {
        //Accepting all the jobs have 1 unit of work to finish
        //key corresponds to deadline and value to profit
        public KeyValuePair<KeyValuePair<string, string>, double>[] JobSequencingWithDeadlines(
            List<KeyValuePair<DateTime, double>> jobListWithDeadlines)
        {
            //Jobs and their profit enlisted in descending order
            List<KeyValuePair<DateTime, double>> tempList = jobListWithDeadlines.OrderByDescending(x => x.Value).ToList();
            //Distinct datetime values are filtered
            List<DateTime> deadLineList = jobListWithDeadlines.Select(x => x.Key.Date).Distinct().ToList();
            deadLineList.Insert(0, DateTime.UtcNow.Date);// today's date is added
            deadLineList.Sort(); // Datetime list sorted, values will be inserted according to this order

            int count = 0;
            int sizeOfArray = deadLineList.Count - 1;
            int repeat = tempList.Count() - 1;
            //Array for keeping desired values
            KeyValuePair<KeyValuePair<string, string>, double>[] deadLineSequence =
                new KeyValuePair<KeyValuePair<string, string>, double>[sizeOfArray];

            for (int i = 0; i < repeat; i++)
            {
                for (int k = deadLineList.IndexOf(tempList[i].Key.Date); k > 0; k--)
                {
                    if (deadLineSequence[k - 1].Key.Value == null)
                    {
                        deadLineSequence[k - 1] = new KeyValuePair<KeyValuePair<string, string>, double>(
                            new KeyValuePair<string, string>(deadLineList[k - 1].ToString("d")
                                , deadLineList[k].ToString("d")), tempList[i].Value);
                        count++;
                        break;
                    }
                }

                if (count + 1 == deadLineList.Count())
                    break;
            }

            return deadLineSequence;
        }
    }
}
