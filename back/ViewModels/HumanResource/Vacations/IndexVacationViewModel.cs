namespace OpenERP.ViewModels.HumanResource.Vacations
{
    public class IndexVacationViewModel
    {
        public int Id { get; set; }
        public string? EmployeeName { get; set; }
        public required string Type { get; set; }
        public required string StartDate { get; set; }
        public required string EndDate { get; set; }
        public string? Reason { get; set; }
        public string? ApprovedByName { get; set; }
    }
}
