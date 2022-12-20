using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Xsl;

namespace adwent2022
{
    internal static class Day19
    {
        static StreamReader GetInput()
        {
            //return File.OpenText("inputs/19.txt");
            return File.OpenText("inputs/example.txt");
        }

        public static void Part1()
        {
            var input = GetInput().ReadToEnd().Split('\n');

            var blueprints = new List<Blueprint>();
            foreach (var line in input)
            {
                var bp = Regex.Matches(line, @"\d+");
                blueprints.Add(new Blueprint(
                    int.Parse(bp[0].Value),
                    int.Parse(bp[1].Value),
                    int.Parse(bp[2].Value),
                    int.Parse(bp[3].Value),
                    int.Parse(bp[4].Value),
                    int.Parse(bp[5].Value),
                    int.Parse(bp[6].Value)));
            }
            
            var qualitySum = 0;
            foreach (var blueprint in blueprints)
            {
                var robots = new Resources(1, 0, 0, 0);
                var resources = new Resources(0, 0, 0, 0);
                var timeLeft = 24;
                var maxGeodeYield = Simulate(blueprint, robots, resources, timeLeft, null);
                Console.WriteLine($"blueprint: {blueprint.BlueprintId}; max geode yeld {maxGeodeYield}");
                qualitySum += (maxGeodeYield * blueprint.BlueprintId);
            }
            
            Console.WriteLine($"got {qualitySum} quality total");
        }


        public static void Part2()
        {

        }

        static int Simulate(Blueprint blueprint, Resources robots, Resources resources, int timeLeft, Type? robotToBuild)
        {
            if (timeLeft == 0)
                return resources.Geode;

            //copy objects
            var localRobots = (Resources)robots.Clone();
            var localResources = (Resources)resources.Clone();

            //yeld materials
            localResources.Add(localRobots);

            //build robot if available
            if (robotToBuild != null)
            {
                localRobots.Add(blueprint.BuildRobot(localResources, robotToBuild, out localResources));
            }

            var availableRobots = blueprint.RobotsBuildable(localResources);
            var ifWait = Simulate(blueprint, localRobots, localResources, timeLeft - 1, null);
            if (!availableRobots.Any()) return ifWait;
            var maxSimulated = availableRobots.Select(e =>Simulate(blueprint, localRobots, localResources, timeLeft - 1, e)).Max();
            return Math.Max(ifWait, maxSimulated);
        }
    }

    class Blueprint
    {
        public Blueprint(
            int blueprintId,
            int oreRobotOreCost,
            int clayRobotOreCost,
            int obsidianRobotOreCost,
            int obsidianRobotClayCost,
            int geodeRobotOreCost,
            int geodeRobotObsidianCost)
        {
            BlueprintId = blueprintId;
            OreRobot = new OreRobot(new Resources(oreRobotOreCost, 0, 0, 0));
            ClayRobot = new ClayRobot(new Resources(clayRobotOreCost, 0, 0, 0));
            ObsidianRobot = new ObsidianRobot(new Resources(obsidianRobotOreCost, obsidianRobotClayCost, 0, 0));
            GeodeRobot = new GeodeRobot(new Resources(geodeRobotOreCost, 0, geodeRobotObsidianCost, 0));
        }
        public int BlueprintId { get; set; }
        public OreRobot OreRobot { get; set; }
        public ClayRobot ClayRobot { get; set; }
        public ObsidianRobot ObsidianRobot { get; set; }
        public GeodeRobot GeodeRobot { get; set; }

        public List<Type> RobotsBuildable(Resources resources)
        {
            var retList = new List<Type>();
            if (OreRobot.CanBeBuilt(resources))
            {
                retList.Add(typeof(OreRobot));
            }
            if (ClayRobot.CanBeBuilt(resources))
            {
                retList.Add(typeof(ClayRobot));
            }
            if (ObsidianRobot.CanBeBuilt(resources))
            {
                retList.Add(typeof(ObsidianRobot));
            }
            if (GeodeRobot.CanBeBuilt(resources))
            {
                retList.Add(typeof(GeodeRobot));
            }
            return retList;
        }

