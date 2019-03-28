using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewDialogOptions", menuName = "Dialog Option")]
public class DialogOptions : DialogBase {
    [System.Serializable]
    public class Options {
        public string optionText;
        public UnityEvent Activated;
    }
    public static Options[] OptionArray;
}
