//Dev!Bird Dialog System
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {
    public static DialogManager instance;
    public void Awake() {
        if (instance != null) Debug.LogWarning("Error: DialogManager object already instanced");
        else instance = this;
    }

    public GameObject DialogPanel;
    public Text DName;
    public Text DText;
    public Image DFaceplate;
    public Queue<DialogBase.DFrame> dq;
    public float delay = 0.001f;
#region Methods
    void Start() {
        dq = new Queue<DialogBase.DFrame>();
    }

    public void Init(DialogBase d) {
        dq.Clear();
        DialogPanel.SetActive(true);
        foreach (DialogBase.DFrame i in d.DialogData) {
            dq.Enqueue(i);
        }
        Send();
    }
    public void Send() {
        if (dq.Count == 0) {
            DialogPanel.SetActive(false);
            return;
        } else {
            DialogBase.DFrame current = dq.Dequeue();
            DName.text = current.name;
            DText.text = current.text;
            DFaceplate.sprite = current.faceplate;
            StartCoroutine(Scroll(current));
        }
    }
    
    IEnumerator Scroll (DialogBase.DFrame frame) {
        DText.text = "";
        foreach(char i in frame.text.ToCharArray()) {
            yield return new WaitForSeconds(delay);
            DText.text += i;
            yield return null;
        }
    }
#endregion
}
