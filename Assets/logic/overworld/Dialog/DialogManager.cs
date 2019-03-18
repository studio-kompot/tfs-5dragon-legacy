//Dev!Bird Dialog System
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {
#region Variables
    public static DialogManager instance;
    public GameObject DialogPanel;
    public Text DName;
    public Text DText;
    public Image DFaceplate;
    public Queue<DialogBase.DFrame> dq;
    public float delay = 0.001f;
    private bool isOptionsType;
    private bool inDialog; //bugfix to garbled text appearing
    public GameObject OptionsContainer;
#endregion
#region Methods
    void Start() {
        dq = new Queue<DialogBase.DFrame>();
    }
    public void Awake() {
        if (instance != null) Debug.LogWarning("Error: DialogManager object already instanced");
        else instance = this;
    }

    public void Init(DialogBase d) {
        dq.Clear();
        if (inDialog) return;
        inDialog = true;
        isOptionsType = (d is DialogOptions);
        DialogPanel.SetActive(true);
        foreach (DialogBase.DFrame i in d.DialogData) {
            dq.Enqueue(i);
        }
        Send();
    }
    public void Send() {
        if (dq.Count == 0) {
            DialogPanel.SetActive(false);
            OptionsContainer.SetActive(isOptionsType);
            inDialog = DialogPanel.activeInHierarchy;
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
