
namespace Win32
{

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Font Quality Enumeration definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public enum FontQuality : byte
    {
        DEFAULT_QUALITY           = 0,
        DRAFT_QUALITY             = 1,
        PROOF_QUALITY             = 2,
        NONANTIALIASED_QUALITY    = 3,
        ANTIALIASED_QUALITY       = 4,
        CLEARTYPE_QUALITY         = 5,
        CLEARTYPE_NATURAL_QUALITY = 6
    }
}