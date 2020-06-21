////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: File Preview
// 
//  Author:      Robert C Steiner
//
// Reference: https://docs.microsoft.com/en-us/archive/msdn-magazine/2007/january/windows-vista-and-office-writing-your-own-preview-handlers
//
// 
//=====================================================[ History ]=====================================================
//  Date        Who      What
//  ----------- ------   ----------------------------------------------------------------------------------------------
//  2/18/2020   RCS      Initial Code.
//====================================================[ Copyright ]====================================================
// 
//  BSD 3-Clause License
//  Copyright (c) 2020, Robert C. Steiner
//  All rights reserved.
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are met:
//  1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following
//  disclaimer.
//  2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the
//  following disclaimer in the documentation and/or other materials provided with the distribution.
//  3. Neither the name of the copyright holder nor the names of its
//  contributors may be used to endorse or promote products derived from
//  this software without specific prior written permission.
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
//  INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//  SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//  SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
//  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
//  USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.Reserved.
// 
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Globalization;
using PanelSourceView;


namespace MsdnMag
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Source Preview Handler Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [PreviewHandler("RCS Source Preview Handler", ".config;.user;.diagram;.g;.grammar;.err;.lst;.production;.lstraw;.ll;.pc;.z;.md;.markdown;.txt;.as;.mx;.ada;.ads;.adb;.asm;.mib;.asp;.au3;.avs;.avsi;.bc;.cln;.bash;.sh;.bsh;.csh;.bash_profile;.bashrc;.profile;.bat;.cmd;.nt;.bb;.c;.lex;.ml;.mli;.sml;.thy;.cmake;.cbl;.cbd;.cdb;.cdc;.cob;.cpy;.copy;.orc;.sco;.csd;.coffee;.litcoffee;.h;.hh;.hpp;.hxx;.cpp;.cxx;.cc;.ino;.cs;.css;.d;.diff;.patch;.erl;.hrl;.src;.em;.forth;.f;.for;.f90;.f95;.f2k;.f23;.f77;.bas;.bi;.hs;.lhs;.las;.htm;.shtml;.shtm;.xhtml;.xht;.hta;.ini;.inf;.url;.wer;.iss;.hex;.java;.js;.jsm;.jsx;.ts;.tsx;.json;.jsp;.kix;.lsp;.lisp;.tex;.sty;.lua;.mak;.mk;.m;.mms;.nim;.tab;.spf;.nfo;.nsi;.nsh;.osx;.mm;.pas;.pp;.p;.inc;.lpr;.pl;.pm;.plx;.php;.php3;.php4;.php5;.phps;.phpt;.phtml;.ps;.ps1;.psm1;.properties;.pb;.py;.pyw;.r;.s;.splus;.r2;.r3;.reb;.reg;.rc;.rb;.rbw;.rs;.scm;.smd;.ss;.st;.scp;.out;.sql;.mot;.srec;.swift;.tcl;.tek;.vb;.vbs;.t2t;.v;.sv;.vh;.svh;.vhd;.vhdl;.pro;.cl;.i;.pack;.ph;.xml;.xaml;.xsl;.xslt;.xsd;.xul;.kml;.svg;.mxml;.xsml;.wsdl;.xlf;.xliff;.xbl;.sxbl;.sitemap;.gml;.gpx;.plist;.vcproj;.vcxproj;.csproj;.csxproj;.vbproj;.dbproj;.yml;.yaml;.sln", "{0A69F069-3FB1-4E66-B726-0131769541F8}")]
    [ProgId("RCS.SourcePreviewHandler")]
    [Guid("8D97DA9C-76DC-4A37-81E0-27E33822EB22")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public sealed class SourcePreviewHandler : FileBasedPreviewHandler
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Create Preview Handler Control.
        /// </summary>
        /// <returns>
        ///  The MsdnMag.PreviewHandlerControl value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override PreviewHandlerControl CreatePreviewHandlerControl()
        {
            return new SourcePreviewHandlerControl();
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  The Source Preview Handler Control Class definition.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private sealed class SourcePreviewHandlerControl : FileBasedPreviewHandlerControl
        {

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>
            ///  Method: load the viewer and file info.
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
                    viewer.Dock            = DockStyle.Fill;
                    viewer.LoadFile(file.FullName);
                    Controls.Add(viewer);
                }
                catch (Exception )
                {
                }
            }
        }
    }
}