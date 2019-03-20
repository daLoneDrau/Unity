using LabLord.Constants;
using LabLord.Flyweights;
using LabLord.Scriptables.Items.Weapons;
using LabLord.Singletons;
using LabLord.UI.SceneControllers.CharWizard;
using LabLord.UI.Widgets;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Scripts.UI.SimpleJSON;
using RPGBase.Singletons;
using RPGBase.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace LabLord.UI.SceneControllers.LabLord
{
    public class LlStepWizard : Singleton<LlStepWizard>, IWatcher
    {
        /// <summary>
        /// flag indicating whether debugging is on.
        /// </summary>
        public bool Debug;
        #region WIZARD STEP CONSTANTS
        /// <summary>
        /// The Gender step.
        /// </summary>
        public const int STEP_0_GENDER = 0;
        /// <summary>
        /// The Attributes step.
        /// </summary>
        public const int STEP_1_ATTRIBUTES = 1;
        /// <summary>
        /// The Race step.
        /// </summary>
        public const int STEP_2_RACE = 2;
        /// <summary>
        /// The Class step.
        /// </summary>
        public const int STEP_3_CLASS = 3;
        /// <summary>
        /// The Age step.
        /// </summary>
        public const int STEP_4_AGE = 4;
        /// <summary>
        /// The Equipment step.
        /// </summary>
        public const int STEP_5_EQUIPMENT = 5;
        /// <summary>
        /// The Spells step.
        /// </summary>
        public const int STEP_6_SPELLS = 6;
        /// <summary>
        /// the index of the dialog spacer element.
        /// </summary>
        public const int SPACER = 7;
        #endregion
        /// <summary>
        /// the current step the wizard is displaying.
        /// </summary>
        public int CurrentStep = STEP_0_GENDER;
        /// <summary>
        /// Callback delegate.
        /// </summary>
        public delegate void Callback();
        #region UI ELEMENT FIELDS
        /// <summary>
        /// the list of navigation menu columns.
        /// </summary>
        public Text[] NavigationColumns;
        /// <summary>
        /// the list of values displayed underneath the navigation columns.
        /// </summary>
        public GameObject[] NavigationValues;
        /// <summary>
        /// the title field for the selection dialog.
        /// </summary>
        public Text DialogTitle;
        /// <summary>
        /// the list of dialog panels to display for each step.
        /// </summary>
        public GameObject[] DialogPanels;
        /// <summary>
        /// The list of dialog buttons (PREV and NEXT).
        /// </summary>
        public Button[] DialogButtons;
        /// <summary>
        /// the tooltip area for steps 0-4.
        /// </summary>
        public GameObject TooltipArea1;
        /// <summary>
        /// the Shop Controller.
        /// </summary>
        public ShopController ShopController;
        #endregion
        #region STEP PARAMETER FIELDS (gender, age, etc...)
        /// <summary>
        /// the character's abilities.
        /// </summary>
        private int[] abilities;
        /// <summary>
        /// the character's age.
        /// </summary>
        private int age;
        /// <summary>
        /// the character's age.
        /// </summary>
        private int clazz = -1;
        /// <summary>
        /// the character's gender.
        /// </summary>
        private int gender;
        /// <summary>
        /// the character's race.
        /// </summary>
        private int race;
        #endregion
        /// <summary>
        /// a utility class for validating that a newly-created character can join at least one class.
        /// </summary>
        private CharacterValidator characterValidator = new CharacterValidator();
        #region MONOBEHAVIOUR messages
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake() { }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        void Start()
        {
            LabLordCharacter playerPC = (LabLordCharacter)LabLordWizardController.Instance.Player.GetComponent<LabLordInteractiveObject>().PcData;
            playerPC.AddWatcher(this);
            playerPC.AddWatcher(ShopController);
            ResetWizard();
        }
        #endregion
        #region SELECTION ASSIGNMENTS
        /// <summary>
        /// Assigns the character's Abilities.
        /// </summary>
        private void AssignAttributes()
        {
            print("assign abilities");
            LabLordCharacter playerPC = (LabLordCharacter)LabLordWizardController.Instance.Player.GetComponent<LabLordInteractiveObject>().PcData;
            bool needsAnimation = playerPC.GetBaseAttributeScore("STR") == 0;
            playerPC.SetBaseAttributeScore("STR", abilities[0]);
            playerPC.SetBaseAttributeScore("DEX", abilities[1]);
            playerPC.SetBaseAttributeScore("CON", abilities[2]);
            playerPC.SetBaseAttributeScore("INT", abilities[3]);
            playerPC.SetBaseAttributeScore("WIS", abilities[4]);
            playerPC.SetBaseAttributeScore("CHA", abilities[5]);
            #region RE-ASSIGN CHARACTERISTICS
            age = -1;
            playerPC.Age = -1;
            clazz = LabLordClass.CLASS_UNASSIGNED;
            playerPC.Clazz = LabLordClass.CLASS_UNASSIGNED;
            race = LabLordRace.RACE_UNASSIGNED;
            playerPC.Race = LabLordRace.RACE_UNASSIGNED;
            #endregion
            // reset race and class
            playerPC = null;
            CompleteStep(needsAnimation);
        }
        /// <summary>
        /// Assigns the character's AGE.
        /// </summary>
        private void AssignAge()
        {
            print("assign age");
            LabLordCharacter pc = (LabLordCharacter)LabLordWizardController.Instance.Player.GetComponent<LabLordInteractiveObject>().PcData;
            bool needsAnimation = false;
            if (pc.Age == -1)
            {
                needsAnimation = true;
            }
            pc.Age = age;
            if (pc.Gold == 0)
            {
                // give gold again
                pc.AdjustGold(Diceroller.Instance.RollXdY(3, 8) * 1000);
            }
            #region RE-ASSIGN CHARACTERISTICS
            // nothing to re-assign
            #endregion
            CompleteStep(needsAnimation);
        }
        /// <summary>
        /// Assigns the character's Class.
        /// </summary>
        private void AssignClass()
        {
            print("assignclass");
            LabLordCharacter pc = (LabLordCharacter)LabLordWizardController.Instance.Player.GetComponent<LabLordInteractiveObject>().PcData;
            bool needsAnimation = false;
            if (pc.Clazz < 0)
            {
                needsAnimation = true;
            }
            for (int clazz = DialogPanels[CurrentStep].transform.childCount - 1; clazz >= 0; clazz--)
            {
                if (Debug)
                {
                    print("check clazz " + LabLordClass.Classes[clazz].Title);
                }
                if (DialogPanels[CurrentStep].transform.GetChild(clazz).GetComponent<Toggle>().isOn)
                {
                    if (Debug)
                    {
                        print("found clazz - computing stats");
                    }
                    pc.Clazz = clazz;
                    pc.ComputeFullStats();
                    if (Debug)
                    {
                        print("done computing stats");
                    }
                    #region SET HIT POINTS
                    int hp = 0;
                    do
                    {
                        hp = LabLordClass.Classes[pc.Clazz].RollHpForNewLevel();
                    } while (hp <= 2);
                    // if HP is less than CON modifier, HP must be at least 1
                    if (hp + (int)pc.GetAttributeModifier("MHP") <= 0)
                    {
                        hp = Math.Abs((int)pc.GetAttributeModifier("MHP")) + 1;
                    }
                    pc.SetBaseAttributeScore("HP", hp);
                    pc.SetBaseAttributeScore("MHP", hp);
                    pc.HealPlayer(999, true);
                    #endregion
                    #region SET THAC0
                    pc.SetBaseAttributeScore("THM", -LabLordClass.Classes[clazz].GetTHAC0ForLevel(1));
                    #endregion
                    #region SET AC
                    pc.SetBaseAttributeScore("AC", 9);
                    #endregion
                    #region SET DMG (unarmed DMG is 1-2)
                    pc.Io.Script.SetLocalVariable("DMG", new int[] { 1, 2 });
                    #endregion
                    #region SET SAVING THROWS
                    for (int save = LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS; save >= LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH; save--)
                    {
                        string abbr = pc.GetAttributeAbbreviation(save);
                        pc.SetBaseAttributeScore(abbr,
                            -LabLordClass.Classes[clazz].GetSavingThrowForLevel(save, 1));
                        abbr = null;
                    }
                    #endregion
                    #region RE-ASSIGN CHARACTERISTICS
                    age = -1;
                    pc.Age = -1;
                    pc.AdjustGold(-pc.Gold);
                    #endregion
                    break;
                }
            }
            CompleteStep(needsAnimation);
        }
        /// <summary>
        /// Assigns the character's gender.
        /// </summary>
        private void AssignGender()
        {
            print("assign gender");
            LabLordInteractiveObject playerIo = LabLordWizardController.Instance.Player.GetComponent<LabLordInteractiveObject>();
            bool needsAnimation = false;
            if (playerIo.PcData.Gender == RPGBase.Constants.Gender.GENDER_NEUTRAL)
            {
                needsAnimation = true;
            }
            bool male = DialogPanels[CurrentStep].transform.GetChild(0).GetComponent<Toggle>().isOn;
            if (male)
            {
                playerIo.PcData.Gender = RPGBase.Constants.Gender.GENDER_MALE;
                gender = RPGBase.Constants.Gender.GENDER_MALE;
            }
            else
            {
                playerIo.PcData.Gender = RPGBase.Constants.Gender.GENDER_FEMALE;
                gender = RPGBase.Constants.Gender.GENDER_FEMALE;
            }
            #region RE-ASSIGN CHARACTERISTICS
            // nothing to re-assign
            #endregion
            CompleteStep(needsAnimation);
        }
        /// <summary>
        /// Assigns the character's Race.
        /// </summary>
        private void AssignRace()
        {
            LabLordCharacter pc = (LabLordCharacter)LabLordWizardController.Instance.Player.GetComponent<LabLordInteractiveObject>().PcData;
            bool needsAnimation = false;
            if (pc.Race < 0)
            {
                needsAnimation = true;
            }
            for (int i = DialogPanels[CurrentStep].transform.childCount - 1; i >= 0; i--)
            {
                if (DialogPanels[CurrentStep].transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    pc.Race = i;
                    // re-initialize validation matrix
                    if (Debug)
                    {
                        print("call init from assignrace");
                    }
                    characterValidator.InitMatrix();
                    characterValidator.CompleteMatrix(pc.Gender, abilities);
                    string[] abbr = new string[] { "STR", "DEX", "CON", "INT", "WIS", "CHA" };
                    for (int x = abbr.Length - 1; x >= 0; x--)
                    {
                        // adjust base scores based on Racial min/maxes
                        LabLordRace raceObj = LabLordRace.Races[i];
                        int min = raceObj.Requirements[x][pc.Gender, 0];
                        int max = raceObj.Requirements[x][pc.Gender, 1];
                        int mod = (int)(pc.GetFullAttributeScore(abbr[x]) - max);
                        if (mod > 0)
                        {
                            if (Debug)
                            {
                                print("ability " + abbr[x] + " too high. modify by " + mod);
                            }
                            // ability score higher than allowed
                            pc.SetBaseAttributeScore(abbr[x],
                                pc.GetBaseAttributeScore(abbr[x]) - mod);
                        }
                    }
                    #region RE-ASSIGN CHARACTERISTICS
                    age = -1;
                    pc.Age = -1;
                    clazz = LabLordClass.CLASS_UNASSIGNED;
                    pc.Clazz = LabLordClass.CLASS_UNASSIGNED;
                    pc.AdjustGold(-pc.Gold);
                    #endregion
                    break;
                }
            }
            // reset class
            CompleteStep(needsAnimation);
        }
        /// <summary>
        /// Completes the current Wizard step.
        /// </summary>
        /// <param name="needsAnimation">if true, the current step needs to play an animation</param>
        private void CompleteStep(bool needsAnimation)
        {
            if (needsAnimation)
            {
                NavigationValues[CurrentStep].GetComponent<Animator>().Play("Moving");
            }
        }
        #endregion
        #region CHARACTER SELECTIONS
        public void NextStep()
        {
            switch (CurrentStep)
            {
                case STEP_0_GENDER:
                    print("moving on from gender");
                    AssignGender();
                    break;
                case STEP_1_ATTRIBUTES:
                    print("moving on from attributes");
                    AssignAttributes();
                    break;
                case STEP_2_RACE:
                    print("moving on from race");
                    AssignRace();
                    break;
                case STEP_3_CLASS:
                    print("moving on from class");
                    AssignClass();
                    break;
                case STEP_4_AGE:
                    print("moving on from age");
                    AssignAge();
                    break;
            }
            CurrentStep++;
            ResetWizard();
        }
        public void PrevStep()
        {
            CurrentStep--;
            ResetWizard();
        }
        public void RollAge()
        {
            LabLordCharacter pc = (LabLordCharacter)LabLordWizardController.Instance.Player.GetComponent<LabLordInteractiveObject>().PcData;
            age = characterValidator.NewAge(pc.Race, pc.Clazz);
            #region RE-ASSIGN CHARACTERISTICS
            age = -1;
            pc.Age = -1;
            #endregion
            SetAgeDialog(pc);
            pc = null;
        }
        /// <summary>
        /// Rolls a new set of ability scores.
        /// </summary>
        public void RollAbilities()
        {
            print("RollAbilities");
            LabLordCharacter pc = (LabLordCharacter)LabLordWizardController.Instance.Player.GetComponent<LabLordInteractiveObject>().PcData;
            abilities = characterValidator.NewAbilities(pc.Gender);
            #region RE-ASSIGN CHARACTERISTICS
            age = -1;
            pc.Age = -1;
            clazz = LabLordClass.CLASS_UNASSIGNED;
            pc.Clazz = LabLordClass.CLASS_UNASSIGNED;
            race = LabLordRace.RACE_UNASSIGNED;
            pc.Race = LabLordRace.RACE_UNASSIGNED;
            pc.SetBaseAttributeScore("STR", 0);
            pc.SetBaseAttributeScore("DEX", 0);
            pc.SetBaseAttributeScore("CON", 0);
            pc.SetBaseAttributeScore("INT", 0);
            pc.SetBaseAttributeScore("WIS", 0);
            pc.SetBaseAttributeScore("CHA", 0);
            #endregion
            // display ability scores
            for (int i = abilities.Length - 1; i >= 0; i--)
            {
                if (Debug)
                {
                    print("display ability " + i + "::" + abilities[i]);
                }
                DialogPanels[CurrentStep].transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = abilities[i].ToString();
            }
            pc = null;
        }
        #endregion
        /// <summary>
        /// Resets the wizard based on the current state.
        /// </summary>
        private void ResetWizard()
        {
            DialogButtons[0].interactable = false;
            DialogButtons[1].interactable = false;
            ResetColumns();
            SetWizard();
        }
        /// <summary>
        /// Resets all wizard navigation menu columns based on the current state.
        /// </summary>
        private void ResetColumns()
        {
            for (int i = NavigationColumns.Length - 1; i >= 0; i--)
            {
                NavigationColumns[i].color = Color.grey;
            }
            NavigationColumns[CurrentStep].color = Color.white;
            for (int i = DialogPanels.Length - 1; i >= 0; i--)
            {
                if (DialogPanels[i] != null)
                {
                    DialogPanels[i].SetActive(false);
                }
            }
        }
        /// <summary>
        /// Indicates a selection has been made.
        /// </summary>
        public void SelectionMade()
        {
            DialogButtons[1].interactable = true;
        }
        #region WIZARD DIALOG SETUP
        /// <summary>
        /// Sets the Abilities dialog.
        /// </summary>
        /// <param name="pc">the character data</param>
        private void SetAbilitiesDialog(LabLordCharacter pc)
        {
            // enable PREV/NEXT buttons always
            DialogButtons[0].interactable = true;
            DialogButtons[1].interactable = true;
            // if abilities were never rolled, get them
            if (abilities == null)
            {
                abilities = characterValidator.NewAbilities(pc.Gender);
            }
            // display ability scores
            for (int i = abilities.Length - 1; i >= 0; i--)
            {
                DialogPanels[CurrentStep].transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = abilities[i].ToString();
            }
            // show tooltip
            TooltipArea1.SetActive(true);
        }
        /// <summary>
        /// Sets the Age dialog.
        /// </summary>
        /// <param name="pc">the character data</param>
        private void SetAgeDialog(LabLordCharacter pc)
        {
            // enable dialog buttons
            DialogButtons[0].interactable = true;
            DialogButtons[1].interactable = true;
            // if age was never rolled, get it
            if (age <= 0)
            {
                age = characterValidator.NewAge(pc.Race, pc.Clazz);
            }
            // display age
            DialogPanels[CurrentStep].transform.GetChild(0).GetComponent<Text>().text = age.ToString();
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(LabLordAge.AGE_TITLE[LabLordAge.GetAgeRank(pc.Race, age)]);
            int[] modArr = LabLordAge.ABILITY_MODIFIERS[LabLordAge.GetAgeRank(pc.Race, age)];
            sb.Append("\n");
            bool atLeastOne = false;
            for (int i = 0, li = modArr.Length; i < li; i++)
            {
                if (modArr[i] != 0) { atLeastOne = true; break; }
            }
            if (atLeastOne)
            {
                string redStart = "<color=red>", redStop = "</color>";
                for (int i = 0, li = modArr.Length; i < li; i++)
                {
                    if (modArr[i] == 0) { continue; }
                    sb.Append(LabLordAbility.Abilities[i].Abbr);
                    sb.Append("\t");
                    if (modArr[i] > 0)
                    {
                        sb.Append("+");
                    }
                    if (modArr[i] < 0)
                    {
                        sb.Append(redStart);
                    }
                    sb.Append(modArr[i]);
                    if (modArr[i] < 0)
                    {
                        sb.Append(redStop);
                    }
                    sb.Append("\n");
                }
            }
            DialogPanels[CurrentStep].transform.GetChild(1).GetComponent<Text>().text = sb.ToString();
            sb.ReturnToPool();
            sb = null;
            if (pc.Age >= 0)
            {
                // set AGE in Nav Menu
                NavigationValues[CurrentStep].transform.GetChild(0).GetComponent<Text>().text = pc.Age.ToString();
            }
            // hide tooltip
            TooltipArea1.SetActive(false);
        }
        /// <summary>
        /// Sets the Class dialog.
        /// </summary>
        /// <param name="pc">the character data</param>
        private void SetClassDialog(LabLordCharacter pc)
        {
            // enable/disable dialog buttons
            DialogButtons[0].interactable = true;
            DialogButtons[1].interactable = false;
            // enable available Class toggles
            for (int clazz = LabLordClass.NUM_CLASSES - 1; clazz >= 0; clazz--)
            {
                if (characterValidator.CheckClass(pc.Race, clazz))
                {
                    DialogPanels[CurrentStep].transform.GetChild(clazz).GetComponent<Toggle>().interactable = true;
                }
                else
                {
                    DialogPanels[CurrentStep].transform.GetChild(clazz).GetComponent<Toggle>().interactable = false;
                }
                // turn off all toggles
                DialogPanels[CurrentStep].transform.GetChild(clazz).GetComponent<Toggle>().isOn = false;
            }
            if (pc.Clazz >= 0)
            {
                // toggle player's set CLASS
                DialogPanels[CurrentStep].transform.GetChild(pc.Clazz).GetComponent<Toggle>().isOn = true;
                // set CLASS in Nav Menu
                NavigationValues[CurrentStep].transform.GetChild(0).GetComponent<Text>().text = LabLordClass.Classes[pc.Clazz].Title;
                // enable NEXT button
                DialogButtons[1].interactable = true;
            }
            // show tooltip
            TooltipArea1.SetActive(true);
        }
        /// <summary>
        /// Sets the Equipment dialog.
        /// </summary>
        /// <param name="pc"></param>
        /// <returns></returns>
        private Callback SetEquipmentDialog(LabLordCharacter pc)
        {
            return ShopController.Setup(pc);
        }
        /// <summary>
        /// Sets the Gender dialog.  If the player has an assigned gender, then that is toggled, other no Gender is toggled.
        /// </summary>
        /// <param name="pc">the character data</param>
        private void SetGenderDialog(LabLordCharacter pc)
        {
            // disable dialog buttons
            DialogButtons[0].interactable = false;
            DialogButtons[1].interactable = false;
            if (pc.Gender != RPGBase.Constants.Gender.GENDER_NEUTRAL)
            {
                DialogPanels[CurrentStep].transform.GetChild(pc.Gender).GetComponent<Toggle>().isOn = true;
                // show selected player gender in navigation
                NavigationValues[CurrentStep].transform.GetChild(0).GetComponent<Text>().text = RPGBase.Constants.Gender.GENDERS[pc.Gender];
                // enable NEXT button
                DialogButtons[1].interactable = true;
            }
            // hide tooltip
            TooltipArea1.SetActive(false);
        }
        /// <summary>
        /// Sets the Race dialog.  If the player has an assigned Race, then that is toggled, other no Race is toggled.
        /// </summary>
        /// <param name="pc">the character data</param>
        private void SetRaceDialog(LabLordCharacter pc)
        {
            // disable dialog buttons
            DialogButtons[0].interactable = true;
            DialogButtons[1].interactable = false;
            // enable available race toggles
            for (int race = LabLordRace.NUM_RACES - 1; race >= 0; race--)
            {
                if (characterValidator.CheckRace(race))
                {
                    DialogPanels[CurrentStep].transform.GetChild(race).GetComponent<Toggle>().interactable = true;
                }
                else
                {
                    DialogPanels[CurrentStep].transform.GetChild(race).GetComponent<Toggle>().interactable = false;
                }
                // turn off all toggles
                DialogPanels[CurrentStep].transform.GetChild(race).GetComponent<Toggle>().isOn = false;
            }
            if (pc.Race >= 0)
            {
                // toggle player's set Race
                DialogPanels[CurrentStep].transform.GetChild(pc.Race).GetComponent<Toggle>().isOn = true;
                // set Race in Nav Menu
                NavigationValues[CurrentStep].transform.GetChild(0).GetComponent<Text>().text = LabLordRace.Races[pc.Race].Title;
                // enable NEXT button
                DialogButtons[1].interactable = true;
            }
            // show tooltip
            TooltipArea1.SetActive(true);
        }
        private void SetWizard()
        {
            // top columns already set.
            // set correct dialog title
            Callback c = null;
            string s = "";
            LabLordCharacter pc = (LabLordCharacter)LabLordWizardController.Instance.Player.GetComponent<LabLordInteractiveObject>().PcData;
            switch (CurrentStep)
            {
                case STEP_0_GENDER:
                    // dialog title should be gender
                    s = "GENDER";
                    SetGenderDialog(pc);
                    break;
                case STEP_1_ATTRIBUTES:
                    // dialog title should be attributes
                    s = "ABILITIES";
                    SetAbilitiesDialog(pc);
                    break;
                case STEP_2_RACE:
                    // dialog title should be race
                    s = "RACE";
                    SetRaceDialog(pc);
                    break;
                case STEP_3_CLASS:
                    // dialog title should be CLASS
                    s = "CLASS";
                    SetClassDialog(pc);
                    break;
                case STEP_4_AGE:
                    // dialog title should be attributes
                    s = "AGE";
                    SetAgeDialog(pc);
                    break;
                case STEP_5_EQUIPMENT:
                    // dialog title should be attributes
                    s = "SHOP";
                    c = SetEquipmentDialog(pc);
                    break;
            }
            DialogTitle.text = s;
            DialogPanels[CurrentStep].SetActive(true);
            if (c != null)
            {
                c();
            }
        }
        #endregion
        #region DIALOG TOOLTIPS
        /// <summary>
        /// Gets the tooltip description for a specific ability score.
        /// </summary>
        /// <param name="parameters">the ability</param>
        /// <returns></returns>
        public string AttributeScoreDecription(string parameters)
        {
            int attribute = Int32.Parse(parameters);
            string s = "";
            switch (attribute)
            {
                case 0:
                    s = LabLordAbility.GetStrengthDescription(abilities[attribute]);
                    break;
                case 1:
                    s = LabLordAbility.GetDexterityDescription(abilities[attribute]);
                    break;
                case 2:
                    s = LabLordAbility.GetConstitutionDescription(abilities[attribute]);
                    break;
                case 3:
                    s = LabLordAbility.GetIntelligenceDescription(abilities[attribute]);
                    break;
                case 4:
                    s = LabLordAbility.GetWisdomDescription(abilities[attribute]);
                    break;
                case 5:
                    s = LabLordAbility.GetCharismaDescription(abilities[attribute]);
                    break;
            }
            return s;
        }
        public string ClassDescription(string parameters)
        {
            int clazz = Int32.Parse(parameters);
            int race = ((LabLordCharacter)LabLordWizardController.Instance.Player.GetComponent<LabLordInteractiveObject>().PcData).Race;
            return LabLordClass.Classes[clazz].GetDescription(abilities, race, DialogPanels[CurrentStep].transform.GetChild(clazz).GetComponent<Toggle>().interactable);
        }
        public string RaceDescription(string parameters)
        {
            int race = Int32.Parse(parameters);
            return LabLordRace.Races[race].GetTooltipDescription(gender, abilities, DialogPanels[CurrentStep].transform.GetChild(race).GetComponent<Toggle>().interactable);
        }
        #endregion
        public void WatchUpdated(Watchable data)
        {
            LabLordCharacter playerPC = (LabLordCharacter)data;
            playerPC.ComputeFullStats();
            if (playerPC.Gender != RPGBase.Constants.Gender.GENDER_NEUTRAL)
            {
                NavigationValues[STEP_0_GENDER].transform.GetChild(0).GetComponent<Text>().text = RPGBase.Constants.Gender.GENDERS[playerPC.Gender];
            }
            if (playerPC.GetBaseAttributeScore("STR") > 0)
            {
                for (int i = 0, li = LabLordAbility.ABILITY_CHA; i <= li; i++)
                {
                    NavigationValues[STEP_1_ATTRIBUTES].transform.GetChild(1).transform.GetChild(i).GetComponent<Text>().text = abilities[i].ToString();
                }
            }
            else
            {
                NavigationValues[STEP_1_ATTRIBUTES].GetComponent<Animator>().Play("Hidden");
            }
            if (playerPC.Race >= 0)
            {
                NavigationValues[STEP_2_RACE].transform.GetChild(0).GetComponent<Text>().text = LabLordRace.Races[playerPC.Race].Title;
            }
            else
            {
                NavigationValues[STEP_2_RACE].transform.GetChild(0).GetComponent<Text>().text = "";
                NavigationValues[STEP_2_RACE].GetComponent<Animator>().Play("Hidden");
            }
            if (playerPC.Clazz >= 0)
            {
                NavigationValues[STEP_3_CLASS].transform.GetChild(0).GetComponent<Text>().text = LabLordClass.Classes[playerPC.Clazz].Title;
            }
            else
            {
                NavigationValues[STEP_3_CLASS].transform.GetChild(0).GetComponent<Text>().text = "";
                NavigationValues[STEP_3_CLASS].GetComponent<Animator>().Play("Hidden");
            }
        }
    }
}
