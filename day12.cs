using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace adwent2022
{
    internal static class Day12
    {
        public static int Part1()
        {
            var grid = File.OpenText("inputs/12.txt").ReadToEnd().Split("\n").Select(e => e.ToCharArray()).ToArray();
            var size = new Tuple<int, int>(grid.Length, grid[1].Length);
            var visited = new List<Tuple<int, int, int>>();
            var pathLen = int.MaxValue;
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] == 'S')
                    {
                        visited.Add(new Tuple<int, int, int>(i, j, 0));
                        break;
                    }
                }

                if (visited.Count > 0)
                {
                    break;
                }
            }

            var toCheck = new Queue<Tuple<int, int, int>>(GetAvailableNodes(visited.First(), grid, visited, size));
            visited.AddRange(toCheck);
            while (toCheck.Any())
            {
                var possible = GetAvailableNodes(toCheck.Dequeue(), grid, visited, size);
                foreach (var variant in possible)
                {
                    if (grid[variant.Item1][variant.Item2] == 'E')
                    {
                        pathLen = Math.Min(pathLen, variant.Item3);
                        continue;
                    }
                    if (!visited.Any(f => f.Item1 == variant.Item1 
                                          && f.Item2 == variant.Item2 
                                          && f.Item3 <= variant.Item3))
                    {
                        visited.Add(variant);
                        toCheck.Enqueue(variant);
                    }
                }
            }

            return pathLen;
        }
        public static int Part2()
        {
            var grid = File.OpenText("inputs/12.txt").ReadToEnd().Split("\n").Select(e => e.ToCharArray()).ToArray();
            var size = new Tuple<int, int>(grid.Length, grid[1].Length);
            var visited = new List<Tuple<int, int, int>>();
            var pathLen = int.MaxValue;
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] == 'S' || grid[i][j] == 'a')
                    {
                        visited.Add(new Tuple<int, int, int>(i, j, 0));
                    }
                }
            }

            var toCheck = new Queue<Tuple<int, int, int>>(visited);
            visited.AddRange(toCheck);
            while (toCheck.Any())
            {
                var possible = GetAvailableNodes(toCheck.Dequeue(), grid, visited, size);
                foreach (var variant in possible)
                {
                    if (grid[variant.Item1][variant.Item2] == 'E')
                    {
                        pathLen = Math.Min(pathLen, variant.Item3);
                        continue;
                    }
                    if (!visited.Any(f => f.Item1 == variant.Item1 
                                          && f.Item2 == variant.Item2 
                                          && f.Item3 <= variant.Item3))
                    {
                        visited.Add(variant);
                        toCheck.Enqueue(variant);
                    }
                }
            }

            return pathLen;
        }

        static IEnumerable<Tuple<int, int, int>> GetAvailableNodes(
            Tuple<int, int, int> crossing,
            char[][] grid,
            List<Tuple<int, int, int>> visited,
            Tuple<int, int> size
        )
        {
            var possible = new List<Tuple<int, int, int>>();
            if (crossing.Item1 - 1 >= 0)
            {
                possible.Add(new Tuple<int, int, int>(crossing.Item1 - 1, crossing.Item2, crossing.Item3 + 1));
            }

            if (crossing.Item2 - 1 >= 0)
            {
                possible.Add(new Tuple<int, int, int>(crossing.Item1, crossing.Item2 - 1, crossing.Item3 + 1));
            }

            if (crossing.Item1 + 1 < size.Item1)
            {
                possible.Add(new Tuple<int, int, int>(crossing.Item1 + 1, crossing.Item2, crossing.Item3 + 1));
            }

            if (crossing.Item2 + 1 < size.Item2)
            {
                possible.Add(new Tuple<int, int, int>(crossing.Item1, crossing.Item2 + 1, crossing.Item3 + 1));
            }

            return possible.Where(e => grid[e.Item1][e.Item2] == 'E'
                ? grid[crossing.Item1][crossing.Item2] == 'z'
                : (grid[e.Item1][e.Item2] <= (grid[crossing.Item1][crossing.Item2] > 'a' ? grid[crossing.Item1][crossing.Item2] : 'a') + 1));
        }
    }
}