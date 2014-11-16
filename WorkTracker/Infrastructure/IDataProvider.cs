using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.Infrastructure
{
    public interface IStringDataProvider
    {
        void AddRecord(string record);
        IList<string> GetAll();
        void OverrideWith(IList<string> records);
    }
}
