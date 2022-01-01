using System.IO;
using System.Linq;

namespace MazePicSolverCSharp
{
    public class FileIoHelper {
        
        private static char? _pathSeparator = null;
        
        private static char PathSeparatorCharacter {
            get {
                if ((_pathSeparator != null)) {
                    return _pathSeparator.Value;
                }
                
                _pathSeparator = Path.Combine(" ", " ").Trim().First();
                return _pathSeparator.Value;
            }
        }
        
        public static string ExtractDirectory(string path) {
            return string.Join(PathSeparatorCharacter.ToString(), path.Split(PathSeparatorCharacter).Take((path.Split(PathSeparatorCharacter).Length - 1)));
        }
        
        public static string ExtractFileName(string path) {
            return path.Split(PathSeparatorCharacter).Last();
        }
    }
}