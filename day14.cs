using System.IO.Compression;
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
    internal static class Day14
    {
        public static void Part1()
        {
            using var input = File.OpenText("inputs/example.txt");
            var rocks = new List<int[]>();
            while (input.Peek() != -1)
            {
                var coordinates = input.ReadLine()
                    .Split(" -> ")
                    .Select(e => e.Split(','))
                    .Select(e => new int[] {int.Parse(e[0]), int.Parse(e[1])})
                    .ToArray();
                for (var i = 0; i < coordinates.Length - 1; i++)
                {
                    if (coordinates[i][0] != coordinates[i + 1][0])
                    {
                        var left = coordinates[i][0] < coordinates[i + 1][0]
                            ? coordinates[i][0]
                            : coordinates[i + 1][0];
                        var right = coordinates[i][0] > coordinates[i + 1][0]
                            ? coordinates[i][0]
                            : coordinates[i + 1][0];
                        var y = coordinates[i][1];
                        for (var x = left; x <= right; x++)
                        {
                            rocks.Add(new int[] {x, y});
                        }
                    }
                    else
                    {
                        var top = coordinates[i][1] < coordinates[i + 1][1] ? coordinates[i][1] : coordinates[i + 1][1];
                        var btm = coordinates[i][1] > coordinates[i + 1][1] ? coordinates[i][1] : coordinates[i + 1][1];
                        var x = coordinates[i][0];
                        for (var y = top; y <= btm; y++)
                        {
                            rocks.Add(new int[] {x, y});
                        }
                    }
                }
            }

            var grain = new int[] {500, 0};
            var sand = new List<int[]>();
            var leftBound = rocks.Select(e => e[0]).Min();
            var rightBound = rocks.Select(e => e[0]).Max();
            var bottom = rocks.Select(e => e[1]).Max() + 1;
            var overflow = false;
            while (!overflow)
            {
                grain = new int[] {500, 0};
                var obstacles = rocks.Concat(sand);
                while (true)
                {
                    if (grain[1] > bottom || leftBound > grain[0] && rightBound < grain[0])
                    {
                        overflow = true;
                        break;
                    }

                    if (!obstacles.Any(e => e[0] == grain[0] && e[1] == grain[1] + 1))
                    {
                        ++grain[1];
                    }
                    else if (
                        // !obstacles.Any(e => e[0] == grain[0] - 1 && e[1] == grain[1]) && 
                        !obstacles.Any(e => e[0] == grain[0] - 1 && e[1] == grain[1] + 1))
                    {
                        --grain[0];
                        ++grain[1];
                    }
                    else if (
                        // !obstacles.Any(e => e[0] == grain[0] + 1 && e[1] == grain[1]) && 
                        !obstacles.Any(e => e[0] == grain[0] + 1 && e[1] == grain[1] + 1))
                    {
                        ++grain[0];
                        ++grain[1];
                    }
                    else
                    {
                        sand.Add(grain);
                        break;
                    }
                }
            }

            DrawMap(rocks, sand, grain);
            Console.WriteLine(sand.Count);
        }

        public static void Part2()
        {
            using var input = File.OpenText("inputs/14.txt");
            var rocks = new List<int[]>();
            while (input.Peek() != -1)
            {
                var coordinates = input.ReadLine()
                    .Split(" -> ")
                    .Select(e => e.Split(','))
                    .Select(e => new int[] {int.Parse(e[0]), int.Parse(e[1])})
                    .ToArray();
                for (var i = 0; i < coordinates.Length - 1; i++)
                {
                    if (coordinates[i][0] != coordinates[i + 1][0])
                    {
                        var left = coordinates[i][0] < coordinates[i + 1][0]
                            ? coordinates[i][0]
                            : coordinates[i + 1][0];
                        var right = coordinates[i][0] > coordinates[i + 1][0]
                            ? coordinates[i][0]
                            : coordinates[i + 1][0];
                        var y = coordinates[i][1];
                        for (var x = left; x <= right; x++)
                        {
                            rocks.Add(new int[] {x, y});
                        }
                    }
                    else
                    {
                        var top = coordinates[i][1] < coordinates[i + 1][1] ? coordinates[i][1] : coordinates[i + 1][1];
                        var btm = coordinates[i][1] > coordinates[i + 1][1] ? coordinates[i][1] : coordinates[i + 1][1];
                        var x = coordinates[i][0];
                        for (var y = top; y <= btm; y++)
                        {
                            rocks.Add(new int[] {x, y});
                        }
                    }
                }
            }

            var grain = new int[] {500, 0};
            var sand = new List<int[]>();
            var leftBound = rocks.Min(e => e[0]);
            var rightBound = rocks.Max(e => e[0]);
            var floor = rocks.Select(e => e[1]).Max() + 1;
            var overflow = false;
            while (!overflow)
            {
                grain = new int[] {500, 0};
                var obstacles = rocks.Concat(sand);
                while (true)
                {
                    if (!obstacles.Any(e => e[0] == grain[0] && e[1] == grain[1] + 1) && grain[1] < floor)
                    {
                        var obs = obstacles.Where(e => e[0] == grain[0] && e[1] > grain[1]);
                        grain[1] = obs.Any() ? obs.Min(e => e[1]) - 1 : floor;
                    }
                    else if (
                        // !obstacles.Any(e => e[0] == grain[0] - 1 && e[1] == grain[1]) && 
                        !obstacles.Any(e => e[0] == grain[0] - 1 && e[1] == grain[1] + 1)
                        && grain[1] < floor
                        && grain[0] >= leftBound)
                    {
                        --grain[0];
                        ++grain[1];
                    }
                    else if (
                        // !obstacles.Any(e => e[0] == grain[0] + 1 && e[1] == grain[1]) && 
                        !obstacles.Any(e => e[0] == grain[0] + 1 && e[1] == grain[1] + 1)
                        && grain[1] < floor
                        && grain[0] <= rightBound)
                    {
                        ++grain[0];
                        ++grain[1];
                    }
                    else
                    {
                        sand.Add(grain);
                        if (grain[0] == 500 && grain[1] == 0)
                            overflow = true;
                        break;
                    }
                }
            }

            var leftHeight = floor - rocks.Concat(sand).Where(e => e[0] == leftBound - 1).Min(e => e[1]);
            var rightHeight = floor - rocks.Concat(sand).Where(e => e[0] == rightBound + 1).Min(e => e[1]);
            for (var i = 0; i <= leftHeight; i++)
            {
                for (var j = leftHeight - i; j > 0; j--)
                {
                    sand.Add(new int[] {leftBound - j, floor - i});
                }
            }

            for (var i = 0; i <= rightHeight; i++)
            {
                for (var j = rightHeight - i; j > 0; j--)
                {
                    sand.Add(new int[] {rightBound + j, floor - i});
                }
            }

            var leftSide = leftHeight * (leftHeight + 1) / 2;
            var rightSide = rightHeight * (rightHeight + 1) / 2;
            DrawMap(rocks, sand, grain);
            Console.WriteLine(sand.Count);
            // Console.WriteLine(sand.LongCount() + leftSide + rightSide);
        }

        private static void DrawMap(List<int[]> rocks, List<int[]> sand, int[] grain)
        {
            Console.CursorTop = 0;
            var xshift = rocks.Concat(sand).Select(e => e[0]).Min() - 1;
            var map = new char[rocks.Concat(sand).Select(e => e[1]).Max() + 2][];
            for (var i = 0; i < map.Length; i++)
            {
                map[i] = new char[(rocks.Concat(sand).Select(e => e[0]).Max() -
                                   rocks.Concat(sand).Select(e => e[0]).Min()) + 3];
                for (var j = 0; j < map[i].Length; j++)
                {
                    map[i][j] = rocks.Any(e => e[0] == j + xshift && e[1] == i) ? '#'
                        : sand.Any(e => e[0] == j + xshift && e[1] == i) ? 'o' : '·';
                }
            }

            for (int i = 0; i < map.Last().Length; i++)
            {
                map.Last()[i] = '#';
            }

            map[0][500 - xshift] = '+';
            map[grain[1]][grain[0] - xshift] = '.';

            foreach (var line in map)
            {
                Console.WriteLine(string.Join("", line));
            }
        }
    }
}