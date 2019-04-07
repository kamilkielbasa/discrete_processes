using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatedAnnealing
{
    class Program
    {
        static void Main(string[] args)
        {
            const int maxIteration = 51;

            string testPath = @"C:\Users\kielbkam\Desktop\Programming Platforms\discrete_processes\SimulatedAnnealing\TestData.txt";
            string logPath = @"C:\Users\kielbkam\Desktop\Programming Platforms\discrete_processes\SimulatedAnnealing\Log.txt";

            File.WriteAllText(logPath, String.Empty);

            for (int currentIteration = 50; currentIteration < maxIteration; ++currentIteration)
            {
                int numberOfMachines = 0;
                int numberOfJobs = 0;

                List<Machine> listOfMachines = new List<Machine>();

                string line1;
                StreamReader file1 = new StreamReader(testPath);
                while ((line1 = file1.ReadLine()) != null)
                {
                    string keyword = String.Empty;

                    if (currentIteration > 9)
                    {
                        keyword = "0" + currentIteration;
                    }
                    else
                    {
                        keyword = "00" + currentIteration;
                    }

                    if (line1 == ("data." + keyword + ":"))
                    {
                        line1 = file1.ReadLine();

                        string[] substrings = line1.Split(null);

                        numberOfJobs = int.Parse(substrings[0]);
                        numberOfMachines = int.Parse(substrings[1]);

                        for (int i = 0; i < numberOfMachines; ++i)
                        {
                            listOfMachines.Add(new Machine(numberOfJobs));
                        }

                        for (int jobIdx = 0; jobIdx < numberOfJobs; ++jobIdx)
                        {
                            line1 = file1.ReadLine();
                            substrings = line1.Split(null);

                            for (int machineIdx = 0; machineIdx < numberOfMachines; ++machineIdx)
                            {
                                listOfMachines[machineIdx].jobs[jobIdx].executionTime = decimal.Parse(substrings[machineIdx]);
                                listOfMachines[machineIdx].jobs[jobIdx].jobId = jobIdx;
                            }
                        }

                        break;
                    }
                }

                // time measurement
                Stopwatch s1 = new Stopwatch();

                s1.Start();
                List<Machine> result = Annealing.StartAnnealing(listOfMachines);
                s1.Stop();

                Console.WriteLine("Time elapsed: {0}", s1.Elapsed);

                string sequence = string.Empty;

                for (int i = 0; i < result.First().jobs.Length; ++i)
                {
                    sequence += result[0].jobs[i].jobId + " ";
                }

                decimal cmax = Machine.FindCmax(result);

                string line = "Test data: " + "data." + currentIteration + " -----ANNAELING ALGORITHM------" + Environment.NewLine +
                              "Iteration:" + currentIteration.ToString() +
                              " Sequence:" + sequence +
                              " Cmax:" + cmax.ToString() +
                              " Time:" + s1.Elapsed.ToString();

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(logPath, true))
                {
                    file.WriteLine(line);
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(logPath, true))
                {
                    file.WriteLine(Environment.NewLine);
                }
            }
        }
    }
}
