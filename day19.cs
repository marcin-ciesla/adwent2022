using System.Net;
using System.Xml.Xsl;

namespace adwent2022
{
    internal static class Day19
    {
        static StreamReader GetInput()
        {
            return File.OpenText("inputs/19.txt");
            // return File.OpenText("inputs/example.txt");
        }

        public static void Part1()
        {
            var input = GetInput().ReadToEnd().Split('\n')
                .Select(e => e.Split(',')
                    .Select(int.Parse)
                    .ToArray())
                .ToList();
            
            Console.WriteLine($"surface =");
        }

        public static void Part2()
        {
            var input = GetInput().ReadToEnd().Split('\n')
                .Select(e => e.Split(',')
                    .Select(int.Parse)
                    .ToArray())
                .ToList();
        }
    }
}