using Assets.Scripts.UI.SimpleJSON;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WoFM.Flyweights;
using WoFM.Singletons;
using WoFM.UI.GlobalControllers;

namespace WoFM.UI.SceneControllers
{
    public class CharBuilderController : MonoBehaviour
    {
        public Text textDescription;
        public void ShowText(int text)
        {
            switch (text)
            {
                case 0:
                    textDescription.text = "";
                    break;
                case 1:
                    textDescription.text = "Your SKILL score reflects your swordsmanship and general fighting expertise; the higher the better.";
                    break;
                case 2:
                    textDescription.text = "Your STAMINA score reflects your general constitution, your will to survive, your determination and overall fitness; the higher your STAMINA score, the longer you will be able to survive.";
                    break;
                case 3:
                    textDescription.text = "Your LUCK score indicates how naturally lucky a person you are. Luck – and magic – are facts of life in the fantasy kingdom you are about to explore. ";
                    break;
            }
        }
        #region MonoBehaviour messages
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        void Start()
        {
            // create player object
            GameObject player = GameController.Instance.NewHero();
            // get IO component
            WoFMInteractiveObject playerIo = player.GetComponent<WoFMInteractiveObject>();
            // add watcher for player stats
            playerIo.PcData.AddWatcher(GetComponent<PlayerWatcher>());
            // re-initialize player stats
            Script.Instance.SendInitScriptEvent(playerIo);

            // remove instances for garbage collection
            player = null;
            playerIo = null;
        }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
        }
        #endregion
        public void RerollStats()
        {
            Script.Instance.SendInitScriptEvent(((WoFMInteractive)Interactive.Instance).GetPlayerIO());
        }
        bool doonce;
        /// <summary>
        /// Loads the next scene.
        /// </summary>
        public void NextScene()
        {
            // go to GAME scene
            GameController.Instance.nextScene = 3;
            GameController.Instance.LoadText("START");
            SceneManager.LoadScene(1);
        }
    }
}
