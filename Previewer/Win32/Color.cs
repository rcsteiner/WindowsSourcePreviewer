////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Simple color type.
// 
//  Author:      Robert C. Steiner
// 
//=====================================================[ History ]=====================================================
//  Date        Who      What
//  ----------- ------   ----------------------------------------------------------------------------------------------
//  6/10/2020   RCS      Initial Code.
//====================================================[ Copyright ]====================================================
// 
//  Copyright 2020 Robert C. Steiner
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software
//  and associated documentation files (the "Software"), to deal in the Software without restriction,
//  including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
//  and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
//  subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
//  AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES
//  OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//  CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;

namespace Win32
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Colors Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public  struct Color
    {
        /// <summary>
        ///  The Value field.
        /// </summary>
        public uint Value;

        /// <summary>
        ///  Get/Set Red.
        /// </summary>
        public uint Red { get { return Value & 0xff; } set {Value = (uint) ((Value & ~0xff) | value); } }

        /// <summary>
        ///  Get/Set Green.
        /// </summary>
        public uint Green { get { return (Value >> 8) & 0xff; } set { Value = (uint)((Value & ~0xff00) | (value<<8)); } }

        /// <summary>
        ///  Get/Set Blue.
        /// </summary>
        public uint Blue { get { return (Value >> 16) & 0xff; } set { Value = (uint)((Value & ~0xff0000) | (value<<16)); } }

        /// <summary>
        ///  Get/Set Alpha.
        /// </summary>
        public uint Alpha { get { return (Value >> 24) & 0xff; } set { Value = (Value & ~0xff000000) | (value << 24); } }

        /// <summary>
        ///  Get Luminous.
        /// Lumininance less than .179 is dark.
        /// </summary>
        public double Luminance { get { return (0.2126 *LValue(Red)) +(0.7152 * LValue(Green)) +(0.0722 * LValue(Blue)); } }

        /// <summary>
        ///  Test if this color is Dark
        /// </summary>
        public bool IsDark { get { return (((Red * 0.299) + (Green * 0.587) + (Blue * 0.114)) < 186); } }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Contrast Ratio.
        /// Contrast ratios can range from 1 to 21 (commonly written 1:1 to 21:1).
        /// </summary>
        /// <param name="background">  The background color.</param>
        /// <returns>
        ///  The double value.
        /// </returns>
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public double ContrastRatio(Color background)
        {
            var l1 = Luminance + .05;
            var l2 = background.Luminance + .05;
            return  (l1 > l2) ? l1  / l2 : l2 /l1;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Lighten the color by an amount.
        /// </summary>
        /// <param name="inAmount">  The in Amount.</param>
        /// <returns>
        ///  The Win32.Color value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Color Lighten( double inAmount)
        {
            return new Color(SatMultiply(Red, inAmount), SatMultiply(Green,  inAmount), SatMultiply(Blue , inAmount));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Support for saturated Multiply Add.
        /// </summary>
        /// <param name="component">      color component</param>
        /// <param name="inAmount">  The in Amount.</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private int SatMultiply(uint component, double inAmount)
        {
            return (int)Math.Min(255, component + 255 * inAmount);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Calcuate the luminance for component per  ITU-R recommendation BT.709.
        /// </summary>
        /// <param name="v">  The uint v.</param>
        /// <returns>
        ///  The double value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private double LValue(uint v)
        {
            var c = v / 255.0;
            return c <= 0.03928 ? c / 12.92 : Math.Pow(((c + 0.055) / 1.055), 2.4);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructor: For Color.
        /// </summary>
        /// <param name="value"> [optional=0] The value.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Color(uint value=0)
        {
            Value = value;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructor: For Color.
        /// </summary>
        /// <param name="red">    The red.</param>
        /// <param name="green">  The green.</param>
        /// <param name="blue">   The blue.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Color(int red, int green, int blue)
        {
            Value = (uint) ((blue << 16) | (green << 8) | red);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructor: For Color.
        /// </summary>
        /// <param name="red">    The red.</param>
        /// <param name="green">  The green.</param>
        /// <param name="blue">   The blue.</param>
        /// <param name="alpha">  The alpha.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Color(int red, int green, int blue, int alpha)
        {
            Value = (uint)((alpha << 24) | (blue << 16) | (green << 8) | red);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Operator Overload conversion to Uint.
        /// </summary>
        /// <param name="c">  The Color c.</param>
        /// <returns>
        ///  The uint result of the uint operation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static implicit operator uint(Color c) { return c.Value;}

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Operator Overload: convertion to Int.
        /// </summary>
        /// <param name="c">  The Color c.</param>
        /// <returns>
        ///  The integer result of the integer operation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static implicit operator int(Color c) { return (int)c.Value; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Operator Overload: convert from int.
        /// </summary>
        /// <param name="c">  The integer c.</param>
        /// <returns>
        ///  The SourcePreview.Color result of the Color operation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static implicit operator Color(int c) {  return new Color((uint)c);}

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Operator Overload: Convert from uint.
        /// </summary>
        /// <param name="c">  The uint c.</param>
        /// <returns>
        ///  The SourcePreview.Color result of the Color operation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static implicit operator Color(uint c) {  return new Color(c);}

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Operator Overload: Convert from uint.
        /// </summary>
        /// <param name="c">  The uint c.</param>
        /// <returns>
        ///  The SourcePreview.Color result of the Color operation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static implicit operator System.Drawing.Color(Color c)
        {
            return  System.Drawing.Color.FromArgb((int)c.Alpha,(int)c.Red,(int)c.Green,(int)c.Blue);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Operator Overload: Convert from System.Drawing.Color.
        /// </summary>
        /// <param name="c">  The uint c.</param>
        /// <returns>
        ///  The SourcePreview.Color result of the Color operation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static implicit operator Color(System.Drawing.Color c)
        {
            return new Color(c.A, c.R, c.G, c.B);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Equate colors.
        /// </summary>
        /// <param name="obj">  The object.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool Equals(object obj)
        {
            return ((Color)obj) == Value;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Hash Code.
        /// </summary>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override int GetHashCode()
        {
            return  (int) Value;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get a string representation of Color.
        /// </summary>
        /// <returns>
        ///  The string representation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override string ToString()
        {
            return $"({Red},{Green},{Blue},{Alpha})";
        }
        // constant colors 

        public const uint ALICE_BLUE              = 0xFFF8F0; 
        public const uint ANTIQUE_WHITE           = 0xD7EBFA; 
        public const uint AQUA                    = 0xFFFF00;
        public const uint AQUA_MARINE             = 0xD4FF7F; 
        public const uint AZURE                   = 0xFFFFF0;
        public const uint BEIGE                   = 0xDCF5F5;
        public const uint BISQUE                  = 0xC4E4FF;
        public const uint BLACK                   = 0x000000;
        public const uint BLANCHED_ALMOND         = 0xCDEBFF; 
        public const uint BLUE                    = 0xFF0000;
        public const uint BLUE_VIOLET             = 0xE22B8A;
        public const uint BROWN                   = 0x2A2AA5;
        public const uint BURLY_WOOD              = 0x87B8DE;
        public const uint CADET_BLUE              = 0xA09E5F;
        public const uint CHART_REUSE             = 0x00FF7F;
        public const uint CHOCOLATE               = 0x1E69D2;
        public const uint CORAL                   = 0x507FFF;
        public const uint CORN_FLOWER_BLUE        = 0xED9564;
        public const uint CORN_SILK               = 0xDCF8FF; 
        public const uint CRIMSON                 = 0x3C14DC;
        public const uint CYAN                    = 0xFFFF00;
        public const uint CYAN_AQUA               = 0xFFFF00;
        public const uint DARK_BLUE               = 0x8B0000;
        public const uint DARK_CYAN               = 0x8B8B00;
        public const uint DARK_GOLDEN_ROD         = 0x0B86B8;
        public const uint DARK_GRAY_DARK_GREY     = 0xA9A9A9;
        public const uint DARK_GREEN              = 0x006400;
        public const uint DARK_KHAKI              = 0x6BB7BD;
        public const uint DARK_MAGENTA            = 0x8B008B;
        public const uint DARK_OLIVE_GREEN        = 0x2F6B55;
        public const uint DARK_ORANGE             = 0x008CFF;
        public const uint DARK_ORCHID             = 0xCC3299;
        public const uint DARK_RED                = 0x00008B;
        public const uint DARK_SALMON             = 0x7A96E9;
        public const uint DARK_SEA_GREEN          = 0x8FBC8F;
        public const uint DARK_SLATE_BLUE         = 0x8B3D48;
        public const uint DARK_SLATE_GRAY         = 0x4F4F2F;
        public const uint DARK_TURQUOISE          = 0xD1CE00;
        public const uint DARK_VIOLET             = 0xD30094;
        public const uint DEEP_PINK               = 0x9314FF; 
        public const uint DEEP_SKY_BLUE           = 0xFFBF00;
        public const uint DIM_GRAY_DIM_GREY       = 0x696969;
        public const uint DODGER_BLUE             = 0xFF901E; 
        public const uint FIREBRICK               = 0x2222B2;
        public const uint FLORAL_WHITE            = 0xF0FAFF; 
        public const uint FOREST_GREEN            = 0x228B22; 
        public const uint GAINSBORO               = 0xDCDCDC;
        public const uint GHOST_WHITE             = 0xFFF8F8; 
        public const uint GOLD                    = 0x00D7FF;
        public const uint GOLDEN_ROD              = 0x20A5DA; 
        public const uint GRAY                    = 0x808080;
        public const uint GRAY_GREY               = 0x808080;
        public const uint GREEN                   = 0x008000;
        public const uint GREEN_YELLOW            = 0x2FFFAD; 
        public const uint HONEYDEW                = 0xF0FFF0;
        public const uint HOT_PINK                = 0xB469FF; 
        public const uint INDIAN_RED              = 0x5C5CCD; 
        public const uint INDIGO                  = 0x82004B;
        public const uint IVORY                   = 0xF0FFFF;
        public const uint KHAKI                   = 0x8CE6F0;
        public const uint LAVENDER                = 0xFAE6E6;
        public const uint LAVENDER_BLUSH          = 0xF5F0FF; 
        public const uint LAWN_GREEN              = 0x00FC7C; 
        public const uint LEMON_CHIFFON           = 0xCDFAFF; 
        public const uint LIGHT_BLUE              = 0xE6D8AD;
        public const uint LIGHT_CORAL             = 0x8080F0;
        public const uint LIGHT_CYAN              = 0xFFFFE0;
        public const uint LIGHT_GOLDEN_ROD_YELLOW = 0xD2FAFA;
        public const uint LIGHT_GRAY_LIGHT_GREY   = 0xD3D3D3;
        public const uint LIGHT_GREEN             = 0x90EE90;
        public const uint LIGHT_PINK              = 0xC1B6FF;
        public const uint LIGHT_SALMON            = 0x7AA0FF;
        public const uint LIGHT_SEA_GREEN         = 0xAAB220;
        public const uint LIGHT_SKY_BLUE          = 0xFACE87;
        public const uint LIGHT_SLATE_GRAY        = 0x998877;
        public const uint LIGHT_STEEL_BLUE        = 0xDEC4B0;
        public const uint LIGHT_YELLOW            = 0xE0FFFF;
        public const uint LIME                    = 0x00FF00;
        public const uint LIME_GREEN              = 0x32CD32; 
        public const uint LINEN                   = 0xE6F0FA;
        public const uint MAGENTA_FUCHSIA         = 0xFF00FF;
        public const uint MAROON                  = 0x000080;
        public const uint MEDIUM_AQUA_MARINE      = 0xAACD66;
        public const uint MEDIUM_BLUE             = 0xCD0000; 
        public const uint MEDIUM_ORCHID           = 0xD355BA; 
        public const uint MEDIUM_PURPLE           = 0xDB7093; 
        public const uint MEDIUM_SEA_GREEN        = 0x71B33C;
        public const uint MEDIUM_SLATE_BLUE       = 0xEE687B;
        public const uint MEDIUM_SPRING_GREEN     = 0x9AFA00;
        public const uint MEDIUM_TURQUOISE        = 0xCCD148; 
        public const uint MEDIUM_VIOLET_RED       = 0x8515C7;
        public const uint MIDNIGHT_BLUE           = 0x701919; 
        public const uint MINT_CREAM              = 0xFAFFF5; 
        public const uint MISTY_ROSE              = 0xE1E4FF; 
        public const uint MOCCASIN                = 0xB5E4FF;
        public const uint NAVAJO_WHITE            = 0xADDEFF; 
        public const uint NAVY                    = 0x800000;
        public const uint OLD_LACE                = 0xE6F5FD; 
        public const uint OLIVE                   = 0x008080;
        public const uint OLIVE_DRAB              = 0x238E6B; 
        public const uint ORANGE                  = 0x00A5FF;
        public const uint ORANGE_RED              = 0x0045FF; 
        public const uint ORCHID                  = 0xD670DA;
        public const uint PALE_GOLDEN_ROD         = 0xAAE8EE;
        public const uint PALE_GREEN              = 0x98FB98; 
        public const uint PALE_TURQUOISE          = 0xEEEEAF; 
        public const uint PALE_VIOLET_RED         = 0x9370DB;
        public const uint PAPAYA_WHIP             = 0xD5EFFF; 
        public const uint PEACH_PUFF              = 0xB9DAFF; 
        public const uint PERU                    = 0x3F85CD;
        public const uint PINK                    = 0xCBC0FF;
        public const uint PLUM                    = 0xDDA0DD;
        public const uint POWDER_BLUE             = 0xE6E0B0; 
        public const uint PURPLE                  = 0x800080;
        public const uint RED                     = 0x0000FF;
        public const uint ROSY_BROWN              = 0x8F8FBC; 
        public const uint ROYAL_BLUE              = 0xE16941; 
        public const uint SADDLE_BROWN            = 0x13458B; 
        public const uint SALMON                  = 0x7280FA;
        public const uint SANDY_BROWN             = 0x60A4F4; 
        public const uint SEA_GREEN               = 0x578B2E; 
        public const uint SEA_SHELL               = 0xEEF5FF; 
        public const uint SIENNA                  = 0x2D52A0;
        public const uint SILVER                  = 0xC0C0C0;
        public const uint SKY_BLUE                = 0xEBCE87; 
        public const uint SLATE_BLUE              = 0xCD5A6A; 
        public const uint SLATE_GRAY              = 0x908070; 
        public const uint SNOW                    = 0xFAFAFF;
        public const uint SPRING_GREEN            = 0x7FFF00; 
        public const uint STEEL_BLUE              = 0xB48246; 
        public const uint TAN                     = 0x8CB4D2;
        public const uint TEAL                    = 0x808000;
        public const uint THISTLE                 = 0xD8BFD8;
        public const uint TOMATO                  = 0x4763FF;
        public const uint TURQUOISE               = 0xD0E040;
        public const uint VIOLET                  = 0xEE82EE;
        public const uint WHEAT                   = 0xB3DEF5;
        public const uint WHITE                   = 0xFFFFFF;
        public const uint WHITE_SMOKE             = 0xF5F5F5; 
        public const uint YELLOW                  = 0x00FFFF;
        public const uint YELLOW_GREEN            = 0x32CD9A; 
    }
}