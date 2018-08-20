using LabLord.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace LabLord.UI.SceneControllers.CharWizard
{
    public class GenderButtonController : MonoBehaviour
    {
        /// <summary>
        /// the Male button.
        /// </summary>
        public Toggle Male;
        /// <summary>
        /// the Female button.
        /// </summary>
        public Toggle Female;
        public void Awake()
        {
            DisableAll();
        }
        /// <summary>
        /// Disables all buttons.
        /// </summary>
        public void DisableAll()
        {
            Male.interactable = false;
            Female.interactable = false;
        }
    }
}
