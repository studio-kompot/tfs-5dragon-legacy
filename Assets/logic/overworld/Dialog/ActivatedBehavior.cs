using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "Button Event")]
public class ActivatedBehavior : ScriptableObject {

    void helloworld() {
        Debug.Log("Hello, World!");
    }

}
