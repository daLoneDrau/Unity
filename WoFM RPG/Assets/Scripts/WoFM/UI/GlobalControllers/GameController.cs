using Assets.Scripts.UI.SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;
using WoFM.Singletons;

namespace WoFM.UI.GlobalControllers
{
    public class GameController : Singleton<GameController>
    {
        void Awake()
        {
            WoFMController.Init();
            WoFMInteractive.Init();
            WoFMScript.Init();
            DontDestroyOnLoad(gameObject);
            // load text file (JSON) version
            string json = System.IO.File.ReadAllText(@"Assets/JS/text.json");
            textFile = JSON.Parse(json);
            LoadText("BACKGROUND");
        }
        /// <summary>
        /// the next scene playing after the 
        /// </summary>
        public int nextScene;
        /// <summary>
        /// the text file.
        /// </summary>
        private JSONNode textFile;
        public void LoadText(string entry)
        {
            textEntry = textFile[entry];
        }
        public string textEntry;
    }
}
