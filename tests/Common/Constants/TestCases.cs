namespace Tests.Common.Constants;

public class Cases
{
    public const int Negative = -1;
    public const int Zero = -1;

    public static readonly DateTime MinDateTime = DateTime.MinValue;
    public static readonly DateTime DateTimeInPast = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    
    public const int InvalidVerificationInterval = Domain.Constants.Standards.MinVerificationInterval - 1;
    public const int InvalidCalibrationInterval = Domain.Constants.Standards.MinCalibrationInterval - 1;

    public const object Null = null;
    public const string EmptyString = "";

    public const string Length4 = "0123";
    public const string Length16 = "0123456789012345";

    public const string Length51 = "012345678901234567890123456789012345678901234567890";
    public const string Length101 = "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
    public const string Length201 = "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
}