using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour {
    AudioSource m;
    public List<AudioClip> Tracks;
	// Use this for initialization
	void Start () {
        m = GetComponent<AudioSource>();
        if (!m || m == null) {
#if UNITY_EDITOR
            Debug.LogError("Jukebox component unable to access its AudioSource component");
#endif
            throw new System.NullReferenceException();
        } else {
            m.clip = Tracks[(int)BattleInit.AMusic];
            if (m.clip != null) m.Play();
        }
        
    }
    void SetMusic(AudioClip a) {
        Tracks.Add(a);
        m.clip = a;
    }
}
