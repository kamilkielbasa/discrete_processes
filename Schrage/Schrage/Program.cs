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
            List<RPQ> listOfUnsortedRPQ = listOfRPQ;
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
        public static void Execute(List<RPQ> listOfRPQ)
        {
            List<RPQ> schrageCopyList = new List<RPQ>();
            foreach (var item in listOfRPQ)
            {
                schrageCopyList.Add(item);
            }

            Tuple<RPQ[], int> U = SchrageAlgorithm.Execute(schrageCopyList);

            Tuple<RPQ[], int> UB = new Tuple<RPQ[], int>(new RPQ[listOfRPQ.Count], int.MaxValue);

            if (U.Item2 < UB.Item2)
            {
                UB = U;
            }

            RPQ b = new RPQ();
            for (int j = 0; j < listOfRPQ.Count; ++j)
            {
                if (UB.Item2 == (UB.Item1[j].executionTime + UB.Item1[j].q))
                {
                    b = UB.Item1[j];
                    break;
                }
            }
            int bIndex = listOfRPQ.FindIndex(x => x == b);

            RPQ a = new RPQ();
            for (int j = 0; j < listOfRPQ.Count; ++j)
            {
                int p = 0;
                for (int s = j; s < bIndex; ++s)
                {
                    p += UB.Item1[s].p;
                }

                if (UB.Item2 == (UB.Item1[j].r + p + UB.Item1[bIndex].q))
                {
                    a = UB.Item1[j];
                    break;
                }
            }
            int aIndex = listOfRPQ.FindIndex(x => x == a);

            if (aIndex == -1) aIndex = 0;

            RPQ c = new RPQ();
            for (int j = aIndex; j < bIndex; ++j)
            {
                if (UB.Item1[j].q < UB.Item1[bIndex].q)
                {
                    c = UB.Item1[j];
                }
            }
            int cIndex = listOfRPQ.FindIndex(x => x == c);

            if (c.r == 0 || c.p == 0 || c.q == 0)
            {
                return;
            }

            List<RPQ> K = new List<RPQ>();

            for (int i = cIndex + 1; i < bIndex; ++i)
            {
                K.Add(listOfRPQ[i]);
            }

            RPQ tmpRPQ = new RPQ();

            tmpRPQ = K.Find(x => x.r == K.Min(RPQ => RPQ.r));
            int rK = tmpRPQ.r;

            tmpRPQ = K.Find(x => x.q == K.Min(RPQ => RPQ.q));
            int qK = tmpRPQ.q;

            int pK = 0;
            for (int j = 0; j < K.Count; ++j)
            {
                pK += K[j].p;
            }

            K.Add(listOfRPQ[cIndex]);
            tmpRPQ = K.Find(x => x.r == K.Min(RPQ => RPQ.r));
            int rK2 = tmpRPQ.r;

            tmpRPQ = K.Find(x => x.q == K.Min(RPQ => RPQ.q));
            int qK2 = tmpRPQ.q;

            int pK2 = 0;
            for (int j = 0; j < K.Count; ++j)
            {
                pK2 += K[j].p;
            }
            K.Remove(c);
            K.Clear();

            int oldR = listOfRPQ[cIndex].r;
            listOfRPQ[cIndex].r = Math.Max(listOfRPQ[cIndex].r, rK + pK);

            int LB = 0;
            List<RPQ> schragePtmnCopyList = new List<RPQ>();
            foreach (var item in listOfRPQ)
            {
                schrageCopyList.Add(item);
            }
            LB = SchragePmtnAlgorithm.Execute(schragePtmnCopyList);

            int hK = rK + qK + pK;
            int hKwithC = rK2 + qK2 + pK2;

            LB = Math.Max(LB, Math.Max(hK, hKwithC));
            
            if (LB < UB.Item2)
            {
                Carlier.Execute(listOfRPQ);
            }

            listOfRPQ[cIndex].r = oldR;

            int oldQ = listOfRPQ[cIndex].q;
            listOfRPQ[cIndex].q = Math.Max(listOfRPQ[cIndex].q, qK + pK);

            List<RPQ> schragePtmnCopyList2 = new List<RPQ>();
            foreach (var item in listOfRPQ)
            {
                schragePtmnCopyList2.Add(item);
            }
            LB = SchragePmtnAlgorithm.Execute(schragePtmnCopyList2);
            LB = Math.Max(LB, Math.Max(hK, hKwithC));

            if (LB < UB.Item2)
            {
                Carlier.Execute(listOfRPQ);
            }

            listOfRPQ[cIndex].q = oldQ;
        }
    }

    public class Carlier2
    {
        public List<RPQ> listOfRPQ;
        public int UB = int.MaxValue;
        public int cmax_without = 0;
        public int cmax_with = 0;

        public Carlier2(List<RPQ> listOfRPQ)
        {
            this.listOfRPQ = listOfRPQ;
        }

        public int find_a(int a, int b, int c)
        {
            int sum = 0;

            for (a = 0; a <= b; ++a)
            {
                sum = 0;

                for (int i = a; i <= b; ++i)
                {
                    sum += listOfRPQ[i].p;
                }

                if (cmax_without == (listOfRPQ[a].r + sum + listOfRPQ[b].q))
                {
                    return a;
                }
            }

            return a;
        }

        public int find_b(int a, int b, int c)
        {
            b = listOfRPQ.Count() - 1;

            for (int i = listOfRPQ.Count() - 1; i > 0; --i)
            {
                if (cmax_without == (listOfRPQ[i].executionTime + listOfRPQ[i].q))
                {
                    b = i;
                    break;
                }
            }

            return b;
        }

        public int find_c(int a, int b, int c)
        {
            c = -1;

            for (int i = a; i <= b; i++)
            {
                if (listOfRPQ[i].q < listOfRPQ[b].q)
                {
                    c = i;
                    break;
                }
            }

            return c;
        }

        public int Execute()
        {
            int Cmax = 0;
            int nr_c = 0;
            int r_c = 0;
            int p_sum = 0;
            int q_c = 0;
            int r_new_for_c = int.MaxValue;
            int q_new_for_c = int.MaxValue;
            int a = 0, b = 0, c = -1;
            int U = 0, LB = 0;

            List<RPQ> copyList = new List<RPQ>();

            copyList.Clear();
            foreach (RPQ item in listOfRPQ)
            {
                copyList.Add(item);
            }

            Tuple<RPQ[], int> schrageOutput = SchrageAlgorithm.Execute(copyList);
            U = schrageOutput.Item2;
            cmax_without = schrageOutput.Item2;

            if (U < UB)
                UB = U;

            b = find_b(a, b, c);
            a = find_a(a, b, c);
            c = find_c(a, b, c);

            if (c == -1)
            {
                return UB;
            }

            nr_c = listOfRPQ[c].taskNumber;
            int nowa_suma = 0;
            for (int i = c + 1; i <= b; ++i)
            {
                r_new_for_c = Math.Min(r_new_for_c, listOfRPQ[i].r);
                p_sum += listOfRPQ[i].p;
                nowa_suma += listOfRPQ[i].p;
                q_new_for_c = Math.Min(q_new_for_c, listOfRPQ[i].q);
            }

            r_c = listOfRPQ[c].r;
            q_c = listOfRPQ[c].q;

            listOfRPQ[c].r = Math.Max(listOfRPQ[c].r, r_new_for_c + p_sum);

            copyList.Clear();
            foreach (RPQ item in listOfRPQ)
            {
                copyList.Add(item);
            }
            LB = SchragePmtnAlgorithm.Execute(copyList);

            if (LB < UB)
            {
                Execute();
            }

            for (int i = 0; i < listOfRPQ.Count(); ++i)
            {
                if (nr_c == listOfRPQ[i].taskNumber)
                {
                    listOfRPQ[i].r = r_c;
                }
            }

            listOfRPQ[c].q = Math.Max(listOfRPQ[c].q, q_new_for_c + p_sum);

            copyList.Clear();
            foreach (RPQ item in listOfRPQ)
            {
                copyList.Add(item);
            }
            LB = SchragePmtnAlgorithm.Execute(copyList);

            if (LB < UB)
            {
                Execute();
            }

            for (int i = 0; i < listOfRPQ.Count(); ++i)
            {
                if (nr_c == listOfRPQ[i].taskNumber)
                {
                    listOfRPQ[i].q = q_c;
                }
            }

            return UB;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Schrage and SchragePmtn!");

            const string pathToDate =
                @"C:\Users\kielbkam\Desktop\semestr 6\discrete_processes\Schrage\Schrage\TestDate\";

            string pathToDate50RPQ = pathToDate + "in50.txt";

            FileReader fr = new FileReader(pathToDate50RPQ);

            List<RPQ> listOfRPQ = fr.Execute();

            Carlier2 carr = new Carlier2(listOfRPQ);

            int cmax = carr.Execute();

            Console.WriteLine("cmax = {0}", cmax);

            Console.ReadKey();
        }
    }
}
