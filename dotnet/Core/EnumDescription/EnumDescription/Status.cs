using System.ComponentModel;

namespace EnumDescription
{
    public enum Status
    {
        [Description("Still in Draft")]
        Draft,
        [Description("Submitted, still not processed")]
        Submitted,
        [Description("Pending administration approval")]
        Pending,
        [Description("Approved by the administration")]
        Approved,
        [Description("Published on the webite")]
        Published
    }
}
