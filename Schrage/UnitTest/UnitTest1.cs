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
        private const string pathToDate =
            @"C:\Users\kielbkam\Desktop\Programming Platforms\discrete_processes\Schrage\Schrage\TestDate\";

        [TestMethod]
        public void TestMethodForSchrage50RPQ()
        {
            string pathToDate50RPQ = pathToDate + "in50.txt";

            FileReader fr = new FileReader(pathToDate50RPQ);

            List<RPQ> listOfRPQ = fr.Execute();

            Tuple<RPQ[], int> output = SchrageAlgorithm.Execute(listOfRPQ);

            RPQ[] listOfSortedRPQ = output.Item1;
            int cmax = output.Item2;

            Assert.AreEqual(1513, cmax);
        }

        [TestMethod]
        public void TestMethodForSchrage100RPQ()
        {
            string pathToDate100RPQ = pathToDate + "in100.txt";

            FileReader fr = new FileReader(pathToDate100RPQ);

            List<RPQ> listOfRPQ = fr.Execute();

            Tuple<RPQ[], int> output = SchrageAlgorithm.Execute(listOfRPQ);

            RPQ[] listOfSortedRPQ = output.Item1;
            int cmax = output.Item2;

            Assert.AreEqual(3076, cmax);
        }

        [TestMethod]
        public void TestMethodForSchrage200RPQ()
        {
            string pathToDate200RPQ = pathToDate + "in200.txt";

            FileReader fr = new FileReader(pathToDate200RPQ);

            List<RPQ> listOfRPQ = fr.Execute();

            Tuple<RPQ[], int> output = SchrageAlgorithm.Execute(listOfRPQ);

            RPQ[] listOfSortedRPQ = output.Item1;
            int cmax = output.Item2;

            Assert.AreEqual(6416, cmax);
        }

        [TestMethod]
        public void TestMethodForSchragePmtn50RPQ()
        {
            string pathToDate50RPQ = pathToDate + "in50.txt";

            FileReader fr = new FileReader(pathToDate50RPQ);

            List<RPQ> listOfRPQ = fr.Execute();

            int cmax = SchragePmtnAlgorithm.Execute(listOfRPQ);

            Assert.AreEqual(1492, cmax);
        }

        [TestMethod]
        public void TestMethodForSchragePmtn100RPQ()
        {
            string pathToDate100RPQ = pathToDate + "in100.txt";

            FileReader fr = new FileReader(pathToDate100RPQ);

            List<RPQ> listOfRPQ = fr.Execute();

            int cmax = SchragePmtnAlgorithm.Execute(listOfRPQ);

            Assert.AreEqual(3070, cmax);
        }

        [TestMethod]
        public void TestMethodForSchragePmtn200RPQ()
        {
            string pathToDate200RPQ = pathToDate + "in200.txt";

            FileReader fr = new FileReader(pathToDate200RPQ);

            List<RPQ> listOfRPQ = fr.Execute();

            int cmax = SchragePmtnAlgorithm.Execute(listOfRPQ);

            Assert.AreEqual(6398, cmax);
        }
    }
}
