using System;
using System.IO;
using System.Linq;

namespace MazePicSolverCSharp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("a picture or directory containing png's needs to be a argument");
                return;  
            }
            var showWorking = false;
            if (args.First().ToLower() == "true" || args.First().ToLower() == "false")
            {
                showWorking = bool.Parse(args.First());
                args = args.Skip(1).ToArray();
            }

            var paths = Array.Empty<string>();
            var path = string.Join(" ", args);
            if (Directory.Exists(path))
            {
                paths = Directory.GetFiles(path).Where(t => t.ToLower().EndsWith(".png") || t.ToLower().EndsWith(".jpg") || t.ToLower().EndsWith(".bmp"))
                    .Where(f => FileIoHelper.ExtractFileName(f).ToLower().Contains("-solution") == false).ToArray();
                
            }
            else if (File.Exists(path))
            {
                paths = new[] {path};
            }

            if (!paths.Any())
            {
                Console.WriteLine("a picture or directory containing png's needs to be a argument");
                return;
            }

            foreach (var item in paths)
            {
                Run(item,showWorking);
            }

        }

        public static void Run(string imagePath, bool showWorking) {
            string newFileName = (FileIoHelper.ExtractFileName(imagePath).Split('.').First() + "-solution.png");
            string outputFile = Path.Combine(FileIoHelper.ExtractDirectory(imagePath), newFileName);
            var map = new Map(imagePath);
            map.SaveSolution(outputFile, showWorking);
        }
        
    }
}
