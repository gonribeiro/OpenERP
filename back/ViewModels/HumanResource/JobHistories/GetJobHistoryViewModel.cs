namespace OpenERP.ViewModels.HumanResource.JobHistories
{
    public class GetJobHistoryViewModel
    {
        public int Id { get; set; }
        public required int EmployeeId { get; set; }
        public required int JobId { get; set; }
        public required int DepartmentId { get; set; }
        public required string StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
