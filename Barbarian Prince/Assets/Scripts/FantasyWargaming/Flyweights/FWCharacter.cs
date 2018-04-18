using Assets.Scripts.FantasyWargaming.Globals;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FantasyWargaming.Flyweights
{
    public class FWCharacter : IOPcData
    {
        /** the list of attributes and their matching names and modifiers. */
        private static object[][] attributeMap = new object[][] {
            new object[] { "PHY", "Physique", FWGlobals.EQUIP_ELEMENT_PHY },
            new object[] { "AGI", "Agility", FWGlobals.EQUIP_ELEMENT_AGI },
            new object[] { "END", "Endurance", FWGlobals.EQUIP_ELEMENT_END },
            new object[] { "BRV", "Bravery", FWGlobals.EQUIP_ELEMENT_BRV },
            new object[] { "CHA", "Charisma", FWGlobals.EQUIP_ELEMENT_CHA },
            new object[] { "INT", "Intelligence", FWGlobals.EQUIP_ELEMENT_INT },
            new object[] { "GRE", "Greed", FWGlobals.EQUIP_ELEMENT_GRE },
            new object[] { "SEL", "Selfishness", FWGlobals.EQUIP_ELEMENT_SEL },
            new object[] { "LUS", "Lust", FWGlobals.EQUIP_ELEMENT_LUS },
            new object[] { "FTH", "Faith", FWGlobals.EQUIP_ELEMENT_FTH },
            new object[] { "PIE", "Piety", FWGlobals.EQUIP_ELEMENT_PIE },
            new object[] { "MAN", "Mana", FWGlobals.EQUIP_ELEMENT_MAN }
        };
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
            /*
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
            */
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
            /*
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
            */
        }
    }
}
