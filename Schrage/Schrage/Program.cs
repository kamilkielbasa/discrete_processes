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

        public RPQ(int r, int p, int q)
        {
            this.r = r;
            this.p = p;
            this.q = q;
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

                        RPQ tmp = new RPQ(int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]));
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
                    sigma[i] = j;
                    i += 1;
                    t += j.p;
                    cmax = Math.Max(cmax, t + j.q);
                }
            }

            return Tuple.Create(sigma, cmax);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
