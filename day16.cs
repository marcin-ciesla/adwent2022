using System.IO.Compression;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using Microsoft.VisualBasic;

namespace adwent2022
{
    internal static class Day16
    {
        public static void Part1()
        {
            var input = File.OpenText("inputs/16.txt").ReadToEnd().Split('\n');
            var valves = new Dictionary<string, int>();
            var tunnels = new List<string[]>();
            foreach (var line in input)
            {
                var re = Regex.Match(line,
                    @"Valve (?'valve'\w\w) has flow rate=(?'flowRate'\d+); tunnels* leads* to valves* (?:(?'destinations'\w\w)(?:, )*)+");
                valves.Add(re.Groups["valve"].Value, int.Parse(re.Groups["flowRate"].Value));
                tunnels.AddRange(re.Groups["destinations"].Captures
                    .Select(e => new string[] {re.Groups["valve"].Value, e.Value}));
            }

            var valvesToOpen = valves.Where(e => e.Value > 0).Select(e => e.Key).ToList();
            var routes = new List<Tuple<string, string, int>>();

            foreach (var valve in valvesToOpen)
            {
                routes.AddRange(FindShortestConnections(valve, tunnels.ToList(), valvesToOpen.ToList())
                    .Select(e => new Tuple<string, string, int>(valve, e.Item1, e.Item2)));
            }

            routes = routes.Distinct().ToList();

            var toOpenFirst = FindShortestConnections("AA", tunnels.ToList(), valvesToOpen.ToList()).ToList();
            var myTimeRemaining = 30;

            var maxPressure = 0;
            foreach (var (valve, toOpen) in toOpenFirst)
            {
                var pressure = valves[valve] * (myTimeRemaining - toOpen);
                var topRemaining = GetMaxPressure(
                    myTimeRemaining - toOpen,
                    valve,
                    valvesToOpen.Where(e => e != valve).ToList(),
                    routes,
                    valves);
                maxPressure = pressure + topRemaining > maxPressure ? pressure + topRemaining : maxPressure;
            }

            Console.WriteLine($"Best I can do is {maxPressure}");
        }

        public static void Part2()
        {
            var input = File.OpenText("inputs/16.txt").ReadToEnd().Split('\n');
            var valves = new Dictionary<string, int>();
            var tunnels = new List<string[]>();
            foreach (var line in input)
            {
                var re = Regex.Match(line,
                    @"Valve (?'valve'\w\w) has flow rate=(?'flowRate'\d+); tunnels* leads* to valves* (?:(?'destinations'\w\w)(?:, )*)+");
                valves.Add(re.Groups["valve"].Value, int.Parse(re.Groups["flowRate"].Value));
                tunnels.AddRange(re.Groups["destinations"].Captures
                    .Select(e => new string[] {re.Groups["valve"].Value, e.Value}));
            }

            var valvesToOpen = valves.Where(e => e.Value > 0).Select(e => e.Key).ToList();
            var routes = new List<Tuple<string, string, int>>();

            foreach (var valve in valvesToOpen)
            {
                routes.AddRange(FindShortestConnections(valve, tunnels.ToList(), valvesToOpen.ToList())
                    .Select(e => new Tuple<string, string, int>(valve, e.Item1, e.Item2)));
            }

            routes = routes.Distinct().ToList();

            var toOpenFirst = FindShortestConnections("AA", tunnels.ToList(), valvesToOpen.ToList()).ToList();
            var myTimeRemaining = 26;
            var elephantTimeRemaining = 26;

            var maxPressure = 0;
            foreach (var (myValve, toOpen) in toOpenFirst)
            {
                var pressure = valves[myValve] * (myTimeRemaining - toOpen);
                foreach (var (elephantValve, toOpenElephant) in toOpenFirst.Where(e => e.Item1 != myValve))
                {
                    var ePressure = valves[elephantValve] * (elephantTimeRemaining - toOpenElephant);
                    var topRemaining = GetMaxPressureWithElephant(
                        myTimeRemaining - toOpen,
                        elephantTimeRemaining - toOpenElephant,
                        myValve,
                        elephantValve,
                        valvesToOpen.Where(e => e != myValve && e != elephantValve).ToList(),
                        routes,
                        valves);
                    maxPressure = pressure + ePressure + topRemaining > maxPressure ? pressure + ePressure + topRemaining : maxPressure;
                }
            }

            Console.WriteLine($"Best we can do is {maxPressure}");
        }

