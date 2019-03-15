using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBattleObject {
    string id { get; set; }
    void doAction();
}