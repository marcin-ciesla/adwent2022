﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace adwent2022
{
    internal static class Day10
    {
        public static int Part1()
        {
            using var input = File.OpenText("inputs/10.txt");
            var cycle = 0;
            var strength = 1;
            var sum = 0;
            var yeldCycles = new int[] { 20, 60, 100, 140, 180, 220};
            while (input.Peek() != -1)
            {
                ++cycle;
                var line = input.ReadLine()!.Split(' ');
                if (yeldCycles.Contains(cycle))
                {
                    sum += cycle * strength;
                }
                switch (line[0])
                {
                    case "noop":
                        break;
                    case "addx":
                        ++cycle;
                        if (yeldCycles.Contains(cycle))
                        {
                            sum += cycle * strength;
                        }
                        strength += int.Parse(line[1]);
                        break;
                    default:
                        throw new Exception("invalid input");
                }
            }
            return sum;
        }

        public static void Part2()
        {
            using var input = File.OpenText("inputs/10.txt");
            var spritePos = 0;
            var xPos = 0;
            var screen = new List<string>();
            var bufor = new StringBuilder();
            while (input.Peek() != -1)
            {
                var line = input.ReadLine()!.Split(' ');
                if (xPos == 40)
                {
                    screen.Add(bufor.ToString());
                    bufor = new StringBuilder();
                    xPos = 0;
                }
                bufor.Append(spritePos - 1 <= xPos && xPos <= spritePos + 1 ? "█" : ".");
                if (line[0] == "addx")
                {
                    if (++xPos == 40)
                    {
                        screen.Add(bufor.ToString());
                        bufor = new StringBuilder();
                        xPos = 0;
                    }
                    spritePos += int.Parse(line[1]);
                    bufor.Append(spritePos - 1 <= xPos && xPos <= spritePos + 1 ? "█" : ".");
                }
                ++xPos;
            }
            screen.Add(bufor.ToString());
            foreach (var line in screen)
            {
                Console.WriteLine(line);
            }
        }

    }
}
