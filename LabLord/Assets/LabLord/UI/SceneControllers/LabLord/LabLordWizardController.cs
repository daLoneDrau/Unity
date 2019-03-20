using LabLord.Flyweights;
using LabLord.Singletons;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LabLord.UI.SceneControllers.LabLord
{
    public class LabLordWizardController : Singleton<LabLordWizardController>
    {
        public GameObject Player;
        public GameObject Equipment;
        #region MonoBehaviour messages
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            // initialize singleton controllers
            //WoFMCombat.Init();
            LabLordController.Init();
            LabLordInteractive.Init();
            LabLordScript.Init();
            DontDestroyOnLoad(gameObject);
            Player = NewHero();
        }
        private GameObject NewHero()
        {
            // create player GameObject
            GameObject player = new GameObject
            {
                name = "player"
            };
            // position player outside the screen
            player.transform.position = new Vector3(-1, 0, 0);
            /********************************************************
             * SETUP LAB_LORD_INTERACTIVE_OBJECT COMPONENT
            /*******************************************************/
            LabLordInteractiveObject playerIo = player.AddComponent<LabLordInteractiveObject>() as LabLordInteractiveObject;
            /********************************************************
             * REGISTER THE IO AND INITIALIZE INTERACTIVE COMPONENTS
            /*******************************************************/
            ((LabLordInteractive)Interactive.Instance).NewHero(playerIo);
            /********************************************************
             * SETUP WATCHERS
            /*******************************************************/
            return player;
        }
        public GameObject NewItem(LabLordScriptable script)
        {
            // create player GameObject
            GameObject item = new GameObject
            {
                name = "item"
            };
            item.transform.parent = Equipment.transform;
            // position player outside the screen
            item.transform.position = new Vector3(-1, 0, 0);
            /********************************************************
             * SETUP LAB_LORD_INTERACTIVE_OBJECT COMPONENT
            /*******************************************************/
            LabLordInteractiveObject io = item.AddComponent<LabLordInteractiveObject>() as LabLordInteractiveObject;
            /********************************************************
            * REGISTER THE IO AND INITIALIZE INTERACTIVE COMPONENTS
            /*******************************************************/
            ((LabLordInteractive)Interactive.Instance).NewItem(io, script);
            /********************************************************
             * SETUP WATCHERS
            /*******************************************************/
            io = null;
            return item;
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        void Start()
        { }
        #endregion
    }
}
