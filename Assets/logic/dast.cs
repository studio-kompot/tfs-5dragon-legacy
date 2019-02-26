//https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=netframework-4.7.2
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace dast
{
    class DastOutput
    {
        bool quit = false;
        Nullable<string> name;
        Nullable<string> faceplate;
        string text;
        Nullable<List<string>> askchoices;
        public DastOutput()
        {
            name = null;
            faceplate = null;
            text = "";
        }
        public class DastInstance
        {
            var tag = new Regex(@"^\[(\w)\]", RegexOptions.Compiled | RegexOptions.Multiline);
            string[] datar;
            var outp = new DastOutput();
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
                Match m = tag.Match(datar[LineNumber]);
                if (outp.quit) LineNumber++;
                else if (m.Success)
                {
                    //TODO: great case statement
                }
            }
        }
    }
}
