using System.IO;
using System.Reflection;

namespace CygX1.Waxy.Http.IntegrationTests
{
    public class TxtFile
    {

        public static string GetFolder()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static string ResolvePath(string fileName)
        {
            return Path.Combine(GetFolder(), fileName);
        }

        public static string ReadText(string fileName)
        {

            return File.ReadAllText(ResolvePath(fileName));
        }
    }
}
