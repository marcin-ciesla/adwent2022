using System.Net;
using System.Xml.Xsl;

namespace adwent2022
{
    internal static class Day18
    {
        static StreamReader GetInput()
        {
            return File.OpenText("inputs/18.txt");
            // return File.OpenText("inputs/example.txt");
        }

        static int XStart;
        static int YStart;
        static int ZStart;
        static int XEnd;
        static int YEnd;
        static int ZEnd;

        public static void Part1()
        {
            var input = GetInput().ReadToEnd().Split('\n')
                .Select(e => e.Split(',')
                    .Select(int.Parse)
                    .ToArray())
                .ToList();
            var hiddenSides = 0;

            var xs = input.Select(e => e[0]).Distinct();
            var ys = input.Select(e => e[1]).Distinct();
            var zs = input.Select(e => e[2]).Distinct();

            foreach (var x in xs)
            {
                foreach (var y in ys)
                {
                    var ztc = input.Where(e => e[0] == x && e[1] == y).Select(e => e[2]);
                    foreach (var z in ztc)
                    {
                        if (ztc.Any(e => e == z + 1))
                        {
                            hiddenSides++;
                        }

                        if (ztc.Any(e => e == z - 1))
                        {
                            hiddenSides++;
                        }
                    }
                }
            }

            foreach (var y in ys)
            {
                foreach (var z in zs)
                {
                    var xtc = input.Where(e => e[2] == z && e[1] == y).Select(e => e[0]);
                    foreach (var x in xtc)
                    {
                        if (xtc.Any(e => e == x + 1))
                        {
                            hiddenSides++;
                        }

                        if (xtc.Any(e => e == x - 1))
                        {
                            hiddenSides++;
                        }
                    }
                }
            }

            foreach (var z in zs)
            {
                foreach (var x in xs)
                {
                    var ytc = input.Where(e => e[2] == z && e[0] == x).Select(e => e[1]);
                    foreach (var y in ytc)
                    {
                        if (ytc.Any(e => e == y + 1))
                        {
                            hiddenSides++;
                        }

                        if (ytc.Any(e => e == y - 1))
                        {
                            hiddenSides++;
                        }
                    }
                }
            }

            // subGroups.Select(sg => sg.OrderBy(e => e.sg.[2]));
            var sides = (input.Count * 6) - hiddenSides;
            Console.WriteLine($"surface = {sides}");
        }

        public static void Part2b()
        {
            var input = GetInput().ReadToEnd().Split('\n')
                .Select(e => e.Split(',')
                    .Select(int.Parse)
                    .ToArray())
                .ToList();
            var hiddenSides = 0;

            var xs = input.Select(e => e[0]).Distinct();
            var ys = input.Select(e => e[1]).Distinct();
            var zs = input.Select(e => e[2]).Distinct();

            foreach (var x in xs)
            {
                foreach (var y in ys)
                {
                    var ztc = input.Where(e => e[0] == x && e[1] == y).Select(e => e[2]);
                    foreach (var z in ztc)
                    {
                        if (ztc.Any(e => e == z + 1))
                        {
                            hiddenSides++;
                        }

                        if (ztc.Any(e => e == z - 1))
                        {
                            hiddenSides++;
                        }

                        if (z > ztc.Min() && z < ztc.Max())
                        {
                            hiddenSides++;
                        }
                    }
                }
            }

            foreach (var y in ys)
            {
                foreach (var z in zs)
                {
                    var xtc = input.Where(e => e[2] == z && e[1] == y).Select(e => e[0]);
                    foreach (var x in xtc)
                    {
                        if (xtc.Any(e => e == x + 1))
                        {
                            hiddenSides++;
                        }

                        if (xtc.Any(e => e == x - 1))
                        {
                            hiddenSides++;
                        }

                        if (x > xtc.Min() && x < xtc.Max())
                        {
                            hiddenSides++;
                        }
                    }
                }
            }

            foreach (var z in zs)
            {
                foreach (var x in xs)
                {
                    var ytc = input.Where(e => e[2] == z && e[0] == x).Select(e => e[1]);
                    foreach (var y in ytc)
                    {
                        if (ytc.Any(e => e == y + 1))
                        {
                            hiddenSides++;
                        }

                        if (ytc.Any(e => e == y - 1))
                        {
                            hiddenSides++;
                        }

                        if (y > ytc.Min() && y < ytc.Max())
                        {
                            hiddenSides++;
                        }
                    }
                }
            }

            var sides = (input.Count * 6) - hiddenSides;
            Console.WriteLine($"surface = {sides}");
        }

