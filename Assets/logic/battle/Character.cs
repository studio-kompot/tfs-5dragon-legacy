using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : ScriptableObject {

    public string cname;
    /// <summary>How strong a character is, used to </summary>
    public int str;
    /// <summary>How speedy a character is</summary>
    public int dex;
    /// <summary>Used to calculate max HP</summary>
    public int con;
    /// <summary>Used to calculate max MP</summary>
    public int inte;
    /// <summary></summary>
    public int wis;
    /// <summary></summary>
    public int cha;
    /// <summary></summary>
    public int ac;
    /// <summary>A Character's health.</summary>
    public int hp;
    /// <summary>A character's mana</summary>
    public int mp;
    public BitArray status;
    public List<Attack> SpAttacks;
    public List<InvItem> Inventory;
    public Sprite image;
    public Attack standardAttack;
}


