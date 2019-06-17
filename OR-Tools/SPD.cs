//
// Copyright 2012 Hakan Kjellerstrand
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.OrTools.LinearSolver;
using Google.OrTools.Sat;

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

class Program
{
    public static void SolveInstance(List<RPQ> listOfRPQ)
    {
        Solver solver = Solver.CreateSolver("SimpleMipProgram", "CBC_MIXED_INTEGER_PROGRAMMING");

        int variablesMaxValue = 0;

        foreach(var job in listOfRPQ)
        {
            variablesMaxValue += job.r + job.p + job.q;
        }

        var alfas = solver.MakeIntVarMatrix(listOfRPQ.Count, listOfRPQ.Count, 0, 1);

        var starts = solver.MakeIntVarArray(listOfRPQ.Count, 0, variablesMaxValue);

        var cmax = solver.MakeIntVar(0, variablesMaxValue, "cmax");

        foreach(var job in listOfRPQ)
        {
            solver.Add(starts[job.taskNumber] >= job.r);
        }

        foreach(var job in listOfRPQ)
        {
            solver.Add(cmax >= starts[job.taskNumber] + job.p + job.q);
        }

        for (int i = 0; i < listOfRPQ.Count; ++i)
        {
            for (int j = i + 1; j < listOfRPQ.Count; ++j)
            {
                var job1 = listOfRPQ[i];
                var job2 = listOfRPQ[j];
                solver.Add(starts[job1.taskNumber] + job1.p <= starts[job2.taskNumber] + alfas[job1.taskNumber, job2.taskNumber] * variablesMaxValue);
                solver.Add(starts[job2.taskNumber] + job2.p <= starts[job1.taskNumber] + alfas[job2.taskNumber, job1.taskNumber] * variablesMaxValue);
                solver.Add(alfas[job1.taskNumber, job2.taskNumber] + alfas[job2.taskNumber, job1.taskNumber] == 1);
            }
        }

        solver.Minimize(cmax);
        Solver.ResultStatus resultStatus = solver.Solve(); 

        if (resultStatus != Solver.ResultStatus.OPTIMAL)
        {
            Console.WriteLine("Solver didn't find optimal solution!");
        }

        Console.WriteLine("Objective value = " + solver.Objective().Value());
    }

    public static void SolveWithCP(List<RPQ> listOfRPQ)
    {
        CpModel model = new CpModel();

        int variablesMaxValue = 0;

        foreach (var job in listOfRPQ)
        {
            variablesMaxValue += job.r + job.p + job.q;
        }

        //var alfas = solver.MakeIntVarMatrix(listOfRPQ.Count, listOfRPQ.Count, 0, 1);
        var alfas = new IntVar[listOfRPQ.Count, listOfRPQ.Count];
        for(int i = 0; i < listOfRPQ.Count; ++i)
        {
            for (int j = 0; j < listOfRPQ.Count; ++j)
            {
                alfas[i,j] = model.NewIntVar(0, 1, "alfas " + i + " " + j);
            }
        }
        //var starts = solver.MakeIntVarArray(listOfRPQ.Count, 0, variablesMaxValue);
        var starts = new IntVar[listOfRPQ.Count];
        for (int i = 0; i < listOfRPQ.Count; ++i)
        {
            starts[i] = model.NewIntVar(0, variablesMaxValue, "starts " + i);
        }

        var cmax = model.NewIntVar(0, variablesMaxValue, "cmax");

        foreach (var job in listOfRPQ)
        {
            model.Add(starts[job.taskNumber] >= job.r);
        }

        foreach (var job in listOfRPQ)
        {
            model.Add(cmax >= starts[job.taskNumber] + job.p + job.q);
        }

        for (int i = 0; i < listOfRPQ.Count; ++i)
        {
            for (int j = i + 1; j < listOfRPQ.Count; ++j)
            {
                var job1 = listOfRPQ[i];
                var job2 = listOfRPQ[j];
                model.Add(starts[job1.taskNumber] + job1.p <= starts[job2.taskNumber] + alfas[job1.taskNumber, job2.taskNumber] * variablesMaxValue);
                model.Add(starts[job2.taskNumber] + job2.p <= starts[job1.taskNumber] + alfas[job2.taskNumber, job1.taskNumber] * variablesMaxValue);
                model.Add(alfas[job1.taskNumber, job2.taskNumber] + alfas[job2.taskNumber, job1.taskNumber] == 1);
            }
        }

        model.Minimize(cmax);

        //Solver.ResultStatus resultStatus = model.Solve();
        CpSolver solver = new CpSolver();
        CpSolverStatus status = solver.Solve(model);

        if (status != CpSolverStatus.Optimal)
        {
            Console.WriteLine("Solver didn't find optimal solution!");
        }

        Console.WriteLine("Objective value = " + solver.ObjectiveValue);
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Hello Solver!");

        FileReader fr = new FileReader(@"C:\Users\kielbkam\Desktop\semestr 6\discrete_processes\Schrage\Schrage\TestDate\in100.txt");// @"C:\Users\kielbkam\Desktop\date.txt");

        List<RPQ> listOfRPQ = fr.Execute();

        SolveWithCP(listOfRPQ);

        listOfRPQ.Clear();
        listOfRPQ = fr.Execute();

        SolveInstance(listOfRPQ);

        Console.ReadKey();
    }
}
