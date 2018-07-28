using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WoFM.UI.GlobalControllers;

namespace WoFM.UI.SceneControllers
{
    public class MainMenuController : MonoBehaviour
    {
        /// <summary>
        /// Loads the next scene.
        /// </summary>
        public void NextScene()
        {
            GameController.Instance.nextScene = 2;
            SceneManager.LoadScene(1);
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
    }
}
