namespace OpenERP.ViewModels.HumanResource.Vacations
{
    public class GetVacationViewModel
    {
        public int Id { get; set; }
        public required string Type { get; set; }
        public required string StartDate { get; set; }
        public required string EndDate { get; set; }
        public string? Reason { get; set; }
        public int? ApprovedById { get; set; }
    }
}
