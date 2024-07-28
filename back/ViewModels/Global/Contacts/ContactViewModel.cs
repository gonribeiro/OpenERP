namespace OpenERP.ViewModels.Global.Contacts
{
    public class ContactViewModel
    {
        public int? Id { get; set; }
        public required string Type { get; set; }
        public required string Information { get; set; }
        public string? ContactName { get; set; }
        public string? ContactRelationType { get; set; }
    }
}
