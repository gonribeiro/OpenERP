using System.ComponentModel.DataAnnotations;

namespace OpenERP.ViewModels.HumanResource.Departments
{
    public class DepartmentViewModel
    {
        [StringLength(40, MinimumLength = 3)]
        public required string Name { get; set; }
        public int? ManagerId { get; set; }
    }
}
