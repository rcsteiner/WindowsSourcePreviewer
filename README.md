# Windows Source Previewer
A windows file explorer extension to show syntax highlighted files in explorer's preview pane.  Supports over 200 file extensions.

### Extensions supported:
**.ada .adb .ads .as .asm .asp .au3 .avs .avsi .bas .bash .bash_profile .bashrc .bat .bb .bi .bsh .c .cbd .cbl .cc .cdb .cdc .cl .cmake .cmd .cob .coffee .config .copy .cpp .cpy .cs .csd .csh .css .csxproj .cxx .d .dbproj .diagram .diff .em .erl .err .f .f23 .f2k .f77 .f90 .f95 .for .forth .g .gml .gpx .grammar .h .hex .hh .hpp .hrl .hs .hta .htm .html .hxx .i .inc .inf .ini .ino .iss .java .js .jsm .json .jsp .jsx .kix .kml .las .lex .lhs .lisp .litcoffee .ll .lpr .lsp .lst .lstraw .lua .m .mak .markdown .md .mib .mk .ml .mli .mm .mms .mot .mx .mxml .nim .nsh .nsi .nt .orc .osx .out .p .pack .pas .patch .pb .pc .ph .php .php3 .php4 .php5 .phps .phpt .phtml .pl .plist .plx .pm .pp .pro .production .profile .properties .ps .ps1 .psm1 .py .pyw .r .r2 .r3 .rb .rbw .rc .reb .reg .rs .s .scm .sco .scp .sh .shtm .shtml .sitemap .smd .sml .spf .splus .sql .src .srec .ss .st .sty .sv .svg .svh .swift .sxbl .t2t .tab .tcl .tek .tex .text .thy .ts .tsx .txt .url .user .v .vb .vbproj .vbs .vcproj .vcxproj .vh .vhd .vhdl .wer .wsdl .xaml .xbl .xht .xhtml .xlf .xliff .xml .xsd .xsl .xslt .xsml .xul .yaml .yml .z**
# Installing
The binary is supplied in the *bin* folder.

 To install:

1) Run the visual studio developer command line tool with *adminstrator permissions*.
2) In the **bin** folder, run __install.bat__.
3) To uninstall, run __uninstall.bat__.

# Snapshots
The include test program can show a file in the viewer panel.
![](Snapshots/TestProgram.png)

In File Explorer.
![](Snapshots/FileExplorerPreview.png)

# Building
The project is built in Visual Studio 2019.  Open the solution and compile.  In the bin folder (debug or release), are batch files  to install (install.bat) and uninstall (uninstall.bat) the previewer into windows.  The TestViewer project is an executable that can be used to test the viewer and run it.

#### Important note: 
*If you wish to build it and debug it, then you **MUST** uninstall it first because the debugger will load the version in the GAC and not run the code you modified and want to test.*

# Design Notes
The basic previewer handler code comes from a 2007 MSDN article by Stephen Toub. [Writing your own preview handlers](https://docs.microsoft.com/en-us/archive/msdn-magazine/2007/january/windows-vista-and-office-writing-your-own-preview-handlers).  There is a lot of file types listed and I haven't tested most.  All are in the style\*.lang files.  So any fixes or updates are welcome.

The design of the source viewer is based on a simple byte buffer with character classifiers (lexers).  After looking at the BOM of the file, it choses an ASCII, UTF8, Unicode Big Endian, Unicode Little Endian, character reader.  Then uses the extension to pick the character classifier. No strings are extracted from the raw byte array, the sub-regions are passed to Win32 text drawing and measuring functions directly.  The goal was to eliminate a lot of string generation and be very fast.  Originally I tried other editors like Fireball and Avalon, but they are slow and the file explorer responded very slow.  

I also include the original file previewers from the Toub article so you can play with them too.  It defaults to a Dark background, but the **default.palette** file can be change to have other schemes. It does support multiple palettes (styles) but I implemented only one so far.
