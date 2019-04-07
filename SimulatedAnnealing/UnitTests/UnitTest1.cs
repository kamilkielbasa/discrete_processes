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

            for (int i = 0; i < 3; ++i)
                listOfMachines.Add(new Machine(4));

            listOfMachines[0].jobs[0].executionTime = 1;
            listOfMachines[0].jobs[1].executionTime = 3;
            listOfMachines[0].jobs[2].executionTime = 2;
            listOfMachines[0].jobs[3].executionTime = 4;

            listOfMachines[1].jobs[0].executionTime = 1;
            listOfMachines[1].jobs[1].executionTime = 4;
            listOfMachines[1].jobs[2].executionTime = 4;
            listOfMachines[1].jobs[3].executionTime = 1;

            listOfMachines[2].jobs[0].executionTime = 3;
            listOfMachines[2].jobs[1].executionTime = 3;
            listOfMachines[2].jobs[2].executionTime = 1;
            listOfMachines[2].jobs[3].executionTime = 2;

            Assert.AreEqual(15, Machine.FindCmax(listOfMachines));

            listOfMachines.Clear();

            for (int i = 0; i < 3; ++i)
                listOfMachines.Add(new Machine(4));

            listOfMachines[0].jobs[0].executionTime = 3;
            listOfMachines[0].jobs[1].executionTime = 2;
            listOfMachines[0].jobs[2].executionTime = 4;
            listOfMachines[0].jobs[3].executionTime = 1;

            listOfMachines[1].jobs[0].executionTime = 4;
            listOfMachines[1].jobs[1].executionTime = 4;
            listOfMachines[1].jobs[2].executionTime = 1;
            listOfMachines[1].jobs[3].executionTime = 1;

            listOfMachines[2].jobs[0].executionTime = 3;
            listOfMachines[2].jobs[1].executionTime = 1;
            listOfMachines[2].jobs[2].executionTime = 2;
            listOfMachines[2].jobs[3].executionTime = 3;

            Assert.AreEqual(17, Machine.FindCmax(listOfMachines));
        }

        [TestMethod]
        public void TestMethod2()
        {
            // Przykład Doktora Makuchowskiego z prezentacji.
            // Testowany jest FindCmax w formie przeszukania grafu.

            List<Machine> listOfMachines = new List<Machine>();

            for (int i = 0; i < 3; ++i)
                listOfMachines.Add(new Machine(4));

            listOfMachines[0].jobs[0].executionTime = 3;
            listOfMachines[0].jobs[1].executionTime = 2;
            listOfMachines[0].jobs[2].executionTime = 4;
            listOfMachines[0].jobs[3].executionTime = 1;

            listOfMachines[1].jobs[0].executionTime = 4;
            listOfMachines[1].jobs[1].executionTime = 4;
            listOfMachines[1].jobs[2].executionTime = 1;
            listOfMachines[1].jobs[3].executionTime = 1;

            listOfMachines[2].jobs[0].executionTime = 3;
            listOfMachines[2].jobs[1].executionTime = 1;
            listOfMachines[2].jobs[2].executionTime = 2;
            listOfMachines[2].jobs[3].executionTime = 3;

            Machine.SwapJobs(listOfMachines, 0, 1);

            Assert.AreEqual(2, listOfMachines[0].jobs[0].executionTime);
            Assert.AreEqual(3, listOfMachines[0].jobs[1].executionTime);

            Assert.AreEqual(1, listOfMachines[2].jobs[0].executionTime);
            Assert.AreEqual(3, listOfMachines[2].jobs[1].executionTime);

            Machine.SwapJobs(listOfMachines, 0, 1);
            Machine.SwapJobs(listOfMachines, 1, 2);

            Assert.AreEqual(4, listOfMachines[0].jobs[1].executionTime);
            Assert.AreEqual(2, listOfMachines[0].jobs[2].executionTime);

            Assert.AreEqual(1, listOfMachines[1].jobs[1].executionTime);
            Assert.AreEqual(4, listOfMachines[1].jobs[2].executionTime);

            Assert.AreEqual(2, listOfMachines[2].jobs[1].executionTime);
            Assert.AreEqual(1, listOfMachines[2].jobs[2].executionTime);
        }

        [TestMethod]
        public void TestMethod3()
        {
            List<Machine> listOfMachines = new List<Machine>();

            for (int i = 0; i < 10; ++i)
                listOfMachines.Add(new Machine(3));

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

            Annealing.computeNext(listOfMachines, nextListOfMachines, true);

            Assert.AreEqual(listOfMachines.Count(), nextListOfMachines.Count());
        }

        [TestMethod]
        public void TestMethod4()
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

            List<Machine> expectedListOfMachines = new List<Machine>();

            for (int i = 0; i < 3; ++i)
                expectedListOfMachines.Add(new Machine(4));

            expectedListOfMachines[0].jobs[0].executionTime = 4;
            expectedListOfMachines[0].jobs[0].jobId = 2;
            expectedListOfMachines[0].jobs[1].executionTime = 3;
            expectedListOfMachines[0].jobs[1].jobId = 3;
            expectedListOfMachines[0].jobs[2].executionTime = 1;
            expectedListOfMachines[0].jobs[2].jobId = 1;
            expectedListOfMachines[0].jobs[3].executionTime = 2;
            expectedListOfMachines[0].jobs[3].jobId = 4;

            expectedListOfMachines[1].jobs[0].executionTime = 1;
            expectedListOfMachines[1].jobs[0].jobId = 2;
            expectedListOfMachines[1].jobs[1].executionTime = 4;
            expectedListOfMachines[1].jobs[1].jobId = 3;
            expectedListOfMachines[1].jobs[2].executionTime = 1;
            expectedListOfMachines[1].jobs[2].jobId = 1;
            expectedListOfMachines[1].jobs[3].executionTime = 4;
            expectedListOfMachines[1].jobs[3].jobId = 4;

            expectedListOfMachines[2].jobs[0].executionTime = 2;
            expectedListOfMachines[2].jobs[0].jobId = 2;
            expectedListOfMachines[2].jobs[1].executionTime = 3;
            expectedListOfMachines[2].jobs[1].jobId = 3;
            expectedListOfMachines[2].jobs[2].executionTime = 3;
            expectedListOfMachines[2].jobs[2].jobId = 1;
            expectedListOfMachines[2].jobs[3].executionTime = 1;
            expectedListOfMachines[2].jobs[3].jobId = 4;

            Machine.InsertJob(listOfMachines, 0, 2);

            int numberOfMachines = listOfMachines.Count();
            int numberOfJobs = listOfMachines.First().jobs.Length;

            for (int i = 0; i < numberOfMachines; ++i)
            {
                for (int j = 0; j < numberOfJobs; ++j)
                {
                    Assert.AreEqual(listOfMachines[i].jobs[j], expectedListOfMachines[i].jobs[j]);
                }
            }
        }
    }
}
