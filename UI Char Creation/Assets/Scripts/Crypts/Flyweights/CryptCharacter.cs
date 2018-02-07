using Assets.Scripts.Crypts.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Crypts.Flyweights
{
    public class CryptCharacter : IOPcData
    {
        /** the list of attributes and their matching names and modifiers. */
        private static object[][] attributeMap = new object[][] {
            new object[] { "AC", "Armour Class", CryptEquipGlobals.EQUIP_ELEMENT_AC },
            new object[] { "ACM", "AC Modifier", CryptEquipGlobals.EQUIP_ELEMENT_AC_MODIFIER },
            new object[] { "BRW", "Brawn", CryptEquipGlobals.EQUIP_ELEMENT_STRENGTH },
            new object[] { "COM", "Common Sense", CryptEquipGlobals.EQUIP_ELEMENT_WISDOM },
            new object[] { "COR", "Coordinaton", CryptEquipGlobals.EQUIP_ELEMENT_DEXTERITY },
            new object[] { "CRM", "Charm", CryptEquipGlobals.EQUIP_ELEMENT_CHARM },
            new object[] { "CRP", "Corruption", CryptEquipGlobals.EQUIP_ELEMENT_CORRUPTION },
            new object[] { "DB", "Damage Bonus", CryptEquipGlobals.EQUIP_ELEMENT_DMG_BONUS },
            new object[] { "EDU", "Education", CryptEquipGlobals.EQUIP_ELEMENT_INTELLIGENCE },
            new object[] { "HIR", "Max Hirelings", CryptEquipGlobals.EQUIP_ELEMENT_MAX_HIRELINGS },
            new object[] { "HRD", "Hardiness", CryptEquipGlobals.EQUIP_ELEMENT_CONSTITUTION },
            new object[] { "LAB", "Melee Bonus", CryptEquipGlobals.EQUIP_ELEMENT_TO_HIT },
            new object[] { "LAN", "Understand Language %", CryptEquipGlobals.EQUIP_ELEMENT_LANGUAGE },
            new object[] { "LEA", "Leadership", CryptEquipGlobals.EQUIP_ELEMENT_CHARISMA },
            new object[] { "LUK", "Luck", CryptEquipGlobals.EQUIP_ELEMENT_LUCK },
            new object[] { "MHP", "Max Hit Points", CryptEquipGlobals.EQUIP_ELEMENT_MHP },
            new object[] { "MAB", "Missile Bonus", CryptEquipGlobals.EQUIP_ELEMENT_MISSILE_BONUS },
            new object[] { "SAN", "Sanity", CryptEquipGlobals.EQUIP_ELEMENT_SANITY },
            new object[] { "SKL", "Skill", CryptEquipGlobals.EQUIP_ELEMENT_SKILL }
        };
        public int Profession { get; set; }
        public Homeland Homeland { get; set; }
        public CharacterOrigin Background { get; set; }
        public override bool CalculateBackstab()
        {
            throw new NotImplementedException();
        }

        public override bool CalculateCriticalHit()
        {
            throw new NotImplementedException();
        }

        public override bool CanIdentifyEquipment(IOEquipItem equipitem)
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

        protected override void AdjustMana(float dmg)
        {
            throw new NotImplementedException();
        }

        protected override void ApplyRulesModifiers()
        {
            int val = (int)GetBaseAttributeScore("STR") + (int)GetAttributeModifier("STR");
            if (val <= 8)
            {
                AdjustAttributeModifier("HIT", -1);
                AdjustAttributeModifier("DMG", -1);
            }
            else if (val >= 13 && val <= 15)
            {
                AdjustAttributeModifier("HIT", 1);
                AdjustAttributeModifier("DMG", 1);
            }
            else if (val >= 16 && val <= 17)
            {
                AdjustAttributeModifier("HIT", 2);
                AdjustAttributeModifier("DMG", 2);
            }
            else if (val >= 18)
            {
                AdjustAttributeModifier("HIT", 3);
                AdjustAttributeModifier("DMG", 3);
            }

            val = (int)GetBaseAttributeScore("DEX") + (int)GetAttributeModifier("DEX");
            if (val <= 8)
            {
                AdjustAttributeModifier("MSS", -1);
                AdjustAttributeModifier("ACM", -1);
            }
            else if (val >= 13 && val <= 15)
            {
                AdjustAttributeModifier("MSS", 1);
                AdjustAttributeModifier("ACM", 1);
            }
            else if (val >= 16 && val <= 17)
            {
                AdjustAttributeModifier("MSS", 2);
                AdjustAttributeModifier("ACM", 2);
            }
            else if (val >= 18)
            {
                AdjustAttributeModifier("MSS", 3);
                AdjustAttributeModifier("ACM", 3);
            }

            val = (int)GetBaseAttributeScore("CON") + (int)GetAttributeModifier("CON");
            if (val <= 8)
            {
                AdjustAttributeModifier("HPB", -1);
            }
            else if (val >= 13 && val <= 15)
            {
                AdjustAttributeModifier("HPB", 1);
            }
            else if (val >= 16 && val <= 17)
            {
                AdjustAttributeModifier("HPB", 2);
            }
            else if (val >= 18)
            {
                AdjustAttributeModifier("HPB", 3);
            }

            val = (int)GetBaseAttributeScore("INT") + (int)GetAttributeModifier("INT");
            if (val <= 8)
            {
                AdjustAttributeModifier("LAN", 0f);
            }
            else if (val >= 9 && val <= 12)
            {
                AdjustAttributeModifier("LAN", .1f);
            }
            else if (val >= 13 && val <= 15)
            {
                AdjustAttributeModifier("LAN", .25f);
            }
            else if (val >= 16 && val <= 17)
            {
                AdjustAttributeModifier("LAN", .50f);
            }
            else if (val >= 18)
            {
                AdjustAttributeModifier("LAN", .75f);
            }

            val = (int)GetBaseAttributeScore("CHA") + (int)GetAttributeModifier("CHA");
            if (val >= 3 && val <= 4)
            {
                AdjustAttributeModifier("CRM", .1f);
                AdjustAttributeModifier("HIR", 1);
            }
            else if (val >= 5 && val <= 6)
            {
                AdjustAttributeModifier("CRM", .2f);
                AdjustAttributeModifier("HIR", 2);
            }
            else if (val >= 7 && val <= 8)
            {
                AdjustAttributeModifier("CRM", .3f);
                AdjustAttributeModifier("HIR", 3);
            }
            else if (val >= 9 && val <= 12)
            {
                AdjustAttributeModifier("CRM", .4f);
                AdjustAttributeModifier("HIR", 4);
            }
            else if (val >= 13 && val <= 15)
            {
                AdjustAttributeModifier("CRM", .5f);
                AdjustAttributeModifier("HIR", 5);
            }
            else if (val >= 16 && val <= 17)
            {
                AdjustAttributeModifier("CRM", .6f);
                AdjustAttributeModifier("HIR", 6);
            }
            else if (val >= 18)
            {
                AdjustAttributeModifier("CRM", .75f);
                AdjustAttributeModifier("HIR", 7);
            }
            AdjustAttributeModifier("MHP", GetAttributeModifier("HPB"));
            AdjustAttributeModifier("AC", GetAttributeModifier("ACM"));
        }
        protected override void ApplyRulesPercentModifiers()
        {
            
        }
        protected override object[][] GetAttributeMap()
        {
            return attributeMap;
        }
        protected override float GetBaseMana()
        {
            throw new NotImplementedException();
        }
        protected override string GetLifeAttribute()
        {
            return "HP";
        }
        /// <summary>
        /// Rolls new statistics for the Hero.
        /// </summary>
        public void NewHeroStepOne()
        {
            do
            {
                SetBaseAttributeScore("STR", Diceroller.Instance.RollXdY(3, 6));
                SetBaseAttributeScore("DEX", Diceroller.Instance.RollXdY(3, 6));
                SetBaseAttributeScore("CON", Diceroller.Instance.RollXdY(3, 6));
                SetBaseAttributeScore("INT", Diceroller.Instance.RollXdY(3, 6));
                SetBaseAttributeScore("WIS", Diceroller.Instance.RollXdY(3, 6));
                SetBaseAttributeScore("CHA", Diceroller.Instance.RollXdY(3, 6));
                SetBaseAttributeScore("AC", CryptEquipGlobals.UNARMOURED_AC);
                SetBaseAttributeScore("LUK", Diceroller.Instance.RolldXPlusY(6, 6));
                SetBaseAttributeScore("SKL", 15);
                SetBaseAttributeScore("SAN", GetBaseAttributeScore("WIS"));
            } while (GetQualifyingClasses() == 0);
        }
        public int GetQualifyingClasses()
        {
            int val = 0;
            if (GetBaseAttributeScore("CON") > 12)
            {
                val |= CryptEquipGlobals.CLASS_BARBARIAN;
            }
            if (GetBaseAttributeScore("STR") > 12)
            {
                val |= CryptEquipGlobals.CLASS_FIGHTER;
            }
            if (GetBaseAttributeScore("INT") > 12)
            {
                val |= CryptEquipGlobals.CLASS_SORCERER;
            }
            if (GetBaseAttributeScore("DEX") > 12)
            {
                val |= CryptEquipGlobals.CLASS_THIEF;
            }
            return val;
        }
    }
}
