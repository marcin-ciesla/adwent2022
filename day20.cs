using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Xsl;

namespace adwent2022
{
    internal static class Day20
    {
        static StreamReader GetInput()
        {
            return File.OpenText("inputs/20.txt");
            //return File.OpenText("inputs/example.txt");
        }

        public static void Part1()
        {
            var input = GetInput().ReadToEnd().Split('\n').Select((e, i) => new int[] { i, int.Parse(e) }).ToArray();
            var mixer = input.ToList();
            //Console.WriteLine(string.Join(',', mixer));
            foreach (var number in input)
            {
                var i = mixer.IndexOf(number);
                var temp = mixer.ToArray();
                var c = number[1] % (temp.Length - 1);
                while (c != 0)
                {
                    if (c > 0)
                    {
                        if (i + 1 == temp.Length)
                        {
                            temp = temp.SkipLast(1).Prepend(number).ToArray();
                            i = 0;
                        }
                        else
                        {
                            (temp[i + 1], temp[i]) = (temp[i], temp[i + 1]);
                            ++i;
                            --c;
                        }
                    }
                    else
                    {
                        if (i - 1 < 0)
                        {
                            temp = temp.Skip(1).Append(number).ToArray();
                            i = temp.Length - 1;
                        }
                        else
                        {
                            (temp[i - 1], temp[i]) = (temp[i], temp[i - 1]);
                            --i;
                            ++c;
                        }
                    }
                }
                mixer = temp.ToList();
            }
            var zeroIndex = mixer.IndexOf(mixer.Where(e => e[1] == 0).First());
            //Console.WriteLine(string.Join(',', mixer));
            Console.WriteLine($"0 at pos {zeroIndex}");
            var oneK = mixer[((zeroIndex + 1000) % input.Length)][1];
            var twoK = mixer[((zeroIndex + 2000) % input.Length)][1];
            var threeK = mixer[((zeroIndex + 3000) % input.Length)][1];
            //var oneK   = mixer[(1000 + mixer.IndexOf(0)+1) % input.Length];
            //var twoK   = mixer[(2000 + mixer.IndexOf(0)+1) % input.Length];
            //var threeK = mixer[(3000 + mixer.IndexOf(0) + 1) % input.Length];
            Console.WriteLine($"1000 - {oneK}, 2000 - {twoK}, 3000 {threeK}");
            Console.WriteLine($"coordinate: {oneK + twoK + threeK}");
        }

        public static void Part2()
        {
            var input = GetInput().ReadToEnd().Split('\n').Select((e, i) => new long[] { i, long.Parse(e) * 811589153 }).ToArray();
            var mixer = input.ToList();
            //Console.WriteLine(string.Join(',', mixer));
            for (int i = 0; i < 10; i++)
            {
                mixer = Mix(input, mixer);
            }
            var zeroIndex = mixer.IndexOf(mixer.Where(e => e[1] == 0).First());
            //Console.WriteLine(string.Join(',', mixer));
            Console.WriteLine($"0 at pos {zeroIndex}");
            var oneK = mixer[((zeroIndex + 1000) % input.Length)][1];
            var twoK = mixer[((zeroIndex + 2000) % input.Length)][1];
            var threeK = mixer[((zeroIndex + 3000) % input.Length)][1];
            Console.WriteLine($"1000 - {oneK}, 2000 - {twoK}, 3000 {threeK}");
            Console.WriteLine($"coordinate: {oneK + twoK + threeK}");
        }

        static List<long[]> Mix(long[][] input, List<long[]> mixer)
        {
            foreach (var number in input)
            {
                var i = mixer.IndexOf(number);
                var temp = mixer.ToArray();
                var c = number[1] % (temp.Length - 1);
                while (c != 0)
                {
                    if (c > 0)
                    {
                        if (i + 1 == temp.Length)
                        {
                            temp = temp.SkipLast(1).Prepend(number).ToArray();
                            i = 0;
                        }
                        else
                        {
                            (temp[i + 1], temp[i]) = (temp[i], temp[i + 1]);
                            ++i;
                            --c;
                        }
                    }
                    else
                    {
                        if (i - 1 < 0)
                        {
                            temp = temp.Skip(1).Append(number).ToArray();
                            i = temp.Length - 1;
                        }
                        else
                        {
                            (temp[i - 1], temp[i]) = (temp[i], temp[i - 1]);
                            --i;
                            ++c;
                        }
                    }
                }
                mixer = temp.ToList();
            }
            return mixer;
        }
    }
}