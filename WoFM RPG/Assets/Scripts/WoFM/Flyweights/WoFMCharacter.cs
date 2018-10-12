using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using UnityEngine;
using WoFM.Constants;

namespace WoFM.Flyweights
{
    public class WoFMCharacter : IOPcData
    {
        /// <summary>
        /// the list of attributes and their matching names and modifiers.
        /// </summary>
        private static object[][] attributeMap = new object[][] {
            new object[] { "SKL", "Skill", WoFMGlobals.EQUIP_ELEMENT_SKILL },
            new object[] { "MSK", "Max Skill", WoFMGlobals.EQUIP_ELEMENT_MAX_SKILL },
            new object[] { "STM", "Stamina", WoFMGlobals.EQUIP_ELEMENT_STAMINA },
            new object[] { "MSTM", "Max Stamina", WoFMGlobals.EQUIP_ELEMENT_MAX_STAMINA },
            new object[] { "LUK", "Luck", WoFMGlobals.EQUIP_ELEMENT_LUCK },
            new object[] { "MLK", "Max Luck", WoFMGlobals.EQUIP_ELEMENT_MAX_LUCK },
            new object[] { "DMG", "Damage", WoFMGlobals.EQUIP_ELEMENT_DAMAGE },
        };
        /// <summary>
        /// Tests the player's skill.
        /// </summary>
        /// <returns></returns>
        public bool TestSkill()
        {
            ComputeFullStats();
            int roll = Diceroller.Instance.RollXdY(2, 6);
            float score = GetFullAttributeScore("SKL");
            Debug.Log("test skill::" + roll + "<=" + score + "?");
            return roll <= score;
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
            return GetFullAttributeScore("MST");
        }
        public override void RecreatePlayerMesh()
        {
            throw new NotImplementedException();
        }
        protected override string GetLifeAttribute()
        {
            return "STM";
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