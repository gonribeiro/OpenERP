namespace OpenERP.ViewModels.Auth.Users
{
    public class GetUserViewModel
    {
        public required int Id { get; set; }
        public required string InactiveDate { get; set; }
        public required string Employee { get; set; }
        public required string Username { get; set; }
        public string? LastPasswordUpdatedAt { get; set; }

        public List<int>? RoleIds { get; set; }
    }
}
