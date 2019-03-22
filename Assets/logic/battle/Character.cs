using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    /// <summary>A Character's health.</summary>
    public int hp;
    /// <summary>A character's mana</summary>
    public int mp;
    public BitArray status;
}

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : Character {

}
[CreateAssetMenu(fileName = "New Party Member", menuName = "Party Member")]
public class PartyMember : Character {
    public int exp;
    Dictionary<string, InvItem> inventory;
}

public static class PartyData {
    public static List<PartyMember> partyMembers;
}