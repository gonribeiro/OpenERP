
using OpenERP.ViewModels.Global.Contacts;

namespace OpenERP.ViewModels.HumanResource.Employees
{
    public class GetEmployeeViewModel
    {
        public int Id { get; set; }
        public required string InactiveDate { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Birthdate { get; set; }
        public required string MaritalStatus { get; set; }
        public required int NationalityId { get; set; }
        public string? PlaceOfBirth { get; set; }
        public List<int>? DepartmentIds { get; set; }
        public List<ContactViewModel>? Contacts { get; set; }
        public required string Address { get; set; }
        public required string ZipCode { get; set; }
        public required int CityId { get; set; }
        public required string SocialSecurityNumber { get; set; }
        public required string PassportNumber { get; set; }
        public required string DriverLicenseNumber { get; set; }
        public int? BankId { get; set; }
        public required string AccountNumber { get; set; }
        public required string RoutingNumber { get; set; }
        public List<ContactViewModel>? Relatives { get; set; }
        public int? PhotoId { get; set; }
        public string? Photo { get; set; }

    }
}
