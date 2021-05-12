using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitDemo.Infrastucture.Interface;
using XUnitDemo.IService;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("XUnitDemo.NUnitTests")]
namespace XUnitDemo.Service
{
    public class BlogService : IBlogService
    {
        private readonly IFileManager _fileManager;
        private readonly ILoggerService _loggerService;
        private ILogger _logger;
        private static readonly List<string> _sensitiveList;
        private int _effectiveSensitiveNum;


        static BlogService()
        {
            _sensitiveList = new List<string> { "Political.txt", "YellowRelated.txt" };
        }

        public BlogService(IFileManager fileManager, ILoggerService loggerService)
        {
            _fileManager = fileManager;
            _loggerService = loggerService;
        }

        #region Constructor

        //#if DEBUG
        //        public BlogService(ILogger logger)
        //        {
        //            _logger = logger;
        //        }
        //#endif

        //[Conditional("DEBUG")]
        //public void SetLogger(ILogger logger)
        //{
        //    _logger = logger;
        //}

        //internal BlogService(ILogger logger)
        //{
        //    _logger = logger;
        //} 
        #endregion

        #region First
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
        #endregion

        #region Second
        //public async Task<string> GetSecurityBlogAsync(string originContent)
        //{
        //    if (string.IsNullOrWhiteSpace(originContent)) return originContent;

        //    var sensitiveList = new List<string> { "Political.txt", "YellowRelated.txt" };
        //    StringBuilder sbOriginContent = new StringBuilder(originContent);
        //    foreach (var item in sensitiveList)
        //    {
        //        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SensitiveWords", item);
        //        if (await _fileManager.IsExistsFileAsync(filePath))
        //        {
        //            var words = await _fileManager.GetStringFromTxtAsync(filePath);
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
        #endregion

        #region Third
        //public async Task<string> GetSecurityBlogAsync(string originContent)
        //{
        //    if (string.IsNullOrWhiteSpace(originContent)) return originContent;
        //    _effectiveSensitiveNum = 0;

        //    StringBuilder sbOriginContent = new StringBuilder(originContent);
        //    foreach (var item in _sensitiveList)
        //    {
        //        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SensitiveWords", item);
        //        if (await _fileManager.IsExistsFileAsync(filePath))
        //        {
        //            var words = await _fileManager.GetStringFromTxtAsync(filePath);
        //            var wordList = words.Split("\r\n");
        //            if (wordList.Any())
        //            {
        //                _effectiveSensitiveNum++;
        //                foreach (var word in wordList)
        //                {
        //                    sbOriginContent.Replace(word, string.Join("", word.Select(s => "*")));
        //                }
        //            }
        //        }
        //    }

        //    return await Task.FromResult(sbOriginContent.ToString());
        //} 
        #endregion

        public async Task<string> GetSecurityBlogAsync(string originContent)
        {
            if (string.IsNullOrWhiteSpace(originContent)) return originContent;
            _effectiveSensitiveNum = 0;

            StringBuilder sbOriginContent = new StringBuilder(originContent);
            foreach (var item in _sensitiveList)
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SensitiveWords", item);
                if (await _fileManager.IsExistsFileAsync(filePath))
                {
                    var words = await _fileManager.GetStringFromTxtAsync(filePath);
                    var wordList = words.Split("\r\n");
                    if (wordList.Any())
                    {
                        _effectiveSensitiveNum++;
                        foreach (var word in wordList)
                        {
                            sbOriginContent.Replace(word, string.Join("", word.Select(s => "*")));
                        }
                    }
                }
            }

            if (originContent != sbOriginContent.ToString())
            {
                _loggerService.LogError($"【{originContent}】含有敏感字符", null);
            }

            return await Task.FromResult(sbOriginContent.ToString());
        }

        public async Task<bool> IsAllSensitiveListIsEffectiveAsync()
        {
            if (_effectiveSensitiveNum == 0) return await Task.FromResult(false);

            return await Task.FromResult(_sensitiveList.Count == _effectiveSensitiveNum);
        }
    }
}

