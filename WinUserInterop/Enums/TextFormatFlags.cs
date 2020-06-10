using System;

namespace Win32
{

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Text Format Flags Enumeration definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Flags]
    public enum TextFormatFlags : uint
    {
        Bottom                    = 8,
        CalculateRectangle        = 1024,    // 0x00000400
        EndEllipsis               = 32768,   // 0x00008000
        ExpandTabs                = 64,      // 0x00000040
        ExternalLeading           = 512,     // 0x00000200
        Default                   = 0,
        HidePrefix                = 1048576, // 0x00100000
        HorizontalCenter          = 1,
        Internal                  = 4096,    // 0x00001000
        Left                      = 0,
        ModifyString              = 65536,   // 0x00010000
        NoClipping                = 256,     // 0x00000100
        NoPrefix                  = 2048,    // 0x00000800
        NoFullWidthCharacterBreak = 524288,  // 0x00080000
        PathEllipsis              = 16384,   // 0x00004000
        PrefixOnly                = 2097152, // 0x00200000
        Right                     = 2,
        RightToLeft               = 131072,  // 0x00020000
        SingleLine                = 32,      // 0x00000020
        TabStop                   = 128,     // 0x00000080
        TextBoxControl            = 8192,    // 0x00002000
        Top                       = 0,
        VerticalCenter            = 4,
        WordBreak                 = 16,      // 0x00000010
        WordEllipsis              = 262144  // 0x00040000
    }
}