        static int GetMaxPressure(
            int myTimeRemaining,
            string currentValve,
            List<string> remainingValves,
            List<Tuple<string, string, int>> routes,
            Dictionary<string, int> valves)
        {
            var maxPressure = 0;
            foreach (var toGo in remainingValves)
            {
                var (_, _, toOpen) = routes.First(e => e.Item1 == currentValve && e.Item2 == toGo);
                if (myTimeRemaining - toOpen <= 0) continue;

                var pressure = valves[toGo] * (myTimeRemaining - toOpen);
                var topRemaining = GetMaxPressure(
                    myTimeRemaining - toOpen,
                    toGo,
                    remainingValves.Where(e => e != toGo).ToList(),
                    routes,
                    valves);
                maxPressure = pressure + topRemaining > maxPressure ? pressure + topRemaining : maxPressure;
            }

            return maxPressure;
        }

        static int GetMaxPressureWithElephant(
            int myTimeRemaining,
            int elephantTimeRemaining,
            string myCurrentValve,
            string elephantCurrentValve,
            List<string> remainingValves,
            List<Tuple<string, string, int>> routes,
            Dictionary<string, int> valves)
        {
            var maxPressure = 0;
            foreach (var toGo in remainingValves)
            {
                int pressure;
                int topRemaining;
                if (myTimeRemaining > elephantTimeRemaining)
                {
                    var (_, _, toOpen) = routes.First(e => e.Item1 == myCurrentValve && e.Item2 == toGo);
                    if (myTimeRemaining - toOpen <= 0) continue;

                    pressure = valves[toGo] * (myTimeRemaining - toOpen);
                    topRemaining = GetMaxPressureWithElephant(
                        myTimeRemaining - toOpen,
                        elephantTimeRemaining,
                        toGo,
                        elephantCurrentValve,
                        remainingValves.Where(e => e != toGo).ToList(),
                        routes,
                        valves);
                }
                else
                {
                    var (_, _, toOpen) = routes.First(e => e.Item1 == elephantCurrentValve && e.Item2 == toGo);
                    if (elephantTimeRemaining - toOpen <= 0) continue;

                    pressure = valves[toGo] * (elephantTimeRemaining - toOpen);
                    topRemaining = GetMaxPressureWithElephant(
                        myTimeRemaining,
                        elephantTimeRemaining - toOpen,
                        myCurrentValve,
                        toGo,
                        remainingValves.Where(e => e != toGo).ToList(),
                        routes,
                        valves);
                }

                maxPressure = pressure + topRemaining > maxPressure ? pressure + topRemaining : maxPressure;
            }

            return maxPressure;
        }

        static List<string[]> GetTunnelsConnected(string valve, IEnumerable<string[]> tunnels)
        {
            return tunnels.Where(e => e[0] == valve).ToList();
        }

        static IEnumerable<Tuple<string, int>> FindShortestConnections(
            string startingValve,
            List<string[]> tunnels,
            List<string> valvesToFind)
        {
            var toCheck = new Queue<Tuple<string, int>>();
            toCheck.Enqueue(new Tuple<string, int>(startingValve, 0));
            var visited = new List<string>();

            while (toCheck.Count > 0 && valvesToFind.Any())
            {
                var current = toCheck.Dequeue();
                var tunnelsConnected = GetTunnelsConnected(current.Item1, tunnels);

                foreach (var tunnel in tunnelsConnected)
                {
                    tunnels.Remove(tunnel);
                    toCheck.Enqueue(new Tuple<string, int>(tunnel[1], current.Item2 + 1));
                    if (visited.Contains(tunnel[1])) continue;
                    if (valvesToFind.All(e => e != tunnel[1])) continue;

                    yield return new Tuple<string, int>(tunnel[1], current.Item2 + 2); //this will include opening valve
                    visited.Add(tunnel[1]);
                    valvesToFind.Remove(current.Item1);
                }
            }
        }
    }
}