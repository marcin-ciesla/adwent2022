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
    internal static class Day8
    {
        public static int Part1()
        {
            using var input = File.OpenText("inputs/8.txt");
            var yCounter = 0;
            var trees = new List<Tree>();
            while (input.Peek() != -1)
            {
                var line = input.ReadLine()!;
                for (int xCounter = 0; xCounter < line.Length; xCounter++)
                {
                    trees.Add(new Tree(xCounter, yCounter, int.Parse(line[xCounter].ToString())));
                }
                ++yCounter;
            }
            return trees.Where(e => e.IsVisible(trees)).Count();
        }

        public static int Part2() {

            using var input = File.OpenText("inputs/8.txt");
            var yCounter = 0;
            var trees = new List<Tree>();
            while (input.Peek() != -1)
            {
                var line = input.ReadLine()!;
                for (int xCounter = 0; xCounter < line.Length; xCounter++)
                {
                    trees.Add(new Tree(xCounter, yCounter, int.Parse(line[xCounter].ToString())));
                }
                ++yCounter;
            }
            var visibleTrees = trees.Where(e => e.IsVisible(trees));
            return visibleTrees.Select(e => e.Visibility(trees)).Max();
        }

        class Tree
        {
            public Tree(int posX, int posY, int height)
            {
                PosX = posX;
                PosY = posY;
                Height = height;
            }

            public int PosX { get; set; }
            public int PosY { get; set; }
            public int Height { get; set; }

            public bool IsVisible(List<Tree> trees)
            {
                return IsVisibleTop(trees) || IsVisibleBottom(trees) || IsVisibleLeft(trees) || IsVisibleRight(trees);
            }
            bool IsVisibleTop(List<Tree> trees)
            {
                return trees.Where(e => e.PosX == PosX && e.PosY < PosY).All(e => e.Height < Height);
            }
            bool IsVisibleBottom(List<Tree> trees)
            {
                return trees.Where(e => e.PosX == PosX && e.PosY > PosY).All(e => e.Height < Height);
            }
            bool IsVisibleLeft(List<Tree> trees)
            {
                return trees.Where(e => e.PosX < PosX && e.PosY == PosY).All(e => e.Height < Height);
            }
            bool IsVisibleRight(List<Tree> trees)
            {
                return trees.Where(e => e.PosX > PosX && e.PosY == PosY).All(e => e.Height < Height);
            }

            public int Visibility(List<Tree> trees)
            {
                return VisibilityTop(trees) * VisibilityBottom(trees) * VisibilityLeft(trees) * VisibilityRight(trees);
            }

            int VisibilityTop(List<Tree> trees)
            {
                var tt = trees.Where(e => e.PosX == PosX && e.PosY < PosY).OrderByDescending(e => e.PosY);
                var st = tt.TakeWhile(e => e.Height < Height);
                return st.Count() == tt.Count() ? st.Count() : st.Count() + 1;
            }

            int VisibilityBottom(List<Tree> trees)
            {
                var tt = trees.Where(e => e.PosX == PosX && e.PosY > PosY).OrderBy(e => e.PosY);
                var st = tt.TakeWhile(e => e.Height < Height);
                return st.Count() == tt.Count() ? st.Count() : st.Count() + 1;
            }

            int VisibilityLeft(List<Tree> trees)
            {
                var tt = trees.Where(e => e.PosX < PosX && e.PosY == PosY).OrderByDescending(e => e.PosX);
                var st = tt.TakeWhile(e => e.Height < Height);
                return st.Count() == tt.Count() ? st.Count() : st.Count() + 1;
            }

            int VisibilityRight(List<Tree> trees)
            {
                var tt = trees.Where(e => e.PosX > PosX && e.PosY == PosY).OrderBy(e => e.PosX);
                var st = tt.TakeWhile(e => e.Height < Height);
                return st.Count() == tt.Count() ? st.Count() : st.Count() + 1;
            }
        }
    }
}
