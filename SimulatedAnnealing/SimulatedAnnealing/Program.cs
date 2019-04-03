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

            for (int i = 0; i < 4; ++i)
                listOfMachines.Add(new Machine(i + 1, 3));

            listOfMachines[3].jobs[0].executionTime = 1;
            listOfMachines[3].jobs[1].executionTime = 1;
            listOfMachines[3].jobs[2].executionTime = 3;

            listOfMachines[2].jobs[0].executionTime = 3;
            listOfMachines[2].jobs[1].executionTime = 4;
            listOfMachines[2].jobs[2].executionTime = 3;

            listOfMachines[0].jobs[0].executionTime = 2;
            listOfMachines[0].jobs[1].executionTime = 4;
            listOfMachines[0].jobs[2].executionTime = 1;

            listOfMachines[1].jobs[0].executionTime = 4;
            listOfMachines[1].jobs[1].executionTime = 1;
            listOfMachines[1].jobs[2].executionTime = 2;

            Annealing.StartAnnealing(listOfMachines);
            
            Console.ReadKey();
        }
    }
}
