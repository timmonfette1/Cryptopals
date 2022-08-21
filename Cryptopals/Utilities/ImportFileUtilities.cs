using System.Reflection;

namespace Cryptopals.Utilities
{
    public class ImportFileUtilities
    {
        private const string InputFileRoot = "InputFiles";

        private readonly string _path;

        private string[] _fileContents;

        public ImportFileUtilities(string filename)
        {
            _path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $@"{InputFileRoot}\{filename}");

            _fileContents = null;
        }

        public string[] ReadFile()
        {
            _fileContents ??= File.ReadAllLines(_path);
            return _fileContents;
        }

        public string ReadFileAsString() => string.Join("", ReadFile());
    }
}
