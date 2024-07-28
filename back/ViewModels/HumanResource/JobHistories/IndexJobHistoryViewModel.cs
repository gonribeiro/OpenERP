namespace OpenERP.ViewModels.HumanResource.JobHistories
{
    public class IndexJobHistoryViewModel
    {
        public int Id { get; set; }
        public required string Job { get; set; }
        public required string Department { get; set; }
        public required string StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
