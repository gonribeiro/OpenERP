namespace OpenERP.ViewModels.HumanResource.Educations
{
    public class GetEducationViewModel
    {
        public int Id { get; set; }
        public required int InstitutionId { get; set; }
        public required string Course { get; set; }
        public required string EducationLevel { get; set; }
        public required string StartDate { get; set; }
        public required string EndDate { get; set; }
    }
}
