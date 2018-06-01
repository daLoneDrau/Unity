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
            BHParty group = new BHParty();
            group.Add(io0);
            group.Configuration.AssignToPosition(BHPartyConfiguration.POSITION_FRONT_CENTER, io0);
            io0.PcData.EncounterParty = group;

            group = new BHParty();
            group.Add(io1);
            group.Configuration.AssignToPosition(BHPartyConfiguration.POSITION_FRONT_CENTER, io1);
            io1.PcData.EncounterParty = group;
            
            print("starting combat");
            // start combat
            ((BHCombat)BHCombat.Instance).InitiateCombat(io0.PcData.EncounterParty, io1.PcData.EncounterParty);
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
            int minPrime = 13, min2nd = 9;
            int recommendedClass = 0;
            if (pc.GetFullAttributeScore("WIS") >= minPrime)
            {
                recommendedClass += BHProfession.Cleric.Val;
            }
            if (pc.GetFullAttributeScore("STR") >= minPrime
                && pc.GetFullAttributeScore("CON") >= min2nd)
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
