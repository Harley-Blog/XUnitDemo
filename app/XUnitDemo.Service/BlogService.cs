using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitDemo.Infrastucture.Interface;
using XUnitDemo.IService;

namespace XUnitDemo.Service
{
    public class BlogService : IBlogService
    {
        private readonly IFileManager _fileManager;
        public BlogService(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public async Task<string> GetSecurityBlogAsync(string originContent)
        {
            if (string.IsNullOrWhiteSpace(originContent)) return originContent;

            var sensitiveList = new List<string> { "Political.txt", "YellowRelated.txt" };
            StringBuilder sbOriginContent = new StringBuilder(originContent);
            foreach (var item in sensitiveList)
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SensitiveWords", item);
                if (await _fileManager.IsExistsFileAsync(filePath))
                {
                    var words = await _fileManager.GetStringFromTxtAsync(filePath);
                    var wordList = words.Split("\r\n");
                    if (wordList.Any())
                    {
                        foreach (var word in wordList)
                        {
                            sbOriginContent.Replace(word, string.Join("", word.Select(s => "*")));
                        }
                    }
                }
            }

            return await Task.FromResult(sbOriginContent.ToString());
        }

        //public async Task<string> GetSecurityBlogAsync(string originContent)
        //{
        //    if (string.IsNullOrWhiteSpace(originContent)) return originContent;

        //    var sensitiveList = new List<string> { "Political.txt", "YellowRelated.txt" };
        //    StringBuilder sbOriginContent = new StringBuilder(originContent);
        //    foreach (var item in sensitiveList)
        //    {
        //        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SensitiveWords", item);
        //        if (File.Exists(filePath))
        //        {
        //            using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //            using StreamReader sr = new StreamReader(fs);
        //            var words = sr.ReadToEnd();
        //            var wordList = words.Split("\r\n");
        //            if (wordList.Any())
        //            {
        //                foreach (var word in wordList)
        //                {
        //                    sbOriginContent.Replace(word, string.Join("", word.Select(s => "*")));
        //                }
        //            }
        //        }
        //    }

        //    return await Task.FromResult(sbOriginContent.ToString());
        //}
    }
}
