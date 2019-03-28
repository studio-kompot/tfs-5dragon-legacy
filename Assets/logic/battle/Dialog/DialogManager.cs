//Dev!Bird Dialog System
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogManager : MonoBehaviour {
    #region Variables
    public static DialogManager instance;
    public GameObject DialogPanel;
    public Text DName;
    public Text DText;
    public Image DFaceplate;
    public Queue<DialogBase.DFrame> dq;
    public float delay = 0.001f;
    StringBuilder builder = new StringBuilder();
    public string StartTag { get; set; }
    public string EndTag { get; set; }
    public string ToPrint { get; set; }
    public int RegionIndex { get; set; }
    public int RegionBuffer { get; set; }
    public bool IsRichText { get; set; }

    //options extension
    private bool isOptionsType;
    public GameObject OptionsContainer;
    private bool inDialog; //bugfix to garbled text appearing
    public GameObject[] OptionButtons;

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
        if (isOptionsType) {
            DialogOptions options = d as DialogOptions;
            /*
            for(var i=0; i<OptionButtons.Length; i++) {
                OptionButtons[i].SetActive(true);
                OptionButtons[i].transform.GetChild(0).gameObject.GetComponent<DText>().text = DialogOptions.OptionArray[i].optionText;
                var eh = OptionButtons[i].GetComponent<UnityEventHandler>();
                eh.eventHandler = DialogOptions.OptionsArray[i].Activated;
            }
            */

        }
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
            inDialog = DialogPanel.activeInHierarchy; //false, in other words
            if (isOptionsType) {
                OptionsContainer.SetActive(true);
                for (var i = 0; i < OptionButtons.Length; i++) {
                    OptionButtons[i].SetActive(true);
                }
            }
            return;
        } else {
            DialogBase.DFrame current = dq.Dequeue();
            DName.text = current.name;
            DText.text = current.text;
            DFaceplate.sprite = current.faceplate;
            StartCoroutine(Scroll(current));
        }
    }

    IEnumerator Scroll(DialogBase.DFrame frame) {
        ClearData();
        ToPrint = frame.text;
        for (int i = 0; i < ToPrint.Length; i++) {
            var j = ToPrint[i];
            yield return new WaitForSeconds(delay);
            IsRichText = (j == '<');
            if (IsRichText) { //begin text formatting
                RegionIndex = 0;
                RichTextParser.ParseText(frame.text);
                if (RegionIndex < RegionBuffer) {
                    builder.Append(StartTag + j + EndTag);
                } else {
                    IsRichText = false;
                }
            } else builder.Append(ToPrint[i]);
            DText.text = builder.ToString();
        }
    }
    void ClearData() {
        DText.text = "";
        builder.Length = 0;
        RegionIndex = 0;
        RegionBuffer = 0;
        IsRichText = false;
    }
    #endregion
}
