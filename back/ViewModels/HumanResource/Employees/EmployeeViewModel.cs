
using OpenERP.ViewModels.Global.Contacts;
using System.ComponentModel.DataAnnotations;

namespace OpenERP.ViewModels.HumanResource.Employees
{
    public class EmployeeViewModel
    {
        public DateTime? InactiveDate { get; set; }
        [StringLength(120, MinimumLength = 3)]
        public required string FirstName { get; set; }
        [StringLength(255, MinimumLength = 3)]
        public required string LastName { get; set; }
        public required DateTime Birthdate { get; set; }
        [StringLength(255, MinimumLength = 3)]
        public required string MaritalStatus { get; set; }
        public required int NationalityId { get; set; }
        public string? PlaceOfBirth { get; set; }
        // Bank
        public int? BankId { get; set; }
        public string? AccountNumber { get; set; }
        public string? RoutingNumber { get; set; }
        // Address
        [StringLength(120, MinimumLength = 3)]
        public required string Address { get; set; }
        [StringLength(40, MinimumLength = 3)]
        public required string ZipCode { get; set; }
        public required int CityId { get; set; }
        // Personal Documents
        [StringLength(40, MinimumLength = 6)]
        public required string SocialSecurityNumber { get; set; }
        public string? PassportNumber { get; set; }
        public string? DriverLicenseNumber { get; set; }

        public List<ContactViewModel>? Contacts { get; set; }
        public List<int>? DepartmentIds { get; set; }
        public List<ContactViewModel>? Relatives { get; set; }
    }
}
