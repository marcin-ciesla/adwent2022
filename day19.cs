using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Xsl;

namespace adwent2022
{
    internal static class Day19
    {
        static StreamReader GetInput()
        {
            return File.OpenText("inputs/19.txt");
            //return File.OpenText("inputs/example.txt");
        }

        public static void Part1()
        {
            var input = GetInput().ReadToEnd().Split('\n');

            var blueprints = new List<int[][]>();
            foreach (var line in input)
            {
                var bp = Regex.Matches(line, @"\d+");
                blueprints.Add(new int[][]
                {
                    new int[]{int.Parse(bp[1].Value), 0, 0, 0},
                    new int[]{int.Parse(bp[2].Value), 0, 0, 0},
                    new int[]{int.Parse(bp[3].Value), int.Parse(bp[4].Value), 0, 0 },
                    new int[]{int.Parse(bp[5].Value), 0, int.Parse(bp[6].Value), 0},
                });
            }

            var qualitySum = 0;

            for (int bp = 0; bp < 3; bp++)
            {
                var robots = new int[] { 1, 0, 0, 0 };
                var resources = new int[] { 0, 0, 0, 0 };
                var maxCost = new int[4];
                for (int i = 0; i < 4; i++)
                {
                    maxCost[i] = blueprints[bp].Max(e => e[i]);
                }
                maxCost[3] = int.MaxValue;
                var timeLeft = 32;
                var toCheck = new Queue<Simulation>();
                var maxYield = 0;
                toCheck.Enqueue(new Simulation
                {
                    Resources = resources,
                    Robots = robots,
                    MaxCost = maxCost,
                    TimeLeft = timeLeft,
                    PendingBuild = -1
                });

                while (toCheck.Count > 0)
                {
                    var yield = Simulate(toCheck.Dequeue(), blueprints[bp], toCheck);
                    maxYield = maxYield < yield ? yield : maxYield;
                }

                Console.WriteLine($"blueprint: {bp + 1}; max geode yeld {maxYield}");
                qualitySum += (maxYield * (bp + 1));
            }

            Console.WriteLine($"got {qualitySum} quality total");
        }

        private static int Simulate(Simulation simulation, int[][] blueprint, Queue<Simulation> toCheck)
        {
            if (simulation.TimeLeft == 0)
            {
                return simulation.Resources[3];
            }
            if (simulation.TimeLeft < 0)
            {
                return 0;
            }

            simulation.TimeLeft = simulation.TimeLeft - 1;

            for (int i = 0; i < 4; i++)
            {
                simulation.Resources[i] = simulation.Resources[i] + simulation.Robots[i];
            }

            if (simulation.PendingBuild != -1)
            {
                ++simulation.Robots[simulation.PendingBuild];
                for (int i = 0; i < 4; i++)
                {
                    simulation.Resources[i] = simulation.Resources[i] - blueprint[simulation.PendingBuild][i];
                }
            }
            var noNewRobot = true;
            for (int i = 3; i > -1; i--)
            {
                //var build = simulation.MaxCost[i] * simulation.TimeLeft > (simulation.Robots[i] * simulation.TimeLeft) + simulation.Resources[i];
                var build = simulation.MaxCost[i] > simulation.Robots[i];

                if (!build) continue;

                build = blueprint[i][0] <= simulation.Resources[0] &&
                        blueprint[i][1] <= simulation.Resources[1] &&
                        blueprint[i][2] <= simulation.Resources[2];

                if (build)
                {
                    simulation.PendingBuild = i;
                    simulation.Robots = simulation.Robots.ToArray();
                    simulation.Resources = simulation.Resources.ToArray();
                    toCheck.Enqueue(simulation);
                    noNewRobot = false;
                    if (i == 3)
                    {
                        break;
                    }
                    continue;
                }

                for (int j = 0; j < 3; j++)
                {
                    build = blueprint[i][j] == 0 || simulation.Robots[j] > 0;
                    if (!build) break;
                }

                if (build)
                {
                    var ff = 0;
                    for (int j = 0; j < 3; j++)
                    {
                        while (blueprint[i][j] > simulation.Resources[j] + (simulation.Robots[j] * ff))
                        {
                            ff++;
                        }
                    }
                    if (simulation.TimeLeft - ff >= 0)
                    {
                        toCheck.Enqueue(new Simulation
                        {
                            MaxCost = simulation.MaxCost,
                            PendingBuild = i,
                            Resources = simulation.Resources.Select((e, i) => e + (simulation.Robots[i] * ff)).ToArray(),
                            Robots = simulation.Robots.ToArray(),
                            TimeLeft = simulation.TimeLeft - ff
                        });
                        noNewRobot = false;
                    }
                }
            }
            if (noNewRobot)
            {
                simulation.PendingBuild = -1;
                simulation.Robots = simulation.Robots.ToArray();
                simulation.Resources = simulation.Resources.ToArray();
                toCheck.Enqueue(simulation);
            }
            return 0;
        }

        public static void Part2()
        {

        }
    }

    struct Simulation
    {
        public int[] Robots;
        public int[] Resources;
        public int[] MaxCost;
        public int PendingBuild;
        public int TimeLeft;
    }
}