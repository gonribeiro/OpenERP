using System.ComponentModel.DataAnnotations;

namespace OpenERP.ViewModels.HumanResource.Jobs
{
    public class JobViewModel
    {
        public int Id { get; set; }
        [StringLength(120, MinimumLength = 3)]
        public required string Name { get; set; }
        public required string Currency { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
    }
}