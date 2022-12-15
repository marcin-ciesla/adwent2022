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
    internal static class Day15
    {
        public static void Part1()
        {
            var input = File.OpenText("inputs/15.txt").ReadToEnd().Split('\n');
            var sensors = new List<int[]>();
            var beacons = new List<int[]>();
            var ranges = new List<int>();
            var checkedLine = 2000000;
            foreach (var line in input)
            {
                var match = Regex.Matches(line, @"\-*\d+");
                sensors.Add(new[] {int.Parse(match[0].Value), int.Parse(match[1].Value)});
                beacons.Add(new[] {int.Parse(match[2].Value), int.Parse(match[3].Value)});
            }

            for (var i = 0; i < sensors.Count; i++)
            {
                // var hDist = sensors[i][0] > beacons[i][0] ? sensors[i][0] - beacons[i][0] : beacons[i][0] - sensors[i][0];
                // var vDist = sensors[i][1] > beacons[i][1] ? sensors[i][1] - beacons[i][1] : beacons[i][1] - sensors[i][1];
                // var scanRange = Math.Abs(sensors[i][0] - beacons[i][0]) + Math.Abs(sensors[i][1] - beacons[i][1]);
                // var beaconDistance = new[]
                //     {Math.Abs(sensors[i][0] - beacons[i][0]), Math.Abs(sensors[i][1] - beacons[i][1])};
                // Console.WriteLine(
                //     $"beacon is horizontally {beaconDistance[0]} and vertically {beaconDistance[1]} away");
                // Console.WriteLine($"Scan range is {scanRange}");
                ranges.Add(Math.Abs(sensors[i][0] - beacons[i][0]) + Math.Abs(sensors[i][1] - beacons[i][1]));
            }

            var sensorsCovering =
                sensors.Where((e, i) => checkedLine >= e[1] - ranges[i] && checkedLine <= e[1] + ranges[i]);

            var lineCover = new List<int>();
            for (var i = 0; i < sensors.Count; i++)
            {
                if (checkedLine >= sensors[i][1] - ranges[i] && checkedLine <= sensors[i][1] + ranges[i])
                {
                    var hdist = ranges[i] - Math.Abs(checkedLine - sensors[i][1]);
                    // lineCover.Add(new []{sensors[i][0] - hdist, sensors[i][0] + hdist});
                    lineCover.AddRange(Enumerable.Range(sensors[i][0] - hdist, (hdist * 2) + 1));
                }
            }

            lineCover = lineCover.Distinct().ToList();
            foreach (var beacon in beacons.Where(e => e[1] == checkedLine))
            {
                lineCover.Remove(beacon[0]);
            }

            Console.WriteLine($"line coverage = {lineCover.Count()}");

            // var ledge = sensorsCovering.Select((t, i) => t[0] - ranges[i]).Min();
            // var redge = sensorsCovering.Select((t, i) => t[0] + ranges[i]).Max();
            // var width = Math.Abs(ledge - redge);
            //
            // Console.WriteLine($"edges: {ledge}, {redge} -> width {width}");
        }

        public static void Part2()
        {
            // var input = File.OpenText("inputs/example.txt").ReadToEnd().Split('\n');
            var input = File.OpenText("inputs/15.txt").ReadToEnd().Split('\n');
            var sensors = new List<int[]>();
            var beacons = new List<int[]>();
            // var maxCoordinate = 20;
            var maxCoordinate = 4000000;
            foreach (var line in input)
            {
                var match = Regex.Matches(line, @"\-*\d+");
                sensors.Add(new[] {int.Parse(match[0].Value), int.Parse(match[1].Value), 0});
                beacons.Add(new[] {int.Parse(match[2].Value), int.Parse(match[3].Value)});
            }

            for (var i = 0; i < sensors.Count; i++)
            {
                sensors[i][2] = Math.Abs(sensors[i][0] - beacons[i][0]) + Math.Abs(sensors[i][1] - beacons[i][1]);
            }

            var lines = new List<List<int[]>>();
            for (var i = 0; i < maxCoordinate; i++)
            {
                var lineScan = sensors
                    .Where((s) => s[1] - s[2] <= i && s[1] + s[2] >= i)
                    .Select(s => new int[]
                    {
                        Math.Max(0, s[0] - (s[2] - Math.Abs(i - s[1]))),
                        Math.Min(maxCoordinate, s[0] + (s[2] - Math.Abs(i - s[1])))
                    }).ToList();


                if (lineScan.Count > 1)
                {
                    var combinedLine = new List<int[]>();
                    lineScan = lineScan.OrderBy(e => e[0]).ToList();
                    var combined = new int[] {lineScan[0][0], lineScan[0][1]};
                    for (var j = 1; j < lineScan.Count; j++)
                    {
                        if (combined[1] + 1 >= lineScan[j][0])
                        {
                            combined[1] = combined[1] < lineScan[j][1] ? lineScan[j][1] : combined[1];
                        }
                        else
                        {
                            combinedLine.Add(new[] {combined[0], combined[1]});
                            combined = new[] {lineScan[j][0], lineScan[j][1]};
                        }
                    }

                    combinedLine.Add(new[] {combined[0], combined[1]});
                    lines.Add(combinedLine);
                }
                else
                {
                    lines.Add(lineScan);
                }
            }

            var suspects = new List<int[]>();
            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].Count > 1)
                {
                    Console.WriteLine($"line {i} scanned ranges:");
                    foreach (var scan in lines[i])
                    {
                        Console.WriteLine($"{scan[0]}-{scan[1]}");
                    }

                    var freq = (long) ((lines[i][0][1] + 1) * (long) 4000000) + i;
                    Console.WriteLine($"estimated freq: {freq}");
                }
            }

            // var grids = new List<int?[][]>();
            //
            // for (var i = 0; i < sensors.Count; i++)
            // {
            //     var sensorGrid = new int?[maxCoordinate + 1][];
            //     for (var checkedLine = 0; checkedLine <= maxCoordinate; checkedLine++)
            //     {
            //         var line = new int?[maxCoordinate + 1];
            //         if (checkedLine >= sensors[i][1] - sensors[i][2] && checkedLine <= sensors[i][1] + sensors[i][2])
            //         {
            //             var hdist = sensors[i][2] - Math.Abs(checkedLine - sensors[i][1]);
            //             for (var j = Math.Max(0, sensors[i][0] - hdist);
            //                  j < Math.Min(maxCoordinate, sensors[i][0] + hdist + 1);
            //                  j++)
            //             {
            //                 line[j] = j;
            //             }
            //         }
            //
            //         sensorGrid[checkedLine] = line;
            //     }
            //
            //     Console.WriteLine($"sensor {i} grid:");
            //     for (var t = 0; t < sensorGrid.Length; t++)
            //     {
            //         Console.WriteLine(
            //             $"{t.ToString().PadLeft(2, '0')}: {string.Join(' ', sensorGrid[t].Select(e => e == null ? "  " : e.ToString().PadLeft(2, '0')))}");
            //     }
            //
            //     grids.Add(sensorGrid);
            // }
            //
            // var combinedGrid = new int?[maxCoordinate + 1][];
            // for (var i = 0; i < combinedGrid.Length; i++)
            // {
            //     combinedGrid[i] = new int?[maxCoordinate + 1];
            // }
            //
            // foreach (var grid in grids)
            // {
            //     for (var i = 0; i < grid.Length; i++)
            //     {
            //         for (var j = 0; j < grid[i].Length; j++)
            //         {
            //             combinedGrid[i][j] = combinedGrid[i][j] ?? grid[i][j];
            //         }
            //     }
            // }
            //
            // Console.WriteLine($"combined grid:");
            // for (var t = 0; t < combinedGrid.Length; t++)
            // {
            //     Console.WriteLine(
            //         $"{t.ToString().PadLeft(2, '0')}: {string.Join(' ', combinedGrid[t].Select(e => e == null ? "  " : e.ToString().PadLeft(2, '0')))}");
            // }
        }
    }
}