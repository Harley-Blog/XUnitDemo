using System.IO;
using System.Threading.Tasks;
using XUnitDemo.Infrastucture.Interface;

namespace XUnitDemo.Infrastucture
{
    public class FileManager : IFileManager
    {
        public async Task<bool> IsExistsFileAsync(string filePath)
        {
            return await Task.FromResult(File.Exists(filePath));
        }

        public async Task<string> GetStringFromTxtAsync(string filePath)
        {
            using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using StreamReader sr = new StreamReader(fs);
             return await sr.ReadToEndAsync();
        }
    }
}
