namespace OpenERP.ViewModels.HumanResource.Jobs
{
    public class GetJobViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Currency { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
    }
}
