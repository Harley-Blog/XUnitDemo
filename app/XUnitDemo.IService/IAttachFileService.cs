using System.Threading.Tasks;

namespace XUnitDemo.IService
{
    public interface IAttachFileService
    {
        public Task<bool> IsValidFileAsync(string filePath);
    }
}
