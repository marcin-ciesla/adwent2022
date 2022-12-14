using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace adwent2022
{
    internal static class Day13
    {
        public static int Part1()
        {
            using var input = File.OpenText("inputs/13.txt");
            var counter = 0;
            var sum = 0;
            while (input.Peek() > -1)
            {
                counter++;
                var a1 = input.ReadLine();
                var a2 = input.ReadLine();

                var j1 = JsonArray.Parse(a1) as JsonArray;
                var j2 = JsonArray.Parse(a2) as JsonArray;

                if (Sort(j1, j2) < 0)
                    sum += counter;

                input.ReadLine();
            }

            return sum;
        }

        public static int Part2()
        {
            var input = File.OpenText("inputs/13.txt").ReadToEnd().Split('\n').Where(e => !string.IsNullOrEmpty(e));
            var adds = new List<string>
            {
                "[[2]]",
                "[[6]]"
            };
            input = input.Concat(adds);
            var parsedInput = input.Select(e => JsonArray.Parse(e) as JsonArray).ToArray();
            parsedInput = bubbleSort(parsedInput);
            parsedInput.ToList().Sort(Sort);
            var test = Sort(parsedInput[135], parsedInput[148]);
            var strarr = parsedInput.Select(e => e.ToJsonString()).ToList();
            var i1 = strarr.IndexOf(adds[0]);
            var i2 = strarr.IndexOf(adds[1]);
            return (i1 + 1) * (i2 + 1);
        }

        static int Sort(JsonNode a, JsonNode b)
        {
            if (a is not JsonArray && b is not JsonArray)
            {
                return a.GetValue<int>() < b.GetValue<int>() ? -1 : a.GetValue<int>() == b.GetValue<int>() ? 0 : 1;
            }
            else if (a is JsonArray && b is not JsonArray)
            {
                return Sort(a, new JsonArray(new JsonNode[] {b.GetValue<int>()}));
            }
            else if (a is not JsonArray && b is JsonArray)
            {
                return Sort(new JsonArray(new JsonNode[] {a.GetValue<int>()}), b);
            }

            var longer = (a as JsonArray).Count > (b as JsonArray).Count
                ? (a as JsonArray).Count
                : (b as JsonArray).Count;

            for (int i = 0; i < longer; i++)
            {
                if (i == (a as JsonArray).Count)
                {
                    return -1;
                }
                else if (i == (b as JsonArray).Count)
                {
                    return 1;
                }

                var comp = Sort(a[i], b[i]);
                if (comp == 0)
                {
                    continue;
                }

                return comp;
            }

            return 0;
        }

        static JsonArray[] bubbleSort(JsonArray[] array)
        {
            var i = 0;
            while (i < array.Length - 1)
            {
                if (Sort(array[i], array[i + 1]) == 1)
                {
                    (array[i], array[i + 1]) = (array[i + 1], array[i]);
                    i -= i == 0 ? 0 : 1;
                }
                else
                    ++i;
            }

            return array;
        }
    }
}