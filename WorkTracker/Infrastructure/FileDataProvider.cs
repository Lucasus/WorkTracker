using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.Infrastructure
{
    public class FileDataProvider : IStringDataProvider
    {
        private string fileName;

        public FileDataProvider(string fileName)
        {
            this.fileName = fileName;
        }

        public void AddRecord(string record)
        {
            createFileIfNotExists(fileName);
            using (var sw = File.AppendText(fileName))
            {
                sw.WriteLine(record);
            }
        }

        public IList<string> GetAll()
        {
            createFileIfNotExists(fileName);
            return File.ReadAllLines(fileName).ToList();
        }

        public void OverrideWith(IList<string> records)
        {
            File.WriteAllLines(fileName, records.ToArray());
        }

        private void createFileIfNotExists(string fileName)
        {
            if (!File.Exists(fileName))
            {
                using (File.Create(fileName)) { }
            }
        }
    }
}
