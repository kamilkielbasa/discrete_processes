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

        public static decimal FindCmax(List<Machine> machineList, Job[] jobs, int numberOfCalcJobs)
        {
            decimal cmax = 0;

            if (numberOfCalcJobs == 1)
            {
                int searchIndex = 0;

                for (int i = 0; i < machineList.Count(); ++i)
                {
                    for (int j = 0; j < jobs.Length; ++j)
                    {
                        if (jobs[0].jobId == machineList[i].jobs[j].jobId)
                        {
                            searchIndex = j;
                            break;
                        }
                    }
                }

                for (int i = 0; i < machineList.Count(); ++i)
                {
                    cmax += machineList[i].jobs[searchIndex].executionTime;
                }
            }
            else
            {
                Job[,] machines = new Job[machineList.Count(), jobs.Length];

                for (int i = 0; i < jobs.Length; ++i)
                {
                    for (int j = 0; j < machineList.Count(); ++j)
                    {
                        for (int k = 0; k < jobs.Length; ++k)
                        {
                            if (jobs[i].jobId == machineList[j].jobs[k].jobId)
                            {
                                machines[j, i].executionTime = machineList[j].jobs[k].executionTime;
                                machines[j, i].jobId = jobs[i].jobId;
                            }
                        }
                    }
                }

                for (int i = 0; i < numberOfCalcJobs; ++i)
                {
                    cmax += machines[0, i].executionTime;
                }

                for (int i = 1; i < machineList.Count(); ++i)
                {
                    cmax += machines[i, numberOfCalcJobs - 1].executionTime;
                }
            }

            return cmax;
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
                decimal cmax = FindCmax(listOfMachines, outputJobs, i + 1);

                for (int j = 0; j < insertedJobsToOutput; ++j)
                {
                    SwapJobs(ref outputJobs, j, j + 1);

                    decimal currentCmax = FindCmax(listOfMachines, outputJobs, i + 1);

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
