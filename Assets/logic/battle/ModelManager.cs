using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour {

    public GameObject[] ContainerList;
    public Transform HealthPanel;

	// Use this for initialization
	void Start () {
        /*Loads in in the order:
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
            Debug.Log(i.name);
        }
#endif
        /*
        foreach (PartyMember i in PartyData.partyMembers) {
            Instantiate(HealthPanel, ContainerList[1].transform);
        }
        */
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
