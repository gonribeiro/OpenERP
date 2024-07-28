namespace OpenERP.ViewModels.Auth.Users
{
    public class IndexUserViewModel
    {
        public required int Id { get; set; }
        public string? InactiveDate { get; set; }
        public required string Username { get; set; }
        public string? Employee { get; set; }
        public string? Roles { get; set; }
        public string? LastPasswordUpdatedAt { get; set; }
    }
}
