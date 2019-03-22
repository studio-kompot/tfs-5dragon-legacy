using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Skytanet.SimpleDatabase;

public class Database : MonoBehaviour {
    public Enum currentScene;
    public SaveFile currSave;
    public string sfName;

    public static List<PartyMember> Party = new List<PartyMember>();
    public Dictionary<string, bool> Events;
    private GameObject Player;
    private string[] currSaveFiles;


    void Start() {
        DontDestroyOnLoad(this.gameObject);
        currSaveFiles = SaveFile.GetSaveFileList();
        //if (currSaveFiles.Contains(sfName))
    }

    void InitSaveFile(string s) {
        currSave = new SaveFile(s ?? sfName);
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
}
