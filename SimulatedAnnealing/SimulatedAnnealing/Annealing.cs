using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatedAnnealing
{
    public class Annealing
    {
        public static void computeNext(List<Machine> currentListOfMachines, List<Machine> newListOfMachines, bool swap)
        {
            newListOfMachines.Clear();

            foreach (var machine in currentListOfMachines)
                newListOfMachines.Add(machine);

            Random random = new Random();

            int randomIndex1 = random.Next(0, currentListOfMachines.Count());
            int randomIndex2 = random.Next(0, currentListOfMachines.Count());

            if (swap == true)
            {
                Machine.SwapJobs(newListOfMachines, randomIndex1, randomIndex2);
            }
            else
            {
                Machine.InsertJob(newListOfMachines, randomIndex1, randomIndex2);
            }
        }

        public static List<Machine> StartAnnealing(List<Machine> listOfMachines)
        {
            double proba;
            double alpha = 0.999;
            double temperature = 400.0;
            double epsilon = 0.001;
            double delta;
            int iteration = -1;
            List<Machine> next = new List<Machine>();
            double distance = (double)Machine.FindCmax(listOfMachines); // cmax

            string logPath = @"C:\Users\kielbkam\Desktop\Programming Platforms\discrete_processes\SimulatedAnnealing\Log.txt";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logPath, true))
            {
                file.WriteLine("alpha = {0}, temperature = {1}, epsilon = {2}", alpha, temperature, epsilon);
            }

            while (temperature > epsilon)
            {
                ++iteration;

                computeNext(listOfMachines, next, true);

                double tmp = (double)Machine.FindCmax(next);
                delta = tmp - distance;

                if (delta < 0)
                {
                    listOfMachines.Clear();
                    listOfMachines = next.ToList();
                    distance += delta;
                }
                else
                {
                    Random random = new Random();
                    proba = random.Next(0, 1);

                    if (Math.Exp((-delta) / temperature) >= proba)
                    {
                        listOfMachines.Clear();
                        listOfMachines = next.ToList();
                        distance += delta;
                    }
                }

                temperature *= alpha;

                //Console.WriteLine("calculating cmax = {0} ...", distance);
            }

            //for (int i = 0; i < listOfMachines.First().jobs.Length; ++i)
                //Console.Write("{0} ", listOfMachines[0].jobs[i].jobId);

            //Console.WriteLine("cmax = {0}", distance);

            return listOfMachines;
        }
    }
}
