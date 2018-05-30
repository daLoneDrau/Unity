using Assets.Scripts.Blueholme.Globals;
using Assets.Scripts.Blueholme.Singletons;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Blueholme.Flyweights
{
    public class BHCharacter : IOPcData
    {
        /// <summary>
        /// the list of attributes and their matching names and modifiers.
        /// </summary>
        private static object[][] attributeMap = new object[][] {
            new object[] { "STR", "Strength", BHGlobals.EQUIP_ELEMENT_STR },
            new object[] { "INT", "Intelligence", BHGlobals.EQUIP_ELEMENT_INT },
            new object[] { "WIS", "Wisdom", BHGlobals.EQUIP_ELEMENT_WIS },
            new object[] { "CON", "Constitution", BHGlobals.EQUIP_ELEMENT_CON },
            new object[] { "DEX", "Dexterity", BHGlobals.EQUIP_ELEMENT_DEX },
            new object[] { "CHA", "Charisma", BHGlobals.EQUIP_ELEMENT_CHA },
            new object[] { "HP", "Hit Points", BHGlobals.EQUIP_ELEMENT_HP },
            new object[] { "MHP", "Endurance", BHGlobals.EQUIP_ELEMENT_MHP },
            new object[] { "SBW", "Save vs. Breath Weapon", BHGlobals.EQUIP_ELEMENT_SBW },
            new object[] { "SMW", "Save vs. Magic Wand", BHGlobals.EQUIP_ELEMENT_SMW },
            new object[] { "SGZ", "Save vs. Gaze", BHGlobals.EQUIP_ELEMENT_SGZ },
            new object[] { "SRP", "Save vs. Ray or Poison", BHGlobals.EQUIP_ELEMENT_SRP },
            new object[] { "SSS", "Save vs. Spell or Staff", BHGlobals.EQUIP_ELEMENT_SSS },
            new object[] { "AC", "Armour Class", BHGlobals.EQUIP_ELEMENT_AC },
            new object[] { "THM", "To Hit Modifier", BHGlobals.EQUIP_ELEMENT_THM },
        };
        protected override void AdjustMana(float dmg)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Applies modifiers to the character's attributes and skills based on the game rules.
        /// </summary>
        protected override void ApplyRulesModifiers()
        {
            // apply constitution modifier to HP
            if (GetBaseAttributeScore("CON") <= 6)
            {
                AdjustAttributeModifier("MHP", -1);
            }
            else if (GetBaseAttributeScore("CON") >=15
                && GetBaseAttributeScore("CON") <= 16)
            {
                AdjustAttributeModifier("MHP", 1);
            }
            else if (GetBaseAttributeScore("CON") == 17)
            {
                AdjustAttributeModifier("MHP", 2);
            }
            else if (GetBaseAttributeScore("CON") >= 18)
            {
                AdjustAttributeModifier("MHP", 3);
            }
            // apply To Hit Modifier for DEX
            if (GetBaseAttributeScore("DEX") <= 8)
            {
                AdjustAttributeModifier("THM", -1);
            }
            else if (GetBaseAttributeScore("CON") >= 13)
            {
                AdjustAttributeModifier("THM", 1);
            }

        }
        protected override void ApplyRulesPercentModifiers()
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
            throw new NotImplementedException();
        }
        protected override string GetLifeAttribute()
        {
            return "HP";
        }
        public BHProfession Profession { get; set; }
        public BHRace Race { get; set; }
        public string ToCharSheetString()
        {
            ComputeFullStats();
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
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
