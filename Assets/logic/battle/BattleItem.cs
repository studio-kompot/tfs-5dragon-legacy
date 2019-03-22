using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class BattleItem {
    public int? hpdelta;
    public int? mpdelta;
    public int? strdelta;
    public int? condelta;
    public int? wisdelta;
    public int? chadelta;
    public int? dexdelta;
    public int? intedelta;
    public BitArray statusmask;
    public VideoClip Animation;
    public List<string> Targets { get; set; }
}

public class Attack : BattleItem {
    public string FlavorText;
}

public class InvItem : BattleItem {
    public int count;
}