using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.Infrastructure
{
    public static class FileExtensions
    {
        public static void AppendToFile(this string text, string fileName)
        {
            using (var sw = File.AppendText(fileName))
            {
                sw.WriteLine(text);
            }
        }
    }
}
