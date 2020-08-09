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
        private readonly TimeSpan MaxLength;
        private int TotalSecunds => (int)MaxLength.TotalSeconds;
        public int MaxResultCount { get; set; } = int.MaxValue;

        private List<Track> Tracks;

        private int MaxResultTime = 0;
        private List<ProblemResult> Results = new List<ProblemResult>();

        public KnapsackProblem(List<Track> songs, TimeSpan length)
        {
            MaxLength = length;
            Tracks = songs.OrderBy(v=>v.TotalSecunds).ToList();
            N = Tracks.Count;
        }

        public async Task<Result> Solve()
        {
            Completition = new TaskCompletionSource<List<ProblemResult>>();
            TaskCounter++;
            //Console.WriteLine("count:" + TaskCounter + " lvl:" + 0);
            Task t = Task.Run(() => Test(0, new ProblemResult()));
            await Completition.Task;

            var result = new Result();
            result.Time = TimeSpan.FromSeconds(Results.FirstOrDefault()?.Time ?? 0);
            result.Songs = Results.Select(r => r.Songs).ToList();
            return result;
        }

        private int TaskCounter = 0;
        TaskCompletionSource<List<ProblemResult>> Completition;
        private readonly object lobj = new object();
        private void Test(int part, ProblemResult res)
        {
            if (Results.Count < MaxResultCount || Results[0].Time < TotalSecunds)
            {
                if (res.Time + Tracks[part].TotalSecunds <= TotalSecunds)
                {
                    var resCopy = (ProblemResult)res.Clone();
                    if (!IsLast(part))
                    {
                        lock (lobj)
                        {
                            TaskCounter++;
                            Task t1 = Task.Run(() => Test(part + 1, resCopy));
                        }
                    }

                    res.Time += Tracks[part].TotalSecunds;
                    res.Songs.Add(Tracks[part]);

                    if (!IsLast(part) && res.Time < TotalSecunds)
                    {
                        lock (lobj)
                        {
                            TaskCounter++;
                            Task t2 = Task.Run(() => Test(part + 1, res));
                        }
                    }
                    else
                        TryAddResult(res);
                }
                else
                    TryAddResult(res);
            }
            lock (lobj)
            {
                TaskCounter--;
                try
                {
                    if (TaskCounter == 0)
                        Completition.SetResult(Results);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void TryAddResult(ProblemResult res)
        {
            if (res.Time > MaxResultTime)
            {
                MaxResultTime = res.Time;
                Results.Clear();
                Results.Add(res);
            }
            else if (res.Time == MaxResultTime)
                Results.Add(res);
        }
        private bool IsLast(int nr) => nr == N - 1;
    }

    public class ProblemResult : ICloneable
    {
        public static int counter = 0;
        public int id;
        public int Time { get; set; }
        public List<Track> Songs { get; private set; } = new List<Track>();

        public ProblemResult()
        {
            id = counter++;
        }
        public object Clone()
        {
            id = counter++;
            var res = new ProblemResult();
            res.Time = Time;
            res.Songs = new List<Track>(Songs);
            return res;
        }
    }
    public class Result
    {
        public TimeSpan Time { get; set; }
        public List<List<Track>> Songs { get; set; }
    }
}
