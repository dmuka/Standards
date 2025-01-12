namespace Domain.Models.MetrologyControl.Contacts;

public class ContactSocial : BaseEntity
{
    public required Social Social { get; set; }
    public required string IdValue { get; set; }
}