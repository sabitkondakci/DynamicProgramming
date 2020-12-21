using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheoryInDetail
{
    /*
 * An implementation of the traveling salesman problem in C# using dynamic programming to improve
 * the time complexity from O(n!) to O(n^2 * 2^n).
 *
 * Time Complexity: O(n^2 * 2^n) Space Complexity: O(n * 2^n)
 *
 * The main source code in Java belongs to William Fiset
 * Repository Origin :https://github.com/williamfiset/Algorithms/blob/master/src/main/java/com/williamfiset/algorithms/graphtheory/TspDynamicProgrammingIterative.java
 */
    public class TravelingSalesmanProblem
    {
        private int N, start;
        private double[,] distance;
        private List<int> tour;
        private double minTourCost;
        private bool ranSolver;

        public TravelingSalesmanProblem(int start, double[,] distance)
        {
            N = distance.GetLength(0);

            if (N <= 2) throw new IndexOutOfRangeException("N <= 2 not yet supported.");
            if (N != distance.GetLength(0)) throw new InvalidOperationException("Matrix must be square (n x n)");
            if (start < 0 || start >= N) throw new IndexOutOfRangeException("Invalid start node.");
            if (N > 32)
                throw new InvalidOperationException(
                    "Matrix too large! A matrix that size for the DP TSP problem with a time complexity of"
                    + "O(n^2*2^n) requires way too much computation for any modern home computer to handle");

            this.start = start;
            this.distance = distance;
            this.tour = new List<int>();
            this.minTourCost = double.PositiveInfinity;
            this.ranSolver = false;
        }

        // Returns the optimal tour for the traveling salesman problem.
        public List<int> GetTour()
        {
            if (!ranSolver)
                Solve();

            return tour;
        }

        // Returns the minimal tour cost.
        public double GetTourCost()
        {
            if (!ranSolver)
                Solve();

            return minTourCost;
        }

        // Solves the traveling salesman problem and caches solution.
        public void Solve()
        {

            if (ranSolver) return;

            int END_STATE = (1 << N) - 1;
            double[,] memo = new double[N, 1 << N];

            // Add all outgoing edges from the starting node to memo table.
            for (int end = 0; end < N; end++)
            {
                if (end == start)
                    continue;

                memo[end, (1 << start) | (1 << end)] = distance[start, end];
            }

            for (int r = 3; r <= N; r++)
            {
                foreach (int subset in Combinations(r, N))
                {
                    if (NotIn(start, subset)) continue;
                    for (int next = 0; next < N; next++)
                    {
                        if (next == start || NotIn(next, subset)) continue;
                        int subsetWithoutNext = subset ^ (1 << next);
                        double minDist = double.PositiveInfinity;

                        for (int end = 0; end < N; end++)
                        {
                            if (end == start || end == next || NotIn(end, subset))
                                continue;

                            double newDistance = memo[end, subsetWithoutNext] + distance[end, next];
                            if (newDistance < minDist)
                            {
                                minDist = newDistance;
                            }
                        }

                        memo[next, subset] = minDist;
                    }
                }
            }

            // Connect tour back to starting node and minimize cost.
            for (int i = 0; i < N; i++)
            {
                if (i == start)
                    continue;

                double tourCost = memo[i, END_STATE] + distance[i, start];
                if (tourCost < minTourCost)
                {
                    minTourCost = tourCost;
                }
            }

            int lastIndex = start;
            int state = END_STATE;
            tour.Add(start);

            // Reconstruct TSP path from memo table.
            for (int i = 1; i < N; i++)
            {

                int bestIndex = -1;
                double bestDist = double.PositiveInfinity;
                for (int j = 0; j < N; j++)
                {
                    if (j == start || NotIn(j, state))
                        continue;

                    double newDist = memo[j, state] + distance[j, lastIndex];
                    if (newDist < bestDist)
                    {
                        bestIndex = j;
                        bestDist = newDist;
                    }
                }

                tour.Add(bestIndex);
                state = state ^ (1 << bestIndex);
                lastIndex = bestIndex;
            }

            tour.Add(start);
            tour.Reverse();

            ranSolver = true;
        }

        private bool NotIn(int elem, int subset)
        {
            return ((1 << elem) & subset) == 0;
        }

        // This method generates all bit sets of size n where r bits
        // are set to one. The result is returned as a list of integer masks.
        public List<int> Combinations(int r, int n)
        {
            List<int> subsets = new List<int>();
            Combinations(0, 0, r, n, subsets);
            return subsets;
        }

        // To find all the combinations of size r we need to recurse until we have
        // selected r elements (aka r = 0), otherwise if r != 0 then we still need to select
        // an element which is found after the position of our last selected element
        private void Combinations(int set, int at, int r, int n, List<int> subsets)
        {

            // Return early if there are more elements left to select than what is available.
            int elementsLeftToPick = n - at;
            if (elementsLeftToPick < r) return;

            // We selected 'r' elements so we found a valid subset!
            if (r == 0)
            {
                subsets.Add(set);
            }
            else
            {
                for (int i = at; i < n; i++)
                {
                    // Try including this element
                    set ^= (1 << i);

                    Combinations(set, i + 1, r - 1, n, subsets);

                    // Backtrack and try the instance where we did not include this element
                    set ^= (1 << i);
                }
            }
        }
    }

}
