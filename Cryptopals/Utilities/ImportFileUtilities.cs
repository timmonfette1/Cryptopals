using System.Reflection;

namespace Cryptopals.Utilities
{
    public class ImportFileUtilities
    {
        private const string InputFileRoot = "InputFiles";

        private readonly string _path;

        public ImportFileUtilities(string filename)
        {
            _path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $@"{InputFileRoot}\{filename}");
        }

        public string[] ReadFile() => File.ReadAllLines(_path);
        public string ReadFileAsString() => string.Join("", ReadFile());
    }
}