        public static void Part2()
        {
            var input = GetInput().ReadToEnd().Split('\n')
                .Select(e => e.Split(',')
                    .Select(int.Parse)
                    .ToArray())
                .ToList();

            XStart = input.Select(e => e[0]).Min() - 1;
            YStart = input.Select(e => e[1]).Min() - 1;
            ZStart = input.Select(e => e[2]).Min() - 1;
            XEnd = input.Select(e => e[0]).Max() + 1;
            YEnd = input.Select(e => e[1]).Max() + 1;
            ZEnd = input.Select(e => e[2]).Max() + 1;

            var start = new int[] {XStart, YStart, ZStart};
            var toCheck = new Queue<int[]>();
            var water = new List<int[]> {start};
            toCheck.Enqueue(start);
            while (toCheck.Any())
            {
                GetAdjacent(toCheck.Dequeue(), water, input, toCheck);
            }
            
            var hiddenSides = 0;
            var xs = water.Select(e => e[0]).Distinct();
            var ys = water.Select(e => e[1]).Distinct();
            var zs = water.Select(e => e[2]).Distinct();

            foreach (var x in xs)
            {
                foreach (var y in ys)
                {
                    var ztc = water.Where(e => e[0] == x && e[1] == y).Select(e => e[2]);
                    foreach (var z in ztc)
                    {
                        if (ztc.Any(e => e == z + 1))
                        {
                            hiddenSides++;
                        }

                        if (ztc.Any(e => e == z - 1))
                        {
                            hiddenSides++;
                        }

                        if (z == ZStart || z == ZEnd)
                        {
                            hiddenSides++;
                        }
                    }
                }
            }

            foreach (var y in ys)
            {
                foreach (var z in zs)
                {
                    var xtc = water.Where(e => e[2] == z && e[1] == y).Select(e => e[0]);
                    foreach (var x in xtc)
                    {
                        if (xtc.Any(e => e == x + 1))
                        {
                            hiddenSides++;
                        }

                        if (xtc.Any(e => e == x - 1))
                        {
                            hiddenSides++;
                        }

                        if (x == XStart || x == XEnd)
                        {
                            hiddenSides++;
                        }
                    }
                }
            }

            foreach (var z in zs)
            {
                foreach (var x in xs)
                {
                    var ytc = water.Where(e => e[2] == z && e[0] == x).Select(e => e[1]);
                    foreach (var y in ytc)
                    {
                        if (ytc.Any(e => e == y + 1))
                        {
                            hiddenSides++;
                        }

                        if (ytc.Any(e => e == y - 1))
                        {
                            hiddenSides++;
                        }

                        if (y == YStart || y == YEnd)
                        {
                            hiddenSides++;
                        }
                    }
                }
            }

            // subGroups.Select(sg => sg.OrderBy(e => e.sg.[2]));
            var sides = (water.Count * 6) - hiddenSides;
            Console.WriteLine($"surface = {sides}");
 
        }

        static void GetAdjacent(int[] particle, List<int[]> water, List<int[]> magma, Queue<int[]> toCheck)
        {
            var potential = new List<int[]>
            {
                new[] {Math.Min(particle[0] + 1, XEnd), particle[1], particle[2]},
                new[] {Math.Max(particle[0] - 1, XStart), particle[1], particle[2]},
                new[] {particle[0], Math.Min(particle[1] + 1, YEnd), particle[2]},
                new[] {particle[0], Math.Max(particle[1] - 1, YStart), particle[2]},
                new[] {particle[0], particle[1], Math.Min(particle[2] + 1, ZEnd)},
                new[] {particle[0], particle[1], Math.Max(particle[2] - 1, ZStart)},
            };
            foreach (var newParticle in potential)
            {
                if (magma.Exists(f=> newParticle[0] == f[0] && newParticle[1] == f[1] && newParticle[2] == f[2])
                    || water.Exists(f=> newParticle[0] == f[0] && newParticle[1] == f[1] && newParticle[2] == f[2]))
                {
                    continue;
                }
                water.Add(newParticle);
                toCheck.Enqueue(newParticle);
            }
        }
    }
}