using LabLord.Constants;
using LabLord.Flyweights;
using LabLord.Singletons;
using RPGBase.Flyweights;
using RPGBase.Pooled;
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
    public class LabLordCharacterSheetController : Singleton<LabLordCharacterSheetController>, IWatcher
    {
        #region UI ELEMENTS
        /// <summary>
        /// the container for Ability Scores.
        /// </summary>
        public GameObject AbilityScores;
        /// <summary>
        /// the container for Derived Scores.
        /// </summary>
        public GameObject DerivedScores;
        #endregion
        #region MonoBehaviour messages
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        void Start()
        {
            LabLordCharacter playerPC = (LabLordCharacter)LabLordWizardController.Instance.Player.GetComponent<LabLordInteractiveObject>().PcData;
            playerPC.AddWatcher(this);
        }
        #endregion
        #region SHEET ADJUSTMENTS
        private void SetAbilities(LabLordCharacter playerPC)
        {
            // clear scores
            for (int ability = LabLordGlobals.EQUIP_ELEMENT_CHA; ability >= LabLordAbility.ABILITY_STR; ability--)
            {
                Transform abilityParent = AbilityScores.transform.GetChild(4)
                    .transform.GetChild(0)
                    .transform.GetChild(ability);
                Text abilityLabel = AbilityScores.transform.GetChild(4)
                    .transform.GetChild(0)
                    .transform.GetChild(ability)
                    .transform.GetChild(0).GetComponent<Text>();
                Text abilityValue = AbilityScores.transform.GetChild(4)
                    .transform.GetChild(0)
                    .transform.GetChild(ability)
                    .transform.GetChild(1).GetComponent<Text>();
                abilityLabel.text = "";
                abilityValue.text = "";
                abilityParent.GetComponent<InteractiveTooltipWidget>().TooltipText = "";
                abilityLabel = null;
                abilityValue = null;
            }
            Text modifierLabel = AbilityScores.transform.GetChild(4)
                .transform.GetChild(1)
                .transform.GetChild(0).GetComponent<Text>();
            modifierLabel.text = "";
            // if abilities have been assigned, fill in sheet
            if (playerPC.GetBaseAttributeScore("STR") > 0)
            {
                for (int ability = LabLordGlobals.EQUIP_ELEMENT_CHA; ability >= LabLordAbility.ABILITY_STR; ability--)
                {
                    Transform abilityParent = AbilityScores.transform.GetChild(4)
                        .transform.GetChild(0)
                        .transform.GetChild(ability);
                    Text abilityLabel = AbilityScores.transform.GetChild(4)
                        .transform.GetChild(0)
                        .transform.GetChild(ability)
                        .transform.GetChild(0).GetComponent<Text>();
                    Text abilityValue = AbilityScores.transform.GetChild(4)
                        .transform.GetChild(0)
                        .transform.GetChild(ability)
                        .transform.GetChild(1).GetComponent<Text>();
                    abilityLabel.text = LabLordAbility.Abilities[ability].Abbr;
                    abilityValue.text = ((int)playerPC.GetFullAttributeScore(LabLordAbility.Abilities[ability].Abbr)).ToString();
                    abilityParent.GetComponent<InteractiveTooltipWidget>().TooltipText = LabLordAbility.Abilities[ability].GetAbilityModifierTextForCharacterSheet(playerPC);

                    abilityLabel = null;
                    abilityValue = null;
                }
            }
        }
        /// <summary>
        /// Sets the Gender and Race section of the character sheet.
        /// </summary>
        /// <param name="playerPC">the PC data</param>
        private void SetGender(LabLordCharacter playerPC)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            if (playerPC.Gender != RPGBase.Constants.Gender.GENDER_NEUTRAL)
            {
                sb.Append(RPGBase.Constants.Gender.GENDERS[playerPC.Gender]);
            }
            if (playerPC.Race >= 0)
            {
                sb.Append(" ");
                sb.Append(LabLordRace.Races[playerPC.Race].Title);
            }
            AbilityScores.transform.GetChild(0).GetComponent<Text>().text = sb.ToString();
            sb.ReturnToPool();
        }
        /// <summary>
        /// Sets the Age section of the character sheet.
        /// </summary>
        /// <param name="playerPC">the PC data</param>
        private void SetAge(LabLordCharacter playerPC)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            if (playerPC.Age >= 0)
            {
                sb.Append(playerPC.Age);
                sb.Append(" years old (");
                sb.Append(LabLordAge.AGE_TITLE[LabLordAge.GetAgeRank(playerPC.Race, playerPC.Age)]);
                sb.Append(")");
            }
            AbilityScores.transform.GetChild(1).GetComponent<Text>().text = sb.ToString();
            sb.ReturnToPool();
        }
        private void SetDerivedScores(LabLordCharacter playerPC)
        {
            Text hpLabel = DerivedScores.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
            Text hpValue = DerivedScores.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
            Text dmgLabel = DerivedScores.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>();
            Text dmgValue = DerivedScores.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>();
            Text thac0Label = DerivedScores.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>();
            Text thac0Value = DerivedScores.transform.GetChild(4).transform.GetChild(1).GetComponent<Text>();
            Text acLabel = DerivedScores.transform.GetChild(6).transform.GetChild(0).GetComponent<Text>();
            Text acValue = DerivedScores.transform.GetChild(6).transform.GetChild(1).GetComponent<Text>();
            hpLabel.text = "";
            hpValue.text = "";
            dmgLabel.text = "";
            dmgValue.text = "";
            thac0Label.text = "";
            thac0Value.text = "";
            acLabel.text = "";
            acValue.text = "";
            // SAVES
            DerivedScores.transform.GetChild(8).GetComponent<Text>().text = "";
            int index = 9;
            DerivedScores.transform.GetChild(index).transform.GetChild(0).GetComponent<Text>().text = "";
            DerivedScores.transform.GetChild(index++).transform.GetChild(1).GetComponent<Text>().text = "";
            DerivedScores.transform.GetChild(index).transform.GetChild(0).GetComponent<Text>().text = "";
            DerivedScores.transform.GetChild(index++).transform.GetChild(1).GetComponent<Text>().text = "";
            DerivedScores.transform.GetChild(index).transform.GetChild(0).GetComponent<Text>().text = "";
            DerivedScores.transform.GetChild(index++).transform.GetChild(1).GetComponent<Text>().text = "";
            DerivedScores.transform.GetChild(index).transform.GetChild(0).GetComponent<Text>().text = "";
            DerivedScores.transform.GetChild(index++).transform.GetChild(1).GetComponent<Text>().text = "";
            DerivedScores.transform.GetChild(index).transform.GetChild(0).GetComponent<Text>().text = "";
            DerivedScores.transform.GetChild(index++).transform.GetChild(1).GetComponent<Text>().text = "";
            DerivedScores.transform.GetChild(index).transform.GetChild(0).GetComponent<Text>().text = "";
            DerivedScores.transform.GetChild(index++).transform.GetChild(1).GetComponent<Text>().text = "";
            DerivedScores.transform.GetChild(index).transform.GetChild(0).GetComponent<Text>().text = "";
            DerivedScores.transform.GetChild(index++).transform.GetChild(1).GetComponent<Text>().text = "";
            if (playerPC.Clazz >= 0)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                // HP
                hpLabel.text = "Hit Points";
                sb.Append(playerPC.Life);
                sb.Append("/");
                sb.Append(playerPC.GetMaxLife());
                hpValue.text = sb.ToString();
                sb.Length = 0;
                // DAMAGE
                if (playerPC.Io.Script.HasLocalVariable("DMG"))
                {
                    dmgLabel.text = "Damage";
                    int[] damages = playerPC.Io.Script.GetLocalIntArrayVariableValue("DMG");
                    sb.Append(Math.Max(1, damages[0] + playerPC.GetFullAttributeScore("DMG")));
                    sb.Append("-");
                    sb.Append(Math.Max(1, damages[1] + playerPC.GetFullAttributeScore("DMG")));
                    dmgValue.text = sb.ToString();
                    sb.Length = 0;
                }
                // THAC0
                thac0Label.text = "THAC0";
                thac0Value.text = (Math.Abs
                    ((int)playerPC.GetFullAttributeScore("THM"))).ToString();
                sb.Length = 0;
                // AC
                acLabel.text = "Armour Class";
                acValue.text = ((int)playerPC.GetFullAttributeScore("AC")).ToString();
                // SAVES
                DerivedScores.transform.GetChild(8).GetComponent<Text>().text = "Saving Throws";
                index = 9;
                int score = 0;
                DerivedScores.transform.GetChild(index).transform.GetChild(1).GetComponent<Text>().text = "vs Breath Attacks";
                score = (Math.Abs
                        ((int)playerPC.GetFullAttributeScore(playerPC.GetAttributeAbbreviation(LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH))));
                if (score < 10)
                {
                    sb.Append(" ");
                }
                sb.Append(score);
                DerivedScores.transform.GetChild(index++).transform.GetChild(0).GetComponent<Text>().text = sb.ToString();
                sb.Length = 0;

                DerivedScores.transform.GetChild(index).transform.GetChild(1).GetComponent<Text>().text = "vs Poison";
                score = (Math.Abs
                        ((int)playerPC.GetFullAttributeScore(playerPC.GetAttributeAbbreviation(LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON))));
                if (score < 10)
                {
                    sb.Append(" ");
                }
                sb.Append(score);
                DerivedScores.transform.GetChild(index++).transform.GetChild(0).GetComponent<Text>().text = sb.ToString();

                DerivedScores.transform.GetChild(index).transform.GetChild(1).GetComponent<Text>().text = "vs Death";
                score = (Math.Abs
                        ((int)playerPC.GetFullAttributeScore(playerPC.GetAttributeAbbreviation(LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH))));
                if (score < 10)
                {
                    sb.Append(" ");
                }
                sb.Append(score);
                DerivedScores.transform.GetChild(index++).transform.GetChild(0).GetComponent<Text>().text = sb.ToString();

                DerivedScores.transform.GetChild(index).transform.GetChild(1).GetComponent<Text>().text = "vs Petrification";
                score = (Math.Abs
                        ((int)playerPC.GetFullAttributeScore(playerPC.GetAttributeAbbreviation(LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY))));
                if (score < 10)
                {
                    sb.Append(" ");
                }
                sb.Append(score);
                DerivedScores.transform.GetChild(index++).transform.GetChild(0).GetComponent<Text>().text = sb.ToString();

                DerivedScores.transform.GetChild(index).transform.GetChild(1).GetComponent<Text>().text = "vs Paralyzation";
                score = (Math.Abs
                        ((int)playerPC.GetFullAttributeScore(playerPC.GetAttributeAbbreviation(LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE))));
                if (score < 10)
                {
                    sb.Append(" ");
                }
                sb.Append(score);
                DerivedScores.transform.GetChild(index++).transform.GetChild(0).GetComponent<Text>().text = sb.ToString();

                DerivedScores.transform.GetChild(index).transform.GetChild(1).GetComponent<Text>().text = "vs Wands";
                score = (Math.Abs
                        ((int)playerPC.GetFullAttributeScore(playerPC.GetAttributeAbbreviation(LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS))));
                if (score < 10)
                {
                    sb.Append(" ");
                }
                sb.Append(score);
                DerivedScores.transform.GetChild(index++).transform.GetChild(0).GetComponent<Text>().text = sb.ToString();

                DerivedScores.transform.GetChild(index).transform.GetChild(1).GetComponent<Text>().text = "vs Spells/Spell-Like Devices";
                score = (Math.Abs
                        ((int)playerPC.GetFullAttributeScore(playerPC.GetAttributeAbbreviation(LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS))));
                if (score < 10)
                {
                    sb.Append(" ");
                }
                sb.Append(score);
                DerivedScores.transform.GetChild(index++).transform.GetChild(0).GetComponent<Text>().text = sb.ToString();

                sb.ReturnToPool();
            }
            hpLabel = null;
            hpValue = null;
            thac0Label = null;
            thac0Value = null;
            acLabel = null;
            acValue = null;
        }
        /// <summary>
        /// Sets the Class, Level, and XP section of the character sheet.
        /// </summary>
        /// <param name="playerPC">the PC data</param>
        private void SetClassLevelXP(LabLordCharacter playerPC)
        {
            // Class and Level
            AbilityScores.transform.GetChild(2).GetComponent<Text>().text = "";
            // XP
            AbilityScores.transform.GetChild(3).GetComponent<Text>().text = "";
            if (playerPC.Clazz >= 0)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                sb.Append(LabLordClass.LEVEL_TITLE[playerPC.Level - 1]);
                sb.Append(" ");
                sb.Append(LabLordClass.Classes[playerPC.Clazz].Title);
                AbilityScores.transform.GetChild(2).GetComponent<Text>().text = sb.ToString();
                sb.Length = 0;
                sb.Append("Experience: ");
                sb.Append(playerPC.Xp);
                sb.Append("/");
                sb.Append(playerPC.GetXpNeededForNextLevel());
                sb.Append(" xp");
                AbilityScores.transform.GetChild(3).GetComponent<Text>().text = sb.ToString();
                sb.ReturnToPool();
            }
        }
        #endregion
        public void WatchUpdated(Watchable data)
        {
            LabLordCharacter pc = (LabLordCharacter)data;
            pc.ComputeFullStats();
            SetGender(pc);
            SetAge(pc);
            SetClassLevelXP(pc);
            SetAbilities(pc);
            SetDerivedScores(pc);
        }
    }
}
