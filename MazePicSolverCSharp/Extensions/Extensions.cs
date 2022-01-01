using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MazePicSolverCSharp
{
    public static class Extensions
    {
        public static void IterateThroughMap<T>(this T[][] items, Action<int, int, T[][]> func)
        {
            for (int i = 0; i < items.Length; i++)
            {
                for (int j = 0; j < items[i].Length; j++)
                {
                    func(i, j, items);
                }
            }
        }

        public static IEnumerable<Point> GetAroungArrayOfArrays<T>(this T[][] arrayOfArrays, Point pos)
        {
            return new []{ new Point(pos.X - 1, pos.Y), new Point(pos.X + 1, pos.Y), new Point(pos.X, pos.Y - 1), new Point(pos.X, pos.Y + 1)}
                .Where(t=> t.X<arrayOfArrays.Length && t.X>-1)
                .Where(t=> t.Y>-1 && t.Y<arrayOfArrays[t.X].Length).ToArray();
        }
    }
}