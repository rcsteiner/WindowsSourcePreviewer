using System.Runtime.InteropServices;

namespace Win32
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The DRAWTEXTPARAMS structure definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [StructLayout(LayoutKind.Sequential)]
    public class DRAWTEXTPARAMS
    {
        /// <summary>
        ///  The structure size, in bytes.
        /// </summary>
        private int _Size = Marshal.SizeOf(typeof(DRAWTEXTPARAMS));

        /// <summary>
        ///  The size of each tab stop, in units equal to the average character width.
        /// </summary>
        public readonly int TabLength;

        /// <summary>
        ///  The left margin, in units equal to the average character width.
        /// </summary>
        public readonly int LeftMargin;

        /// <summary>
        ///  The right margin, in units equal to the average character width.
        /// </summary>
        public readonly int RightMargin;


        /// <summary>
        ///  Receives the number of characters processed by DrawTextEx, including white-space characters.
        /// The number can be the length of the string or the index of the first line that falls below the drawing area.
        /// Note that DrawTextEx always processes the entire string if the DT_NOCLIP formatting flag is specified.
        /// </summary>
        public readonly uint LengthDrawn;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructor: For DRAWTEXTPARAMS.
        /// </summary>
        /// <param name="tabLength">   [optional=4] The tab Length.</param>
        /// <param name="leftMargin">  [optional=0] The left Margin.</param>
        /// <param name="rightMargin"> [optional=0] The right Margin.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public DRAWTEXTPARAMS(int tabLength, int leftMargin=0, int rightMargin=0)
        {
            TabLength     = tabLength;
            LeftMargin    = leftMargin;
            RightMargin   = rightMargin;
            LengthDrawn   = 0;
        }
    }

}