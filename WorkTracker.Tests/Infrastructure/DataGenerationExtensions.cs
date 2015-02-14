using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTracker.Business;

namespace WorkTracker.Tests
{
    public static class DataGenerationExtensions
    {
        public static Tuple<StateNamesEnum, string> On(this StateNamesEnum stateChange, string dateWhenChangedTo)
        {
            return new Tuple<StateNamesEnum, string>(stateChange, dateWhenChangedTo);
        }
    }
}

