using System.IO;

namespace CashFlower.Test.Shared
{
    public static class FileHelper
    {
        public static void DeleteFile(string fileName)
        {
            System.IO.File.Delete(fileName);
        }

        public static void ClearFile(string fileName)
        {
            var writer = new StreamWriter(fileName, false);
            writer.Write("");
            writer.Close();
        }

        public static string ReadFileString(string fileName)
        {
            var reader = new StreamReader(fileName);
            var result = reader.ReadToEnd();
            reader.Close();
            return result;
        }
    }
}
