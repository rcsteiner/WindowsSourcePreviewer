////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Manage multiple languages.
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
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Scan
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Language Manager Class definition.  Manage multiple languages.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class LanguageManager
    {
        /// <summary>
        ///  Get a language by extension.
        /// </summary>
        public ILanguage this[string ext] { get { return Find(ext); } }

        /// <summary>
        ///  The Extenstion Map field.
        /// </summary>
        public Dictionary<string, string> ExtenstionMap;

        /// <summary>
        ///  The Languages field.
        /// </summary>
        public Dictionary<string, ILanguage> Languages;

        /// <summary>
        ///  Get the language file extension.
        /// </summary>
        private const string EXTENSION_MAP = "Extension.Map";

        /// <summary>
        ///  Get the language file extension.
        /// </summary>
        private const string LANG_EXTENSION = ".lang";

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Default Constructor: LanguageManager, loads all resources from this module.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public LanguageManager()
        {
            Languages = new Dictionary<string, ILanguage>();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Find.
        /// </summary>
        /// <param name="ext"> The extent.</param>
        /// <returns>
        ///  The Scan.ILanguage value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ILanguage Find(string ext)
        {

            // load extension map if not loaded
            if (ExtenstionMap == null)
            {
                var buffer = new StreamBuffer();
                var r = new BinaryFormatter();
                var s = buffer.GetResourceStream(EXTENSION_MAP);
                if (s != null)
                {
                    ExtenstionMap = (Dictionary<string, string>)r.Deserialize(s);
                    s.Close();
                }

              //  SaveExtensionList();
            }


            if (ExtenstionMap.TryGetValue(ext, out var langName))
            {
                // see if language is loaded
                if (!Languages.TryGetValue(langName, out ILanguage lang))
                {
                    var buffer = new StreamBuffer();
                    var s = buffer.GetResourceStream(langName + ".lang");
                    if (s != null)
                    {
                        buffer.Initialize(s, langName);
                        lang = new Language(buffer);
                        Languages.Add(langName, lang);

                        s.Close();
                    }
                }
                return lang;
            }
            return Find(".txt");
        }




        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Load From Folder.
        /// </summary>
        /// <param name="folder"> The folder.</param>
        /// <param name="search"> The search.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //public void LoadAllFromFolder(string folder, string search = LANG_EXTENSION)
        //{
        //    if (Directory.Exists(folder))
        //    {
        //        foreach (var file in Directory.EnumerateFiles(folder, search))
        //        {
        //            var lang = new Language(file);
        //            Languages.Add(lang.Name, lang);
        //        }
        //    }
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Rebuild Dictionary and save it in the build space.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void RebuildDictionary()
        {
            LoadFromResources();
            WriteDictionary();
            SaveExtensionList();


        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Save Extension List.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SaveExtensionList()
        {
            var s = string.Join(";", ExtenstionMap.Keys);
            File.WriteAllText("h:\\Extenstions.text",s);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Write Dictionary.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private  void WriteDictionary()
        {
            // write dictionary
            var mapPath = @"..\..\..\Previewer\Style\Extension.Map";
            var fullPath = Path.GetFullPath(mapPath);
            Stream fout = File.OpenWrite(fullPath);
            var w = new BinaryFormatter();
            w.Serialize(fout, ExtenstionMap);
            fout.Close();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Load From Resources.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void LoadFromResources()
        {
            //    var assembly = typeof(LanguageManager).Assembly;
            //    var names    = assembly.GetManifestResourceNames();
            var buffer = new StreamBuffer();
            ExtenstionMap = new Dictionary<string, string>(400);

            foreach (var langName in buffer.GetResourceStreams(LANG_EXTENSION))
            {
                var lang = new Language(buffer);
                Languages.Add(langName, lang);
                foreach (var e in lang.Extensions)
                {
                    // b.Append($@" {e}");
                    if (!ExtenstionMap.ContainsKey(e))
                    {
                        ExtenstionMap.Add(e, lang.Name);
                    }
                }

            }

            if (Languages.Count == 0)
            {
                Languages.Add("Default", new Language("Default", "-+=><|^%*&/!", ":;?.,][{})($#@", "."));
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>
        /////  Method: Add a language.  If the language already is loaded, then replace it.
        ///// </summary>
        ///// <param name="language">  The language.</param>
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //public void Add(Language language)
        //{
        //    for (var index = 0; index < Languages.Count; index++)
        //    {
        //        var lang = Languages[index];
        //        if (lang.Name == language.Name)
        //        {
        //            Languages[index] = language;
        //            return;
        //        }
        //    }

        //    Languages.Add(language);
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>
        /////  Method: Save All.
        ///// </summary>
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //public void SaveAll()
        //{
        //    foreach (var language in Languages.Values)
        //    {
        //        language.Save(language.Name + LANG_EXTENSION);
        //    }
        //}

    }
}


