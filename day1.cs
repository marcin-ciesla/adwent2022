using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace adwent2022
{
    internal static class Day1
    {
        public static int Part1()
        {
            using var input = File.OpenText("inputs/1.txt");
            var topCallory = 0;
            var currentCallory = 0;

            while (input.Peek() != -1)
            {
                var line = input.ReadLine();
                if (line == string.Empty)
                {
                    topCallory = topCallory > currentCallory ? topCallory : currentCallory;
                    currentCallory = 0;
                    continue;
                }
                currentCallory += Convert.ToInt32(line);
            }
            return topCallory > currentCallory ? topCallory : currentCallory;
        }
        public static int Part2()
        {
            using var input = File.OpenText("inputs/1.txt");
            var topCallory = new int[3] { 0, 0, 0 };
            var currentCallory = 0;
            while (input.Peek() != -1)
            {
                var line = input.ReadLine();
                if (line == string.Empty)
                {
                    if (topCallory.Any(tc => currentCallory > tc))
                    {
                        topCallory = topCallory.OrderBy(e => e).ToArray();
                        topCallory[0] = currentCallory;
                    }
                    currentCallory = 0;
                    continue;
                }
                currentCallory += Convert.ToInt32(line);
            }
            if (topCallory.Any(tc => currentCallory > tc))
            {
                topCallory = topCallory.OrderBy(e => e).ToArray();
                topCallory[0] = currentCallory;
            }
            return topCallory.Sum();
        }

    }

}