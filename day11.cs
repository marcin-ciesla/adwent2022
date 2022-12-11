using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace adwent2022
{
    internal static class Day11
    {
        public static int Part1()
        {
            var monkeyInputs = File.OpenText("inputs/11.txt").ReadToEnd().Split("\n\n");
            var monkeys = new Monkey[monkeyInputs.Length];
            for (var i = 0; i < monkeyInputs.Length; i++)
            {
                var lines = monkeyInputs[i].Split('\n');
                var items = Regex.Matches(lines[1], @"\d+").Select(c => long.Parse(c.Value));
                double? opNo = null;
                if (double.TryParse(Regex.Match(lines[2], @"\d+").Value ?? "", out var t))
                    opNo = t;
                var operation = Regex.Match(lines[2], @"[\+\*]").Value switch
                {
                    "+" => new Func<long, long>(d =>
                    {
                        var exam = (d + (opNo ?? d)) / 3;
                        return (int) Math.Floor(exam);
                    }),
                    "*" => new Func<long, long>(d =>
                    {
                        var exam = (d * (opNo ?? d)) / 3;
                        return (int) Math.Floor(exam);
                    }),
                    _ => throw new Exception("invalid Input")
                };
                var test = int.Parse(Regex.Match(lines[3], @"\d+").Value);
                var ifTrue = int.Parse(Regex.Match(lines[4], @"\d+").Value);
                var ifFalse = int.Parse(Regex.Match(lines[5], @"\d+").Value);
                monkeys[i] = new Monkey(items, operation, test, ifTrue, ifFalse);
            }

            for (var i = 0; i < 20; i++)
            {
                var c = 0;
                foreach (var monkey in monkeys)
                {
                    monkey.ExamineItems(monkeys, 1);
                    Console.WriteLine($"{i} - {c}");
                    monkeys.ToList().ForEach(e => Console.WriteLine(string.Join(',', e.Items)));
                    ++c;
                }
            }

            var sorted = monkeys.OrderByDescending(e => e.ItemsExamined).ToArray();
            return sorted[0].ItemsExamined * sorted[1].ItemsExamined;
        }

        public static long Part2(int rounds)
        {
            var monkeyInputs = File.OpenText("inputs/11.txt").ReadToEnd().Split("\n\n");
            var monkeys = new Monkey[monkeyInputs.Length];
            for (var i = 0; i < monkeyInputs.Length; i++)
            {
                var lines = monkeyInputs[i].Split('\n');
                var items = Regex.Matches(lines[1], @"\d+").Select(c => long.Parse(c.Value));
                long? opNo = null;
                if (long.TryParse(Regex.Match(lines[2], @"\d+").Value ?? "", out var t))
                    opNo = t;
                var test = int.Parse(Regex.Match(lines[3], @"\d+").Value);
                var operation = Regex.Match(lines[2], @"[\+\*]").Value switch
                {
                    "+" => new Func<long, long>(d => d + (opNo ?? d)),
                    "*" => new Func<long, long>(d => d * (opNo ?? d)),
                    _ => throw new Exception("invalid Input")
                };
                var ifTrue = int.Parse(Regex.Match(lines[4], @"\d+").Value);
                var ifFalse = int.Parse(Regex.Match(lines[5], @"\d+").Value);
                monkeys[i] = new Monkey(items,operation,test,ifTrue,ifFalse);
            }
            var nww = 1;
            foreach (var t in monkeys)
            {
                nww = (nww * t.Test) / Nwd(nww, t.Test);
            }
            for (var i = 0; i < rounds; i++)
            {
                Console.WriteLine($"round {i}:");
                var c = 0;
                foreach (var monkey in monkeys)
                {
                    monkey.ExamineItems(monkeys, nww);
                    Console.WriteLine($"monkey {c} examined {monkey.ItemsExamined} items");
                    ++c;
                }
            }

            var sorted = monkeys.OrderByDescending(e => e.ItemsExamined).ToArray();
            return (long)sorted[0].ItemsExamined * (long)sorted[1].ItemsExamined;
        }

        static int Nwd(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                {
                    a = a % b;
                }
                else
                {
                    b = b % a;
                }
            }

            return a != 0 ? a : b;
        }
    }

    class Monkey
    {
        public Monkey(
            IEnumerable<long> items,
            Func<long, long> operation,
            int test,
            int ifTrue,
            int ifFalse)
        {
            Items = items;
            Operation = operation;
            Test = test;
            TrueThrow = ifTrue;
            FalseThrow = ifFalse;
        }

        public IEnumerable<long> Items { get; set; }
        public Func<long, long> Operation { get; set; }
        public int Test { get; set; }
        public int TrueThrow { get; set; }
        public int FalseThrow { get; set; }
        public int ItemsExamined { get; set; } = 0;

        public void ExamineItems(Monkey[] monkeys, int reductor)
        {
            foreach (var item in Items)
            {
                ++ItemsExamined;
                var examinedItem = Operation(item);
                if (examinedItem > reductor)
                {
                    examinedItem %= reductor;
                }
                if (examinedItem % Test == 0)
                {
                    monkeys[TrueThrow].Items = monkeys[TrueThrow].Items.Append(examinedItem);
                }
                else
                {
                    monkeys[FalseThrow].Items = monkeys[FalseThrow].Items.Append(examinedItem);
                }
            }
            Items = new List<long>();
        }
    }
}