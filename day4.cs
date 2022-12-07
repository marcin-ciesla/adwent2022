using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adwent2022
{
    internal static class Day4
    {
        public static int Part1()
        {
            using var input = File.OpenText("inputs/4.txt");
            var counter = 0;
            while (input.Peek() != -1)
            {
                var assigments = input.ReadLine().Split(',').Select(e => e.Split('-').ToArray()).ToArray();
                if ((int.Parse(assigments[0][0]) <= int.Parse(assigments[1][0])
                    && int.Parse(assigments[0][1]) >= int.Parse(assigments[1][1]))
                    || (int.Parse(assigments[1][0]) <= int.Parse(assigments[0][0])
                    && int.Parse(assigments[1][1]) >= int.Parse(assigments[0][1])))
                {
                    counter++;
                }
            }
            return counter;
        }
        public static int Part2()
        {
            using var input = File.OpenText("inputs/4.txt");
            var counter = 0;
            while (input.Peek() != -1)
            {
                var assigments = input.ReadLine().Split(',').Select(e => e.Split('-').ToArray()).ToArray();
                if ((
                    int.Parse(assigments[0][0]) <= int.Parse(assigments[1][1]) //as <= bf
                    && 
                    int.Parse(assigments[1][1]) <= int.Parse(assigments[0][1]) //bf <= af
                    ) 
                    ||
                    (
                    int.Parse(assigments[1][0]) <= int.Parse(assigments[0][1]) //as <= bf
                    &&
                    int.Parse(assigments[0][1]) <= int.Parse(assigments[1][1]) //bf <= af
                    ))
                {
                    counter++;
                }
            }
            return counter;
        }
    }
}
