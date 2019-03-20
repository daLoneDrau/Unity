using RPGBase.Flyweights;
using RPGBase.Pooled;
using System;
using LabLord.Constants;
using System.Collections.Generic;
using RPGBase.Constants;
using RPGBase.Singletons;

namespace LabLord.Flyweights
{
    public class LabLordCharacter : IOPcData
    {
        /// <summary>
        /// the list of attributes and their matching names and modifiers.
        /// </summary>
        private static object[][] attributeMap = new object[][] {
            #region ABILITIES - 6
            new object[] { "STR", "Strength", LabLordGlobals.EQUIP_ELEMENT_STR },
            new object[] { "DEX", "Dexterity", LabLordGlobals.EQUIP_ELEMENT_DEX },
            new object[] { "CON", "Constitution", LabLordGlobals.EQUIP_ELEMENT_CON },
            new object[] { "INT", "Intelligence", LabLordGlobals.EQUIP_ELEMENT_INT },
            new object[] { "WIS", "Wisdom", LabLordGlobals.EQUIP_ELEMENT_WIS },
            new object[] { "CHA", "Charisma", LabLordGlobals.EQUIP_ELEMENT_CHA },
            #endregion
            #region SAVING THROWS - 7
            new object[] { "SBW", "Save vs. Breath Weapon", LabLordGlobals.EQUIP_ELEMENT_SAVE_V_BREATH },
            new object[] { "SVP", "Save vs. Poison", LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON },
            new object[] { "SVD", "Save vs. Death", LabLordGlobals.EQUIP_ELEMENT_SAVE_V_DEATH },
            new object[] { "SPT", "Save vs. Petrify", LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PETRIFY },
            new object[] { "SPR", "Save vs. Paralyze", LabLordGlobals.EQUIP_ELEMENT_SAVE_V_PARALYZE },
            new object[] { "SVW", "Save vs. Wands", LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS },
            new object[] { "SSS", "Save vs. Spell or Staff", LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS },
            #endregion
            #region THIEF SKILLS - 7
            new object[] { "TPL", "Pick Locks", LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS },
            new object[] { "FRT", "Find and Remove Traps", LabLordGlobals.EQUIP_ELEMENT_THIEF_FIND_REMOVE_TRAPS },
            new object[] { "TPP", "Pick Pockets", LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_POCKETS },
            new object[] { "TMS", "Move Silently", LabLordGlobals.EQUIP_ELEMENT_THIEF_MOVE_SILENTLY },
            new object[] { "E17", "Climb Walls", LabLordGlobals.EQUIP_ELEMENT_THIEF_CLIMB_WALLS },
            new object[] { "THS", "Hide in Shadows", LabLordGlobals.EQUIP_ELEMENT_THIEF_HIDE_SHADOWS },
            new object[] { "THN", "Hear Noise", LabLordGlobals.EQUIP_ELEMENT_THIEF_HEAR_NOISE },
            #endregion
            #region STRENGTH MODIFIERS - 3
            new object[] { "THM", "To Hit", LabLordGlobals.EQUIP_ELEMENT_THM },
            new object[] { "DMG", "Damage", LabLordGlobals.EQUIP_ELEMENT_DMG },
            new object[] { "EFD", "Force Doors", LabLordGlobals.EQUIP_ELEMENT_FORCE_DOORS },
            #endregion
            #region  DEXTERITY MODIFIERS - 3
            new object[] { "AC", "AC", LabLordGlobals.EQUIP_ELEMENT_AC },
            new object[] { "E24", "Missile Weapons", LabLordGlobals.EQUIP_ELEMENT_MISSILE_THM },
            new object[] { "E25", "Initiative", LabLordGlobals.EQUIP_ELEMENT_INITIATIVE },
            #endregion
            #region  CONSTITUTION MODIFIERS
            new object[] { "HP", "Hit Points", LabLordGlobals.EQUIP_ELEMENT_HP },
            new object[] { "MHP", "Max Hit Points", LabLordGlobals.EQUIP_ELEMENT_MHP },
            new object[] { "E28", "vs Resurrection", LabLordGlobals.EQUIP_SURVIVE_RESURRECTION },
            new object[] { "E29", "vs Shock", LabLordGlobals.EQUIP_SURVIVE_POLYMORPH },
            #endregion
            #region  INTELLIGENCE MODIFIERS
            new object[] { "E30", "Additional Languages", LabLordGlobals.EQUIP_ELEMENT_ADDL_LANGUAGES },
            new object[] { "E31", "Language Proficiency", LabLordGlobals.EQUIP_ELEMENT_LANGUAGE_PROFICIENCY },
            new object[] { "E32", "Chance Learn Spell", LabLordGlobals.EQUIP_ELEMENT_LEARN_SPELL },
            new object[] { "E33", "Min Spells/Lvl", LabLordGlobals.EQUIP_ELEMENT_MIN_SPELLS },
            new object[] { "E34", "Max Spells/Lvl", LabLordGlobals.EQUIP_ELEMENT_MAX_SPELLS },
            #endregion
            #region  WISDOM MODIFIERS
            new object[] { "E35", "Divine Spell Failure", LabLordGlobals.EQUIP_ELEMENT_DIVINE_SPELL_FAILURE },
            new object[] { "E36", "Bonus Level 1 Spells", LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_1_SPELLS },
            new object[] { "E37", "Bonus Level 2 Spells", LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_2_SPELLS },
            new object[] { "E38", "Bonus Level 3 Spells", LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_3_SPELLS },
            new object[] { "E39", "Bonus Level 4 Spells", LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_4_SPELLS },
            #endregion
            #region  CHARISMA MODIFIERS
            new object[] { "E40", "Reaction Adjustment", LabLordGlobals.EQUIP_ELEMENT_REACTION_ADJUSTMENT },
            new object[] { "E41", "Max Hirelings", LabLordGlobals.EQUIP_ELEMENT_NUM_HIRELIGNS },
            new object[] { "E42", "Retainer Morale", LabLordGlobals.EQUIP_ELEMENT_RETAINER_MORALE }
            #endregion
        };
        /// <summary>
        /// the age field.
        /// </summary>
        private int age = -1;
        /// <summary>
        /// The character's age property.
        /// </summary>
        public int Age
        {
            get { return age; }
            set
            {
                age = value;
                NotifyWatchers();
            }
        }
        /// <summary>
        /// Gets the XP needed to attain the next level.
        /// </summary>
        /// <returns></returns>
        public int GetXpNeededForNextLevel()
        {
            return LabLordClass.Classes[clazz].LevelRequirements[Level + 1];
        }
        /// <summary>
        /// the clazz field
        /// </summary>
        private int clazz = -1;
        /// <summary>
        /// The character's class.
        /// </summary>
        public int Clazz
        {
            get { return clazz; }
            set
            {
                clazz = value;
                NotifyWatchers();
            }
        }
        /// <summary>
        /// the race field.
        /// </summary>
        private int race = -1;
        /// <summary>
        /// The character's race.
        /// </summary>
        public int Race
        {
            get { return race; }
            set
            {
                race = value;
                NotifyWatchers();
            }
        }
        protected override void AdjustMana(float dmg)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Applies modifiers to the character's attributes and skills based on the game rules.
        /// </summary>
        protected override void ApplyRulesModifiers()
        {
            // apply RACIAL modifiers
            if (Race >= 0)
            {
                for (int elementType = LabLordRace.Races[Race].Modifiers.Length - 1; elementType >= 0; elementType--)
                {
                    EquipmentItemModifier mod = LabLordRace.Races[Race].Modifiers[elementType];

                    if (mod == null)
                    {
                        continue;
                    }
                    if (mod.Percent
                        || mod.PerLevel
                        || mod.Value == 0f)
                    {
                        continue;
                    }
                    AdjustAttributeModifier((string)attributeMap[elementType][0], mod.Value);
                }
            }
            // apply ABILITY modifiers
            for (int ability = LabLordGlobals.EQUIP_ELEMENT_CHA; ability >= 0; ability--)
            {
                int abilityScore = (int)GetBaseAttributeScore((string)attributeMap[ability][0]);
                Dictionary<int, EquipmentItemModifier> modifiers = LabLordAbility.Abilities[ability].GetModifiersForScore(abilityScore);
                if (modifiers != null)
                {
                    foreach (KeyValuePair<int, EquipmentItemModifier> entry in modifiers)
                    {
                        // do something with entry.Value or entry.Key
                        AdjustAttributeModifier((string)attributeMap[entry.Key][0], entry.Value.Value);
                    }
                    modifiers = null;
                }
            }
            // apply CLASS modifiers
            if (clazz >= 0)
            {
            }
            // FIGHTERS get +1 HP for 19 CON
            if (clazz == LabLordGlobals.CLASS_FIGHTER
                && GetFullAttributeScore("CON")>= 19f)
            {
                AdjustAttributeModifier((string)attributeMap[LabLordGlobals.EQUIP_ELEMENT_MHP][0], 1f);
            }
        }
        protected override void ApplyRulesPercentModifiers()
        {

        }
        protected override void ApplyRulesPerLevelModifiers()
        {

        }
        /// <summary>
        /// Called when a player dies.
        /// </summary>
        public override void BecomesDead()
        {
            base.BecomesDead();
        }
        public override bool CalculateBackstab()
        {
            return false;
        }
        public override bool CalculateCriticalHit()
        {
            return false;
        }

