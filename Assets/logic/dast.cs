//https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=netframework-4.7.2
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace dast {
    class DastOutput {
        bool quit = false;
        string? name;
        string? faceplate;
        string text;
        Nullable<List<string>> askchoices;
    }
    public DastOutput() {
      name = null;
      faceplate = null;
      text = "";
    }
    public class DastInstance {
            var tag = new Regex("^\[(\w)\]", RegexOptions.Compiled | RegexOptions.Multiline);
            string[] datar;
      int LineNumber = 0;
      public DastInstance(string path, Database obj) {
        datar.FormatWith(obj);
        var outp = new DastOutput();
        using (var f = new FileStream(path, FileMode.Open, FileAccess.Read)){
          datar = f.ReadAllLines(path,Encoding.UTF8);
        }
      }
      IEnumerable<DastOutput> RunFile(string path, Database obj) {
        if (LineNumber < datar || quit) yield "\0\0\0";
      }
    }