        public Resources BuildRobot(Resources resources, Type robotType, out Resources resourcesLeft)
        {
            if (robotType == OreRobot.GetType())
            {
                return OreRobot.Build(resources, out resourcesLeft);
            }
            else if (robotType == ClayRobot.GetType())
            {
                return ClayRobot.Build(resources, out resourcesLeft);
            }
            else if (robotType == ObsidianRobot.GetType())
            {
                return ObsidianRobot.Build(resources, out resourcesLeft);
            }
            else if (robotType == GeodeRobot.GetType())
            {
                return GeodeRobot.Build(resources, out resourcesLeft);
            }
            else
            {
                throw new Exception("unsupported");
            }
        }
    }

    abstract class Robot
    {
        public Robot(Resources cost)
        {
            Cost = cost;
        }

        public Resources Cost { get; set; }
        public bool CanBeBuilt(Resources resources)
        {
            return Cost.Ore <= resources.Ore
                && Cost.Clay <= resources.Clay
                && Cost.Obsidian <= resources.Obsidian
                && Cost.Geode <= resources.Geode;
        }
        public abstract Resources Build(Resources resources, out Resources resourcesLeft);
    }

    class OreRobot : Robot
    {
        public OreRobot(Resources cost) : base(cost)
        {
        }

        public override Resources Build(Resources resources, out Resources resourcesLeft)
        {
            if (CanBeBuilt(resources))
            {
                resourcesLeft = resources.Spend(Cost);
                return new Resources(1, 0, 0, 0);
            }
            else
            {
                resourcesLeft = resources;
                return new Resources(0, 0, 0, 0);
            }
        }
    }
    class ClayRobot : Robot
    {
        public ClayRobot(Resources cost) : base(cost)
        {
        }

        public override Resources Build(Resources resources, out Resources resourcesLeft)
        {
            if (CanBeBuilt(resources))
            {
                resourcesLeft = resources.Spend(Cost);
                return new Resources(0, 1, 0, 0);
            }
            else
            {
                resourcesLeft = resources;
                return new Resources(0, 0, 0, 0);
            }
        }
    }
    class ObsidianRobot : Robot
    {
        public ObsidianRobot(Resources cost) : base(cost)
        {
        }

        public override Resources Build(Resources resources, out Resources resourcesLeft)
        {
            if (CanBeBuilt(resources))
            {
                resourcesLeft = resources.Spend(Cost);
                return new Resources(0, 0, 1, 0);
            }
            else
            {
                resourcesLeft = resources;
                return new Resources(0, 0, 0, 0);
            }
        }
    }
    class GeodeRobot : Robot
    {
        public GeodeRobot(Resources cost) : base(cost)
        {
        }

        public override Resources Build(Resources resources, out Resources resourcesLeft)
        {
            if (CanBeBuilt(resources))
            {
                resourcesLeft = resources.Spend(Cost);
                return new Resources(0, 0, 0, 1);
            }
            else
            {
                resourcesLeft = resources;
                return new Resources(0, 0, 0, 0);
            }
        }
    }

    class Resources : ICloneable
    {
        public Resources(int ore, int clay, int obsidian, int geode)
        {
            Ore = ore;
            Clay = clay;
            Obsidian = obsidian;
            Geode = geode;
        }
        public int Ore { get; set; }
        public int Clay { get; set; }
        public int Obsidian { get; set; }
        public int Geode { get; set; }
        public Resources Add(Resources resources)
        {
            Ore += resources.Ore;
            Clay += resources.Clay;
            Obsidian += resources.Obsidian;
            Geode += resources.Geode;
            return this;
        }
        public Resources Spend(Resources resources)
        {
            Ore -= resources.Ore;
            Clay -= resources.Clay;
            Obsidian -= resources.Obsidian;
            Geode -= resources.Geode;
            return this;
        }

        public object Clone()
        {
            return new Resources(Ore, Clay, Obsidian, Geode);
        }
    }
}