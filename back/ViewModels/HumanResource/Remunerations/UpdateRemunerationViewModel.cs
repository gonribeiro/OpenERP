using System.ComponentModel.DataAnnotations.Schema;

namespace OpenERP.ViewModels.HumanResource.Remunerations
{
    public class UpdateRemunerationViewModel
    {
        public required string Currency { get; set; }
        public required decimal BaseSalary { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? Commission { get; set; }
        public decimal? OtherAllowances { get; set; }
        public string? OtherAllowancesDescription { get; set; }
        public required DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}