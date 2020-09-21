using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ConvertKeywords
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Keyword structure definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public struct Keyword : IComparable<Keyword>
    {
        /// <summary>
        ///  The Group identifier field.
        /// </summary>
        public int GroupId;

        /// <summary>
        ///  The Text field.
        /// </summary>
        public string Text;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="text">    The text.</param>
        /// <param name="groupId"> The group identifier.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Keyword(string text, int groupId)
        {
            Text = text;
            GroupId = groupId;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the
        ///  other.
        /// </summary>
        /// <param name="x">
        ///                  and <paramref name="y" />, as
        ///                  shown in the following table.
        ///                  Value
        ///                  Meaning
        ///                  Less than zero
        ///                  <paramref name="x" /> is less than <paramref name="y" />.
        ///                  Zero
        ///                  <paramref name="x" /> equals <paramref name="y" />.
        ///                  Greater than zero
        ///                  <paramref name="x" /> is greater than <paramref name="y" />.
        ///</param>
        /// <param name="y"> The second object to compare.</param>
        /// <returns>
        ///  A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as
        ///  shown in the following table.
        ///  Value
        ///  Meaning
        ///  Less than zero
        ///  <paramref name="x" /> is less than <paramref name="y" />.
        ///  Zero
        ///  <paramref name="x" /> equals <paramref name="y" />.
        ///  Greater than zero
        ///  <paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int Compare(Keyword x, Keyword y)
        {
            return x.Text.CompareTo(y.Text);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Compares the current instance with another object of the same type and returns an integer that indicates
        ///  whether the current instance precedes, follows, or occurs in the same position in the sort order as the
        ///  other object.
        /// </summary>
        /// <param name="other">
        ///                      in the sort order.
        ///                      Zero
        ///                      This instance occurs in the same position in the sort order as <paramref name="other"
        ///                      />.
        ///                      Greater than zero
        ///                      This instance follows <paramref name="other" /> in the sort order.
        ///</param>
        /// <returns>
        ///  A value that indicates the relative order of the objects being compared. The return value has these
        ///  meanings:
        ///  Value
        ///  Meaning
        ///  Less than zero
        ///  This instance precedes <paramref name="other" /> in the sort order.
        ///  Zero
        ///  This instance occurs in the same position in the sort order as <paramref name="other" />.
        ///  Greater than zero
        ///  This instance follows <paramref name="other" /> in the sort order.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int CompareTo(Keyword other)
        {
            var textComparison = string.Compare(Text, other.Text, StringComparison.Ordinal);
            if (textComparison != 0)
            {
                return textComparison;
            }

            return GroupId.CompareTo(other.GroupId);
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Keywords Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Keywords
    {
        /// <summary>
        ///  The Group field.
        /// </summary>
        public string Group;

        /// <summary>
        ///  The Words field.
        /// </summary>
        public List<string> Words;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="group"> The group.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Keywords(string group)
        {
            Group = group;
            Words = new List<string>();
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Langs Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Langs
    {
        /// <summary>
        ///  The Languages field.
        /// </summary>
        public List<Language> Languages;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Langs()
        {
            Languages = new List<Language>();
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Language Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Language
    {
        /// <summary>
        ///  Get/Set Keyword Chars.
        /// </summary>
        public string KeyContinue { get; set; }

        /// <summary>
        ///  Get/Set Key Start.
        /// </summary>
        public string KeyStart { get; set; }

        /// <summary>
        ///  The Block Comment field.
        /// </summary>
        public string[] BlockComment;

        /// <summary>
        ///  The Extensions field.
        /// </summary>
        public List<string> Extensions;

        /// <summary>
        ///  The Keywords field.
        /// </summary>
        public List<Keywords> Keywords;

        /// <summary>
        ///  The Line Comment field.
        /// </summary>
        public string LineComment;

        /// <summary>
        ///  The Name field.
        /// </summary>
        public string Name;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="name"> The name.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Language(string name)
        {
            Name = name;
            Keywords = new List<Keywords>();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="name">        The name.</param>
        /// <param name="extensions">  The extensions.</param>
        /// <param name="lineComment"> The line Comment.</param>
        /// <param name="start">       The start.</param>
        /// <param name="end">         The end.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Language(string name, string extensions, string lineComment, string start, string end)
        {
            Name = name;
            Extensions = new List<string>();
            foreach (var e in extensions.Split(' '))
            {
                Extensions.Add($".{e}");
            }


            LineComment = lineComment;
            Keywords = new List<Keywords>();

            if (!string.IsNullOrEmpty(start))
            {
                BlockComment = new[] { start, end };
            }
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Program Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    class Program
    {
        /// <summary>
        ///  The Key Continue field.
        /// </summary>
        public static List<char> KeyContinue = new List<char>(256);

        /// <summary>
        ///  The Key Start field.
        /// </summary>
        public static List<char> KeyStart = new List<char>(256);

        /// <summary>
        ///  The OPS field.
        /// </summary>
        public static string OPS = @"-+=><|^%*&/!~";

        /// <summary>
        ///  The Ops field.
        /// </summary>
        public static string Ops;

        /// <summary>
        ///  The PUNCT field.
        /// </summary>
        public static string PUNCT = @":;?.,][{})($#@\`";

        /// <summary>
        ///  The Punct field.
        /// </summary>
        public static string Punct;

        /// <summary>
        ///  The group Map field.
        /// </summary>
        private static List<int> _groupMap = new List<int>(1000);

        /// <summary>
        ///  The keywords field.
        /// </summary>
        private static List<Keyword> _keywords = new List<Keyword>(1000);

        /// <summary>
        ///  The builder field.
        /// </summary>
        private static StringBuilder builder = new StringBuilder(256);

        /// <summary>
        ///  The extent Map field.
        /// </summary>
        private static Dictionary<string, string> ExtMap = new Dictionary<string, string>(100);

        /// <summary>
        ///  The langs field.
        /// </summary>
        private static Langs langs;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Compute Ops.
        /// </summary>
        /// <param name="lang"> The lang.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void ComputeOps(Language lang)
        {
            Ops = OPS;
            Punct = PUNCT;
            KeyContinue.Clear();
            KeyStart.Clear();
            _keywords.Clear();

            for (var grpIndex = 0; grpIndex < lang.Keywords.Count; grpIndex++)
            {
                var grp = lang.Keywords[grpIndex];
                foreach (var w in grp.Words)
                {
                    _keywords.Add(new Keyword(w, grpIndex));

                    for (var index = 0; index < w.Length; index++)
                    {
                        var c = w[index];
                        if (index == 0)
                        {
                            if (!KeyStart.Contains(c))
                            {
                                KeyStart.Add(c);
                            }
                        }

                        else if (!KeyContinue.Contains(c))
                        {
                            KeyContinue.Add(c);
                        }

                        var i = Ops.IndexOf(c);
                        if (i >= 0)
                        {
                            Ops = Ops.Remove(i, 1);
                        }

                        i = Punct.IndexOf(c);
                        if (i >= 0)
                        {
                            Punct = Punct.Remove(i, 1);
                        }
                    }
                }
            }

            lang.KeyContinue = ConvertToString(KeyContinue);
            lang.KeyStart = ConvertToString(KeyStart);
            _keywords.Sort();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Convert To String.
        /// </summary>
        /// <param name="table"> The table.</param>
        /// <returns>
        ///  The string value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static string ConvertToString(List<char> table)
        {
            table.Sort();
            builder.Length = 0;
            foreach (var c in table)
            {
                builder.Append(c);
            }

            return builder.ToString();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Main.
        ///  Writes all the language files .lang in the styles folder
        ///  Writes binary Dictionary<string, string> to Extension.map
        /// </summary>
        /// <param name="args"> The arguments.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        static void Main(string[] args)
        {
           var file = File.OpenRead(@"..\..\..\..\keywords\baseKeywords.xml");
     //       var file = File.OpenRead(@"h:\GLSL.xml");
            XmlReader reader = new XmlTextReader(file);

            ReadKeywords(reader);
            WriteLangs();

            WriteDictionary();
            //Stream fin = File.OpenRead(@$"..\..\..\..\Previewer\Style\Extension.Map");
            //var x = new BinaryFormatter();
            //var y = x.Deserialize(fin);
            //fin.Close();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Read Keywords.
        /// </summary>
        /// <param name="reader"> The reader.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void ReadKeywords(XmlReader reader)
        {
            langs = new Langs();
            Language Language = null;
            Keywords keyWords = null;

            reader.MoveToContent();
            // Parse the file and display each of the nodes.
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        Console.Write("<{0}>", reader.Name);
                        if (reader.Name == "Language")
                        {
                            var name = reader.GetAttribute("name");
                            var ext = reader.GetAttribute("ext");
                            var commentLine = reader.GetAttribute("commentLine");
                            var commentStart = reader.GetAttribute("commentStart");
                            var commentEnd = reader.GetAttribute("commentEnd");
                            Language = new Language(name, ext, commentLine, commentStart, commentEnd);
                            langs.Languages.Add(Language);
                        }
                        else if (reader.Name == "Keywords")
                        {
                            var name = reader.GetAttribute("name");
                            keyWords = new Keywords(name);
                            Language?.Keywords.Add(keyWords);
                        }
                        break;
                    case XmlNodeType.Text:
                        keyWords?.Words.AddRange(reader.Value.Split(' '));
                        keyWords?.Words.Sort();
                        Console.Write(reader.Value);
                        break;
                    case XmlNodeType.XmlDeclaration:
                        Console.Write("<?xml version='1.0'?>");
                        break;
                    case XmlNodeType.EndElement:
                        Console.Write("</{0}>", reader.Name);
                        break;
                }
            }

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Write Dictionary.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void WriteDictionary()
        {
            // write dictionary
            Stream fout = File.OpenWrite(@$"..\..\..\..\Previewer\Style\Extension.Map");
            var w = new BinaryFormatter();
            w.Serialize(fout, ExtMap);
            fout.Close();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Write Langs.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void WriteLangs()
        {
            StringBuilder b = new StringBuilder(20000);

            foreach (var lang in langs.Languages)
            {
                ComputeOps(lang);

                b.Clear();
                b.AppendLine($@"Name = ""{lang.Name}""");
                b.Append(@"Ext =");
                foreach (var e in lang.Extensions)
                {
                    b.Append($@" {e}");
                    if (!ExtMap.ContainsKey(e))
                    {
                        ExtMap.Add(e, lang.Name);
                    }
                }

                b.AppendLine();
                if (lang.LineComment != null)
                {
                    b.AppendLine($@"LineComment =""{lang.LineComment}""");
                }

                if (lang.BlockComment != null)
                {
                    b.Append("BlockComment =");
                    b.Append($@" ""{lang.BlockComment[0]}""");
                    b.AppendLine($@" ""{lang.BlockComment[1]}""");
                }

                b.AppendLine("Delimiter = DQuote \"\\\"\" \"\\\\\" \"\\\"\"");
                b.AppendLine("Delimiter = SQuote \"'\" \"\\\\\" \"'\"");
                b.AppendLine($"Operators = \"{Ops}\"");
                b.AppendLine($@"Punctuation =""{Punct}""");
                b.AppendLine($@"KeyStart = ""{lang.KeyStart}"" ");
                b.AppendLine($@"KeyContinue = ""{lang.KeyContinue}"" ");

                b.Append("Group = ");
                foreach (var grp in lang.Keywords)
                {
                    b.Append($" {grp.Group}");
                }
                b.AppendLine();


                b.Append("Keyword = ");
                foreach (var k in _keywords)
                {
                    b.Append($" {k.Text} {k.GroupId}");
                }

                b.AppendLine();

                File.WriteAllText(@$"..\..\..\..\Previewer\Style\{lang.Name}.lang", b.ToString());
            }
        }
    }
}




