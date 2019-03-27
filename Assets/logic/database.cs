using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Skytanet.SimpleDatabase;

public class Database : MonoBehaviour {
#region Variables
    public Enum currentScene;
    public SaveFile currSave;
    public string sfName;

    public static List<PartyMember> Party;
    public Dictionary<string, bool> Events;
    private GameObject Player;
    private string[] currSaveFiles;
#endregion
#region Methods
    void Start() {
        DontDestroyOnLoad(this.gameObject);
        currSaveFiles = SaveFile.GetSaveFileList();
        //if (currSaveFiles.Contains(sfName))
    }

    void InitSaveFile(string s) {
        currSave = new SaveFile(s ?? sfName);
    }

    public static int CalcAbilScore(int x) {
        return (int)System.Math.Floor((double)((x - 10) / 2));
    }

    void DoSave() {
        currSave.Set("position",(Vector2)Player.transform.localPosition);
        currSave.Set("Scene", SceneManager.GetActiveScene().name);
        currSave.Set("Party",Party);//party
        currSave.Set("Events",Events);
    }
    void DoLoad() {
        throw new System.NotImplementedException();
        return;

    }
#endregion
}
