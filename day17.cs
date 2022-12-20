namespace adwent2022
{
    internal static class Day17
    {
        static StreamReader GetInput()
        {
            return File.OpenText("inputs/17.txt");
            // return File.OpenText("inputs/example.txt");
        }

        public static int Part1(int len)
        {
            var input = GetInput().ReadToEnd();
            var currents = new Queue<int>(input.Select(e => e == '<' ? -1 : 1));
            var ccount = 0;
            var height = 0;
            // 7 width

            //Figures
            var bar = new int[][]
            {
                "··####·".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray()
            };

            var plus = new int[][]
            {
                "···#···".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "··###··".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "···#···".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray()
            };

            var boomerang = new int[][]
            {
                "··###··".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "····#··".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "····#··".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray()
            };

            var pole = new int[][]
            {
                "··#····".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "··#····".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "··#····".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "··#····".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray()
            };

            var box = new int[][]
            {
                "··##···".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "··##···".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray()
            };
            var figures = new Queue<int[][]>(new int[][][] {bar, plus, boomerang, pole, box});
            var fall = false;
            var lines = new List<int[]>();
            var floor = new int[7];
            Array.Fill(floor, 2);
            lines.Add(floor);

            for (var i = 0; i < len; i++)
            {
                while (lines.Last().Sum() == 0)
                {
                    lines.Remove(lines.Last());
                }

                for (var l = 0; l < 3; l++)
                {
                    var line = new int[7];
                    Array.Fill(line, 0);
                    lines.Add(line);
                }

                var figure = figures.Cycle();
                var figBottom = lines.Count;
                lines.AddRange(figure);
                while (true)
                {
                    if (fall)
                    {
                        fall = !fall;
                        var untouched = figBottom - 1;
                        var touched = figure.Length + 1;
                        var shiftArea = lines.Skip(untouched).Take(touched).Select(e => e.ToArray()).ToList();
                        var collision = false;
                        for (var j = 0; j < figure.Length; j++)
                        {
                            for (var pixel = 0; pixel < shiftArea[j].Length; pixel++)
                            {
                                if (shiftArea[j + 1][pixel] != 1)
                                {
                                    continue;
                                }

                                if (shiftArea[j][pixel] == 2)
                                {
                                    collision = true;
                                    break;
                                }

                                shiftArea[j][pixel] = shiftArea[j + 1][pixel];
                                shiftArea[j + 1][pixel] = 0;
                            }

                            if (collision)
                            {
                                break;
                            }
                        }

                        if (collision)
                        {
                            for (var j = 0; j < touched; j++)
                            {
                                lines[untouched + j] = lines[untouched + j].Select(e => e == 1 ? 2 : e).ToArray();
                            }

                            // Draw(lines);
                            break;
                        }

                        for (var j = 0; j < touched; j++)
                        {
                            lines[untouched + j] = shiftArea[j];
                        }

                        // Draw(lines);
                        --figBottom;
                    }
                    else
                    {
                        fall = !fall;
                        var direction = currents.Cycle();
                        ++ccount;
                        var touched = figure.Length;
                        var shiftArea = lines.Skip(figBottom).Take(touched).Select(e => e.ToArray()).ToList();
                        var collision = false;
                        for (var line = 0; line < shiftArea.Count; ++line)
                        {
                            var shift = shiftArea[line].Select(e => e == 1 ? 0 : e).ToArray();
                            for (var pixel = 0; pixel < shiftArea[line].Length; pixel++)
                            {
                                if (shiftArea[line][pixel] != 1)
                                {
                                    continue;
                                }

                                if (pixel + direction >= 0 && pixel + direction < shiftArea[line].Length &&
                                    shiftArea[line][pixel + direction] < 2)
                                {
                                    shift[pixel + direction] = shiftArea[line][pixel];
                                }
                                else
                                {
                                    collision = true;
                                    break;
                                }
                            }

                            if (collision)
                            {
                                break;
                            }


                            shiftArea[line] = shift;
                        }

                        if (collision)
                        {
                            // Draw(lines);
                            continue;
                        }

                        for (var j = 0; j < touched; j++)
                        {
                            lines[figBottom + j] = shiftArea[j];
                        }
                        // Draw(lines);
                    }
                }

                while (lines.Last().Sum() == 0)
                {
                    lines.Remove(lines.Last());
                }

                // height = lines.Count - 1;
                // Console.WriteLine($"after {i+1} rock dropped we are {height} high");
            }

            height = lines.Count - 1;
            Console.WriteLine($"after last rock dropped we are {height} high");
            return height;
        }

        public static void Part2()
        {
            var input = GetInput().ReadToEnd();
            var currents = new Queue<int>(input.Select(e => e == '<' ? -1 : 1));
            var height = 0;
            // 7 width

            //Figures
            var bar = new int[][]
            {
                "··####·".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray()
            };

            var plus = new int[][]
            {
                "···#···".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "··###··".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "···#···".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray()
            };

            var boomerang = new int[][]
            {
                "··###··".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "····#··".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "····#··".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray()
            };

            var pole = new int[][]
            {
                "··#····".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "··#····".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "··#····".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "··#····".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray()
            };

            var box = new int[][]
            {
                "··##···".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray(),
                "··##···".ToCharArray().Select(e => e == '#' ? 1 : 0).ToArray()
            };
            var figures = new Queue<int[][]>(new int[][][] {bar, plus, boomerang, pole, box});
            var fall = false;
            var lines = new List<int[]>();
            var floor = new int[7];
            Array.Fill(floor, 2);
            lines.Add(floor);

            var rocks = 1000000000000;
            var fallen = 0;
            var currentsUsed = 0;

            var firstCycles = new List<Tuple<int, int, int, int>>();
            var secondCycles = new List<Tuple<int, int, int, int>>();
            var thirdCycles = new List<Tuple<int, int, int, int>>();
            var fourthCycles = new List<Tuple<int, int, int, int>>();
            var fifthCycles = new List<Tuple<int, int, int, int>>();

            var cycleRocks = 0;
            var cycleLines = 0;

            while (rocks-- > 0)
            {
                fallen++;

                for (var l = 0; l < 3; l++)
                {
                    var line = new int[7];
                    Array.Fill(line, 0);
                    lines.Add(line);
                }

                var figure = figures.Cycle();
                var figBottom = lines.Count;
                lines.AddRange(figure);
                while (true)
                {
                    if (fall)
                    {
                        fall = !fall;
                        var untouched = figBottom - 1;
                        var touched = figure.Length + 1;
                        var shiftArea = lines.Skip(untouched).Take(touched).Select(e => e.ToArray()).ToList();
                        var collision = false;
                        for (var j = 0; j < figure.Length; j++)
                        {
                            for (var pixel = 0; pixel < shiftArea[j].Length; pixel++)
                            {
                                if (shiftArea[j + 1][pixel] != 1)
                                {
                                    continue;
                                }

                                if (shiftArea[j][pixel] == 2)
                                {
                                    collision = true;
                                    break;
                                }

                                shiftArea[j][pixel] = shiftArea[j + 1][pixel];
                                shiftArea[j + 1][pixel] = 0;
                            }

                            if (collision)
                            {
                                break;
                            }
                        }

                        if (collision)
                        {
                            for (var j = 0; j < touched; j++)
                            {
                                lines[untouched + j] = lines[untouched + j].Select(e => e == 1 ? 2 : e).ToArray();
                            }

                            // Draw(lines);
                            break;
                        }

                        for (var j = 0; j < touched; j++)
                        {
                            lines[untouched + j] = shiftArea[j];
                        }

                        // Draw(lines);
                        --figBottom;
                    }
                    else
                    {
                        fall = !fall;
                        var direction = currents.Cycle();
                        currentsUsed++;
                        var touched = figure.Length;
                        var shiftArea = lines.Skip(figBottom).Take(touched).Select(e => e.ToArray()).ToList();
                        var collision = false;
                        for (var line = 0; line < shiftArea.Count; ++line)
                        {
                            var shift = shiftArea[line].Select(e => e == 1 ? 0 : e).ToArray();
                            for (var pixel = 0; pixel < shiftArea[line].Length; pixel++)
                            {
                                if (shiftArea[line][pixel] != 1)
                                {
                                    continue;
                                }

                                if (pixel + direction >= 0 && pixel + direction < shiftArea[line].Length &&
                                    shiftArea[line][pixel + direction] < 2)
                                {
                                    shift[pixel + direction] = shiftArea[line][pixel];
                                }
                                else
                                {
                                    collision = true;
                                    break;
                                }
                            }

                            if (collision)
                            {
                                break;
                            }


                            shiftArea[line] = shift;
                        }

                        if (collision)
                        {
                            // Draw(lines);
                            continue;
                        }

                        for (var j = 0; j < touched; j++)
                        {
                            lines[figBottom + j] = shiftArea[j];
                        }
                        // Draw(lines);
                    }
                }

                while (lines.Last().Sum() == 0)
                {
                    lines.Remove(lines.Last());
                }


                #region find cycle

                var newCycle = new Tuple<int, int, int, int>(fallen % 5, currentsUsed % currents.Count, fallen,
                    lines.Count - 1);
                if (firstCycles.Any(e => e.Item1 == newCycle.Item1 && e.Item2 == newCycle.Item2))
                {
                    if (secondCycles.Any(e => e.Item1 == newCycle.Item1 && e.Item2 == newCycle.Item2))
                    {
                        if (thirdCycles.Any(e => e.Item1 == newCycle.Item1 && e.Item2 == newCycle.Item2))
                        {
                            if (fourthCycles.Any(e => e.Item1 == newCycle.Item1 && e.Item2 == newCycle.Item2))
                            {
                                var secondCycle = secondCycles.FindIndex(e =>
                                    e.Item1 == newCycle.Item1 && e.Item2 == newCycle.Item2)!;
                                var thirdCycle = thirdCycles.FindIndex(e =>
                                    e.Item1 == newCycle.Item1 && e.Item2 == newCycle.Item2)!;

                                if (secondCycle == thirdCycle)
                                {
                                    var sc = secondCycles[secondCycle];
                                    var tc = thirdCycles[thirdCycle];
                                    cycleRocks = tc.Item3 - sc.Item3;
                                    cycleLines = tc.Item4 - sc.Item4;
                                    break;
                                }
                            }
                            else
                            {
                                fourthCycles.Add(newCycle);
                            }
                        }
                        else
                        {
                            thirdCycles.Add(newCycle);
                        }
                    }
                    else
                    {
                        secondCycles.Add(newCycle);
                    }
                }
                else
                {
                    firstCycles.Add(newCycle);
                }

                #endregion
            }
            var cycled = rocks / cycleRocks;
            var linesCycled = cycleLines * cycled;

            var rest = rocks % cycleRocks;
            ThrowRocks((int) rest, lines, figures, currents);

            height = lines.Count - 1;

            var sum = linesCycled + height;

            Console.WriteLine($"after last rock dropped we are {sum} high");
        }

        static void Draw(List<int[]> lines)
        {
            lines.Reverse();
            Console.CursorTop = 0;
            Console.WriteLine(
                $"{string.Join('\n', lines.Select(e => string.Join("", e.Select(f => f switch {2 => '#', 1 => '0', _ => '·'}).ToArray())))}");
            lines.Reverse();
        }

        static int[][] Cycle(this Queue<int[][]> queue)
        {
            var item = queue.Dequeue();
            queue.Enqueue(item);
            return item;
        }

        static int Cycle(this Queue<int> queue)
        {
            var item = queue.Dequeue();
            queue.Enqueue(item);
            return item;
        }

        static void ThrowRocks(int rocks, List<int[]> lines, Queue<int[][]> figures, Queue<int> currents)
        {
            var fall = false;

            for (var i = 0; i < rocks; i++)
            {
                for (var l = 0; l < 3; l++)
                {
                    var line = new int[7];
                    Array.Fill(line, 0);
                    lines.Add(line);
                }

                var figure = figures.Cycle();
                var figBottom = lines.Count;
                lines.AddRange(figure);
                while (true)
                {
                    if (fall)
                    {
                        fall = !fall;
                        var untouched = figBottom - 1;
                        var touched = figure.Length + 1;
                        var shiftArea = lines.Skip(untouched).Take(touched).Select(e => e.ToArray()).ToList();
                        var collision = false;
                        for (var j = 0; j < figure.Length; j++)
                        {
                            for (var pixel = 0; pixel < shiftArea[j].Length; pixel++)
                            {
                                if (shiftArea[j + 1][pixel] != 1)
                                {
                                    continue;
                                }

                                if (shiftArea[j][pixel] == 2)
                                {
                                    collision = true;
                                    break;
                                }

                                shiftArea[j][pixel] = shiftArea[j + 1][pixel];
                                shiftArea[j + 1][pixel] = 0;
                            }

                            if (collision)
                            {
                                break;
                            }
                        }

                        if (collision)
                        {
                            for (var j = 0; j < touched; j++)
                            {
                                lines[untouched + j] = lines[untouched + j].Select(e => e == 1 ? 2 : e).ToArray();
                            }

                            // Draw(lines);
                            break;
                        }

                        for (var j = 0; j < touched; j++)
                        {
                            lines[untouched + j] = shiftArea[j];
                        }

                        // Draw(lines);
                        --figBottom;
                    }
                    else
                    {
                        fall = !fall;
                        var direction = currents.Cycle();
                        var touched = figure.Length;
                        var shiftArea = lines.Skip(figBottom).Take(touched).Select(e => e.ToArray()).ToList();
                        var collision = false;
                        for (var line = 0; line < shiftArea.Count; ++line)
                        {
                            var shift = shiftArea[line].Select(e => e == 1 ? 0 : e).ToArray();
                            for (var pixel = 0; pixel < shiftArea[line].Length; pixel++)
                            {
                                if (shiftArea[line][pixel] != 1)
                                {
                                    continue;
                                }

                                if (pixel + direction >= 0 && pixel + direction < shiftArea[line].Length &&
                                    shiftArea[line][pixel + direction] < 2)
                                {
                                    shift[pixel + direction] = shiftArea[line][pixel];
                                }
                                else
                                {
                                    collision = true;
                                    break;
                                }
                            }

                            if (collision)
                            {
                                break;
                            }


                            shiftArea[line] = shift;
                        }

                        if (collision)
                        {
                            // Draw(lines);
                            continue;
                        }

                        for (var j = 0; j < touched; j++)
                        {
                            lines[figBottom + j] = shiftArea[j];
                        }
                        // Draw(lines);
                    }
                }

                while (lines.Last().Sum() == 0)
                {
                    lines.Remove(lines.Last());
                }

                // var coverage = new int[7];
                // Array.Fill(coverage, 0);
                // var toSkip = 0;
                // for (var sk = 1; sk < (hasFloor ? lines.Count : lines.Count - 1); sk++)
                // {
                //     coverage = coverage.Zip(lines[^sk]).Select(e => e.First + e.Second).ToArray();
                //     if (coverage.Any(e => e == 0)) continue;
                //     toSkip = lines.Count - sk;
                //     hasFloor = false;
                //     break;
                // }
                //
                // heightStored += toSkip;
                // lines = lines.Skip(toSkip).ToList();
                // height = lines.Count - 1;
                // Console.WriteLine($"after {i+1} rock dropped we are {height} high");
            }
        }
    }
}