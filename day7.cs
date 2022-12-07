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
    internal static class Day7
    {
        static int Total = 0;
        public static int Part1()
        {
            var input = File.OpenText("inputs/7.txt");
            input.ReadLine();
            var root = new Dir("/");
            ReadCommand(input, root);
            CalculateAndAddToSum(root, 0);
            return Total;
        }

        public static int Part2()
        {
            {
                var input = File.OpenText("inputs/7.txt");
                input.ReadLine();
                var root = new Dir("/");
                ReadCommand(input, root);
                var totalUsed = CalculateAndAddToSum(root, 0);
                var toFree = 30000000 - (70000000 - totalUsed);
                Console.WriteLine($"to free - {toFree}");
                Total = totalUsed;
                FindSmallesValidDir(root, 0, toFree);
                return Total;
            }
        }

        static int CalculateAndAddToSum(Dir dir, int sum)
        {
            var dirSum = dir.SubDirs.Select(e => CalculateAndAddToSum(e, sum)).Sum() + dir.TotalSize;
            if (dirSum <= 100000)
            {
                Total += dirSum;
            }
            return dirSum;
        }

        static int FindSmallesValidDir(Dir dir, int sum, int toFree)
        {
            var dirSum = dir.SubDirs.Select(e => FindSmallesValidDir(e, sum, toFree)).Sum() + dir.TotalSize;
            Console.WriteLine($"dir {dir.Name} - {dirSum}");
            if (dirSum >= toFree && dirSum < Total)
            {
                Total = dirSum;
            }
            return dirSum;
        }

        static void ReadCommand(StreamReader input, Dir currentDir)
        {
            var command = input.ReadLine()!.Split(' ');
            switch (command[1])
            {
                case "cd":
                    ReadCommand(input, currentDir.OpenSubDir(command[2]));
                    break;
                case "ls":
                    while (input.Peek() != -1 && input.Peek() != 36)
                    {
                        var line = input.ReadLine()!.Split(' ');
                        if (line[0] == "dir")
                        {
                            currentDir.SubDirs.Add(new Dir(line[1], currentDir));
                        }
                        else
                        {
                            currentDir.TotalSize += int.Parse(line[0]);
                        }
                    }
                    if (input.Peek() != -1)
                    {
                        ReadCommand(input, currentDir);
                    }
                    break;
                default:
                    throw new Exception("something went wrong");
            }
        }

        class Dir
        {
            public Dir(string name)
            {
                Name = name;
            }
            public Dir(string name, Dir parent)
            {
                Name = name;
                ParentDir = parent;
            }

            public string Name { get; set; }
            public List<Dir> SubDirs { get; set; } = new List<Dir>();
            public Dir? ParentDir { get; set; }
            public int TotalSize { get; set; }

            public Dir OpenSubDir(string name)
            {
                return name == ".." ? ParentDir! : SubDirs.First(x => x.Name == name);
            }
        }
    }
}
