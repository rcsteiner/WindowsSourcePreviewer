////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Editor commands.
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
namespace Editor
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Edit Commands Enumeration definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public enum EditCommands
    {
        ESC,                // end command
        HELP,               // help or F1

        PAGE_UP,            // pg up
        PAGE_DOWN,          // pg down

        DOC_HOME,           // cntrl-home
        DOC_END,            // cntrl-end

        LINE_HOME,          // home
        LINE_END,           // end
        LINE_UP,            // up
        LINE_DOWN,          // down
        LINE_DUP,           // cntrl-d
        LINE_GOTO,          // cntrl-g

        WORD_LEFT,          // cntrl-left
        WORD_RIGHT,         // cntrl-right

        CHAR_LEFT,          // left
        CHAR_RIGHT,         // right

        INSERT_TOGGLE,      // insert
        DELETE,             // delete
        SELECT_ALL         // cntrl-a


    }
}