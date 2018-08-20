using Assets.Scripts.UI.SimpleJSON;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LabLord.Flyweights;
using LabLord.Singletons;
using LabLord.UI.GlobalControllers;
using LabLord.Constants;
using LabLord.UI.SceneControllers.CharWizard;

namespace LabLord.UI.SceneControllers
{
    public class CharBuilderController : Singleton<CharBuilderController>
    {
        /// <summary>
        /// the controller for the Race buttons
        /// </summary>
        public RaceButtonController raceButtonController;
        /// <summary>
        /// the controller for the Class buttons
        /// </summary>
        public ClassButtonController ClassButtonController;
        /// <summary>
        /// the toggle group for the race buttons.
        /// </summary>
        public ToggleGroup raceToggle;
        /// <summary>
        /// the toggle group for the class buttons.
        /// </summary>
        public ToggleGroup classToggle;
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
        void Awake()
        {
        }
        // Use this for initialization
        void Start()
        {
            LabLordInteractiveObject playerIo = ((LabLordInteractive)Interactive.Instance).NewHero();
            playerIo.PcData.AddWatcher(GetComponent<PlayerWatcher>());
            Script.Instance.SendInitScriptEvent(playerIo);
        }
        public void RaceClick(bool value)
        {
            if (raceToggle.AnyTogglesOn())
            {
                IEnumerable<Toggle> result = raceToggle.ActiveToggles();
                foreach (Toggle t in result)
                {
                    print(t.name);
                    if (t.name.ToLower().Contains("dwarf"))
                    {
                        int classOptions = GetValidClassesForRace(LabLordGlobals.RACE_DWARF);
                        ClassButtonController.SetOptions(classOptions);
                    }
                    else if (t.name.ToLower().Contains("halfelf"))
                    {
                        int classOptions = GetValidClassesForRace(LabLordGlobals.RACE_HALF_ELF);
                        ClassButtonController.SetOptions(classOptions);
                    }
                    else if (t.name.ToLower().Contains("elf"))
                    {
                        int classOptions = GetValidClassesForRace(LabLordGlobals.RACE_ELF);
                        ClassButtonController.SetOptions(classOptions);
                    }
                    else if (t.name.ToLower().Contains("gnome"))
                    {
                        int classOptions = GetValidClassesForRace(LabLordGlobals.RACE_GNOME);
                        ClassButtonController.SetOptions(classOptions);
                    }
                    else if (t.name.ToLower().Contains("halfling"))
                    {
                        int classOptions = GetValidClassesForRace(LabLordGlobals.RACE_HALFLING);
                        ClassButtonController.SetOptions(classOptions);
                    }
                    else if (t.name.ToLower().Contains("orc"))
                    {
                        int classOptions = GetValidClassesForRace(LabLordGlobals.RACE_HALF_ORC);
                        ClassButtonController.SetOptions(classOptions);
                    }
                    else if (t.name.ToLower().Contains("human"))
                    {
                        int classOptions = GetValidClassesForRace(LabLordGlobals.RACE_HUMAN);
                        ClassButtonController.SetOptions(classOptions);
                    }
                }
            }
        }
        /// <summary>
        /// Re-rolls the character's attributes.
        /// </summary>
        public void RerollStats()
        {
            raceButtonController.DisableAll();
            raceToggle.SetAllTogglesOff();
            ClassButtonController.DisableAll();
            classToggle.SetAllTogglesOff();
            LabLordInteractiveObject playerIo = (LabLordInteractiveObject)Interactive.Instance.GetIO(0);
            Script.Instance.SendInitScriptEvent(playerIo);
        }
        /// <summary>
        /// Enables all valid race buttons based on the character's attributes.
        /// </summary>
        public void EnableRaces()
        {
            raceButtonController.SetOptions(GetValidRaces());
        }
        private bool ValidateAssassin(int race, LabLordCharacter pc)
        {
            bool valid = false;
            if (race == LabLordGlobals.RACE_DWARF
                || race == LabLordGlobals.RACE_ELF
                || race == LabLordGlobals.RACE_GNOME
                || race == LabLordGlobals.RACE_HALF_ELF
                || race == LabLordGlobals.RACE_HALF_ORC
                || race == LabLordGlobals.RACE_HUMAN)
            {
                if (pc.GetBaseAttributeScore("STR") >= 12
                    && pc.GetBaseAttributeScore("DEX") >= 12
                    && pc.GetBaseAttributeScore("INT") >= 12)
                {
                    valid = true;
                }
            }
            return valid;
        }
        /// <summary>
        /// Gets the flags for possible Classes a character could choose, depending on the character's race and attributes.
        /// </summary>
        /// <param name="race">the specific race</param>
        /// <returns><see cref="int"/></returns>
        public int GetValidClassesForRace(int race)
        {
            int validClasses = 0;
            LabLordInteractiveObject playerIo = (LabLordInteractiveObject)Interactive.Instance.GetIO(0);
            LabLordCharacter pc = (LabLordCharacter)playerIo.PcData;
            // ASSASSIN
            if (ValidateAssassin(race, pc))
            {
                validClasses += LabLordGlobals.CLASS_ASSASSIN;
            }
            // CLERIC
            if (race == LabLordGlobals.RACE_DWARF
                || race == LabLordGlobals.RACE_ELF
                || race == LabLordGlobals.RACE_GNOME
                || race == LabLordGlobals.RACE_HALF_ELF
                || race == LabLordGlobals.RACE_HALF_ORC
                || race == LabLordGlobals.RACE_HUMAN)
            {
                if (pc.GetBaseAttributeScore("WIS") >= 12)
                {
                    validClasses += LabLordGlobals.CLASS_CLERIC;
                }
            }
            // DRUID
            if (race == LabLordGlobals.RACE_HUMAN)
            {
                if (pc.GetBaseAttributeScore("WIS") >= 12
                && pc.GetBaseAttributeScore("CHA") >= 15)
                {
                    validClasses += LabLordGlobals.CLASS_DRUID;
                }
            }
            // FIGHTER
            if (race == LabLordGlobals.RACE_DWARF
                || race == LabLordGlobals.RACE_ELF
                || race == LabLordGlobals.RACE_GNOME
                || race == LabLordGlobals.RACE_HALFLING
                || race == LabLordGlobals.RACE_HALF_ELF
                || race == LabLordGlobals.RACE_HALF_ORC
                || race == LabLordGlobals.RACE_HUMAN)
            {
                if (pc.GetBaseAttributeScore("STR") >= 12)
                {
                    validClasses += LabLordGlobals.CLASS_FIGHTER;
                }
            }
            // ILLUSIONIST
            if (race == LabLordGlobals.RACE_GNOME
                || race == LabLordGlobals.RACE_HUMAN)
            {
                if (pc.GetBaseAttributeScore("INT") >= 15
                && pc.GetBaseAttributeScore("DEX") >= 16)
                {
                    validClasses += LabLordGlobals.CLASS_ILLUSIONIST;
                }
            }
            // MAGIC-USER
            if (race == LabLordGlobals.RACE_ELF
                || race == LabLordGlobals.RACE_HALF_ELF
                || race == LabLordGlobals.RACE_HUMAN)
            {
                if (pc.GetBaseAttributeScore("INT") >= 12)
                {
                    validClasses += LabLordGlobals.CLASS_MAGIC_USER;
                }
            }
            // MONK
            if (race == LabLordGlobals.RACE_HUMAN)
            {
                if (pc.GetBaseAttributeScore("STR") >= 12
                && pc.GetBaseAttributeScore("DEX") >= 15
                && pc.GetBaseAttributeScore("WIS") >= 12)
                {
                    validClasses += LabLordGlobals.CLASS_MONK;
                }
            }
            // PALADIN
            if (race == LabLordGlobals.RACE_HUMAN)
            {
                if (pc.GetBaseAttributeScore("STR") >= 12
                && pc.GetBaseAttributeScore("INT") >= 9
                && pc.GetBaseAttributeScore("WIS") >= 13
                && pc.GetBaseAttributeScore("CHA") >= 17)
                {
                    validClasses += LabLordGlobals.CLASS_PALADIN;
                }
            }
            // RANGER
            if (race == LabLordGlobals.RACE_HALF_ELF
                || race == LabLordGlobals.RACE_HUMAN)
            {
                if (pc.GetBaseAttributeScore("STR") >= 12
                && pc.GetBaseAttributeScore("INT") >= 12
                && pc.GetBaseAttributeScore("WIS") >= 12
                && pc.GetBaseAttributeScore("CON") >= 15)
                {
                    validClasses += LabLordGlobals.CLASS_RANGER;
                }
            }
            // THIEF
            if (race == LabLordGlobals.RACE_DWARF
                || race == LabLordGlobals.RACE_ELF
                || race == LabLordGlobals.RACE_GNOME
                || race == LabLordGlobals.RACE_HALFLING
                || race == LabLordGlobals.RACE_HALF_ELF
                || race == LabLordGlobals.RACE_HALF_ORC
                || race == LabLordGlobals.RACE_HUMAN)
            {
                if (pc.GetBaseAttributeScore("DEX") >= 12)
                {
                    validClasses += LabLordGlobals.CLASS_THIEF;
                }
            }
            return validClasses;
        }
        public int GetValidRaces()
        {
            int validRaces = 0;
            LabLordInteractiveObject playerIo = (LabLordInteractiveObject)Interactive.Instance.GetIO(0);
            LabLordCharacter pc = (LabLordCharacter)playerIo.PcData;
            if (pc.GetBaseAttributeScore("CON") >= 9)
            {
                if (GetValidClassesForRace(LabLordGlobals.RACE_DWARF) > 0)
                {
                    validRaces += LabLordGlobals.RACE_DWARF;
                }
                if (GetValidClassesForRace(LabLordGlobals.RACE_HALF_ORC) > 0)
                {
                    validRaces += LabLordGlobals.RACE_HALF_ORC;
                }
            }
            if (pc.GetBaseAttributeScore("INT") >= 9)
            {
                if (GetValidClassesForRace(LabLordGlobals.RACE_ELF) > 0)
                {
                    validRaces += LabLordGlobals.RACE_ELF;
                }
            }
            if (pc.GetBaseAttributeScore("DEX") >= 8
                && pc.GetBaseAttributeScore("CON") >= 9)
            {
                if (GetValidClassesForRace(LabLordGlobals.RACE_GNOME) > 0)
                {
                    validRaces += LabLordGlobals.RACE_GNOME;
                }
            }
            if (pc.GetBaseAttributeScore("DEX") >= 9
                && pc.GetBaseAttributeScore("CON") >= 9)
            {
                if (GetValidClassesForRace(LabLordGlobals.RACE_HALFLING) > 0)
                {
                    validRaces += LabLordGlobals.RACE_HALFLING;
                }
            }
            if (GetValidClassesForRace(LabLordGlobals.RACE_HALF_ELF) > 0)
            {
                validRaces += LabLordGlobals.RACE_HALF_ELF;
            }
            validRaces += LabLordGlobals.RACE_HUMAN;
            return validRaces;
        }
        bool doonce;
        // Update is called once per frame
        void Update()
        {
        }
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
