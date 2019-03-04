using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exit : MonoBehaviour {
	// Use this for initialization
	public void exitGame() {
		Application.Quit();
		Debug.Log("Exiting...");
	}
}
