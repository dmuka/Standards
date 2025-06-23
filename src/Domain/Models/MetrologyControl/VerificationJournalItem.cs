using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.MetrologyControl;

public class VerificationJournalItem : Control, ICacheable
{
    public static string GetCacheKey()
    {
        return Cache.VerificationJournal;
    }
}