using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGenerator
{
    public class KnapsackProblem
    {
        public int N { get; set; }
        public int K { get; set; }
        //public int[,] wynik;
        //public Tile[] v;
        public int[] waga;
        public int[] wartosc;

        public int vmax = 0;
        public List<Result> Results = new List<Result>();


        public void Solve()
        {
            Test(0, new Result());
        }
        private void Test(int part, Result res)
        {
            if (part < N - 1)
                Test(part + 1, res);
            if (res.waga+waga[part]<=K)
            {
                res.waga += waga[part];
                res.vmax += wartosc[part];
                res.parts.Add(part);
                if(part<N-1)
                    Test(part + 1, res);
                else
                {
                    if(res.vmax>vmax)
                    {
                        vmax = res.vmax;
                        Results.Clear();
                        Results.Add((Result)res.Clone());
                    }
                    else if(res.vmax==vmax)
                    {
                        Results.Add((Result)res.Clone());
                    }
                }
                res.waga -= waga[part];
                res.vmax -= wartosc[part];
                res.parts.Remove(part);
            }
        }
        //public void Solve()
        //{
        //    wynik = new int[N + 1, K + 1];
        //    for (int a = 0; a <= K; a++)
        //        wynik[0, a] = 0;
        //    for (int p = 1; p <= N; p++)
        //    {
        //        for (int a = 0; a <= K; a++)
        //        {
        //            wynik[p, a] = wynik[p - 1, a];
        //            if (a >= 1 && wynik[p, a - 1] > wynik[p, a])
        //                wynik[p, a] = wynik[p, a - 1];
        //            if (a >= w[p] && wynik[p - 1, a - w[p]] + c[p] > wynik[p, a])
        //                wynik[p, a] = wynik[p - 1, a - w[p]] + c[p];
        //        }
        //    }
        //}
        //public void Solve()
        //{
        //    v = new Tile[K + 1];
        //    for (int i = 0; i < v.Length; i++)
        //        v[i] = new Tile();

        //    for (int i = 0; i < N; i++)
        //    {
        //        for (int j = 1; j <= K; j++)
        //        {
        //            if (waga[i] <= j)
        //            //to co dla poprzedniego klocka lub
        //            //to co dla wagi mniejszej o wagę tego klocka
        //            {
        //                if (v[j - waga[i]].W + wartosc[i] > v[j].W)
        //                {
        //                    v[j].W = v[j - waga[i]].W + wartosc[i];
        //                    v[j].part = i;
        //                    v[j].before = v[j - waga[i]].Copy();
        //                    Console.WriteLine($"v[{i},{j}] dodany klocek {i} do klocków z v[{i},{j - waga[i]}]");
        //                }
        //                else
        //                {
        //                    var t = i - 1 > 0 ? i - 1 : 0;
        //                    Console.WriteLine($"v[{i},{j}] zostają klocki z v[{t},{j}]");
        //                }
        //            }
        //        }
        //    }
        //}
    }
    //public class Tile
    //{
    //    public int W;
    //    public int part=-1;
    //    public Tile before;
    //    public Tile Copy()
    //    {
    //        return new Tile() { W = W, part = part, before = before };
    //    }
    //}
    public class Result:ICloneable
    {
        public int vmax = 0;
        public int waga = 0;
        public List<int> parts = new List<int>();

        public object Clone()
        {
            var res = new Result();
            res.vmax = vmax;
            res.waga = waga;
            res.parts = new List<int>(parts);
            return res;
        }
    }
}
