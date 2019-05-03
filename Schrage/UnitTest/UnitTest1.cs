using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schrage;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethodFor50RPQ()
        {
            string pathToTestData =
              @"C:\Users\kielbkam\Desktop\Programming Platforms\discrete_processes\Schrage\Schrage\TestDate\in50.txt";

            FileReader fr = new FileReader(pathToTestData);

            List<RPQ> listOfRPQ = fr.Execute();

            Tuple<RPQ[], int> output = SchrageAlgorithm.Execute(listOfRPQ);

            RPQ[] listOfSortedRPQ = output.Item1;
            int cmax = output.Item2;

            Assert.AreEqual(1513, cmax);
        }

        [TestMethod]
        public void TestMethodFor100RPQ()
        {
            string pathToTestData =
              @"C:\Users\kielbkam\Desktop\Programming Platforms\discrete_processes\Schrage\Schrage\TestDate\in100.txt";

            FileReader fr = new FileReader(pathToTestData);

            List<RPQ> listOfRPQ = fr.Execute();

            Tuple<RPQ[], int> output = SchrageAlgorithm.Execute(listOfRPQ);

            RPQ[] listOfSortedRPQ = output.Item1;
            int cmax = output.Item2;

            Assert.AreEqual(3076, cmax);
        }

        [TestMethod]
        public void TestMethodFor200RPQ()
        {
            string pathToTestData =
              @"C:\Users\kielbkam\Desktop\Programming Platforms\discrete_processes\Schrage\Schrage\TestDate\in200.txt";

            FileReader fr = new FileReader(pathToTestData);

            List<RPQ> listOfRPQ = fr.Execute();

            Tuple<RPQ[], int> output = SchrageAlgorithm.Execute(listOfRPQ);

            RPQ[] listOfSortedRPQ = output.Item1;
            int cmax = output.Item2;

            Assert.AreEqual(6416, cmax);
        }
    }
}
