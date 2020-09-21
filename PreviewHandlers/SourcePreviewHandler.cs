//   Copyright 2020 Robert C. Steiner
// 
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software
//   and associated documentation files (the "Software"), to deal in the Software without restriction,
//   including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
//   and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
//   subject to the following conditions:
// 
//   The above copyright notice and this permission notice shall be included in
//   all copies or substantial portions of the Software.
// 
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//   INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
//   AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES
//   OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//   CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PanelSourceView;

namespace SourcePreview
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///     The Constants Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static class Constants
    {
        /// <summary>
        ///     The Extensions field.
        /// </summary>
        public const string Extensions =
            ".ada;.adb;.ads;.as;.asm;.asp;.au3;.avs;.avsi;.bas;.bash;.bash_profile;.bashrc;.bat;.bb;.bi;.bsh;.c;.cbd;.cbl;.cc;.cdb;.cdc;.cg;.cl;.cmake;.cmd;.cob;.coffee;.copy;.cpp;.cpy;.cs;.csd;.csh;.csproj;.css;.csxproj;.cxx;.d;.dbproj;.diagram;.diff;.em;.erl;.err;.f;.f23;.f2k;.f77;.f90;.f95;.for;.forth;.frag;.g;.glsl;.gml;.gpx;.grammar;.h;.hex;.hh;.hlsl;.hpp;.hrl;.hs;.hta;.htm;.html;.hxx;.i;.inc;.inf;.ini;.ino;.iss;.java;.js;.jsm;.json;.jsp;.jsx;.kix;.kml;.lang;.las;.lex;.lhs;.lisp;.litcoffee;.ll;.log;.lpr;.lsp;.lst;.lstraw;.lua;.m;.mak;.markdown;.md;.mib;.mk;.ml;.mli;.mm;.mms;.mot;.mx;.mxml;.nim;.nsh;.nsi;.nt;.orc;.osx;.out;.p;.pack;.pas;.patch;.pb;.pc;.ph;.php;.php3;.php4;.php5;.phps;.phpt;.phtml;.pl;.plist;.plx;.pm;.pp;.pro;.production;.profile;.properties;.ps;.ps1;.psm1;.py;.pyw;.r;.r2;.r3;.rb;.rbw;.rc;.reb;.reg;.rs;.s;.scm;.sco;.scp;.sh;.shtm;.shtml;.sitemap;.sln;.smd;.sml;.spf;.splus;.sql;.src;.srec;.ss;.st;.sty;.sv;.svg;.svh;.swift;.sxbl;.t2t;.tab;.tcl;.tek;.tex;.text;.thy;.ts;.tsx;.txt;.url;.v;.vb;.vbproj;.vbs;.vcproj;.vcxproj;.vert;.vh;.vhd;.vhdl;.wer;.wsdl;.xaml;.xbl;.xht;.xhtml;.xlf;.xliff;.xml;.xsd;.xsl;.xslt;.xsml;.xul;.yaml;.yml;.z";
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///     The Source Preview Handler Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [PreviewHandler("RCS Source File Preview Handler", Constants.Extensions, "{0A69F069-3FB1-4E66-B726-0131769541F9}")]
    [ProgId("RCS.SourcePreviewHandler")]
    [Guid("8D97DA9C-76DC-4A37-81E0-27E33822EB23")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public sealed class SourcePreviewHandler : FileBasedPreviewHandler
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Method: Create Preview Handler Control.
        /// </summary>
        /// <returns>
        ///     The MsdnMag.PreviewHandlerControl value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override PreviewHandlerControl CreatePreviewHandlerControl()
        {
            return new SourcePreviewHandlerControl();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     The Source Preview Handler Control Class definition.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private sealed class SourcePreviewHandlerControl : FileBasedPreviewHandlerControl
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>
            ///     Method: load the viewer and file info.
            /// </summary>
            /// <param name="file">  The file.</param>
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public override void Load(FileInfo file)
            {
                // FastBox(file);

                try
                {
                    var viewer = new ViewerPanel();
                    viewer.ShowLineNumbers = false;
                    // viewer.ReadOnly        = true;
                    viewer.Dock = DockStyle.Fill;
                    viewer.LoadFile(file.FullName);
                    Controls.Add(viewer);
                }
                catch (Exception)
                {
                }
            }

            /// <summary>Previews a keyboard message.</summary>
            /// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the window message to process.</param>
            /// <returns>
            /// <see langword="true" /> if the message was processed by the control; otherwise, <see langword="false" />.</returns>
            protected override bool ProcessKeyPreview(ref Message m)
            {
                Trace.WriteLine($"SourcePreviewHandlerControl:ProcessKeyPreview {m.WParam}");
                return base.ProcessKeyPreview(ref m);
            }

            /// <summary>Previews a keyboard message.</summary>
            /// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the window message to process.</param>
            /// <returns>
            /// <see langword="true" /> if the message was processed by the control; otherwise, <see langword="false" />.</returns>
            /// <summary>Processes a command key.</summary>
            /// <param name="msg">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the Win32 message to process.</param>
            /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process.</param>
            /// <returns>
            /// <see langword="true" /> if the keystroke was processed and consumed by the control; otherwise, <see langword="false" /> to allow further processing.</returns>
            protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
            {
                Trace.WriteLine($"SourcePreviewHandlerControl:ProcessCmdKey {keyData}");
                return base.ProcessCmdKey(ref msg, keyData);
            }

        }
    }
}