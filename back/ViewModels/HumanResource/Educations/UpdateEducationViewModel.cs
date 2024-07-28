namespace OpenERP.ViewModels.HumanResource.Educations
{
    public class UpdateEducationViewModel
    {
        public int Id { get; set; }
        public required int InstitutionId { get; set; }
        public required string Course { get; set; }
        public required string EducationLevel { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
    }
}
