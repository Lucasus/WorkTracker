using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.Tests
{
    public static class StringExtensions
    {
        public static DateTime Date(this string date)
        {
            return DateTime.Parse(date);
        }
    }
}
