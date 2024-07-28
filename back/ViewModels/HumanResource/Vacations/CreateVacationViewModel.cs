namespace OpenERP.ViewModels.HumanResource.Vacations
{
    public class CreateVacationViewModel
    {
        public int Id { get; set; }
        public required int EmployeeId { get; set; }
        public required string Type { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public string? Reason { get; set; }
        public int? ApprovedById { get; set; }
    }
}
