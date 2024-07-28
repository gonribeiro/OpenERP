namespace OpenERP.Models.Global
{
    public class Audit
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
        public string Action { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
    }
}
