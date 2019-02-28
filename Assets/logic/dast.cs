//https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=netframework-4.7.2
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Utilities;
using System.IO;
using UnityEngine;

namespace dast
{
    class DastOutput
    {
        bool quit = false;
        string name;
        string faceplate;
        string text;
        System.Nullable<List<string>> askchoices;
        public DastOutput()
        {
            name = null;
            faceplate = null;
            text = "";
        }
        public class DastInstance
        {
            static class lex
            {
                static Regex tag = new Regex(@"^\[(\w)\]", RegexOptions.Compiled);
                static Regex iftype = new Regex(@"if([a-z]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            string[] datar;
            DastOutput outp = new DastOutput();
            int LineNumber = 0;
            public DastInstance(string path, Database obj)
            {
                datar.FormatWith(obj);
                using (var f = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    datar = f.ReadAllLines(path, Encoding.UTF8);
                }
            }
            IEnumerable<DastOutput> RunFile(string path, Database obj)
            {
                outp.quit = (LineNumber < datar.Length || quit);
                Match m = lex.tag.Match(datar[LineNumber]);
                if (outp.quit) LineNumber++;
                else if (m.Success)
                {
                    //TODO: great case statement
                }
            }
        }
    }
}
