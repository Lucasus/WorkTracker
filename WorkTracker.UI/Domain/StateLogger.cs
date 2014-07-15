using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.Domain
{
    public class StateLogger
    {
        public StateLogger()
        {
        }

        public void LogState(State state)
        {
            var message = "V1.0," + state.Name + "," + DateTime.Now.ToLongTimeString();

            string path = @"c:\temp\MyTest.txt";
            // This text is added only once to the file. 
            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (var sw = File.CreateText(path))
                {
                    sw.WriteLine("Hello");
                    sw.WriteLine("And");
                    sw.WriteLine("Welcome");
                }
            }

            // This text is always added, making the file longer over time 
            // if it is not deleted. 
            using (var sw = File.AppendText(path))
            {
                sw.WriteLine("This");
                sw.WriteLine("is Extra");
                sw.WriteLine("Text");
            }	
        }
    }
}
