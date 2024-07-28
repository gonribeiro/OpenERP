namespace OpenERP.Models.Auth
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public DateTime? InactiveDate { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
