using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleInit {
    //TODO: MOAR CODE!!!!!!

    public static List<Enemy> Enemies;
    public enum BBackground {
      field,
      cave,
      volcano
    }
    public static BBackground ABackground;
    public enum MMusic {
      standard,
      miniboss,
      boss,
      special
    }
    public static MMusic AMusic;
}
