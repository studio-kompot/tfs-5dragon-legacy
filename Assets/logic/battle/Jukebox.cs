using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour {
    AudioSource m;
	// Use this for initialization
	void Start () {
        m = GetComponent<AudioSource>();
        if (!m || m == null) {
#if UNITY_EDITOR
            Debug.LogError("Jukebox component unable to access its AudioSource component");
#endif
            throw new System.NullReferenceException();
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void SetMusic(AudioClip a) { }
}
