using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.MetrologyControl;

public class CalibrationJournalItem : Control, ICacheable
{
    public static string GetCacheKey()
    {
        return Cache.CalibrationJournal;
    }
}