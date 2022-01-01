using System;
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
        
        public MapDot GetDot(Point point)
        {
            return this.GetDot(point.X, point.Y);
        }

        public MapDot GetDot(int x, int y)
        {
            return MapDots[x][y];
        }

        public void SaveSolution(string savePath, bool showWorking)
        {
            this.Solve();
            ImagerHelpers.SaveImage(MapDots, Size, savePath, showWorking);
        }

        public void Solve()
        {
            if (Solved)
            {
                return;
            }

            var positions = GetStartAndEndPoint();
            var currentloc = positions.start;

            while (!Solved)
            {
                var dot = GetDot(currentloc);
                dot.PathUsed = true;
                var allOptions = MapDots.GetAroungArrayOfArrays(currentloc);
                var forward = allOptions.Where(t => !GetDot(t).PathUsed && !GetDot(t).Wall && !GetDot(t).DeadEnd)
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

                var nextDot = GetDot(forward.First());
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
                    if (start!=null)
                    {
                        throw new Exception("Can't have multiple starts");

                    }
                    start = new Point(x, y);
                }
                if (map[x][y].EndPoint)
                {
                    if (end!=null)
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