using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pEvent : MonoBehaviour {
    ///<summary>Initates "Pass mode", meaning the player can pass onto the object and activate it from there.</summary>
    public bool PassMode;
    // Use this for initialization
    void Start () {
        GetComponent<BoxCollider>().isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    Collider OnTriggerEnter(Collider c) { }
}
