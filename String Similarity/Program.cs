using System;
using System.IO;
using System.Linq;

namespace String_Similarity
{
    class Program
    {
        class Result
        {
            public int ZValue { get; }
            public int R { get; }
            public Result(int zValue, int r)
            {
                ZValue = zValue;
                R = r;
            }
        }
        static Result ComputeZ(int zIdx, string s, int[] zValues, int r, int l)
        {
            if (zIdx > r)
            {
                l = r = zIdx;
                while (r < s.Length && s[r - l] == s[r])
                {
                    r++;
                }
                r--;
                return new Result(r - l + 1, r);
            }
            else
            {
                int k = zIdx - l;
                if (zValues[k] < r - zIdx + 1)
                {
                    return new Result(zValues[k], r);
                }
                else
                {
                    l = zIdx;
                    while (r < s.Length && s[r - l] == s[r])
                    {
                        r++;
                    }
                    r--;
                    return new Result(r - l + 1, r);
                }
            }
        }

        static int[] CalculateZValues(string str)
        {
            int[] zValues = new int[str.Length];
            zValues[0] = str.Length;
            var maxR = 0;
            var l = 0;
            for (int i = 1; i < str.Length; ++i)
            {
                if (i > maxR)
                {
                    l = maxR = i;
                    while (maxR < str.Length && str[maxR - l] == str[maxR])
                    {
                        maxR++;
                    }
                    zValues[i] = maxR - l;
                    maxR--;
                }
                else
                {
                    int k = i - l;
                    if (zValues[k] < maxR - i + 1)
                    {
                        zValues[i] = zValues[k];
                    }
                    else
                    {
                        l = i;
                        while (maxR < str.Length && str[maxR - l] == str[maxR])
                        {
                            maxR++;
                        }
                        zValues[i] = maxR - l;
                        maxR--;
                    }
                }
            }
            return zValues;
        }
        static void Main(string[] args)
        {
            var results = File.ReadAllLines("TestCase00Results.txt").Select(l => ulong.Parse(l)).ToArray();
            using (var reader = File.OpenText("TestCase00.txt"))
            {
                var caseCount = int.Parse(reader.ReadLine());
                for (int i = 0; i < caseCount; ++i)
                {
                    var str = reader.ReadLine();
                    var zValues = CalculateZValues(str);
                    ulong actual = 0;
                    foreach (var zValue in zValues)
                    {
                        actual += (uint)zValue;
                    }

                    var expected = results[i];
                    Console.WriteLine($"same: {actual == expected} actual: {actual} expected: {expected}");
                }
            }
        }
    }
}
