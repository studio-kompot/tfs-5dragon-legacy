using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour {

    public List<Sprite> Backgrounds;
    private Image current;

	// Use this for initialization
	void Start () {
        current = GetComponent<Image>();
        current.sprite = Backgrounds[(int)BattleInit.ABackground];
	}
}
