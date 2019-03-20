using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;
using LabLord.Singletons;
using RPGBase.Scripts.UI.SimpleJSON;

namespace LabLord.UI.GlobalControllers
{
    public class GameController : Singleton<GameController>
    {
        void Awake()
        {
            // LabLordController.Init();
            // LabLordInteractive .Init();
            // LabLordScript.Init();
            DontDestroyOnLoad(gameObject);
        }
        /// <summary>
        /// the next scene playing after the text scene
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
