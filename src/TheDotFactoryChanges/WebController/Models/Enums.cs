namespace WebControllers.Models
{
    public enum ConfigurationCommentStyle
    {

        [System.Runtime.Serialization.EnumMember(Value = @"c")]
        C = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"cpp")]
        Cpp = 1,

    }

    public enum ConfigurationRotation
    {

        [System.Runtime.Serialization.EnumMember(Value = @"RotateZero")]
        RotateZero = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"RotateNinety")]
        RotateNinety = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"RotateOneEighty")]
        RotateOneEighty = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"RotateTwoSeventy")]
        RotateTwoSeventy = 3,

    }

    public enum ConfigurationPaddingRemovalHorizontal
    {

        [System.Runtime.Serialization.EnumMember(Value = @"None")]
        None = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"Tighest")]
        Tighest = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"Fixed")]
        Fixed = 2,

    }

    public enum ConfigurationPaddingRemovalVertical
    {

        [System.Runtime.Serialization.EnumMember(Value = @"None")]
        None = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"Tighest")]
        Tighest = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"Fixed")]
        Fixed = 2,

    }

    public enum ConfigurationBitLayout
    {

        [System.Runtime.Serialization.EnumMember(Value = @"RowMajor")]
        RowMajor = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"ColumnMajor")]
        ColumnMajor = 1,

    }

    public enum ConfigurationByteOrder
    {

        [System.Runtime.Serialization.EnumMember(Value = @"LsbFirst")]
        LsbFirst = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"MsbFirst")]
        MsbFirst = 1,

    }

    public enum ConfigurationByteFormat
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Binary")]
        Binary = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"Hex")]
        Hex = 1,

    }
}
