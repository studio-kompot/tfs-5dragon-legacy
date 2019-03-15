//Dev!Bird Dialog System
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogButton : MonoBehaviour {
    AudioSource beep;

    void Awake() {
        beep = GetComponent<AudioSource>();
#if UNITY_EDITOR
        if (beep == null) Debug.Log("it's not there chief");
#endif
    }

    public void NextLine() {
        if (DialogManager.instance != null) {
            DialogManager.instance.Send();
            beep.Play();
        }
#if UNITY_EDITOR
        else {
            Debug.Log("DialogManager's not there chief");
        }
#endif
    }
	
	// Update is called once per frame
	void Update () {
		if (gameObject.activeInHierarchy && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
            DialogManager.instance.Send();
    }
}
