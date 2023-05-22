using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MazePicSolverCSharp
{
    class Map
    {
        private MapDot[][] MapDots;
        private bool Solved = false;
        public Size Size => new Size(MapDots.Length, MapDots.First().Length);

        public Map(string imagePath)
        {
            MapDots = ImagerHelpers.LoadMapDotsFromImage(imagePath);
        }

        public void BruteForceSaveSolution(string savePath, bool showWorking)
        {
            MapDots.IterateThroughMap((x, y, mapDots) => mapDots[x][y].Reset());
            this.BruteForceSolve();
            ImagerHelpers.SaveImage(MapDots, Size, savePath, showWorking);
        }

        public void MostDirectRouteSolveSaveSolution(string savePath)
        {
            MapDots.IterateThroughMap((x, y, mapDots) => mapDots[x][y].Reset());
            var (startPoint, endPoint) = GetStartAndEndPoint();
            var end = MapDots[endPoint.X][endPoint.Y];
            var currentOptions = new List<Point>() { startPoint };
            uint steps = 0;
            while (end.ShortestFromStart == 0)
            {
                steps++;
                var newOptions = currentOptions.SelectMany(q => MapDots.GetAroundArrayOfArrays(q)
                        .Where(w => !MapDots.GetPoint(w).Wall && MapDots.GetPoint(w).ShortestFromStart == 0 &&
                                    MapDots.GetPoint(w).StartPoint == false))
                    .Distinct().ToList();
                if (newOptions.Count == 0)
                {
                    break;
                }

                Console.WriteLine($"{steps} -- {newOptions.Count}");
                foreach (var item in newOptions)
                    MapDots.GetPoint(item).ShortestFromStart = steps;


                currentOptions = newOptions;
            }

            if (end.ShortestFromStart != 0)
            {
                var currentPoint = endPoint;
                var lookFor = MapDots.GetPoint(currentPoint).ShortestFromStart - 1;
                while (lookFor != 0)
                {
                    currentPoint = MapDots.GetAroundArrayOfArrays(currentPoint).First(q =>
                        MapDots.GetPoint(q).ShortestFromStart == lookFor);
                    lookFor--;
                    MapDots.GetPoint(currentPoint).PathUsed = true;
                }
            }

            ImagerHelpers.SaveImage(MapDots, Size, savePath);
        }

        private void BruteForceSolve()
        {
            if (Solved)
            {
                return;
            }

            var positions = GetStartAndEndPoint();
            var currentloc = positions.start;

            while (!Solved)
            {
                var dot = MapDots.GetPoint(currentloc);
                dot.PathUsed = true;
                var allOptions = MapDots.GetAroundArrayOfArrays(currentloc);
                var forward = allOptions.Where(t =>
                        !MapDots.GetPoint(t).PathUsed && !MapDots.GetPoint(t).Wall &&
                        !MapDots.GetPoint(t).DeadEnd)
                    .ToArray();

                if (!forward.Any())
                {
                    if (dot.StartPoint)
                    {
                        throw new Exception("ok we are F6$^£");
                    }

                    currentloc = dot.PreviousLocation;
                    dot.PathUsed = false;
                    dot.DeadEnd = true;
                    continue;
                }

                var nextDot = MapDots.GetPoint(forward.First());
                nextDot.PreviousLocation = dot.Location;
                if (nextDot.EndPoint)
                {
                    Solved = true;
                }
                else
                {
                    currentloc = nextDot.Location;
                }
            }
        }

        public (Point start, Point end) GetStartAndEndPoint()
        {
            Point? start = null;
            Point? end = null;

            MapDots.IterateThroughMap((x, y, map) =>
            {
                if (map[x][y].StartPoint)
                {
                    if (start != null)
                    {
                        throw new Exception("Can't have multiple starts");
                    }

                    start = new Point(x, y);
                }

                if (map[x][y].EndPoint)
                {
                    if (end != null)
                    {
                        throw new Exception("Can't have multiple starts");
                    }

                    end = new Point(x, y);
                }
            });

            if (start == null || end == null)
            {
                throw new Exception("No start or end Found");
            }

            return (start.Value, end.Value);
        }
    }
}