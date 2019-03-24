using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichTextParser {
#region Variables
    private string TagContent { get; set; }
    public static int CurrentIndex { get; set; }
#endregion
    #region Methods
    public RichTextParser(string txt) {
        TagContent = txt;
    }

    public static RichTextParser ParseText(string txt) {
        var begintag = txt.IndexOf('<');
        var endtag = txt.IndexOf('>');
        var tag = txt.Substring(begintag, endtag - begintag + 1);
        return new RichTextParser(tag);
    }

    public static void GetValue(string txt) {
        var dManager = DialogManager.instance;
        for (int i = CurrentIndex; i < txt.Length; i++) {
            var remain = txt.Substring(i, txt.Length - i);
            if (remain.StartsWith('<'.ToString())) {
                var parsed = ParseText(dManager.ToPrint);
                if (parsed.TagContent.Contains('/'.ToString())) {
                    dManager.EndTag = parsed.TagContent;
                    dManager.ToPrint = dManager.ToPrint.Replace(parsed.TagContent, string.Empty);
                    i += parsed.TagContent.Length - 1;
                    return;
                } else {
                    dManager.StartTag = parsed.TagContent;
                    dManager.ToPrint = dManager.ToPrint.Replace(parsed.TagContent, string.Empty);
                    var len = remain.IndexOf('/') - remain.IndexOf('>') - 2;
                    var RegionBuffer = len;
                    i += parsed.TagContent.Length - 1;
                }
            }
        }
    }
    #endregion
}
