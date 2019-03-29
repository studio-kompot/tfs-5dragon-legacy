using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pEvent : MonoBehaviour {
    /**
     *<summary>
     *<list type="number">

     * <item>
     * <term>Sign</term>
     * <description>A static object that doesn't move</description>
     * </item>
     *</list>
     *</summary>
     */
    public enum Type { debug, sign, npc, cutscene }
    public Type mode;
    public DialogBase dialog;
    public GameObject DialogPanel;
    public bool active;

    public void Trigger() {
        DialogManager.instance.Init(dialog);
    }
    public void Update() {
        //TODO: Add more appropriate trigger
        if (Input.GetKey(KeyCode.I) && mode == Type.debug) DialogManager.instance.Init(dialog);
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return)) && (mode == Type.sign || mode == Type.npc)) DialogManager.instance.Init(dialog);
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        active = (collision.gameObject.tag == "playerCollision") ;
    }
}
