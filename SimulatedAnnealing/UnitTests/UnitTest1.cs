using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulatedAnnealing;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Przykład Doktora Makuchowskiego z prezentacji.
            // Testowany jest FindCmax w formie przeszukania grafu.
   
            List<Machine> listOfMachines = new List<Machine>();

            for (int i = 0; i < 4; ++i)
                listOfMachines.Add(new Machine(i + 1, 3));

            listOfMachines[0].jobs[0].executionTime = 1;
            listOfMachines[0].jobs[1].executionTime = 1;
            listOfMachines[0].jobs[2].executionTime = 3;

            listOfMachines[1].jobs[0].executionTime = 3;
            listOfMachines[1].jobs[1].executionTime = 4;
            listOfMachines[1].jobs[2].executionTime = 3;

            listOfMachines[2].jobs[0].executionTime = 4;
            listOfMachines[2].jobs[1].executionTime = 1;
            listOfMachines[2].jobs[2].executionTime = 2;

            listOfMachines[3].jobs[0].executionTime = 2;
            listOfMachines[3].jobs[1].executionTime = 4;
            listOfMachines[3].jobs[2].executionTime = 1;

            Assert.AreEqual(15, Machine.FindCmax(listOfMachines));
        }

        [TestMethod]
        public void TestMethod2()
        {
            List<Machine> listOfMachines = new List<Machine>();

            for (int i = 0; i < 10; ++i)
                listOfMachines.Add(new Machine(i + 1, 3));

            for (int i = 0; i < listOfMachines.Count(); ++i)
            {
                for (int j = 0; j < listOfMachines[0].jobs.Length; ++j)
                {
                    listOfMachines[i].jobs[j].jobId = j;
                    listOfMachines[i].jobs[j].executionTime = (i + j) * 100;
                }
            }

            List<Machine> nextListOfMachines = new List<Machine>();
            Assert.AreEqual(0, nextListOfMachines.Count());

            List<Machine> copyListOfMachines = listOfMachines.ToList();

            for (int i = 0; i < listOfMachines.Count(); ++i)
                Assert.AreEqual(copyListOfMachines[i], listOfMachines[i]);

            Annealing.computeNext(listOfMachines, nextListOfMachines);

            Assert.AreEqual(listOfMachines.Count(), nextListOfMachines.Count());
        }
    }
}
