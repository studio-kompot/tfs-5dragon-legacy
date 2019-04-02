using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleInit {
    //TODO: MOAR CODE!!!!!!

    public static List<Enemy> Enemies;
    public enum EMusic {
        standard,
        miniboss,
        boss
    }
    public enum EBackground {
        field,
        volcano,
        cave
    }
    public static EMusic AMusic = EMusic.standard;
    public static EBackground ABackground = EBackground.field;
    static void Clear() {
        Enemies = new List<Enemy>();
        AMusic = 0;
        ABackground = 0;
    }
}
