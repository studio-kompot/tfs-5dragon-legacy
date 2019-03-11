using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogButton : MonoBehaviour {

    public void NextLine() {
        DialogManager.instance.Send();
    }
	
	// Update is called once per frame
	void Update () {
		if (gameObject.activeInHierarchy && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
            DialogManager.instance.Send();
    }
}
