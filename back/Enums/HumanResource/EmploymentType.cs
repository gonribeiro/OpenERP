using System.ComponentModel;

namespace OpenERP.Enums.HumanResource
{
    public enum EmploymentType
    {
        [Description("Employee")]
        Employee,

        [Description("Contractor")]
        Contractor,

        [Description("Temporary Worker")]
        TemporaryWorker,

        [Description("Consultant")]
        Consultant,

        [Description("Freelancer")]
        Freelancer,

        [Description("Agency Temporary Worker")]
        AgencyTemporaryWorker,

        [Description("Unpaid Intern")]
        UnpaidIntern,

        [Description("Paid Intern")]
        PaidIntern,

        [Description("Volunteer")]
        Volunteer
    }
}
