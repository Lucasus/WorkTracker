using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.Infrastructure
{
    public class TimeProvider : WorkTracker.Infrastructure.ITimeProvider
    {
        public DateTime CurrentDate
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}
