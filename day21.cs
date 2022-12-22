using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Xsl;

namespace adwent2022
{
    internal static class Day21
    {
        static StreamReader GetInput()
        {
            return File.OpenText("inputs/21.txt");
            // return File.OpenText("inputs/example.txt");
        }

        public static void P0()
        {
            var inp = GetInput().ReadToEnd().Split('\n').Select(e => e.Split(':'));
            var keys = inp.Select(e => e[0]).ToList();
            var vals = string.Join(' ', inp.Select(e => e[1]));
            foreach (var key in keys)
            {
                Console.WriteLine($" {key}: {Regex.Matches(vals, key).Count}");
            }
        }

        public static void Part2()
        {
            var input = GetInput().ReadToEnd().Split('\n')
                .Select(e => e.Split(':')); //.Select(f => new KeyValuePair<string, string>(f[0], f[1].Trim()));
            var numbers = new Dictionary<string, long>();
            var equations = new Dictionary<string, string>();
            foreach (var line in input)
            {
                var numbersFinder = Regex.Match(line[1], @"\d+");
                if (numbersFinder.Success)
                {
                    numbers.Add(line[0], long.Parse(numbersFinder.Groups[0].Value));
                }
                else
                {
                    equations.Add(line[0], line[1].Trim());
                }
            }

            var unknown = "humn";
            var unknowns = new List<string>() { "humn" };
            while (true)
            {
                unknown = equations.First(e => e.Value.Contains(unknown)).Key;
                unknowns.Add(unknown);
                if (unknown == "root")
                {
                    break;
                }
            }

            var rootEq = Regex.Match(equations["root"], @"(?'first'\w+)\s(?'operator'[-+/*])\s(?'second'\w+)");
            long result = 0;
            if (unknowns.Contains(rootEq.Groups["first"].Value))
            {
                unknown = rootEq.Groups["first"].Value;
                result = CalculateNode(rootEq.Groups["second"].Value, numbers, equations);
            }
            else if (unknowns.Contains(rootEq.Groups["second"].Value))
            {
                unknown = rootEq.Groups["second"].Value;
                result = CalculateNode(rootEq.Groups["first"].Value, numbers, equations);
            }

            while (!numbers.ContainsKey(unknown))
            {
                var eq = Regex.Match(equations[unknown], @"(?'first'\w+)\s(?'operator'[-+/*])\s(?'second'\w+)");
                long reductor;
                if (unknowns.Contains(eq.Groups["first"].Value))
                {
                    reductor = numbers.ContainsKey(eq.Groups["second"].Value) 
                        ? numbers[eq.Groups["second"].Value] 
                        : CalculateNode(eq.Groups["second"].Value, numbers, equations);
                    unknown = eq.Groups["first"].Value;
                    result = eq.Groups["operator"].Value switch
                    {
                        "+" => result - reductor,
                        "-" => reductor + result,
                        "*" => result / reductor,
                        "/" => reductor * result,
                        _ => throw new Exception("unsupported equation"),
                    };
                }
                else if (unknowns.Contains(eq.Groups["second"].Value))
                {
                    reductor = numbers.ContainsKey(eq.Groups["first"].Value) 
                        ? numbers[eq.Groups["first"].Value] 
                        : CalculateNode(eq.Groups["first"].Value, numbers, equations);
                    unknown = eq.Groups["second"].Value;
                    result = eq.Groups["operator"].Value switch
                    {
                        "+" => result - reductor,
                        "-" => reductor - result,
                        "*" => result / reductor,
                        "/" => reductor / result,
                        _ => throw new Exception("unsupported equation"),
                    };
                }
            }

            Console.WriteLine($"humn = {result}");
        }

