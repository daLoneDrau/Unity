using Assets.Scripts.FantasyWargaming.Globals;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.FantasyWargaming.Flyweights
{
    public class FWCharacter : IOPcData
    {
        /// <summary>
        /// the character's <see cref="Bogey"/>s
        /// </summary>
        private Bogey[] bogeys = new Bogey[0];
        /// <summary>
        /// the character's height.
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// the character's <see cref="StarSign"/>.
        /// </summary>
        public StarSign Sign { get; set; }
        /// <summary>
        /// The character's family's social group.
        /// </summary>
        public int SocialGroup { get; set; }
        /// <summary>
        /// The character's surplus Agility.
        /// </summary>
        public int SurplusAgility { get; private set; }
        /// <summary>
        /// The character's surplus Physique.
        /// </summary>
        public int SurplusPhysique { get; private set; }
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
            new object[] { "MEND", "Endurance", FWGlobals.EQUIP_ELEMENT_END },
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
        protected override void AdjustMana(float dmg)
        {
            throw new NotImplementedException();
        }
        protected override void ApplyRulesModifiers()
        {
            // apply modifiers for Bogeys
            for (int i = bogeys.Length - 1; i >= 0; i--)
            {
                bogeys[i].Apply(this);
            }
            int lea = (int)(GetFullAttributeScore("CHA") * 3 + GetFullAttributeScore("PHY") + GetFullAttributeScore("INT") + GetFullAttributeScore("BRV"));
            lea /= 10;
            AdjustAttributeModifier("LEA", lea);
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
        public void ClearBogeys()
        {
            bogeys = new Bogey[0];
        }
        public override void ComputeFullStats()
        {
            base.ComputeFullStats();
            SurplusAgility = 0;
            // TODO - apply encumbrance for armour
            // calculate surplus physique and agility
            int wpnId = GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
            if (wpnId >= 0)
            {
                FWInteractiveObject wpnIo= (FWInteractiveObject)Interactive.Instance.GetIO(wpnId);
                if (wpnIo.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW))
                {
                    // TODO - calculate surplus agility
                }
                else
                {
                    int surplus = (int)(GetFullAttributeScore("AGI") - ((FWItemData)wpnIo.ItemData).MinMeleeAgility);
                    if (surplus < 0)
                    {
                        SurplusAgility = surplus;
                    } else
                    {
                        SurplusAgility = (int)Math.Ceiling((float)surplus / 2f);
                    }
                }
            }
        }
        public override float GetFullDamage()
        {
            throw new NotImplementedException();
        }

        public override float GetMaxLife()
        {
            return GetFullAttributeScore("MEND");
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
            return "END";
        }
        public string ToCharSheetString()
        {
            ComputeFullStats();
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append(Name);
            sb.Append("\n");
            sb.Append("Astrological Sign: ");
            sb.Append(Sign);
            sb.Append("\n");
            sb.Append("Ability Scores\n");
            sb.Append("--------------\n");
            sb.Append("Physique:     \t");
            sb.Append((int)GetFullAttributeScore("PHY"));
            sb.Append("\tHeight: ");
            sb.Append(ToEnglishLength(Height));
            sb.Append("\n");
            sb.Append("Agility:      \t");
            sb.Append((int)GetFullAttributeScore("AGI"));
            sb.Append("\tWeight: ");
            sb.Append(Weight);
            sb.Append("\n");
            sb.Append("Endurance:    \t");
            sb.Append((int)GetFullAttributeScore("END"));
            sb.Append("/");
            sb.Append((int)GetFullAttributeScore("MEND"));
            sb.Append("\n\n");
            sb.Append("Intelligence: \t");
            sb.Append((int)GetFullAttributeScore("INT"));
            sb.Append("\n");
            sb.Append("Faith:        \t");
            sb.Append((int)GetFullAttributeScore("FTH"));
            sb.Append("\n\n");
            sb.Append("Charisma:     \t");
            sb.Append((int)GetFullAttributeScore("CHA"));
            sb.Append("\tLeadership: ");
            sb.Append((int)GetFullAttributeScore("LEA"));
            sb.Append("\n");
            sb.Append("Greed:        \t");
            sb.Append((int)GetFullAttributeScore("GRE"));
            sb.Append("\n");
            sb.Append("Selfishness:  \t");
            sb.Append((int)GetFullAttributeScore("SEL"));
            sb.Append("\n");
            sb.Append("Lust:         \t");
            sb.Append((int)GetFullAttributeScore("LUS"));
            sb.Append("\n");
            sb.Append("Bravery:      \t");
            sb.Append((int)GetFullAttributeScore("BRV"));
            sb.Append("\n");
            sb.Append("Social Class: \t");
            sb.Append((int)GetFullAttributeScore("SOC"));
            sb.Append("\tFather: ");
            sb.Append(ToSocialString());
            sb.Append("\n");
            if (bogeys.Length > 0)
            {
                sb.Append("Miscellaneous Traits:\t");
                for (int i = bogeys.Length - 1; i >= 0; i--)
                {
                    if (i < bogeys.Length - 1)
                    {
                        sb.Append("\t\t");
                    }
                    sb.Append(bogeys[i].Title);
                    if (i > 0)
                    {
                        sb.Append("\n");
                    }
                }
            }
            sb.Append("\n");
            sb.Append("Weapon:");
            int wpnIoId = GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
            if (wpnIoId >= 0)
            {
                BaseInteractiveObject io = Interactive.Instance.GetIO(wpnIoId);
                FWItemData item = (FWItemData)io.ItemData;
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
                sb.Append("\tRange:\t");
                sb.Append(this.ToEnglishLength((int)(item.Range * 12f)));
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        private string ToSocialString()
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            switch (SocialGroup)
            {
                case 0:
                    switch ((int)GetBaseAttributeScore("SOC"))
                    {
                        case 3:
                        case 4:
                            sb.Append("Serf");
                            break;
                        case 5:
                            sb.Append("Outlaw");
                            break;
                        case 6:
                            sb.Append("Bordar");
                            break;
                        case 7:
                            sb.Append("Cottar");
                            break;
                        case 8:
                            sb.Append("Poor Villein");
                            break;
                        case 9:
                            sb.Append("Wealthy Villein");
                            break;
                        case 10:
                            sb.Append("Sokeman");
                            break;
                        case 11:
                            sb.Append("Poor Freeman");
                            break;
                        case 12:
                            sb.Append("Wealthy Freeman");
                            break;
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                            sb.Append("Reeve");
                            break;
                    }
                    sb.Append(" (Rural Areas)");
                    break;
                case 1:
                    switch ((int)GetBaseAttributeScore("SOC"))
                    {
                        case 3:
                        case 4:
                        case 5:
                            sb.Append("Escaped Serf");
                            break;
                        case 6:
                            sb.Append("Beggar");
                            break;
                        case 7:
                            sb.Append("Thief");
                            break;
                        case 8:
                            sb.Append("Unemployed Laborer");
                            break;
                        case 9:
                            sb.Append("Servant");
                            break;
                        case 10:
                            sb.Append("Employed Laborer");
                            break;
                        case 11:
                            sb.Append("Employed Skilled Laborer");
                            break;
                        case 12:
                        case 13:
                            sb.Append("Guild Journeyman");
                            break;
                        case 14:
                            sb.Append("Guild Officer");
                            break;
                        case 15:
                            sb.Append("Guildmaster");
                            break;
                        case 16:
                            sb.Append("Mayor");
                            break;
                        case 17:
                        case 18:
                            sb.Append("Lord Mayor");
                            break;
                    }
                    sb.Append(" (Townsman)");
                    break;
                case 3:
                    switch ((int)GetBaseAttributeScore("SOC"))
                    {
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                            sb.Append("Outlaw");
                            break;
                        case 8:
                        case 9:
                            sb.Append("Conscript Peasant");
                            break;
                        case 10:
                            sb.Append("Mercenary Archer");
                            break;
                        case 11:
                            sb.Append("Man-at-arms");
                            break;
                        case 12:
                            sb.Append("Mercenary Sergeant");
                            break;
                        case 13:
                            sb.Append("Mercenary Captain");
                            break;
                        case 14:
                            sb.Append("Landless Knight");
                            break;
                        case 15:
                            sb.Append("Knight-errant");
                            break;
                        case 16:
                            sb.Append("Poor Baron");
                            break;
                        case 17:
                            sb.Append("Wealthy Baron");
                            break;
                        case 18:
                            sb.Append("Earl");
                            break;
                    }
                    sb.Append(" (Chivalric)");
                    break;
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
