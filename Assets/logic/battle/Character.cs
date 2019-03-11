using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BitUtils;

public class Character : ScriptableObject {

    public string cname;
    /// <summary>How strong a character is, used to </summary>
    public int str;
    /// <summary> </summary>
    public int dex;
    /// <summary>Used to calculate max HP</summary>
    public int con;
    /// <summary>Used to calculate max MP</summary>
    public int inte;
    /// <summary></summary>
    public int wis;
    /// <summary></summary>
    public int cha;
    public int hp;
    public int mp;
    public BitFlags status;
}

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : Character {

}
[CreateAssetMenu(fileName = "New Party Member", menuName = "Party Member")]
public class PartyMember : Character {
    public int exp;
    //Inventory inventory;
}
