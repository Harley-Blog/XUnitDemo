using System.IO;
using System.Threading.Tasks;
using XUnitDemo.IService;

namespace XUnitDemo.Service
{
    public class AttachFileService : IAttachFileService
    {
        public async Task<bool> IsValidFileAsync(string filePath)
        {
            filePath = filePath?.Trim();
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return false;
            }
            if (filePath.Length > 36 || filePath.Length < 14)
            {
                return false;
            }
            FileInfo fi = new FileInfo(filePath);
            if (!".gpeg".Equals(fi.Extension?.ToLower()))
            {
                return false;
            }

            return await Task.FromResult(true);
        }
    }
}
