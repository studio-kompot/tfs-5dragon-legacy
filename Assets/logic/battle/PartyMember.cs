using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Party Member", menuName = "Party Member")]
public class PartyMember : Character {
    public int exp;
    Dictionary<string, InvItem> inventory;
}
