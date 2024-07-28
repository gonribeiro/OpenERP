
namespace OpenERP.ViewModels.HumanResource.Employees
{
    public class IndexEmployeeViewModel
    {
        public int Id { get; set; }
        public string? InactiveDate { get; set; }
        public required string Name { get; set; }
        public string? City { get; set; }
        public string? Nationality { get; set; }
        public string? Departments { get; set; }
        public string? Birthdate { get; set; }
    }
}
