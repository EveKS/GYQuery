using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GYQuery.Utility
{
    public interface IFileManager
    {
        bool IsExist(string filePath);
        bool IsFolderExist(string folderPath);
        Task MakeFiles(string fileKeyPath, string fileSavePath, int findNum, Encoding encoding);
    }
    class FileManager : IFileManager
    {
        private readonly IFileReader _fileReader;
        private readonly IFileWriter _fileWriter;
        private readonly IGoogleQuery _googleQuery;

        public FileManager() : this(new FileReader(), new FileWriter(), new GoogleQuery())
        { }

        FileManager(IFileReader fileReader, IFileWriter fileWriter, IGoogleQuery googleQuery)
        {
            _fileReader = fileReader;
            _fileWriter = fileWriter;
            _googleQuery = googleQuery;
        }

        public bool IsExist(string filePath)
        {
            bool isExist = File.Exists(filePath);
            return isExist;
        }

        public bool IsFolderExist(string folderPath)
        {
            bool isExist = Directory.Exists(folderPath);
            return isExist;
        }

        public async Task MakeFiles(string fileKeyPath, string fileSavePath, int findNum, Encoding encoding)
            => await Task.Run(async () =>
        {
            var tmp1 = await _fileReader.Read(fileKeyPath, encoding);
            foreach (var name in tmp1)
            {
                var tmp2 = _googleQuery.PageParser(GetQuery(name, findNum));
                await _fileWriter.Write(fileSavePath, name, findNum, encoding, tmp2);
            }
        });
        private static string GetQuery(string name, int findNum)
            => string.Format("https://www.google.de/search?q={0}&start={1}&num={2}",
                HttpUtility.UrlEncode(name), 0, findNum);
    }
}
