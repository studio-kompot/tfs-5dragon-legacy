using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dast;

public class pEvent : MonoBehaviour {
	///<summary>Initates "Pass mode", meaning the player can pass onto the object and activate it from there.</summary>
	public bool PassMode;
	GameObject manager;
	Dialog OutArray;
	// Use this for initialization
	void Start () {
		manager = GameObject.Find("Manager");
	}
	// Update is called once per frame
	void Update () {

	}
	private void OnCollisionEnter(Collision c) {
		manager.SendMessage("StartDialogue",OutArray);
	}
}
