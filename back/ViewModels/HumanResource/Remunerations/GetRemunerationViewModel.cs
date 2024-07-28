namespace OpenERP.ViewModels.HumanResource.Remunerations
{
    public class GetRemunerationViewModel
    {
        public int Id { get; set; }
        public required string Currency { get; set; }
        public required decimal BaseSalary { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? Commission { get; set; }
        public decimal? OtherAllowances { get; set; }
        public string? OtherAllowancesDescription { get; set; }
        public required string StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
