using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LabLord.UI.GlobalControllers;
using UnityEngine.UI;
using RPGBase.Scripts.UI.SimpleJSON;

namespace LabLord.UI.SceneControllers
{
    public class MainMenuController : MonoBehaviour
    {
        /// <summary>
        /// the configuration file.
        /// </summary>
        private JSONNode configFile;
        public GameObject systemsPanel;
        public GameObject buttonPrefab;
        /// <summary>
        /// The cursor texture.
        /// </summary>
        public Texture2D PointerTexture;
        /// <summary>
        /// Loads the next scene.
        /// </summary>
        public void NextScene()
        {
            GameController.Instance.nextScene = 2;
            SceneManager.LoadScene(1);
        }
        public void LoadA(string scenename)
        {
            Debug.Log("sceneName to load: " + scenename);
            SceneManager.LoadScene(scenename);
        }
        /// <summary>
        /// Quits the game.
        /// </summary>
        public void Quit()
        {
#if UNITY_EDITOR
            Debug.Log("Unity Editor");
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        #region MonoBehavior
        public void Awake()
        {
            print("Main Menu Awake");
            //*******************************//
            // LOAD GAME RESOURCES
            //*******************************//
            // load text file (JSON) version
            string json = System.IO.File.ReadAllText(@"Assets/JS/config.json");
            configFile = JSON.Parse(json);
            JSONArray array = configFile["systems"].AsArray;
            for (int i = 0, li = array.Count; i < li; i++)
            {
                JSONNode node = array[i];
                print((string)node["title"]);
                // create new button for game system
                GameObject newButton = Instantiate(buttonPrefab) as GameObject;
                newButton.transform.GetChild(0).GetComponent<Text>().text = (string)node["title"];
                newButton.GetComponent<Button>().onClick.AddListener(() => LoadA((string)node["scene"]));
                // parent panel to new button
                newButton.transform.SetParent(systemsPanel.transform, false);
                //newButton.GetComponent<>
            }
        }
        #endregion
    }
}
