using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FungusTrigger : MonoBehaviour {
    public Flowchart fc;
    public bool ready = false;
    
    void OnCollisionEnter(UnityEngine.Collision c) {
        var go = c.gameObject;
        var player = c.gameObject.transform.parent;
        if (go.tag == "PlayerCollision") {
            switch (PlayerMovement.dir) {
                case 0:

                    break;
                case 1:

                    break;
                case 2:

                    break;
                case 3:

                    break;
            }
        }
    }
}
