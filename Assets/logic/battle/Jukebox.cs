using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour {
    AudioSource m;
    public AudioClip[] Tracks;
    public int? TrackNumber;
	// Use this for initialization
	void Start () {
        m = GetComponent<AudioSource>();
        if (!m || m == null) {
#if UNITY_EDITOR
            Debug.LogError("Jukebox component unable to access its AudioSource component");
#endif
            throw new System.NullReferenceException();
        } else {
            m.clip = Tracks[TrackNumber ?? 0];
            if (m.clip != null) m.Play();
        }
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void SetMusic(AudioClip a) { }
}
