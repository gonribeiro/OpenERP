using System.ComponentModel;

namespace OpenERP.Enums.Global
{
    public enum Currency
    {
        [Description("USD")] // United States Dollar
        USD,

        [Description("EUR")] // Euro
        EUR,

        [Description("JPY")] // Japanese Yen
        JPY,

        [Description("GBP")] // British Pound Sterling
        GBP,

        [Description("AUD")] // Australian Dollar
        AUD,

        [Description("CAD")] // Canadian Dollar
        CAD,

        [Description("CHF")] // Swiss Franc
        CHF,

        [Description("CNY")] // Chinese Yuan Renminbi
        CNY,

        [Description("SEK")] // Swedish Krona
        SEK,

        [Description("NZD")] // New Zealand Dollar
        NZD,

        [Description("MXN")] // Mexican Peso
        MXN,

        [Description("SGD")] // Singapore Dollar
        SGD,

        [Description("HKD")] // Hong Kong Dollar
        HKD,

        [Description("NOK")] // Norwegian Krone
        NOK,

        [Description("KRW")] // South Korean Won
        KRW,

        [Description("TRY")] // Turkish Lira
        TRY,

        [Description("RUB")] // Russian Ruble
        RUB,

        [Description("INR")] // Indian Rupee
        INR,

        [Description("BRL")] // Brazilian Real
        BRL,

        [Description("ZAR")] // South African Rand
        ZAR
    }
}