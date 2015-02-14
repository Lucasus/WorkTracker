using System;

namespace WorkTracker.Infrastructure
{
    public interface ITimeProvider
    {
        DateTime CurrentDate { get; }
    }
}