        private static long CalculateNode(string root, Dictionary<string, long> numbers,
            Dictionary<string, string> equations)
        {
            var equationStack = new Stack<KeyValuePair<string, string>>();
            equationStack.Push(equations.First(e => e.Key == root));
            long result = 0;
            while (equationStack.Any())
            {
                var eq = Regex.Match(equationStack.Peek().Value, @"(?'first'\w+)\s(?'operator'[-+/*])\s(?'second'\w+)");
                var canEvaluate = true;
                if (!numbers.ContainsKey(eq.Groups["first"].Value))
                {
                    canEvaluate = false;
                    equationStack.Push(equations.First(e => e.Key == eq.Groups["first"].Value));
                }

                if (!numbers.ContainsKey(eq.Groups["second"].Value))
                {
                    canEvaluate = false;
                    equationStack.Push(equations.First(e => e.Key == eq.Groups["second"].Value));
                }

                if (canEvaluate)
                {
                    var key = equationStack.Pop().Key;
                    result = eq.Groups["operator"].Value switch
                    {
                        "+" => numbers[eq.Groups["first"].Value] + numbers[eq.Groups["second"].Value],
                        "-" => numbers[eq.Groups["first"].Value] - numbers[eq.Groups["second"].Value],
                        "*" => numbers[eq.Groups["first"].Value] * numbers[eq.Groups["second"].Value],
                        "/" => numbers[eq.Groups["first"].Value] / numbers[eq.Groups["second"].Value],
                        _ => throw new Exception("unsupported equation"),
                    };
                    if (!numbers.ContainsKey(key))
                    {
                        numbers.Add(key, result);
                    }
                }
            }

            return result;
        }

        public static void Part1()
        {
            {
                var input = GetInput().ReadToEnd().Split('\n')
                    .Select(e => e.Split(':')); //.Select(f => new KeyValuePair<string, string>(f[0], f[1].Trim()));
                var numbers = new Dictionary<string, long>();
                var equations = new Dictionary<string, string>();
                foreach (var line in input)
                {
                    var numbersFinder = Regex.Match(line[1], @"\d+");
                    if (numbersFinder.Success)
                    {
                        numbers.Add(line[0], long.Parse(numbersFinder.Groups[0].Value));
                    }
                    else
                    {
                        equations.Add(line[0], line[1].Trim());
                    }
                }

                var equationStack = new Stack<KeyValuePair<string, string>>();

                equationStack.Push(equations.First(e => e.Key == "root"));

                while (equationStack.Any())
                {
                    var eq = Regex.Match(equationStack.Peek().Value,
                        @"(?'first'\w+)\s(?'operator'[-+/*])\s(?'second'\w+)");
                    var canEvaluate = true;
                    if (!numbers.ContainsKey(eq.Groups["first"].Value))
                    {
                        canEvaluate = false;
                        equationStack.Push(equations.First(e => e.Key == eq.Groups["first"].Value));
                    }

                    if (!numbers.ContainsKey(eq.Groups["second"].Value))
                    {
                        canEvaluate = false;
                        equationStack.Push(equations.First(e => e.Key == eq.Groups["second"].Value));
                    }

                    if (canEvaluate)
                    {
                        var key = equationStack.Pop().Key;
                        switch (eq.Groups["operator"].Value)
                        {
                            case "+":
                                numbers.Add(key,
                                    numbers[eq.Groups["first"].Value] + numbers[eq.Groups["second"].Value]);
                                break;
                            case "-":
                                numbers.Add(key,
                                    numbers[eq.Groups["first"].Value] - numbers[eq.Groups["second"].Value]);
                                break;
                            case "*":
                                numbers.Add(key,
                                    numbers[eq.Groups["first"].Value] * numbers[eq.Groups["second"].Value]);
                                break;
                            case "/":
                                numbers.Add(key,
                                    numbers[eq.Groups["first"].Value] / numbers[eq.Groups["second"].Value]);
                                break;
                            default:
                                throw new Exception("unsupported equation");
                        }
                    }
                }

                Console.WriteLine($"root equals {numbers["root"]}");
            }
        }
    }
}