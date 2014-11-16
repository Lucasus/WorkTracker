using System.Collections.Generic;
using WorkTracker.Entities;

namespace WorkTracker.Repositories
{
    public interface IDailyStatsRepository
    {
        IList<DailyStats> GetAll();
        void ReplaceWith(IList<DailyStats> allStats);
    }
}
