using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace adwent2022
{
    internal static class Day9
    {
        public static int Part1()
        {
            var visited = new HashSet<string>();
            using var input = File.OpenText("inputs/9.txt");
            var tail = new int[] { 0, 0 };
            var head = new int[] { 0, 0 };
            visited.Add($"{tail[0]},{tail[1]}");
            while (input.Peek() != -1)
            {
                var line = input.ReadLine()!.Split(' ');
                var direction = line[0];
                var distance = int.Parse(line[1]);
                switch (direction)
                {
                    case "U":
                        head[1] += distance;
                        break;
                    case "D":
                        head[1] -= distance;
                        break;
                    case "L":
                        head[0] -= distance;
                        break;
                    case "R":
                        head[0] += distance;
                        break;
                    default:
                        throw new Exception("invalid input");
                }
                while (!IsTouching(head, tail))
                {
                    if (IsDiagonal(head, tail))
                    {
                        if ("DU".Contains(direction))
                            tail[0] = head[0];
                        else
                            tail[1] = head[1];
                    }
                    if ("DU".Contains(direction))
                    {
                        _ = head[1] > tail[1] ? ++tail[1] : --tail[1];
                    }
                    else
                    {
                        _ = head[0] > tail[0] ? ++tail[0] : --tail[0];
                    }
                    visited.Add($"{tail[0]},{tail[1]}");
                }
            }
            return visited.Count;
        }

        public static int Part2(int len)
        {
            var visited = new HashSet<string>();
            using var input = File.OpenText("inputs/9.txt");
            var rope = new int[len][];
            for (int i = 0; i < rope.Length; i++)
            {
                rope[i] = new int[] { 0, 0 };
            }
            visited.Add($"{rope[^1][0]},{rope[^1][1]}");
            var counter = 0;
            while (input.Peek() != -1)
            {
                var line = input.ReadLine()!.Split(' ');
                var direction = line[0];
                var distance = int.Parse(line[1]);
                for (int i = 0; i < distance; i++)
                {
                    Console.Write($"{counter++} => ");
                    switch (direction)
                    {
                        case "U":
                            ++rope[0][1];
                            break;
                        case "D":
                            --rope[0][1];
                            break;
                        case "L":
                            --rope[0][0];
                            break;
                        case "R":
                            ++rope[0][0];
                            break;
                        default:
                            throw new Exception("invalid input");
                    }
                    rope = CalculateRope(rope);
                    visited.Add($"{rope[^1][0]},{rope[^1][1]}");
                }
            }
            return visited.Count;
        }

        public static bool IsTouching(int[] head, int[] tail)
        {
            return Math.Abs(head[0] - tail[0]) <= 1 && Math.Abs(head[1] - tail[1]) <= 1;
        }

        public static bool IsDiagonal(int[] head, int[] tail)
        {
            return head[0] - tail[0] != 0 && head[1] - tail[1] != 0;
        }
        public static int[][] CalculateRope(int[][] rope)
        {
            for (int i = 0; i < rope.Length - 1; i++)
            {
                while (!IsTouching(rope[i], rope[i + 1]))
                {
                    if (IsDiagonal(rope[i], rope[i + 1]))
                    {
                        if (Math.Abs(rope[i][0] - rope[i + 1][0]) == 1)
                            _ = rope[i][0] > rope[i + 1][0] ? ++rope[i + 1][0] : --rope[i + 1][0];
                        else
                            _ = rope[i][1] > rope[i + 1][1] ? ++rope[i + 1][1] : --rope[i + 1][1];
                    }
                    if (rope[i][0] == rope[i + 1][0])
                    {
                        _ = rope[i][1] > rope[i + 1][1] ? ++rope[i + 1][1] : --rope[i + 1][1];
                    }
                    else
                    {
                        _ = rope[i][0] > rope[i + 1][0] ? ++rope[i + 1][0] : --rope[i + 1][0];
                    }
                }
            }
            foreach (var item in rope)
            {
                Console.Write($"{item[0]},{item[1]}|");
            }
            Console.Write('\n');
            return rope;
        }
    }
}
