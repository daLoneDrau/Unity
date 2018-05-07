using Assets.Scripts.FantasyWargaming.Flyweights;
using RPGBase.Constants;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.FantasyWargaming.Scriptables.Mobs
{
    public class HeroBase : MobBase
    {
        /// <summary>
        /// Berserk check passed with CONTROL results.
        /// </summary>
        public const int BERSERK_CONTROL = 0;
        /// <summary>
        /// Berserk check passed with BERSERK results.
        /// </summary>
        public const int BERSERK_BERSERK = 1;
        /// <summary>
        /// the berserk check matrix.
        /// </summary>
        private readonly int[][] BERSERK_MATRIX = new int[][]{
        new int[]{ 498, BERSERK_CONTROL },
        new int[]{ 500, BERSERK_BERSERK },
        new int[]{ 594, BERSERK_CONTROL },
        new int[]{ 600, BERSERK_BERSERK },
        new int[]{ 689, BERSERK_CONTROL },
        new int[]{ 700, BERSERK_BERSERK },
        new int[]{ 781, BERSERK_CONTROL },
        new int[]{ 800, BERSERK_BERSERK },
        new int[]{ 869, BERSERK_CONTROL },
        new int[]{ 900, BERSERK_BERSERK },
        new int[]{ 953, BERSERK_CONTROL },
        new int[]{ 1000, BERSERK_BERSERK },
        new int[]{ 1035, BERSERK_CONTROL },
        new int[]{ 1100, BERSERK_BERSERK },
        new int[]{ 1119, BERSERK_CONTROL },
        new int[]{ 1200, BERSERK_BERSERK },
        new int[]{ 1207, BERSERK_CONTROL },
        new int[]{ 1208, BERSERK_BERSERK }
        };
        /// <summary>
        /// Morale check passed with GOOD results.
        /// </summary>
        public const int MORALE_GOOD = 0;
        /// <summary>
        /// Morale check passed with DITHER results.
        /// </summary>
        public const int MORALE_DITHER = 1;
        /// <summary>
        /// Morale check passed with SELFISH results.
        /// </summary>
        public const int MORALE_SELFISH = 2;
        /// <summary>
        /// Morale check passed with PANIC results.
        /// </summary>
        public const int MORALE_PANIC = 3;
        /// <summary>
        /// Morale check passed with FLEE results.
        /// </summary>
        public const int MORALE_FLEE = 4;
        /// <summary>
        /// the morale check matrix.
        /// </summary>
        private readonly int[][] MORALE_MATRIX = new int[][]{
        new int[]{ 801, MORALE_GOOD },
        new int[]{ 812, MORALE_DITHER },
        new int[]{ 826, MORALE_SELFISH },
        new int[]{ 850, MORALE_PANIC },
        new int[]{ 900, MORALE_FLEE },
        new int[]{ 905, MORALE_GOOD },
        new int[]{ 931, MORALE_DITHER },
        new int[]{ 945, MORALE_SELFISH },
        new int[]{ 966, MORALE_PANIC },
        new int[]{ 1000, MORALE_FLEE },
        new int[]{ 1012, MORALE_GOOD },
        new int[]{ 1042, MORALE_DITHER },
        new int[]{ 1060, MORALE_SELFISH },
        new int[]{ 1076, MORALE_PANIC },
        new int[]{ 1100, MORALE_FLEE },
        new int[]{ 1121, MORALE_GOOD },
        new int[]{ 1153, MORALE_DITHER },
        new int[]{ 1169, MORALE_SELFISH },
        new int[]{ 1182, MORALE_PANIC },
        new int[]{ 1200, MORALE_FLEE },
        new int[]{ 1233, MORALE_GOOD },
        new int[]{ 1264, MORALE_DITHER },
        new int[]{ 1275, MORALE_SELFISH },
        new int[]{ 1285, MORALE_PANIC },
        new int[]{ 1300, MORALE_FLEE },
        new int[]{ 1344, MORALE_GOOD },
        new int[]{ 1370, MORALE_DITHER },
        new int[]{ 1382, MORALE_SELFISH },
        new int[]{ 1390, MORALE_PANIC },
        new int[]{ 1400, MORALE_FLEE },
        new int[]{ 1454, MORALE_GOOD },
        new int[]{ 1477, MORALE_DITHER },
        new int[]{ 1489, MORALE_SELFISH },
        new int[]{ 1494, MORALE_PANIC },
        new int[]{ 1500, MORALE_FLEE },
        new int[]{ 1565, MORALE_GOOD },
        new int[]{ 1583, MORALE_DITHER },
        new int[]{ 1594, MORALE_SELFISH },
        new int[]{ 1597, MORALE_PANIC },
        new int[]{ 1600, MORALE_FLEE },
        new int[]{ 1780, MORALE_GOOD },
        new int[]{ 1790, MORALE_DITHER },
        new int[]{ 1798, MORALE_SELFISH },
        new int[]{ 1800, MORALE_PANIC },
        new int[]{ 1886, MORALE_GOOD },
        new int[]{ 1894, MORALE_DITHER },
        new int[]{ 1900, MORALE_SELFISH },
        new int[]{ 1990, MORALE_GOOD },
        new int[]{ 1997, MORALE_DITHER },
        new int[]{ 2000, MORALE_SELFISH },
        new int[]{ 2093, MORALE_GOOD },
        new int[]{ 2099, MORALE_DITHER },
        new int[]{ 2100, MORALE_SELFISH },
        new int[]{ 2196, MORALE_GOOD },
        new int[]{ 2200, MORALE_DITHER },
        new int[]{ 2298, MORALE_GOOD },
        new int[]{ 2300, MORALE_DITHER },
        new int[]{ 2301, MORALE_GOOD }
        };
        public override int OnBerserkCheck()
        {
            Console.WriteLine("Hero OnBerserkCheck");
            Debug.Log("Hero OnBerserkCheck");
            FWCharacter pc = (FWCharacter)Io.PcData;
            pc.ComputeFullStats();
            if (pc.GetFullAttributeScore("BRV") >= 12 && pc.GetFullAttributeScore("INT") <= 9)
            {
                int val = 0;
                // -1 if morale check failed
                if (GetLocalIntVariableValue("morale_check")== MORALE_DITHER)
                {
                    val--;
                }
                // TODO - halve the combat level. value is negative for normal characters, positive for Vikings

                // +1 for BRV 14 or INT 6 or 7
                if (pc.GetFullAttributeScore("BRV") == 14
                    || (pc.GetFullAttributeScore("INT") == 6
                    || pc.GetFullAttributeScore("INT") == 7))
                {
                    val++;
                }
                // +2 for BRV 15 or 16 or INT 5
                if (pc.GetFullAttributeScore("INT") == 5
                    || (pc.GetFullAttributeScore("BRV") == 15
                    || pc.GetFullAttributeScore("BRV") == 16))
                {
                    val += 2;
                }
                // +3 for BRV GTE 17 or INT LTE 3
                if (pc.GetFullAttributeScore("INT") <= 3
                    || pc.GetFullAttributeScore("BRV") >= 17)
                {
                    val += 3;
                }
                // +1 for no shield
                if (pc.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_SHIELD) < 0)
                {
                    val++;
                }
                // +1 for no armor
                if (pc.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO) < 0)
                {
                    val++;
                }
                // TODO +1 for being a VIKING
                // TODO +2 for being berserk in last two days
                // TODO -1 for being alone
                // TODO -1 for a dead/fleeing party member
                // TODO -2 for being exhausted
                // TODO -2 for being party leader
                // -3 for no weapon
                if (pc.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON) < 0)
                {
                    val -= 3;
                }
                // TODO -3 for wounds being LTE 3
                // TODO - -1 below half endurance
                // TODO - -1 below 4 endurance
                // TODO - -4 if already fleeing
                int roll = Diceroller.Instance.RolldX(6);
                roll += (int)val;
                if (roll < 4)
                {
                    roll = 4;
                }
                roll *= 100;
                roll += Diceroller.Instance.RolldX(100);
                int result = 0;
                // check morale matrix for result
                for (int i = 0, li = BERSERK_MATRIX.Length, last = 0; i < li; i++)
                {
                    // if we're on the last result, take it
                    if (i + 1 == li)
                    {
                        result = BERSERK_MATRIX[i][1];
                        break;
                    }
                    if (roll > last && roll <= BERSERK_MATRIX[i][0])
                    {
                        result = BERSERK_MATRIX[i][1];
                        break;
                    }
                    last = BERSERK_MATRIX[i][0];
                }
                Io.Script.SetLocalVariable("berserk_check", result);
                // TODO set time of last berserk check
            }
            else
            {
                Io.Script.SetLocalVariable("berserk_check", BERSERK_CONTROL);
            }
            return base.OnBerserkCheck();
        }
        public override int OnMoraleCheck()
        {
            Console.WriteLine("Hero ONMORALECHECK");
            Debug.Log("Hero ONMORALECHECK");
            FWCharacter pc = (FWCharacter)Io.PcData;
            // add combat level and bravery
            pc.ComputeFullStats();
            float val = pc.GetFullAttributeScore("BRV");
            // TODO - add combat level
            // TODO - +1 for each victory this day
            // TODO - +1 for each level lower than party leader
            // +1 for Selfishness LTE 8
            if (pc.GetFullAttributeScore("SEL") <= 8)
            {
                val++;
            }
            // TODO - +2 being unharmed
            // TODO - +2 being party leader
            // TODO - +2 all party members alive
            // TODO - +2 least brave member of party passed the morale test
            // TODO - -1 mage facing more powerful mage
            // TODO - -1 being wounded
            // -1 for Intelligence LTE 8
            if (pc.GetFullAttributeScore("INT") <= 8)
            {
                val--;
            }
            // TODO - -1 party member fleeing
            // TODO - -1 has no shield (COMBAT ONLY)
            // TODO - -1 has no armor (COMBAT ONLY)
            // TODO - -1 taking missile fire
            // TODO - -1 for each defeat this day
            // TODO - -1 party leader level is lower than character's
            // -1 for Selfishness GTE 14
            if (pc.GetFullAttributeScore("SEL") <= 14)
            {
                val++;
            }
            // TODO - -1 below half endurance
            // TODO - -1 below 4 endurance
            // TODO - -4 if already fleeing
            int roll = Diceroller.Instance.RolldX(6);
            roll += (int)val;
            if (roll < 8)
            {
                roll = 8;
            }
            roll *= 100;
            roll += Diceroller.Instance.RolldX(100);
            int result = 0;
            // check morale matrix for result
            for (int i = 0, li = MORALE_MATRIX.Length, last = 0; i < li; i++)
            {
                // if we're on the last result, take it
                if (i + 1 == li)
                {
                    result = MORALE_MATRIX[i][1];
                    break;
                }
                if (roll > last && roll <= MORALE_MATRIX[i][0])
                {
                    result = MORALE_MATRIX[i][1];
                    break;
                }
                last = MORALE_MATRIX[i][0];
            }
            Io.Script.SetLocalVariable("morale_check", result);
            return base.OnMoraleCheck();
        }
    }
}
