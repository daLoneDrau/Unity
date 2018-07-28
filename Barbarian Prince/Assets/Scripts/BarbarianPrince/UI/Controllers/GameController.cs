using Assets.Scripts.BarbarianPrince.Singletons;
using Assets.Scripts.BarbarianPrince.UI.Controllers;
using Assets.Scripts.RPGBase.Singletons;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.BarbarianPrince.UI.Controllers
{
    public class GameController : Singleton<GameController>
    {
        /// <summary>
        /// flag indicating an invalid state.
        /// </summary>
        public const int STATE_INVALID = -1;
        public const int STATE_LOADING = 0;
        public const int STATE_START_MENU = 1;
        public const int STATE_CHOOSE_ACTION = 2;
        public const int STATE_GAME = 3;
        /// <summary>
        /// message state - updates continue after the user clears the messages.
        /// </summary>
        public const int STATE_MESSAGE = 4;
        /*****************
         * ANIMATORS
         ****************/
        /// Animator component for the HUD
        private Animator hudAnim;
        /*****************
         * UI COMPONENTS
         ****************/
        [SerializeField]
        private Text loadingText;
        [SerializeField]
        private GameObject hudPanel;
        [SerializeField]
        private GameObject startMenu;
        [SerializeField]
        private GameObject msgPanel;
        [SerializeField]
        private Text msgText;
        [SerializeField]
        private GameObject charPanel;
        [SerializeField]
        private GameObject timetrack;
        private int nextState;
        /// <summary>
        /// the last state the game was in.
        /// </summary>
        private int lastState;
        private int currentState = STATE_LOADING;
        public int CurrentState { get { return currentState; } }
        protected GameController() { } // guarantee this will be always a singleton only - can't use the constructor!
                                       // Use this for initialization
        void Awake()
        {
            // set the desired aspect ratio, I set it to fit every screen 
            float targetaspect = (float)Screen.width / (float)Screen.height;

            // determine the game window's current aspect ratio
            float windowaspect = (float)Screen.width / (float)Screen.height;

            // current viewport height should be scaled by this amount
            float scaleheight = windowaspect / targetaspect;

            // obtain camera component so we can modify its viewport
            Camera camera = Camera.main;
            // if scaled height is less than current height, add letterbox
            if (scaleheight < 1.0f)
            {
                Rect rect = camera.rect;

                rect.width = 1.0f;
                rect.height = scaleheight;
                rect.x = 0;
                rect.y = (1.0f - scaleheight) / 2.0f;

                camera.rect = rect;
            }
            else // add container box
            {
                float scalewidth = 1.0f / scaleheight;

                Rect rect = camera.rect;

                rect.width = scalewidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scalewidth) / 2.0f;
                rect.y = 0;

                camera.rect = rect;
            }

            hudAnim = (Animator)hudPanel.GetComponent(typeof(Animator));

            // initialize all singletons
            //Debug.Log(DemoController.Instance);
            BPController.Init();
            BPInteractive.Init();
            BPScript.Init();
        }
        /// <summary>
        /// Handler for user clicking start button
        /// </summary>
        public void ClickStart()
        {
            print("start");
            startMenu.SetActive(false);
            currentState = STATE_GAME;
            // start the game timer
            RPGTime.Instance.Init();
            // start the master scrip event 001
            StartCoroutine(MasterScript.Instance.E001TheAdventureBegins());
        }
        private void HideUI()
        {
            loadingText.transform.parent.gameObject.SetActive(false);
            startMenu.SetActive(false);
            msgPanel.SetActive(false);
        }
        private void MessageCycle()
        {
            // assume message screen is displayed

            // update the world in the background
            WorldController.Instance.DisplayMap();
        }
        void Start()
        {

        }
        /// <summary>
        /// Displays a message to the player.
        /// </summary>
        /// <param name="msg">the message</param>
        public void ShowMessage(string msg)
        {
            // PAUSE THE GAME
            RPGTime.Instance.Pause();
            // capture current state
            lastState = currentState;
            // go to message state
            currentState = STATE_MESSAGE;
            // show message panel and message
            HideUI();
            msgPanel.SetActive(true);
            msgText.text = msg;
        }
        public void HideMessage()
        {
            // UNPAUSE THE GAME
            RPGTime.Instance.Unpause();
            currentState = lastState;
            HideUI();
            msgText.text = "";
        }
        /// <summary>
        /// Starts a loading sequence. When loading is complete, the game will either go to the next state, or if that is invalid, to the last state.
        /// </summary>
        /// <param name="next">the next state (which can be <see cref="STATE_INVALID"/>)</param>
        public void StartLoad(int next)
        {
            // PAUSE THE GAME
            RPGTime.Instance.Pause();
            nextState = next;
            lastState = currentState;
            currentState = STATE_LOADING;
            HideUI();
            loadingText.text = "Loading";
            loadingText.transform.parent.gameObject.SetActive(true);
        }
        public void StopLoad()
        {
            print("StopLoad - go to " + nextState + " or " + lastState);
            // unpause
            RPGTime.Instance.Unpause();
            HideUI();
            if (nextState > 0)
            {
                currentState = nextState;
            }
            else
            {
                currentState = lastState;
            }
        }
        public void UpdateTimeTrack(string msg)
        {
            timetrack.GetComponent<Text>().text = msg;
        }
        private float lastLoad;
        // Update is called once per frame
        void Update()
        {
            GameCycle();
        }
        private bool AnimatorIsPlaying(Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;
        }
        private bool AnimatorIsPlaying(Animator animator, string stateName)
        {
            return AnimatorIsPlaying(animator) && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }
        private void GameCycle()
        {
            switch (currentState)
            {
                case STATE_LOADING:
                    LoadCycle();
                    break;
                case STATE_START_MENU:
                    // wait for user to select start
                    startMenu.SetActive(true);
                    break;
                case STATE_CHOOSE_ACTION:
                    break;
                case STATE_GAME:
                    // if splash screen - do not render goto end

                    // check player input

                    // check keyboard and mouse input

                    // if rendering menus goto end

                    // if rendering scene goto end

                    // render
                    WorldController.Instance.DisplayMap();
                    WorldController.Instance.DrawHero();

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
                    HudCycle();
                    break;
                case STATE_MESSAGE:
                    MessageCycle();
                    break;
            }
            // update the mouse position at the end of the frame
            MouseListener.Instance.UpdateMousePosition();
        }
        /*************************
         * GAME CYCLE STATES
         ************************/
        /// <summary>
        /// Displays a load cycle; the game screen shows the word loading with dots after it.
        /// </summary>
        private void LoadCycle()
        {
            print("Loading");
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
            // IGNORE ALL MOUSE/KEYBOARD INPUT
        }
        /// <summary>
        /// Displays a load cycle; the game screen shows the word loading with dots after it.
        /// </summary>
        private void HudCycle()
        {
            print("Hud Menu Showing");
            // IGNORE ALL MOUSE/KEYBOARD INPUT
            // check to see if hud slide intro animation is playing or played
            if (!AnimatorIsPlaying(hudAnim, "HUDSlideAnimation"))
            {
                print("show hud animation");
                hudAnim.SetTrigger("ShowMenu");
            }
            // if slide finished need to show menu onscreen
            // if exiting hud, slide offscreen
        }
    }
}
