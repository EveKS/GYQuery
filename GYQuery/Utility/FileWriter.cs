using GYQuery.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYQuery.Utility
{
    interface IFileWriter
    {
        Task Write(string savePath, string fileName, int findNum, Encoding enc, Task<IEnumerable<QueryModel>> result);
    }
    class FileWriter : IFileWriter
    {
        private static readonly Encoding DEFAULT_ENCODING = Encoding.UTF8;
        public async Task Write(string savePath, string fileName, int findNum, Encoding enc, Task<IEnumerable<QueryModel>> result)
            => await Task.Run(async () =>
            {
                using (StreamWriter sw = new StreamWriter($"{savePath}\\{fileName}.txt", false,  enc ?? DEFAULT_ENCODING))
                {
                    var tmp = await result;
                    await sw.WriteLineAsync($"{fileName}\t{findNum}");
                    foreach (var res in tmp)
                        await sw.WriteLineAsync($"{res.FindPade}\t{res.DomenName}");
                }
            });
    }
}
