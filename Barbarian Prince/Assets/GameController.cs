using Assets.Scripts.BarbarianPrince.Singletons;
using Assets.Scripts.BarbarianPrince.UI.Controllers;
using Assets.Scripts.RPGBase.Singletons;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    [SerializeField]
    private Text loadingText;
    [SerializeField]
    private GameObject startMenu;
    public void ClickStart()
    {
        print("start");
        startMenu.SetActive(false);
        currentState = STATE_GAME;
        RPGTime.Instance.Init(); // start the game timer
        ((BPInteractive)Interactive.Instance).NewHero();
    }
    protected GameController() { } // guarantee this will be always a singleton only - can't use the constructor!
                                   // Use this for initialization
    void Awake()
    {
        // initialize all singletons
        //Debug.Log(DemoController.Instance);
        BPController.Init();
        BPInteractive.Init();
        BPScript.Init();
    }
    private const int STATE_LOADING = 0;
    private const int STATE_START_MENU = 1;
    private const int STATE_GAME = 2;
    private int currentState = STATE_LOADING;
    private void GameCycle()
    {
        switch (currentState)
        {
            case STATE_LOADING:
                if (WorldController.Instance.LoadComplete)
                {
                    loadingText.transform.parent.gameObject.SetActive(false);
                    currentState = STATE_START_MENU;
                    startMenu.SetActive(true);
                }
                else
                {
                    loadingText.transform.parent.gameObject.SetActive(true);
                    float now = Time.realtimeSinceStartup * 1000;
                    PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                    sb.Append(loadingText.text);
                    if (now - lastLoad > 333f)
                    {
                        sb.Append(".");
                    }
                    loadingText.text = sb.ToString();
                    sb.ReturnToPool();
                    print("############################################iter");
                }
                break;
            case STATE_START_MENU:
                // wait for user to select start
                break;
            case STATE_GAME:
                // if splash screen - do not render goto end

                // check player input

                // check keyboard and mouse input

                // if rendering menus goto end

                // if rendering scene goto end

                // render
                WorldController.Instance.DisplayMap();

                // check script timers

                // check if player is dead

                // draw particles

                // render cursor

                // AFTER RENDERING

                // DEPENDING ON GAME STATE, UPDATE GAME
                // if menus off,
                // execute script stack
                // update damage spheres
                // update missiles
                break;
        }
    }
    void Start () {
		
	}
    private float lastLoad;
	// Update is called once per frame
	void Update () {
        GameCycle();
    }
}
