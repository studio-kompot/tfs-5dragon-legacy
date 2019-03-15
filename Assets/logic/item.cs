using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#region Header
public interface IItem {
    string id;
    int count;
    string description;
    void doAction();
}

abstract public class StatPotion : IItem {
    string id = "potion-stat-";
    int count = 0;
    string description = "";
    int toAdd;
    enum Type {
        health,
        magic,
        maxhealth,
        maxmagic
    }
    void doAction() {

    }
}
#endregion

public class SmallHealth : StatPotion {
    void SmallHealth() {

    }
}