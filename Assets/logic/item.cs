using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#region Header

public class Item {
    int count;
    string description;
}

abstract public class StatPotion : Item {
    int count = 0;
    string description = "";
    int type;

    string id {
        get { return id; }
        set { id = "potion-stat-" + value; }
    }

    void doAction() {
        throw new System.NotImplementedException();
    }

    enum Type {
        health,
        magic,
        maxhealth,
        maxmagic
    }
    #endregion

    public class SmallHealth : StatPotion {
        SmallHealth() {
            type = (int)Type.health;
        }
    }
}