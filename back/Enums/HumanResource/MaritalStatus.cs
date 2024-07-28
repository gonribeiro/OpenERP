using System.ComponentModel;

namespace OpenERP.Enums.HumanResource
{
    public enum MaritalStatus
    {
        [Description("Married")]
        Married,

        [Description("Single")]
        Single,

        [Description("Divorced")]
        Divorced,

        [Description("Widower")]
        Widower
    }
}
