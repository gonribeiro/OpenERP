using System.ComponentModel;

namespace OpenERP.Enums.Global
{
    public enum ContactType
    {
        [Description("Email")]
        Email,

        [Description("Phone")]
        Phone,

        [Description("Mobile")]
        Mobile,

        [Description("Web")]
        Web,

        [Description("Linkedin")]
        Linkedin,

        [Description("Telegram")]
        Telegram,

        [Description("Facebook")]
        Facebook,

        [Description("Instagram")]
        Instagram,
    }
}