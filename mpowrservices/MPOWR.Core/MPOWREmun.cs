using System.ComponentModel;

namespace MPOWR.Core
{
    public enum FieldType
    {
        Text,
        Select,
        Date,
        Time
    }

    public enum EnumUserType
    {
        Country = 1,
        District = 2,
        Geo = 3,
        SubRegional = 5,
        Regional = 6,
        WorldWide = 4,
        US = 138,
        [Description("Regional")]
        Region,
        [Description("World Wide")]
        WW,
        [Description("Sub-Regional")]
        Subregion
    };

}
