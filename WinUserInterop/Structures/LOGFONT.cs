
using System.Runtime.InteropServices;
using System.Text;

namespace Win32
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///     The LOGFONT structure definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct LOGFONT
    {
        public int                lfHeight;
        public int                lfWidth;
        public int                lfEscapement;
        public int                lfOrientation;
        public FontWeight         lfWeight;
        [MarshalAs(UnmanagedType.U1)] 
        public bool               lfItalic;
        [MarshalAs(UnmanagedType.U1)] 
        public bool               lfUnderline;
        [MarshalAs(UnmanagedType.U1)] 
        public bool               lfStrikeOut;
        public FontCharSet        lfCharSet;
        public FontPrecision      lfOutPrecision;
        public FontClipPrecision  lfClipPrecision;
        public FontQuality        lfQuality;
        public FontPitchAndFamily lfPitchAndFamily;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string lfFaceName;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Method: Get a string representation of LOGFONT.
        /// </summary>
        /// <returns>
        ///     The string representation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat(" Height:         {0}\n", lfHeight);
            sb.AppendFormat(" Width:          {0}\n", lfWidth);
            sb.AppendFormat(" Escapement:     {0}\n", lfEscapement);
            sb.AppendFormat(" Orientation:    {0}\n", lfOrientation);
            sb.AppendFormat(" Weight:         {0}\n", lfWeight);
            sb.AppendFormat(" Italic:         {0}\n", lfItalic);
            sb.AppendFormat(" Underline:      {0}\n", lfUnderline);
            sb.AppendFormat(" StrikeOut:      {0}\n", lfStrikeOut);
            sb.AppendFormat(" CharSet:        {0}\n", lfCharSet);
            sb.AppendFormat(" OutPrecision:   {0}\n", lfOutPrecision);
            sb.AppendFormat(" ClipPrecision:  {0}\n", lfClipPrecision);
            sb.AppendFormat(" Quality:        {0}\n", lfQuality);
            sb.AppendFormat(" PitchAndFamily: {0}\n", lfPitchAndFamily);
            sb.AppendFormat(" FaceName:       {0}\n", lfFaceName);
            return sb.ToString();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Method: Clone this font.
        /// </summary>
        /// <returns>
        ///     The Win32.LOGFONT value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public LOGFONT Clone()
        {
            return (LOGFONT) MemberwiseClone();
        }
    }
}