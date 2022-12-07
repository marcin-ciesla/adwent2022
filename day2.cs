using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace adwent2022
{
    internal static class Day2
    {
        public static int Part1()
        {
            using var input = File.OpenText("inputs/2.txt");
            var score = 0;
            while (input.Peek() != -1)
            {
                var split = input.ReadLine().Split(' ');
                score += MapFigure(split[1]);
                score += GetResult(MapFigure(split[0]),MapFigure(split[1]));
            }
            return score;
        }
        static int MapFigure(string input)
        {
            return input switch
            {
                "A" or "X" => 1,
                "B" or "Y" => 2,
                "C" or "Z" => 3,
                _ => throw new Exception("invalid input"),
            };
        }
        static int GetResult(int him, int me)
        {
            if (him == me)
            {
                return 3;
            }
            return him switch
            {
                1 => me == 2 ? 6 : 0,
                2 => me == 3 ? 6 : 0,
                3 => me == 1 ? 6 : 0,
                _ => throw new Exception("invalid input"),
            };
        }

        public static int Part2()
        {
            using var input = File.OpenText("inputs/2.txt");
            var score = 0;
            while (input.Peek() != -1)
            {
                var split = input.ReadLine().Split(' ');
                score += GetResultFixed(MapFigure(split[0]), split[1]);
            }
            return score;
        }
        static int GetResultFixed(int him, string expectedResult)
        {
            if (expectedResult == "Y")
                return 3 + him;

            return him switch
            {
                1 => expectedResult == "Z" ? 6 + 2 : 3,
                2 => expectedResult == "Z" ? 6 + 3 : 1,
                3 => expectedResult == "Z" ? 6 + 1 : 2,
                _ => throw new Exception("invalid input"),
            };
        }
    }
}
