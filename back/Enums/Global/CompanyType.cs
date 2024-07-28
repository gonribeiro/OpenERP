using System.ComponentModel;

namespace OpenERP.Enums.Global
{
    public enum CompanyType
    {
        [Description("Bank")]
        Bank,

        [Description("Supply")]
        Supply,

        [Description("Distributor")]
        Distributor,

        [Description("Consultancy")]
        Consultancy,

        [Description("IT")]
        IT,

        [Description("Education")]
        Education,

        [Description("Healthcare")]
        Healthcare,

        [Description("Construction")]
        Construction,

        [Description("Transport Logistics")]
        TransportLogistics,

        [Description("Entertainment")]
        Entertainment,

        [Description("Hospitality Tourism")]
        HospitalityTourism,

        [Description("Financial Services")]
        FinancialServices
    }
}