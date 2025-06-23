using Domain.Models.MetrologyControl;
using Domain.Models.Standards;

namespace Application.UseCases.DTOs;

public class CalibrationJournalItemDto : ControlDto
{
    public static CalibrationJournalItemDto ToDto(CalibrationJournalItem calibrationJournalItem)
    {
        return new CalibrationJournalItemDto
        {
            Id = calibrationJournalItem.Id,
            Comments = calibrationJournalItem.Comments,
            StandardId = calibrationJournalItem.Standard.Id,
            PlaceId = calibrationJournalItem.Place.Id,
            Date = calibrationJournalItem.Date,
            ValidTo = calibrationJournalItem.ValidTo,
            CertificateId = calibrationJournalItem.CertificateId,
            CertificateImage = calibrationJournalItem.CertificateImage
        };
    }

    public static CalibrationJournalItem ToEntity(
        CalibrationJournalItemDto calibrationJournalItemDto,
        Place place,
        Standard standard)
    {
        return new CalibrationJournalItem
        {
            Id = calibrationJournalItemDto.Id,
            Comments = calibrationJournalItemDto.Comments,
            Standard = standard,
            Place = place,
            Date = calibrationJournalItemDto.Date,
            ValidTo = calibrationJournalItemDto.ValidTo,
            CertificateId = calibrationJournalItemDto.CertificateId,
            CertificateImage = calibrationJournalItemDto.CertificateImage
        };
    }
}