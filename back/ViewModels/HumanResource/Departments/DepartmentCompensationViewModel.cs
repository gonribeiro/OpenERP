namespace OpenERP.ViewModels.HumanResource.Departments
{
    public class DepartmentCompensationViewModel
    {
        public string DepartmentName { get; set; }
        public decimal TotalCompensation { get; set; }
        public List<CompensationEmployeeViewModel> Users { get; set; } = new List<CompensationEmployeeViewModel>();
        public decimal SharedCompensation { get; set; }
    }
}
