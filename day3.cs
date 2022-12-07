using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adwent2022
{
    internal static class Day3
    {
        public static int Part1()
        {
            using var input = File.OpenText("inputs/3.txt");
            var indexes = Enumerable.Range('a', 26).Select(i => (char)i)
                .Concat(Enumerable.Range('A', 26).Select(i => (char)i))
                .ToList();
            var sum = 0;
            while (input.Peek() != -1)
            {
                var backpack = input.ReadLine();
                var sackItemCount = backpack.Length / 2;
                var items = new List<char>();
                for (int i = 0; i < sackItemCount; i++)
                {
                    if (backpack[(sackItemCount)..].Contains(backpack[i]) && !items.Contains(backpack[i]))
                    {
                        items.Add(backpack[i]);
                    }
                }
                sum += items.Select(e => indexes.IndexOf(e) + 1).Sum();
            }
            return sum;
        }
        public static int Part2()
        {
            using var input = File.OpenText("inputs/3.txt");
            var indexes = Enumerable.Range('a', 26).Select(i => (char)i)
                .Concat(Enumerable.Range('A', 26).Select(i => (char)i))
                .ToList();
            var sum = 0;
            while (input.Peek() != -1)
            {
                var backpack1 = input.ReadLine();
                var backpack2 = input.ReadLine();
                var backpack3 = input.ReadLine();

                var bps = new List<string> { backpack1, backpack2, backpack3 }.OrderBy(e => e.Length);

                var badges = new List<char>();
                for (int i = 0; i < bps.First().Length ; i++)
                {
                    if (bps.Skip(1).All(e=> e.Contains(bps.First()[i])) && !badges.Contains(bps.First()[i]))
                    {
                        badges.Add(bps.First()[i]);
                    }
                }
                sum += badges.Select(e => indexes.IndexOf(e) + 1).Sum();
            }
            return sum;
        }
    }
}
