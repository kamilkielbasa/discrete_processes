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
            @"C:\Users\pati\Desktop\discrete_processes\Schrage\Schrage\TestDate\";

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

        [TestMethod]
        public void TestMethodForCarlier50RPQ()
        {
            string pathToDate50RPQ = pathToDate + "in50.txt";

            FileReader fr = new FileReader(pathToDate50RPQ);

            List<RPQ> listOfRPQ = fr.Execute();

            int UB = int.MaxValue;
            int cmax = Carlier.Execute(listOfRPQ, UB);

            Assert.AreEqual(1492, cmax);
        }

        [TestMethod]
        public void TestMethodForCarlier100RPQ()
        {
            string pathToDate100RPQ = pathToDate + "in100.txt";

            FileReader fr = new FileReader(pathToDate100RPQ);

            List<RPQ> listOfRPQ = fr.Execute();

            int UB = int.MaxValue;
            int cmax = Carlier.Execute(listOfRPQ, UB);

            Assert.AreEqual(3070, cmax);
        }

        [TestMethod]
        public void TestMethodForCarlier200RPQ()
        {
            string pathToDate200RPQ = pathToDate + "in200.txt";

            FileReader fr = new FileReader(pathToDate200RPQ);

            List<RPQ> listOfRPQ = fr.Execute();

            int UB = int.MaxValue;
            int cmax = Carlier.Execute(listOfRPQ, UB);

            Assert.AreEqual(6398, cmax);
        }

        [TestMethod]
        public void TestMethodForCarlierData000()
        {
            string pathToTestDate = pathToDate + "data.000.txt";

            FileReader fr = new FileReader(pathToTestDate);

            List<RPQ> listOfRPQ = fr.Execute();

            int UB = int.MaxValue;
            int cmax = Carlier.Execute(listOfRPQ, UB);

            Assert.AreEqual(228, cmax);
        }

        [TestMethod]
        public void TestMethodForCarlierData001()
        {
            string pathToTestDate = pathToDate + "data.001.txt";

            FileReader fr = new FileReader(pathToTestDate);

            List<RPQ> listOfRPQ = fr.Execute();

            int UB = int.MaxValue;
            int cmax = Carlier.Execute(listOfRPQ, UB);

            Assert.AreEqual(3026, cmax);
        }

        [TestMethod]
        public void TestMethodForCarlierData002()
        {
            string pathToTestDate = pathToDate + "data.002.txt";

            FileReader fr = new FileReader(pathToTestDate);

            List<RPQ> listOfRPQ = fr.Execute();

            int UB = int.MaxValue;
            int cmax = Carlier.Execute(listOfRPQ, UB);

            Assert.AreEqual(3665, cmax);
        }

        [TestMethod]
        public void TestMethodForCarlierData003()
        {
            string pathToTestDate = pathToDate + "data.003.txt";

            FileReader fr = new FileReader(pathToTestDate);

            List<RPQ> listOfRPQ = fr.Execute();

            int UB = int.MaxValue;
            int cmax = Carlier.Execute(listOfRPQ, UB);

            Assert.AreEqual(3309, cmax);
        }

        [TestMethod]
        public void TestMethodForCarlierData004()
        {
            string pathToTestDate = pathToDate + "data.004.txt";

            FileReader fr = new FileReader(pathToTestDate);

            List<RPQ> listOfRPQ = fr.Execute();

            int UB = int.MaxValue;
            int cmax = Carlier.Execute(listOfRPQ, UB);

            Assert.AreEqual(3191, cmax);
        }

        [TestMethod]
        public void TestMethodForCarlierData005()
        {
            string pathToTestDate = pathToDate + "data.005.txt";

            FileReader fr = new FileReader(pathToTestDate);

            List<RPQ> listOfRPQ = fr.Execute();

            int UB = int.MaxValue;
            int cmax = Carlier.Execute(listOfRPQ, UB);

            Assert.AreEqual(3618, cmax);
        }

        [TestMethod]
        public void TestMethodForCarlierData006()
        {
            string pathToTestDate = pathToDate + "data.006.txt";

            FileReader fr = new FileReader(pathToTestDate);

            List<RPQ> listOfRPQ = fr.Execute();

            int UB = int.MaxValue;
            int cmax = Carlier.Execute(listOfRPQ, UB);

            Assert.AreEqual(3446, cmax);
        }

        [TestMethod]
        public void TestMethodForCarlierData007()
        {
            string pathToTestDate = pathToDate + "data.007.txt";

            FileReader fr = new FileReader(pathToTestDate);

            List<RPQ> listOfRPQ = fr.Execute();

            int UB = int.MaxValue;
            int cmax = Carlier.Execute(listOfRPQ, UB);

            Assert.AreEqual(3821, cmax);
        }

        [TestMethod]
        public void TestMethodForCarlierData008()
        {
            string pathToTestDate = pathToDate + "data.008.txt";

            FileReader fr = new FileReader(pathToTestDate);

            List<RPQ> listOfRPQ = fr.Execute();

            int UB = int.MaxValue;
            int cmax = Carlier.Execute(listOfRPQ, UB);

            Assert.AreEqual(3634, cmax);
        }
    }
}
