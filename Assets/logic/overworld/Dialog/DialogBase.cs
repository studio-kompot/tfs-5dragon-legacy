//Dev!Bird Dialog System
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "Dialog")]
public class DialogBase : ScriptableObject {
    [System.Serializable]
    public class DFrame {
        /// <summary>Who's saying it</summary>
        public string name;
        /// <summary>The picture by the side of the text</summary>
        public Sprite faceplate;
        /// <summary>What's being said</summary>
        [TextArea(4, 8)]
        public string text;
    }
    public DFrame[] DialogData;
}
