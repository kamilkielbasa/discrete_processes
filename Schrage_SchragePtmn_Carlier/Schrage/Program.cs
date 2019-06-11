using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schrage
{
    public class RPQ
    {
        public int r { get; set; }
        public int p { get; set; }
        public int q { get; set; }
        public int executionTime { get; set; }
        public int taskNumber { get; set; }

        public RPQ()
        {
            this.r = 0;
            this.p = 0;
            this.q = 0;
            this.executionTime = 0;
            this.taskNumber = 0;
        }

        public RPQ(int r, int p, int q, int taskNumber)
        {
            this.r = r;
            this.p = p;
            this.q = q;
            this.taskNumber = taskNumber;
            this.executionTime = 0;
        }

        public RPQ(RPQ rpq)
        {
            this.r = rpq.r;
            this.p = rpq.p;
            this.q = rpq.q;
            this.taskNumber = rpq.taskNumber;
            this.executionTime = 0;
        }
    }

    public class FileReader
    {
        public string path { get; }

        public FileReader(string pathToFile)
        {
            this.path = pathToFile;
        }

        public List<RPQ> Execute()
        {
            List<RPQ> listOfRPQ = new List<RPQ>();

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line = sr.ReadLine();
                    string[] tokens = line.Split(null);

                    tokens = tokens.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    int numberOfRPQ = int.Parse(tokens[0]);

                    for (int i = 0; i < numberOfRPQ; ++i)
                    {
                        line = sr.ReadLine();
                        tokens = line.Split(null);
                        tokens = tokens.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        RPQ tmp = new RPQ(int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]), i);
                        listOfRPQ.Add(tmp);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return listOfRPQ;
        }
    }

    public class SchrageAlgorithm
    {
        public static Tuple<RPQ[], int> Execute(List<RPQ> listOfRPQ)
        {
            int i = 0;
            RPQ[] sigma = new RPQ[listOfRPQ.Count];
            List<RPQ> listOfSortedRPQ = new List<RPQ>();
            List<RPQ> listOfUnsortedRPQ = listOfRPQ;
            int t = listOfUnsortedRPQ.Min(RPQ => RPQ.r);

            int cmax = 0;

            while (listOfSortedRPQ.Any() == true || listOfUnsortedRPQ.Any() == true)
            {
                while (listOfUnsortedRPQ.Any() == true && listOfUnsortedRPQ.Min(RPQ => RPQ.r) <= t)
                {
                    RPQ j = listOfUnsortedRPQ.Find(x => x.r == listOfUnsortedRPQ.Min(RPQ => RPQ.r));
                    listOfSortedRPQ.Add(j);
                    listOfUnsortedRPQ.Remove(j);
                }

                if (listOfSortedRPQ.Any() == false)
                {
                    t = listOfUnsortedRPQ.Min(RPQ => RPQ.r);
                }
                else
                {
                    RPQ j = listOfSortedRPQ.Find(x => x.q == listOfSortedRPQ.Max(RPQ => RPQ.q));
                    listOfSortedRPQ.Remove(j);
                    t += j.p;
                    sigma[i] = j;
                    sigma[i].executionTime = t;
                    cmax = Math.Max(cmax, t + j.q);
                    i += 1;
                }
            }

            return Tuple.Create(sigma, cmax);
        }
    }

    public class SchragePmtnAlgorithm
    {
        public static int Execute(List<RPQ> listOfRPQ)
        {
            int cmax = 0;
            List<RPQ> listOfSortedRPQ = new List<RPQ>();
            List<RPQ> listOfUnsortedRPQ = listOfRPQ.Select(RPQ => new RPQ(RPQ)).ToList();
            int t = 0;
            RPQ l = new RPQ();

            while (listOfSortedRPQ.Any() == true || listOfUnsortedRPQ.Any() == true)
            {
                while (listOfUnsortedRPQ.Any() == true && listOfUnsortedRPQ.Min(RPQ => RPQ.r) <= t)
                {
                    RPQ j = listOfUnsortedRPQ.Find(x => x.r == listOfUnsortedRPQ.Min(RPQ => RPQ.r));
                    listOfSortedRPQ.Add(j);
                    listOfUnsortedRPQ.Remove(j);

                    if (j.q > l.q)
                    {
                        l.p = t - j.r;
                        t = j.r;

                        if (j.p > 0)
                        {
                            listOfSortedRPQ.Add(l);
                        }
                    }
                }

                if (listOfSortedRPQ.Any() == false)
                {
                    t = listOfUnsortedRPQ.Min(RPQ => RPQ.r);
                }
                else
                {
                    RPQ j = listOfSortedRPQ.Find(x => x.q == listOfSortedRPQ.Max(RPQ => RPQ.q));
                    listOfSortedRPQ.Remove(j);
                    l = j;
                    t += j.p;
                    cmax = Math.Max(cmax, t + j.q);
                }
            }

            return cmax;
        }
    }


    public class Carlier
    {
        static int counter = 0;

        public static int Execute(List<RPQ> listOfRPQ, int UB)
        {
            counter++;
            List<RPQ> copyListOfRPQ = listOfRPQ.Select(RPQ => new RPQ(RPQ)).ToList();

            var schrageOutput = SchrageAlgorithm.Execute(copyListOfRPQ);
            int U = schrageOutput.Item2;
            List<RPQ> PI = schrageOutput.Item1.ToList();

            if (U < UB)
                UB = U;

            int a = 0;
            int b = 0;
            int c = -1;

            // wyznaczamy b
            int cPiJ = 0;
            List<int> executionTime = new List<int>();
            for (int i = 0; i < PI.Count(); i++)
            {
                cPiJ = Math.Max(cPiJ, Math.Max(PI[i].r, cPiJ) + PI[i].p);
                executionTime.Add(cPiJ);

                if (cPiJ + PI[i].q == U)
                    b = i;
            }

            Console.WriteLine("b = {0}", b);

            // wyznaczamy a
            int sum_tasks = 0;
            for (int i = b; i > 0; i--)
            {
                sum_tasks += PI[i].p;

                if (U == sum_tasks + PI[i].r + PI[b].q &&
                    executionTime[i - 1] != PI[i].p)
                {
                    a = i;
                    break;
                }
            }

            Console.WriteLine("a = {0}", a);

            // wyznaczamy c
            for (int i = a; i <= b; i++)
            {
                if (PI[i].q < PI[b].q)
                    c = i;
            }

            Console.WriteLine("c = {0}", c);

            if (c < 0)
                return UB;

            // wyznaczanie r', q' i p'
            int pprim = 0;
            int rprim = PI[c + 1].r;
            int qprim = PI[c + 1].q;

            for (int i = c + 1; i <= b; i++)
            {
                pprim += PI[i].p;
                
                if (rprim > PI[i].r)
                    rprim = PI[i].r;

                if (qprim > PI[i].q)
                    qprim = PI[i].q;
            }

            int oldPiRC = PI[c].r;
            PI[c].r = Math.Max(PI[c].r, rprim + pprim);

            int pbis = 0;
            int rbis = PI[c].r;
            int qbis = PI[c].q;

            for (int i = c; i <= b; i++)
            {
                pbis += PI[i].p;

                if (rbis > PI[i].r)
                    rbis = PI[i].r;

                if (qbis > PI[i].q)
                    qbis = PI[i].q;
            }

            int LB = 0;

            LB = SchragePmtnAlgorithm.Execute(PI);
            LB = Math.Max(LB, Math.Max(rprim + qprim + pprim, rbis + pbis + qbis));

            Console.WriteLine("Left son");

            if (LB < UB)
                UB = Carlier.Execute(PI, UB);

            PI[c].r = oldPiRC;

            int oldPiQC = PI[c].q;

            PI[c].q = Math.Max(PI[c].q, qprim + pprim);
            pbis = 0;
            rbis = PI[c].r;
            qbis = PI[c].q;

            for (int i = c; i <= b; i++)
            {
                pbis += PI[i].p;

                if (rbis > PI[i].r)
                    rbis = PI[i].r;

                if (qbis > PI[i].q)
                    qbis = PI[i].q;
            }

            LB = SchragePmtnAlgorithm.Execute(PI);
            LB = Math.Max(LB, Math.Max(rprim + qprim + pprim, rbis + pbis + qbis));

            Console.WriteLine("Right son");

            if (LB < UB)
                UB = Carlier.Execute(PI, UB);

            PI[c].q = oldPiQC;

          //  if (counter >= 1000)
          //      return UB;

            return UB;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Schrage and SchragePmtn!");

            const string pathToDate =
                @"C:\Users\pati\Desktop\discrete_processes\Schrage\Schrage\TestDate\";

            string pathToDate50RPQ = pathToDate + "data.005.txt";

            FileReader fr = new FileReader(pathToDate50RPQ);

            List<RPQ> listOfRPQ = fr.Execute();

            int UB = int.MaxValue;
            int cmax = Carlier.Execute(listOfRPQ, UB);

            Console.WriteLine("cmax = {0}", cmax);

            Console.ReadKey();
        }
    }
}
