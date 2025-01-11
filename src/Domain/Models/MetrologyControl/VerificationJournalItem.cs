using System.Windows.Input;
using Domain.Constants;
using Domain.Models.DTOs;
using Domain.Models.Interfaces;
using Domain.Models.Standards;

namespace Domain.Models.MetrologyControl;

public class VerificationJournalItem : Control, ICacheable
{
    public static string GetCacheKey()
    {
        return Cache.VerificationJournal;
    }
    
    public static VerificationJournalItemDto ToDto(VerificationJournalItem verificationJournalItem)
    {
        return new VerificationJournalItemDto
        {
            Id = verificationJournalItem.Id,
            Comments = verificationJournalItem.Comments,
            StandardId = verificationJournalItem.Standard.Id,
            PlaceId = verificationJournalItem.Place.Id,
            Date = verificationJournalItem.Date,
            ValidTo = verificationJournalItem.ValidTo,
            CertificateId = verificationJournalItem.CertificateId,
            CertificateImage = verificationJournalItem.CertificateImage
        };
    }

    public static VerificationJournalItem ToEntity(
        VerificationJournalItemDto verificationJournalItemDto,
        Place? place,
        Standard? standard)
    {
        return new VerificationJournalItem
        {
            Id = verificationJournalItemDto.Id,
            Comments = verificationJournalItemDto.Comments,
            Standard = standard,
            Place = place,
            Date = verificationJournalItemDto.Date,
            ValidTo = verificationJournalItemDto.ValidTo,
            CertificateId = verificationJournalItemDto.CertificateId,
            CertificateImage = verificationJournalItemDto.CertificateImage
        };
    }
}