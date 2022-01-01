using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace MazePicSolverCSharp
{
    public class ImagerHelpers
    {
        public static MapDot[][] LoadMapDotsFromImage(string imagePath)
        {
            MapDot[][] dots;
            List<Color> r = new List<Color>();
            using (var image = new Bitmap(imagePath))
            {
                dots = Enumerable.Range(0, image.Size.Width).Select(t => new MapDot[image.Size.Height - 1]).ToArray();



                dots.IterateThroughMap((x, y, map) =>
                {
                    if (x == 14)
                    {
                        var b= true;
                    }
                    var pixel = image.GetPixel(x, y);
                    r.Add(pixel);
                    if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                    {
                        map[x][y] = new MapDot(true, new Point(x, y));
                    }
                    else if (pixel.R < 255 && pixel.G > 0 && pixel.G <= 255)
                    {
                        map[x][y] = new MapDot(false, new Point(x, y), true);
                    }
                    else if (pixel.G < 255 && pixel.R > 0 && pixel.R <= 255)
                    {
                        map[x][y] = new MapDot(false, new Point(x, y), false, true);
                    }
                    else
                    {
                        map[x][y] = new MapDot(false, new Point(x, y));
                    }
                });
            }

            var ber = r.Distinct().ToArray();
            return dots;
        }

        public static void SaveImage(MapDot[][] map, Size size, string savePath, bool showWorking = false)
        {
            using (var image = new Bitmap(size.Width, size.Height))
            {
                Graphics graphics = Graphics.FromImage(image);
                var blackPen = new Pen(Brushes.Black);
                var yellowPen = new Pen(Brushes.Yellow);
                var whitePen = new Pen(Brushes.White);
                var redPen = new Pen(Brushes.Red);
                for (int x = 0;
                     (x
                      <= (size.Width - 1));
                     x++)
                {
                    for (int y = 0;
                         (y
                          <= (size.Height - 1));
                         y++)
                    {
                        MapDot mapDot = map[x][y];
                        var point = new Point(x, y);
                        var rectangle = new Rectangle(point, new Size(1, 1));
                        if (mapDot.Wall)
                        {
                            DrawPixel(graphics, blackPen, rectangle);
                        }
                        else if ((mapDot.PathUsed || mapDot.EndPoint))
                        {
                            DrawPixel(graphics, redPen, rectangle);
                        }
                        else if ((mapDot.EverBeenUsed && showWorking))
                        {
                            DrawPixel(graphics, yellowPen, rectangle);
                        }
                        else
                        {
                            DrawPixel(graphics, whitePen, rectangle);
                        }
                    }
                }

                image.Save(savePath, ImageFormat.Png);
            }
        }

        private static void DrawPixel(Graphics graphic, Pen pen, Rectangle rectangle)
        {
            graphic.FillRectangle(pen.Brush, rectangle);
            graphic.DrawRectangle(pen, rectangle);
        }
    }
}