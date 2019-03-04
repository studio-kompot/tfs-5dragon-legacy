//https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=netframework-4.7.2
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Utilities;
using System.IO;
using UnityEngine;

namespace dast
{
    public class Dialogue
    {
        bool quit = false;
        string name;
        string faceplate;
        string text;
        List<string> askchoices;
        public Dialogue()
        {
            name = null;
            faceplate = null;
            text = "";
        }
    }
        public class DastInstance
        {
            static class Lex
            {
                public static Regex tag = new Regex(@"^\[(\w)\]", RegexOptions.Compiled);
                public static Regex iftype = new Regex(@"if([a-z]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            string[] datar;
            Dialogue outp = new Dialogue();
            int LineNumber = 0;
            bool quit = false;
            public DastInstance(string path, Database obj, DataStruct format)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        var tmp = sr.ReadToEnd();
                        tmp.FormatWith(format, obj);
                        datar = tmp.Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.None);
                    }
                }
                catch (FileNotFoundException e)
                {
                    Debug.LogErrorFormat("{0}: Unable to load <i>{1}</i>", e.GetType().Name, path);
                }
            }
            /**
             * <summary></summary>
             * 
             */
            IEnumerable<Dialogue> RunFile(Database obj)
            {
                var comment = false;
                string[] command;
                while (!comment)
                {
                    outp.quit = (LineNumber < datar.Length || quit);
                    Match m = Lex.tag.Match(datar[LineNumber]);
                    if (outp.quit)
                    {
                        LineNumber++;
                    }
                    else if (m.Success)
                    {
                        if (Regex.IsMatch(m.Value, @","))
                        {
                            command = m.Value.Split(',');
                        }
                        else
                        {
                            command = m.Value.Split(' ');
                        }

                        switch (command[0])
                        {
                            case "rem":
                            case "#":
                            case "comment": comment = true; break;
                            case "chr":
                                //TODO: Add logic

                                break;
                        }
                        yield return outp;
                    }
                }
            }
        }
    }
