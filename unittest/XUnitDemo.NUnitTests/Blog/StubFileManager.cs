using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using XUnitDemo.Infrastucture.Interface;

namespace XUnitDemo.NUnitTests.Blog
{
    public class StubFileManager : IFileManager
    {
        public async Task<string> GetStringFromTxtAsync(string filePath)
        {
            var sensitiveList = new List<string> { "Political.txt", "YellowRelated.txt" };
            if (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SensitiveWords", "Political.txt").Equals(filePath))
            {
                return await Task.FromResult( "0000\r\n1111\r\n2222");
            }

            if (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SensitiveWords", "YellowRelated.txt").Equals(filePath))
            {
                return await Task.FromResult("3333\r\n4444\r\n5555");
            }
            return null;
        }

        public async Task<bool> IsExistsFileAsync(string filePath)
        {
            return await Task.FromResult(true);
        }
    }
}
