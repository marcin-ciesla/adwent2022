using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adwent2022
{
    internal static class Day6
    {
        public static int Part1()
        {
            var input = File.OpenText("inputs/6.txt").ReadToEnd();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i..(i+4)].Distinct().Count() == 4)
                    return i+4;
            }
            return 0;
        }

        public static int Part2()
        {
            {
                var input = File.OpenText("inputs/6.txt").ReadToEnd();
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i..(i + 14)].Distinct().Count() == 14)
                        return i + 14;
                }
                return 0;
            }
        }
    }
}
