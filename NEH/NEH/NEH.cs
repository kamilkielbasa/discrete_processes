using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEH
{
    public class NEH
    {
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T tmp = lhs;
            lhs = rhs;
            rhs = tmp;
        }

        private static void SwapJobs(ref Job[] jobs, int firstIdex, int secondIndex)
        {
            Swap(ref jobs[firstIdex], ref jobs[secondIndex]);
        }

        private static void JobMemoryMove(Job[] jobs)
        {
            for (int i = jobs.Length - 1; i >= 1; --i)
            {
                jobs[i] = jobs[i - 1];
            }

            jobs[0].jobId = 0;
            jobs[0].executionTime = 0;
        }

        private static int FindMaxWeight(Weight[] weights)
        {
            int index = 0;
            Weight output = weights[index];

            for (int i = 1; i < weights.Length; ++i)
            {
                if (weights[i].value > output.value)
                {
                    index = i;
                    output = weights[i];
                }
            }

            weights[index].value = 0;
            return output.jobId;
        }

        public static Job[] NEHAlgorithm(List<Machine> listOfMachines)
        {
            int numberOfJobs = listOfMachines.First().jobs.Length;
            int numberOfMachines = listOfMachines.Count();

            Weight[] weights = new Weight[numberOfJobs];
            Job[] outputJobs = new Job[numberOfJobs];

            // sumowanie wag
            for (int i = 0; i < numberOfJobs; i++)
            {
                for (int j = 0; j < numberOfMachines; j++)
                {
                    weights[i].value += listOfMachines[j].jobs[i].executionTime;
                    weights[i].jobId = listOfMachines[j].jobs[i].jobId;
                }
            }

            // ułożenie wag od max => min
            Array.Sort(weights, (x, y) => x.value.CompareTo(y.value));
            Array.Reverse(weights);

            // zmienne pomocnicze
            int insertedJobsToOutput = 0;
            Job[] theBestFoundJobs = new Job[numberOfJobs];

            // włożenie pierwszej wagi
            outputJobs[0].jobId = FindMaxWeight(weights);
            ++insertedJobsToOutput;

            for (int i = 1; i < numberOfJobs; ++i)
            {
                JobMemoryMove(outputJobs); // gucci
                outputJobs[0].jobId = FindMaxWeight(weights); // gucci

                Array.Copy(outputJobs, theBestFoundJobs, outputJobs.Length);
                decimal cmax = Machine.FindCmax2(listOfMachines, i + 1);

                for (int j = 0; j < insertedJobsToOutput; ++j)
                {
                    SwapJobs(ref outputJobs, j, j + 1);

                    decimal currentCmax = Machine.FindCmax2(listOfMachines, i + 1);

                    if (currentCmax < cmax)
                    {
                        cmax = currentCmax;
                        Array.Copy(outputJobs, theBestFoundJobs, outputJobs.Length);
                    }

                    // ostatnia iteracja petli
                    if (j == insertedJobsToOutput - 1)
                    {
                        Array.Copy(theBestFoundJobs, outputJobs, outputJobs.Length);
                    }
                }

                ++insertedJobsToOutput;
            }

            return outputJobs;
        }
    }
}
