using LabLord.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace LabLord.UI.SceneControllers.CharWizard
{
    public class RaceButtonController : MonoBehaviour
    {
        /// <summary>
        /// the Dwarf button.
        /// </summary>
        public Toggle Dwarf;
        /// <summary>
        /// the Elf button.
        /// </summary>
        public Toggle Elf;
        /// <summary>
        /// the Gnome button.
        /// </summary>
        public Toggle Gnome;
        /// <summary>
        /// the Half_Elf button.
        /// </summary>
        public Toggle Half_Elf;
        /// <summary>
        /// the Halfling button.
        /// </summary>
        public Toggle Halfling;
        /// <summary>
        /// the Half_Orc button.
        /// </summary>
        public Toggle Half_Orc;
        /// <summary>
        /// the Human button.
        /// </summary>
        public Toggle Human;
        public void Awake()
        {
            DisableAll();
        }
        /// <summary>
        /// Disables all buttons.
        /// </summary>
        public void DisableAll()
        {
            Dwarf.interactable = false;
            Elf.interactable = false;
            Gnome.interactable = false;
            Halfling.interactable = false;
            Half_Elf.interactable = false;
            Half_Orc.interactable = false;
            Human.interactable = false;
        }
        /// <summary>
        /// Enables buttons based on the options available.
        /// </summary>
        /// <param name="options"></param>
        public void SetOptions(int options)
        {
            DisableAll();
            if ((options & LabLordRace.RACE_DWARF) == LabLordRace.RACE_DWARF)
            {
                Dwarf.interactable = true;
            }
            if ((options & LabLordRace.RACE_ELF) == LabLordRace.RACE_ELF)
            {
                Elf.interactable = true;
            }
            if ((options & LabLordRace.RACE_GNOME) == LabLordRace.RACE_GNOME)
            {
                Gnome.interactable = true;
            }
            if ((options & LabLordRace.RACE_HALFLING) == LabLordRace.RACE_HALFLING)
            {
                Halfling.interactable = true;
            }
            if ((options & LabLordRace.RACE_HALF_ELF) == LabLordRace.RACE_HALF_ELF)
            {
                Half_Elf.interactable = true;
            }
            if ((options & LabLordRace.RACE_HALF_ORC) == LabLordRace.RACE_HALF_ORC)
            {
                Half_Orc.interactable = true;
            }
            if ((options & LabLordRace.RACE_HUMAN) == LabLordRace.RACE_HUMAN)
            {
                Human.interactable = true;
            }
        }
    }
}
