﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEH
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
                this.jobs = new Job[numberOfJobs];
            }
            catch (Exception ex)
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

        public static decimal FindCmax2(List<Machine> listOfMachines, int numberOfCalcJobs)
        {
            int numberOfMachines = listOfMachines.Count();
            int numberOfJobs = numberOfCalcJobs;

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
    }
}
