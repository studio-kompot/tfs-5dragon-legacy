using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skytanet.SimpleDatabase;

public class SaveGames : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (string i in SaveFile.GetSaveFileList()) { }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
