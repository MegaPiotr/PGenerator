using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGenerator
{
    public class KnapsackProblem
    {
        private readonly int N;
        private readonly int MaxLength;

        private int[] TimeMin;

        private int MaxResultTime = 0;
        private List<Result> Results = new List<Result>();

        public KnapsackProblem(List<int> songs, int length)
        {
            MaxLength = length;
            TimeMin = songs.ToArray();
            N = TimeMin.Length;
        }
        public KnapsackProblem(List<Track> songs, int length)
        {
            //MaxLength = length;
            //TimeMin = songs.ToArray();
            //N = TimeMin.Length;
        }

        public List<Result> Solve()
        {   
            Test(0, new Result());
            return Results;
        }
        private void Test(int part, Result res)
        {
            if (part < N - 1)
                Test(part + 1, res);
            if (res.Time + TimeMin[part] <= MaxLength)
            {
                res.Time += TimeMin[part];
                res.Songs.Add(part);
                if (part < N - 1)
                    Test(part + 1, res);
                else
                {
                    if (res.Time > MaxResultTime)
                    {
                        MaxResultTime = res.Time;
                        Results.Clear();
                        Results.Add((Result)res.Clone());
                    }
                    else if (res.Time == MaxResultTime)
                    {
                        Results.Add((Result)res.Clone());
                    }
                }
                res.Time -= TimeMin[part];
                res.Songs.Remove(part);
            }
        }
    }

    public class Result : ICloneable
    {
        public int Time { get; set; }
        public List<int> Songs { get; private set; } = new List<int>();

        public object Clone()
        {
            var res = new Result();
            res.Time = Time;
            res.Songs = new List<int>(Songs);
            return res;
        }
    }
}
