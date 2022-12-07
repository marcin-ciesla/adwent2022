using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adwent2022
{
    internal static class Day5
    {
        public static string Part1()
        {
            using var input = File.OpenText("inputs/5.txt");
            var line = input.ReadLine();
            var m = Regex.Matches(line, @"([\[\]\w\s]{3,4})");
            var stacks = new Stack<char>[m.Count];
            for (int i = 0; i < stacks.Length; i++)
            {
                stacks[i] = new Stack<char>();
            }
            while (!string.IsNullOrWhiteSpace(line))
            {
                m = Regex.Matches(line, @"([\[\]\w\s]{3,4})");
                for (int i = 0; i < m.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(m[i].Value))
                    {
                        stacks[i].Push(m[i].Value[1]);
                    }
                }
                line = input.ReadLine();
            }
            for (int i = 0; i < stacks.Length; i++)
            {
                stacks[i].Pop();
                stacks[i] = new Stack<char>(stacks[i]);
            }

            while (!input.EndOfStream)
            {
                line = input.ReadLine();
                var move = Regex.Match(line, @"(\d+).+(\d+).+(\d+)");
                for (int i = 0; i < int.Parse(move.Groups[1].Value); i++)
                {
                    stacks[int.Parse(move.Groups[3].Value) - 1].Push(stacks[int.Parse(move.Groups[2].Value) - 1].Pop());
                }
            }
            return string.Join("", stacks.Select(e => e.Peek()));
        }

        public static string Part2()
        {
            using var input = File.OpenText("inputs/5.txt");
            var line = input.ReadLine();
            var m = Regex.Matches(line, @"([\[\]\w\s]{3,4})");
            var stacks = new string[m.Count];
            for (int i = 0; i < stacks.Length; i++)
            {
                stacks[i] = "";
            }
            while (!string.IsNullOrWhiteSpace(line))
            {
                m = Regex.Matches(line, @"([\[\]\w\s]{3,4})");
                for (int i = 0; i < m.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(m[i].Value))
                    {
                        stacks[i] += (m[i].Value[1]);
                    }
                }
                line = input.ReadLine();
            }
            stacks = stacks.Select(e => string.Join("",e[..^1].Reverse())).ToArray();
            while (input.Peek() != -1)
            {
                line = input.ReadLine();
                var move = Regex.Match(line, @"(\d+).+(\d+).+(\d+)");
                var boxes = int.Parse(move.Groups[1].Value);
                var from = int.Parse(move.Groups[2].Value) - 1;
                var to = int.Parse(move.Groups[3].Value) - 1;
                stacks[to] = $"{stacks[to]}{stacks[from][^boxes..]}";
                stacks[from] = stacks[from][..^boxes];
            }
            return string.Join("", stacks.Select(e => e.Last()));
        }
    }
}
