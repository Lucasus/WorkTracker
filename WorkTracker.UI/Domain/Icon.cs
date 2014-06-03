using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.UI
{
    public class Icon
    {
        public Icon(string path)
        {
            this.Path = path;
        }

        public string Path { get; private set; }
    }
}
