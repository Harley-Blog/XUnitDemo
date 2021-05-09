using System.IO;
using System.Threading.Tasks;

namespace XUnitDemo.Infrastucture.Interface
{
    public interface IFileManager
    {
        public Task<bool> IsExistsFileAsync(string filePath);
        public Task<string> GetStringFromTxtAsync(string filePath);
    }
}
