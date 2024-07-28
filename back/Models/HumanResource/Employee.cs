using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenERP.Models.Auth;
using OpenERP.Models.Global;
using OpenERP.Enums.HumanResource;

namespace OpenERP.Models.HumanResource
{
    public class Employee
    {
        public int Id { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? InactiveDate { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public required string FirstName { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public required string LastName { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime Birthdate { get; set; }
        [StringLength(255, MinimumLength = 3)]
        public string? PlaceOfBirth { get; set; }
        [Required]
        [EnumDataType(typeof(MaritalStatus), ErrorMessage = "The value of the field Type is invalid.")]
        public required MaritalStatus MaritalStatus { get; set; }
        public required int NationalityId { get; set; }
        // Bank
        public int? BankId { get; set; }
        public string? AccountNumber { get; set; }
        public string? RoutingNumber { get; set; }
        // Address
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public required string Address { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public required string ZipCode { get; set; }
        [Required]
        public required int CityId { get; set; }
        // Personal Documents
        [Required]
        [StringLength(40, MinimumLength = 6)]
        public required string SocialSecurityNumber { get; set; }
        public string? PassportNumber { get; set; }
        public string? DriverLicenseNumber { get; set; }

        public User User { get; set; }
        [ForeignKey("NationalityId")]
        public Country Nationality { get; set; }
        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public ICollection<DepartmentEmployee> DepartmentEmployee { get; set; }
        [ForeignKey("CityId")]
        public City City { get; set; }
        [ForeignKey("BankId")]
        public Company Bank { get; set; }
        public ICollection<JobHistory> JobHistories { get; set; }
        public ICollection<EducationHistory> EducationHistories { get; set; }
        public ICollection<Remuneration> Remunerations { get; set; }
        public ICollection<Vacation> Vacations { get; set; }
        public ICollection<Vacation> ApprovedVacations { get; set; }
        public ICollection<FileStorage> FileStorages { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
