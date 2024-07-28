namespace OpenERP.ViewModels.HumanResource.Educations
{
    public class IndexEducationViewModel
    {
        public int Id { get; set; }
        public required string Institution { get; set; }
        public required string Course { get; set; }
        public required string EducationLevel { get; set; }
        public required string StartDate { get; set; }
        public required string EndDate { get; set; }
    }
}
