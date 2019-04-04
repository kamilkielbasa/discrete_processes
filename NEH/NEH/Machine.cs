using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEH
{
    public class Machine
    {
        public int machineId { get; set; }
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

        public Machine(int machineId, int numberOfJobs)
        {
            try
            {
                this.machineId = machineId;
                this.jobs = new Job[numberOfJobs];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic exception catched: {0}", ex.ToString());
            }
        }

        public static decimal FindCmax(List<Machine> listOfMachines)
        {
            decimal cmax = 0;

            for (int i = 0; i < listOfMachines.Count(); ++i)
                cmax += listOfMachines[i].jobs[0].executionTime;

            for (int i = 1; i < listOfMachines[0].jobs.Length; ++i)
                cmax += listOfMachines[listOfMachines.Count() - 1].jobs[i].executionTime;

            return cmax;
        }
    }
}
