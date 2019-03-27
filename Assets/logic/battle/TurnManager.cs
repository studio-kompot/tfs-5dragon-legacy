using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    #region Variables
    public GameObject[] ContainerList;
    public Transform HealthPanel;
    public Queue<BattleItem> BattleQueue;
    private static System.Random rand = new System.Random(20);

    #endregion
    #region Methods
    // Use this for initialization
    void Start() {/*
         * Loads in in the order:
         * EnemyContainer
         * HealthPanelContainer
         * OptionsPanel
         */
        ContainerList = GameObject.FindGameObjectsWithTag("Container");
#if UNITY_EDITOR
        foreach (GameObject i in ContainerList) {
            if (!i) {
                Debug.LogError("Not Found");
            }
            //Debug.Log(i.name);
        }
#endif
        foreach (PartyMember i in Database.Party) {
            var current = Instantiate(HealthPanel, ContainerList[1].transform);

        }
    }

    SortedList<int, Character> cTurnOrder() {
        var rep = new SortedList<int, Character>();
        foreach (Character i in Database.Party) {
            if (i == null) { //can't happen, hopefully.
                Debug.LogError("Uh oh! A party member has been passed as null!");
                break;
            }
            rep.Add(rand.Next(1, 20) + Database.CalcAbilScore(i.dex), i);
        }

        foreach (Character i in BattleInit.Enemies) {
            if (i == null) { //can't happen, hopefully.
                Debug.LogError("Uh oh! An enemy list member has been passed as null!");
                break;
            }
            rep.Add(rand.Next(1, 20) + Database.CalcAbilScore(i.dex), i);
        }
#if UNITY_EDITOR && TEMP
        foreach (var i in rep) {
            Debug.Log("{0}: {1}".Format(i.Key.ToString(), i.Value.name));
        }
#endif
        return rep;
}

// Update is called once per frame
void Update() {

}
#endregion
}
