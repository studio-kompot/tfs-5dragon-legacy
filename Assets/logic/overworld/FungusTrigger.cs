/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FungusTrigger : MonoBehaviour {
    //public Flowchart fc;
    public bool ready = false;
    public string ToCall;
    private bool inProxim;
    
    void OnCollisionEnter(UnityEngine.Collision c) {
        var go = c.gameObject;
        var player = c.gameObject.transform.parent;
        inProxim = (go.tag == "playerCollision");
    }
    private void OnCollisionExit(UnityEngine.Collision collision) {
        inProxim = false;
    }
    private void Update() {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return) && inProxim) {
            Flowchart.BroadcastFungusMessage(ToCall == "" ? "error" : ToCall);
        }
    }
}
*/