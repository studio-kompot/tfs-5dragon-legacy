using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

#region Variables
    public GameObject[] ContainerList;
    public Transform HealthPanel;
    public Queue<BattleItem> BattleQueue;

#endregion
    #region Methods
    // Use this for initialization
    void Start () {/*
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
	
	// Update is called once per frame
	void Update () {
		
	}
#endregion
}