        public override bool CanIdentifyEquipment(IOEquipItem equipitem)
        {
            throw new NotImplementedException();
        }
        public override void ComputeFullStats()
        {
            // get damage from weapon
            int wpnId = GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
            if (wpnId >= 0
                && Interactive.Instance.HasIO(wpnId))
            {
                BaseInteractiveObject wpnIo = Interactive.Instance.GetIO(wpnId);
                Io.Script.SetLocalVariable("DMG", wpnIo.Script.GetLocalIntArrayVariableValue("DMG"));
            }
            else
            {
                Io.Script.SetLocalVariable("DMG", new int[] { 1, 2 });
            }
            base.ComputeFullStats();
        }
        protected override object[][] GetAttributeMap()
        {
            return attributeMap;
        }
        protected override float GetBaseMana()
        {
            throw new NotImplementedException();
        }
        public override float GetFullDamage()
        {
            throw new NotImplementedException();
        }
        public override float GetMaxLife()
        {
            return GetFullAttributeScore("MHP");
        }
        public override void RecreatePlayerMesh()
        {
            
        }
        protected override string GetLifeAttribute()
        {
            return "HP";
        }
        public int GetSavingThrow(int type)
        {
            return LabLordSaveThrow.GetSavingThrow(type, clazz, Level);
        }
        public string ToCharSheetString()
        {
            ComputeFullStats();
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            /*
            sb.Append(Name);
            sb.Append("\t");
            sb.Append(Race.Title);
            sb.Append(" ");
            sb.Append(Profession.Title);
            sb.Append("\n");
            sb.Append("AC: ");
            sb.Append((int)GetFullAttributeScore("AC"));
            sb.Append("\t");
            sb.Append("HP: ");
            sb.Append((int)Life);
            sb.Append("/");
            sb.Append((int)GetMaxLife());
            sb.Append("\n--------------\n");
            sb.Append("STR:\t");
            sb.Append((int)GetFullAttributeScore("STR"));
            sb.Append("\tINT:\t");
            sb.Append((int)GetFullAttributeScore("INT"));
            sb.Append("\nDEX:\t");
            sb.Append((int)GetFullAttributeScore("DEX"));
            sb.Append("\tWIS:\t");
            sb.Append((int)GetFullAttributeScore("WIS"));
            sb.Append("\nCON:\t");
            sb.Append((int)GetFullAttributeScore("CON"));
            sb.Append("\tCHA:\t");
            sb.Append((int)GetFullAttributeScore("CHA"));
            sb.Append("\n");
            sb.Append("Weapon:");
            int wpnIoId = GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
            if (wpnIoId >= 0)
            {
                BaseInteractiveObject io = Interactive.Instance.GetIO(wpnIoId);
                BHItemData item = (BHItemData)io.ItemData;
                sb.Append(item.ItemName);
                sb.Append("\t\tDamage:\t");
                switch (item.Dice)
                {
                    case Dice.ONE_D10:
                        sb.Append("D10");
                        break;
                    case Dice.ONE_D12:
                        sb.Append("D12");
                        break;
                    case Dice.ONE_D2:
                        sb.Append("D2");
                        break;
                    case Dice.ONE_D20:
                        sb.Append("D20");
                        break;
                    case Dice.ONE_D3:
                        sb.Append("D3");
                        break;
                    case Dice.ONE_D4:
                        sb.Append("D4");
                        break;
                    case Dice.ONE_D6:
                        sb.Append("D6");
                        break;
                    case Dice.ONE_D8:
                        sb.Append("D8");
                        break;
                }
                if (item.DmgModifier > 0)
                {
                    sb.Append(" + ");
                    sb.Append(item.DmgModifier);
                }
            }
            */
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        /// <summary>
        /// Converts a length to english feet and inches.
        /// </summary>
        /// <param name="val">the value</param>
        /// <returns></returns>
        private string ToEnglishLength(int val)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(val / 12);
            sb.Append("' ");
            if (val % 12 > 0)
            {
                sb.Append(val % 12);
                sb.Append("\"");
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
    }
}