using System.ComponentModel;

namespace OpenERP.Enums.HumanResource
{
    public enum VacationType
    {
        [Description("Annual Leave")]
        AnnualLeave,

        [Description("Sick Leave")]
        SickLeave,

        [Description("Unpaid Leave")]
        UnpaidLeave,

        [Description("Maternity Leave")]
        MaternityLeave,

        [Description("Paternity Leave")]
        PaternityLeave,

        [Description("Bereavement Leave")]
        BereavementLeave,

        [Description("Compensatory Time Off")]
        CompensatoryTimeOff,

        [Description("Study Leave")]
        StudyLeave,

        [Description("Sabbatical")]
        Sabbatical
    }
}
