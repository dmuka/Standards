namespace Domain.Models.MetrologyControl.Contacts;

public class Social : BaseEntity
{
    public required SocialInfo SocialInfo { get; set; }
    public required string SocialIdValue { get; set; }
    public required Contact Contact { get; set; }
}