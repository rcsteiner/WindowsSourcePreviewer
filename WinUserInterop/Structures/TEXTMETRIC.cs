using System;
using System.Runtime.InteropServices;

namespace Win32
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The TEXTMETRICA structure definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct TEXTMETRICA
    {
        public int tmHeight;
        public int tmAscent;
        public int tmDescent;
        public int tmInternalLeading;
        public int tmExternalLeading;
        public int tmAveCharWidth;
        public int tmMaxCharWidth;
        public int tmWeight;
        public int tmOverhang;
        public int tmDigitizedAspectX;
        public int tmDigitizedAspectY;
        public byte tmFirstChar;
        public byte tmLastChar;
        public byte tmDefaultChar;
        public byte tmBreakChar;
        public byte tmItalic;
        public byte tmUnderlined;
        public byte tmStruckOut;
        public byte tmPitchAndFamily;
        public byte tmCharSet;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The TEXTMETRIC structure definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct TEXTMETRIC
    {
        public int tmHeight;
        public int tmAscent;
        public int tmDescent;
        public int tmInternalLeading;
        public int tmExternalLeading;
        public int tmAveCharWidth;
        public int tmMaxCharWidth;
        public int tmWeight;
        public int tmOverhang;
        public int tmDigitizedAspectX;
        public int tmDigitizedAspectY;
        public char tmFirstChar; // this assumes the ANSI charset; for the UNICODE charset the type is char (or short)
        public char tmLastChar; // this assumes the ANSI charset; for the UNICODE charset the type is char (or short)
        public char tmDefaultChar; // this assumes the ANSI charset; for the UNICODE charset the type is char (or short)
        public char tmBreakChar; // this assumes the ANSI charset; for the UNICODE charset the type is char (or short)
        public byte tmItalic;
        public byte tmUnderlined;
        public byte tmStruckOut;
        public byte tmPitchAndFamily;
        public byte tmCharSet;
    }

    //[Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    //public struct TEXTMETRICW
    //{
    //    public int tmHeight;
    //    public int tmAscent;
    //    public int tmDescent;
    //    public int tmInternalLeading;
    //    public int tmExternalLeading;
    //    public int tmAveCharWidth;
    //    public int tmMaxCharWidth;
    //    public int tmWeight;
    //    public int tmOverhang;
    //    public int tmDigitizedAspectX;
    //    public int tmDigitizedAspectY;
    //    public ushort tmFirstChar;
    //    public ushort tmLastChar;
    //    public ushort tmDefaultChar;
    //    public ushort tmBreakChar;
    //    public byte tmItalic;
    //    public byte tmUnderlined;
    //    public byte tmStruckOut;
    //    public byte tmPitchAndFamily;
    //    public byte tmCharSet;
    //}
}