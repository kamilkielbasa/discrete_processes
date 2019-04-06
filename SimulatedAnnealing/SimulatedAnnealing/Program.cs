using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatedAnnealing
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Machine> listOfMachines = new List<Machine>();

            for (int i = 0; i < 3; ++i)
                listOfMachines.Add(new Machine(4));

            listOfMachines[0].jobs[0].executionTime = 1;
            listOfMachines[0].jobs[1].executionTime = 4;
            listOfMachines[0].jobs[2].executionTime = 3;
            listOfMachines[0].jobs[3].executionTime = 2;

            listOfMachines[1].jobs[0].executionTime = 1;
            listOfMachines[1].jobs[1].executionTime = 1;
            listOfMachines[1].jobs[2].executionTime = 4;
            listOfMachines[1].jobs[3].executionTime = 4;

            listOfMachines[2].jobs[0].executionTime = 3;
            listOfMachines[2].jobs[1].executionTime = 2;
            listOfMachines[2].jobs[2].executionTime = 3;
            listOfMachines[2].jobs[3].executionTime = 1;

            for (int i = 0; i < listOfMachines.Count(); ++i)
            {
                for (int j = 0; j < listOfMachines.First().jobs.Length; ++j)
                {
                    listOfMachines[i].jobs[j].jobId = j + 1;
                }
            }

            Annealing.StartAnnealing(listOfMachines);
            
            Console.ReadKey();
        }
    }
}
