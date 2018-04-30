using Assets.Scripts.FantasyWargaming.Globals;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FantasyWargaming.Flyweights
{
    public class FWCharacter : IOPcData
    {
        /// <summary>
        /// the character's <see cref="Bogey"/>s
        /// </summary>
        private Bogey[] bogeys = new Bogey[0];
        /// <summary>
        /// the character's <see cref="StarSign"/>.
        /// </summary>
        public StarSign Sign { get; set; }
        /// <summary>
        /// the character's height.
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// the character's weight.
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// the list of attributes and their matching names and modifiers.
        /// </summary>
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
            new object[] { "MAN", "Mana", FWGlobals.EQUIP_ELEMENT_MAN },
            new object[] { "SOC", "Social Class", FWGlobals.EQUIP_ELEMENT_SOC },
            new object[] { "LEA", "Leadership", FWGlobals.EQUIP_ELEMENT_LEA }
        };
        /// <summary>
        /// Gives the character a <see cref="Bogey"/>
        /// </summary>
        /// <param name="b">the bogey</param>
        public void AddBogey(Bogey b)
        {
            bogeys = ArrayUtilities.Instance.ExtendArray(b, bogeys);
        }
        protected override void ApplyRulesModifiers()
        {
            // apply modifiers for Bogeys
            for (int i = bogeys.Length - 1; i >= 0; i--)
            {
                bogeys[i].Apply(this);
            }
        }
        protected override void ApplyRulesPercentModifiers()
        {

        }
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
        /// <summary>
        /// Determines if the character has a specific <see cref="Bogey"/>.
        /// </summary>
        /// <param name="b">the bogey being checked</param>
        /// <returns>true if the character has the bogey; false otherwise</returns>
        public bool HasBogey(Bogey b)
        {
            bool has = false;
            for (int i = bogeys.Length - 1; i >= 0; i--)
            {
                if (bogeys[i].Val == b.Val)
                {
                    has = true;
                    break;
                }
            }
            return has;
        }
        public override void RecreatePlayerMesh()
        {
            throw new NotImplementedException();
        }

        protected override void AdjustMana(float dmg)
        {
            throw new NotImplementedException();
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
        private string ToEnglishLength(int val)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        public string ToCharSheetString()
        {
            ComputeFullStats();
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("Astrological Sign: ");
            sb.Append(Sign);
            sb.Append("\n");
            sb.Append("Ability Scores\n");
            sb.Append("--------------\n");
            sb.Append("Physique:     \t");
            sb.Append((int)GetFullAttributeScore("PHY"));
            sb.Append("\tHeight: ");
            sb.Append(Height);
            sb.Append("\n");
            sb.Append("Agility:      \t");
            sb.Append((int)GetFullAttributeScore("AGI"));
            sb.Append("\tWeight: ");
            sb.Append(Weight);
            sb.Append("\n");
            sb.Append("Endurance:    \t");
            sb.Append((int)GetFullAttributeScore("END"));
            sb.Append("\n\n");
            sb.Append("Intelligence: \t");
            sb.Append((int)GetFullAttributeScore("INT"));
            sb.Append("\n");
            sb.Append("Faith:        \t");
            sb.Append((int)GetFullAttributeScore("FTH"));
            sb.Append("\n\n");
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
    }
}
