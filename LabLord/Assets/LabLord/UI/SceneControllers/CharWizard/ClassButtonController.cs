using LabLord.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace LabLord.UI.SceneControllers.CharWizard
{
    public class ClassButtonController : MonoBehaviour
    {
        /// <summary>
        /// the Assassin button.
        /// </summary>
        public Toggle Assassin;
        /// <summary>
        /// the Cleric button.
        /// </summary>
        public Toggle Cleric;
        /// <summary>
        /// the Druid button.
        /// </summary>
        public Toggle Druid;
        /// <summary>
        /// the Fighter button.
        /// </summary>
        public Toggle Fighter;
        /// <summary>
        /// the Illusionist button.
        /// </summary>
        public Toggle Illusionist;
        /// <summary>
        /// the Magic-User button.
        /// </summary>
        public Toggle Magic_User;
        /// <summary>
        /// the Monk button.
        /// </summary>
        public Toggle Monk;
        /// <summary>
        /// the Paladin button.
        /// </summary>
        public Toggle Paladin;
        /// <summary>
        /// the Ranger button.
        /// </summary>
        public Toggle Ranger;
        /// <summary>
        /// the Thief button.
        /// </summary>
        public Toggle Thief;
        public void Awake()
        {
            DisableAll();
        }
        public void DisableAll()
        {
            Assassin.interactable = false;
            Cleric.interactable = false;
            Druid.interactable = false;
            Fighter.interactable = false;
            Illusionist.interactable = false;
            Magic_User.interactable = false;
            Monk.interactable = false;
            Paladin.interactable = false;
            Ranger.interactable = false;
            Thief.interactable = false;
        }
        public void SetOptions(int options)
        {
            DisableAll();
            print("SetOptions(" + options);
            if ((options & LabLordGlobals.CLASS_ASSASSIN) == LabLordGlobals.CLASS_ASSASSIN)
            {
                Assassin.interactable = true;
            }
            if ((options & LabLordGlobals.CLASS_CLERIC) == LabLordGlobals.CLASS_CLERIC)
            {
                Cleric.interactable = true;
            }
            if ((options & LabLordGlobals.CLASS_DRUID) == LabLordGlobals.CLASS_DRUID)
            {
                Druid.interactable = true;
            }
            if ((options & LabLordGlobals.CLASS_FIGHTER) == LabLordGlobals.CLASS_FIGHTER)
            {
                Fighter.interactable = true;
            }
            if ((options & LabLordGlobals.CLASS_ILLUSIONIST) == LabLordGlobals.CLASS_ILLUSIONIST)
            {
                Illusionist.interactable = true;
            }
            if ((options & LabLordGlobals.CLASS_MONK) == LabLordGlobals.CLASS_MAGIC_USER)
            {
                Magic_User.interactable = true;
            }
            if ((options & LabLordGlobals.CLASS_MONK) == LabLordGlobals.CLASS_MONK)
            {
                Monk.interactable = true;
            }
            if ((options & LabLordGlobals.CLASS_PALADIN) == LabLordGlobals.CLASS_PALADIN)
            {
                Paladin.interactable = true;
            }
            if ((options & LabLordGlobals.CLASS_RANGER) == LabLordGlobals.CLASS_RANGER)
            {
                Ranger.interactable = true;
            }
            if ((options & LabLordGlobals.CLASS_THIEF) == LabLordGlobals.CLASS_THIEF)
            {
                Thief.interactable = true;
            }
        }
    }
}
