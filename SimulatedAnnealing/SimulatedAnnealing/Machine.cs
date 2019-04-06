using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatedAnnealing
{
    public struct FinishTime
    {
        public decimal executionTime;
        public decimal summaryTime;
    }

    public class Machine
    {
        public Job[] jobs { get; set; }

        public Machine(int numberOfJobs)
        {
            try
            {
                jobs = new Job[numberOfJobs];
            }
            catch(Exception ex)
            {
                Console.WriteLine("Generic exception catched: {0}", ex.ToString());
            }
        }

        public static decimal FindCmax(List<Machine> listOfMachines)
        {
            int numberOfMachines = listOfMachines.Count();
            int numberOfJobs = listOfMachines.First().jobs.Length;

            FinishTime[,] finishTime = new FinishTime[numberOfMachines, numberOfJobs];

            // wpisanie czasu wykonań każdej z prac.
            for (int i = 0; i < numberOfMachines; ++i)
            {
                for (int j = 0; j < numberOfJobs; ++j)
                {
                    finishTime[i, j].executionTime = listOfMachines[i].jobs[j].executionTime;
                }
            }

            // czas wykonania pierwszej maszyny, pierwszej pracy jest czasem łączenego wykonania
            finishTime[0, 0].summaryTime = finishTime[0, 0].executionTime;

            // liczymy czas łączny dla pierwszej maszyny potrzebny do daleszego porównania.
            for (int i = 1; i < numberOfJobs; ++i)
                finishTime[0, i].summaryTime = finishTime[0, i].executionTime + finishTime[0, i - 1].summaryTime;

            for (int currMachIdx = 1; currMachIdx < numberOfMachines; ++currMachIdx)
            {
                finishTime[currMachIdx, 0].summaryTime = finishTime[currMachIdx, 0].executionTime + finishTime[currMachIdx - 1, 0].summaryTime;

                for (int currJobIdx = 1; currJobIdx < numberOfJobs; ++currJobIdx)
                {
                    if (finishTime[currMachIdx - 1, currJobIdx].summaryTime > finishTime[currMachIdx, currJobIdx - 1].summaryTime)
                    {
                        finishTime[currMachIdx, currJobIdx].summaryTime =
                          finishTime[currMachIdx, currJobIdx].executionTime +
                          finishTime[currMachIdx - 1, currJobIdx].summaryTime;
                    }
                    else
                    {
                        finishTime[currMachIdx, currJobIdx].summaryTime =
                          finishTime[currMachIdx, currJobIdx].executionTime +
                          finishTime[currMachIdx, currJobIdx - 1].summaryTime;
                    }
                }
            }

            return finishTime[numberOfMachines - 1, numberOfJobs - 1].summaryTime;
        }

        private static void Swap<T>(ref T lhs, ref T rhs)
        {
            T tmp = lhs;
            lhs = rhs;
            rhs = tmp;
        }

        public static void SwapJobs(List<Machine> listOfMachines, int firstJobIdx, int secondJobIdx)
        {
            foreach (Machine machine in listOfMachines)
            {
                Swap(ref machine.jobs[firstJobIdx], ref machine.jobs[secondJobIdx]);
            }
        }

        public static void InsertJob(List<Machine> listOfMachines, int srcJobIdx, int dstJobIdx)
        {
            int numberOfMachines = listOfMachines.Count();
            int numberOfJobs = listOfMachines.First().jobs.Length;

            List<List<Job>> list = new List<List<Job>>();

            for (int i = 0; i < numberOfMachines; ++i)
            {
                list.Add(new List<Job>());

                for (int j = 0; j < numberOfJobs; ++j)
                {
                    if (j != srcJobIdx)
                    {
                        list[i].Add(listOfMachines[i].jobs[j]);
                    }
                }
            }

            for (int i = 0; i < numberOfMachines; ++i)
            {
                for (int j = 0; j < numberOfJobs; ++j)
                {
                    if (j == dstJobIdx)
                    {
                        list[i].Insert(j, listOfMachines[i].jobs[srcJobIdx]);
                    }
                }
            }

            listOfMachines.Clear();

            for (int i = 0; i < numberOfMachines; ++i)
            {
                listOfMachines.Add(new Machine(numberOfJobs));

                for (int j = 0; j < numberOfJobs; ++j)
                {
                    listOfMachines[i].jobs[j] = list[i][j];
                }
            }
        }
    }
}
