using Assets.Scripts.BarbarianPrince.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private BoardManager boardManager;
	// Use this for initialization
	void Awake () {
        InitGame();
	}

    private void InitGame()
    {
        boardManager = GetComponent<BoardManager>();
        boardManager.SetupScene();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
