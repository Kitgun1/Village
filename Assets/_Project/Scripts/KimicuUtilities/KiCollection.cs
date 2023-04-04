using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable CheckNamespace

namespace KimicuUtilities
{
    public static class KiCollection
    {
        public static void Clamp<T>(this List<T> list, int maximum = 1)
        {
            if (maximum < 1) maximum = 0;

            if (list.Count > maximum)
            {
                list.RemoveRange(list.Count - maximum, list.Count - maximum);
            }
            else if (list.Count < maximum)
            {
                int count = maximum - list.Count;
                for (int i = 0; i < count; i++)
                {
                    list.Add(default);
                }
            }
        }

        public static List<List<T>> SplitList<T>(List<T> list, int percentRatio)
        {
            int count = list.Count;
            int middleIndex = (int)Math.Floor(count * percentRatio / 100.0);
            List<T> firstList = list.GetRange(0, middleIndex);
            List<T> secondList = list.GetRange(middleIndex, count - middleIndex);
            List<List<T>> result = new List<List<T>> { firstList, secondList };
            return result;
        }

        public static List<List<T>> SplitList<T>(this List<T> list, int n, double[] percentages)
        {
            if (percentages.Length != n) throw new ArgumentException("Number of percentages should be equal to n");

            double sumPercentages = percentages.Sum();

            if (sumPercentages > 100 || sumPercentages < 0) throw new ArgumentException("Sum of percentages should be in range (0, 100)");

            List<List<T>> result = new List<List<T>>();

            int startIndex = 0;
            for (int i = 0; i < n; i++)
            {
                int count = (int)(list.Count * percentages[i] / 100);
                if (i == n - 1)
                {
                    count = list.Count - startIndex;
                }

                List<T> sublist = list.GetRange(startIndex, count);
                startIndex += count;

                result.Add(sublist);
            }

            return result;
        }

        public static List<List<T>> DivideList<T>(this List<T> list, int count)
        {
            int size = list.Count / count;
            int remainder = list.Count % count;

            var result = new List<List<T>>();

            for (int i = 0; i < count; i++)
            {
                int start = i * size + Math.Min(i, remainder);
                int end;
                if (i < remainder) end = start + size + (1);
                else end = start + size + (0);

                List<T> sublist = list.GetRange(start, end - start);
                result.Add(sublist);
            }

            return result;
        }
    }
}