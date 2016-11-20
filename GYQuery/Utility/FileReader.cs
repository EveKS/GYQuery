using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYQuery.Utility
{
    interface IFileReader
    {
        Task<IEnumerable<string>> Read(string filePath, Encoding enc);
    }
    class FileReader : IFileReader
    {
        private static readonly Encoding DEFAULT_ENCODING = Encoding.UTF8;
        public async Task<IEnumerable<string>> Read(string filePath, Encoding enc)
            => await Task.Run(async () =>
              {
                  using (StreamReader sr = new StreamReader(filePath, enc ?? DEFAULT_ENCODING))
                  {
                      var tmp = new List<string>(100);
                      while (!sr.EndOfStream)
                      {
                          tmp.Add(await sr.ReadLineAsync());
                      }
                      return tmp;
                  }
              });
    }
}
