using Assets.Scripts.Blueholme.Flyweights;
using Assets.Scripts.Blueholme.Globals;
using Assets.Scripts.Blueholme.Scriptables.Items;
using Assets.Scripts.Blueholme.Scriptables.Mobs;
using RPGBase.Constants;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Blueholme.Singletons
{
    public class BHCharacterCreator : MonoBehaviour
    {
        private PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
        [SerializeField]
        private Text text0;
        [SerializeField]
        private BHInteractiveObject io0;
        private BHInteractiveObject io1;
        // Use this for initialization
        void Start()
        {
            BHController.Init();
            BHInteractive.Init();
            BHScript.Init();
            BHCombat.Init();
            ((BHCombat)BHCombat.Instance).Output = text0;
            io0 = ((BHInteractive)Interactive.Instance).NewHero();
            io0.PcData.Name = "Gotzstaf";
            io0.PcData.Gender = Gender.GENDER_MALE;
            io1 = ((BHInteractive)Interactive.Instance).NewHero();
            io1.PcData.Name = "Tuste";
            io1.PcData.Gender = Gender.GENDER_MALE;
            CreateCharacter(io0);
            CreateCharacter(io1);
            ((BHCharacter)io0.PcData).Race = BHRace.Halfling;
            ((BHCharacter)io1.PcData).Race = BHRace.Halfling;
            BHInteractiveObject wpnIO = ((BHInteractive)Interactive.Instance).NewItem(new Handaxe());
            wpnIO.ItemData.Equip(io0);
            wpnIO = ((BHInteractive)Interactive.Instance).NewItem(new Shortsword());
            wpnIO.ItemData.Equip(io1);
            sb.Append(((BHCharacter)io0.PcData).ToCharSheetString());
            sb.Append("\n----------------------------\n");
            sb.Append(((BHCharacter)io1.PcData).ToCharSheetString());
            sb.Append("\n----------------------------\n");
            text0.text = sb.ToString();
            sb.Length = 0;
            // put them in combat
            Combat();
        }
        private void Combat()
        {
            // set targets for each
            io0.Script.SetLocalVariable("target_practice", io1.RefId);
            io1.Script.SetLocalVariable("target_practice", io0.RefId);
            print("starting combat");
            // start combat
            ((BHCombat)BHCombat.Instance).InitiateCombat(new List<BHInteractiveObject> { io0 }, new List<BHInteractiveObject> { io1 });
            /*
            FWCharacter pc0 = (FWCharacter)io0.PcData;
            FWCharacter pc1 = (FWCharacter)io1.PcData;
            List<FWInteractiveObject> combatants = new List<FWInteractiveObject>
            {
                io0,
                io1
            };
            InitiativeComparer sorter = new InitiativeComparer();
            //FlurryComparer flurrySorter = new FlurryComparer();
            bool combatIsOver = false;
            while (!combatIsOver)
            {
                ((FWCombat)FWCombat.Instance).FlurryNumber++;
                sb.Length = 0;
                sb.Append(text0.text);
                sb.Append("\n\n");
                // print combatants' health
                sb.Append(pc0.Name);
                sb.Append(": ");
                sb.Append(pc0.Life);
                sb.Append("/");
                sb.Append(pc0.GetMaxLife());
                sb.Append("\n");
                sb.Append(pc1.Name);
                sb.Append(": ");
                sb.Append(pc1.Life);
                sb.Append("/");
                sb.Append(pc1.GetMaxLife());
                sb.Append("\n\n");
                // go through each phase
                // PRE-COMBAT
                PreCombat();
                for (int i = combatants.Count - 1; i >= 0; i--)
                {
                    combatants[0].Script.SetLocalVariable("initial_strike_dmg", 0);
                    combatants[0].Script.SetLocalVariable("initial_strike_result", -1);
                }


                // COMBAT
                // 1. characters with longer weapons or SURPLUS AGI GTE 4+ opponent attach first
                // sort combatants by weapon length or surplus agility
                combatants.Sort(sorter);
                print(combatants[0].PcData.Name + " goes first");
                float startingEnd = combatants[1].PcData.Life;
                FWCombat.Instance.StrikeCheck(combatants[0], Interactive.Instance.GetIO(combatants[0].PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON)), FWCombat.INITIAL_STRIKE, combatants[1].RefId);
                float currEnd = combatants[1].PcData.Life;
                // 2. opponents counterattack unless killed or END LTE 1/2
                if (!combatants[1].PcData.IsDead()
                    && currEnd / startingEnd > .5f)
                {
                    PooledStringBuilder sb1 = StringBuilderPool.Instance.GetStringBuilder();
                    sb1.Append(combatants[1].PcData.Name);
                    sb1.Append(" counterattacks!\n");
                    Messages.Instance.Add(sb1.ToString());
                    sb1.ReturnToPool();
                    sb1 = null;
                    FWCombat.Instance.StrikeCheck(combatants[1], Interactive.Instance.GetIO(combatants[1].PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON)), FWCombat.INITIAL_STRIKE, combatants[0].RefId);
                }
                else if (currEnd / startingEnd <= .5f)
                {
                    PooledStringBuilder sb1 = StringBuilderPool.Instance.GetStringBuilder();
                    sb1.Append(combatants[1].PcData.Name);
                    sb1.Append(" staggers...\n");
                }
                ((FWCombat)FWCombat.Instance).FlurryNumber++;
                // 3. simultaneous flurry of blows
                // combatants are re-sorted by who caused the most damage
                // each combatant needs to choose their action
                for (int i = combatants.Count - 1; i >= 0; i--)
                {
                    Script.Instance.SendIOScriptEvent(combatants[0], FWGlobals.SM_302_COMBAT_FLURRY, null, "");
                }
                // POST-COMBAT
                // 1. check morale
                // 2. go back to combat phase
                while (!Messages.Instance.IsEmpty)
                {
                    sb.Append(Messages.Instance.Dequeue);
                }
                text0.text = sb.ToString();
                break;
            }
            */
        }
        // Update is called once per frame
        void Update()
        {

        }
        /********************************
         * Character Creation procedure *
         *******************************/
        private void CreateCharacter(BHInteractiveObject io)
        {
            BHCharacter pc = (BHCharacter)io.PcData;
            int valid = 0, rc = 0;
            do
            {
                do
                {
                    // roll race
                    RollRace(pc);
                    // roll initial values
                    RollScores(pc);
                    // set profession
                    SetPlayerProfession(pc, BHProfession.Fighter);
                    // validate
                    valid = Validate(pc);
                }
                while (valid == 0);
            } while ((valid & BHProfession.Fighter.Val) != BHProfession.Fighter.Val);
            switch (rc)
            {
                case 1:
                    print("Recommended Class: Fighter");
                    break;
                case 2:
                    print("Recommended Class: Mage");
                    break;
                case 3:
                    print("Recommended Class: Cleric");
                    break;
            }
        }
        private void RollRace(BHCharacter pc)
        {
            switch (Diceroller.Instance.RolldX(6))
            {
                case 1:
                case 2:
                case 3:
                    pc.Race = BHRace.Human;
                    break;
                case 4:
                    pc.Race = BHRace.Dwarf;
                    break;
                case 5:
                    pc.Race = BHRace.Elf;
                    break;
                case 6:
                    pc.Race = BHRace.Halfling;
                    break;
            }
        }
        private void RollScores(BHCharacter pc)
        {
            pc.SetBaseAttributeScore("STR", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("DEX", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("CON", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("INT", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("WIS", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("CHA", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("AC", 9);
        }
        private void SetPlayerProfession(BHCharacter pc, BHProfession profession)
        {
            pc.Profession = profession;
            int hp = 0;
            do
            {
                // set hit points
                pc.SetBaseAttributeScore("MHP", profession.HitDice.Roll());
                if (pc.Race.Val == BHRace.Elf.Val
                    || pc.Race.Val == BHRace.Halfling.Val)
                {
                    pc.SetBaseAttributeScore("MHP", Diceroller.Instance.RolldX(6));
                }
                pc.ComputeFullStats();
                hp = (int)pc.GetFullAttributeScore("MHP");
            } while (hp <= 0);

            pc.SetBaseAttributeScore("HP", pc.GetBaseAttributeScore("MHP"));
            pc.HealPlayer(999, true);
        }
        private int Validate(BHCharacter pc)
        {
            pc.ComputeFullStats();
            int minPrime = 13;
            int recommendedClass = 0;
            if (pc.GetFullAttributeScore("WIS") >= minPrime)
            {
                recommendedClass += BHProfession.Cleric.Val;
            }
            if (pc.GetFullAttributeScore("STR") >= minPrime)
            {
                recommendedClass += BHProfession.Fighter.Val;
            }
            if (pc.GetFullAttributeScore("INT") >= minPrime)
            {
                recommendedClass += BHProfession.MagicUser.Val;
            }
            if (pc.GetFullAttributeScore("DEX") >= minPrime)
            {
                recommendedClass += BHProfession.Thief.Val;
            }
            return recommendedClass;
        }
    }
}
