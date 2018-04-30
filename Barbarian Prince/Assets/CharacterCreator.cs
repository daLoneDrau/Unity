using Assets.Scripts.FantasyWargaming.Flyweights;
using Assets.Scripts.FantasyWargaming.Globals;
using RPGBase.Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.FantasyWargaming.Singletons
{
    public class CharacterCreator : MonoBehaviour
    {
        [SerializeField]
        private FWInteractiveObject io;
        // Use this for initialization
        void Start()
        {
            FWController.Init();
            FWInteractive.Init();
            io = ((FWInteractive)Interactive.Instance).NewHero();
            CreateCharacter();
        }
        // Update is called once per frame
        void Update()
        {

        }
        /********************************
         * Character Creation procedure *
         *******************************/
        private void CreateCharacter()
        {
            FWCharacter pc = (FWCharacter)io.PcData;
            int valid = 0;
            do
            {
                print("looping");
                // get star sign
                pc.Sign = Diceroller.Instance.GetRandomIndex<StarSign>();
                // roll initial values
                RollScores(pc);
                // adjust base scores based on star sign
                ApplySign(pc);
                // assign bogeys
                SetBogey(pc);
                // validate
                valid = Validate(pc);
            }
            while (valid == 0);
            // set height and weight
            pc.Height = 55 + (int)pc.GetFullAttributeScore("PHY");
            pc.Weight = 50 + 10 * (int)pc.GetFullAttributeScore("END");
            print(pc.ToCharSheetString());
        }
        private void SetBogey(FWCharacter pc)
        {
            switch (Diceroller.Instance.RolldX(6))
            {
                case 1:
                case 2:
                    Bogey b = GetRandomBogey();
                    if (b != null)
                    {
                        pc.AddBogey(b);
                    }
                    break;
                case 3:
                case 4:
                    b = GetRandomBogey();
                    if (b != null
                        && !pc.HasBogey(b))
                    {
                        pc.AddBogey(b);
                    }
                    b = GetRandomBogey();
                    if (b != null
                        && !pc.HasBogey(b))
                    {
                        pc.AddBogey(b);
                    }
                    break;
                case 5:
                case 6:
                    b = GetRandomBogey();
                    if (b != null
                        && !pc.HasBogey(b))
                    {
                        pc.AddBogey(b);
                    }
                    b = GetRandomBogey();
                    if (b != null
                        && !pc.HasBogey(b))
                    {
                        pc.AddBogey(b);
                    }
                    b = GetRandomBogey();
                    if (b != null
                        && !pc.HasBogey(b))
                    {
                        pc.AddBogey(b);
                    }
                    break;
            }
        }
        private Bogey GetRandomBogey()
        {
            Bogey o = null;
            switch (Diceroller.Instance.RolldX(100))
            {
                case 35:
                    o = Bogey.Homosexual;
                    break;
                case 41:
                    o = Bogey.Ugly;
                    break;
                case 45:
                    o = Bogey.Nearsighted;
                    break;
                case 49:
                    o = Bogey.Stammer;
                    break;
                case 51:
                    o = Bogey.Limp;
                    break;
                case 55:
                    o = Bogey.Weak;
                    break;
                case 57:
                    o = Bogey.Shy;
                    break;
                case 61:
                    o = Bogey.Smelly;
                    break;
                case 63:
                    o = Bogey.Insomniac;
                    break;
                case 67:
                    o = Bogey.Alcoholic;
                    break;
                case 38:
                    o = Bogey.Beauty;
                    break;
                case 42:
                    o = Bogey.Amiable;
                    break;
                case 44:
                    o = Bogey.Extrovert;
                    break;
                case 46:
                    o = Bogey.LevelHeaded;
                    break;
                case 48:
                    o = Bogey.Orator;
                    break;
                case 50:
                    o = Bogey.Charismatic;
                    break;
                case 52:
                    o = Bogey.Robust;
                    break;
                case 73:
                    o = Bogey.Dolorous;
                    break;
                case 74:
                    o = Bogey.Nimble;
                    break;
                case 96:
                    o = Bogey.Lucky;
                    break;
                default:
                    break;
            }
            return o;
        }
        private int Validate(FWCharacter pc)
        {
            pc.ComputeFullStats();
            int minPrime = 13;
            int min2nd = 9;
            int maxVice = 10;
            int recommendedClass = 0;
            if (pc.GetFullAttributeScore("PHY") + pc.GetFullAttributeScore("AGI") + pc.GetFullAttributeScore("END") + pc.GetFullAttributeScore("BRV") >= minPrime * 4)
            {
                recommendedClass += 1;
            }
            if (pc.GetFullAttributeScore("INT") + pc.GetFullAttributeScore("FTH") >= minPrime * 2)
            {
                recommendedClass += 2;
            }
            if (pc.GetFullAttributeScore("FTH") + pc.GetFullAttributeScore("PIE") >= minPrime * 2
                && (pc.GetFullAttributeScore("GRE") + pc.GetFullAttributeScore("SEL") + pc.GetFullAttributeScore("LUS")) <= maxVice * 3)
            {
                recommendedClass += 3;
            }
            return recommendedClass;
        }
        private void ApplySign(FWCharacter pc)
        {
            switch (pc.Sign)
            {
                case StarSign.Aquarius:
                    pc.SetBaseAttributeScore("PHY", pc.GetBaseAttributeScore("PHY") - 1);
                    pc.SetBaseAttributeScore("CHA", pc.GetBaseAttributeScore("CHA") + 1);
                    pc.SetBaseAttributeScore("GRE", pc.GetBaseAttributeScore("GRE") + 1);
                    pc.SetBaseAttributeScore("FTH", pc.GetBaseAttributeScore("FTH") + 2);
                    break;
                case StarSign.Pisces:
                    pc.SetBaseAttributeScore("PHY", pc.GetBaseAttributeScore("PHY") - 2);
                    pc.SetBaseAttributeScore("END", pc.GetBaseAttributeScore("END") - 2);
                    pc.SetBaseAttributeScore("SEL", pc.GetBaseAttributeScore("SEL") + 1);
                    pc.SetBaseAttributeScore("LUS", pc.GetBaseAttributeScore("LUS") - 2);
                    pc.SetBaseAttributeScore("BRV", pc.GetBaseAttributeScore("BRV") - 1);
                    pc.SetBaseAttributeScore("FTH", pc.GetBaseAttributeScore("FTH") + 3);
                    pc.SetBaseAttributeScore("SOC", pc.GetBaseAttributeScore("SOC") - 1);
                    break;
                case StarSign.Aries:
                    pc.SetBaseAttributeScore("PHY", pc.GetBaseAttributeScore("PHY") + 1);
                    pc.SetBaseAttributeScore("AGI", pc.GetBaseAttributeScore("AGI") - 1);
                    pc.SetBaseAttributeScore("GRE", pc.GetBaseAttributeScore("GRE") + 1);
                    pc.SetBaseAttributeScore("BRV", pc.GetBaseAttributeScore("BRV") + 2);
                    pc.SetBaseAttributeScore("SOC", pc.GetBaseAttributeScore("SOC") + 1);
                    break;
                case StarSign.Taurus:
                    pc.SetBaseAttributeScore("PHY", pc.GetBaseAttributeScore("PHY") + 2);
                    pc.SetBaseAttributeScore("END", pc.GetBaseAttributeScore("END") + 1);
                    pc.SetBaseAttributeScore("CHA", pc.GetBaseAttributeScore("CHA") - 1);
                    pc.SetBaseAttributeScore("SEL", pc.GetBaseAttributeScore("SEL") - 1);
                    pc.SetBaseAttributeScore("INT", pc.GetBaseAttributeScore("INT") - 2);
                    pc.SetBaseAttributeScore("FTH", pc.GetBaseAttributeScore("FTH") - 1);
                    break;
                case StarSign.Gemini:
                    pc.SetBaseAttributeScore("AGI", pc.GetBaseAttributeScore("AGI") - 2);
                    pc.SetBaseAttributeScore("END", pc.GetBaseAttributeScore("END") - 1);
                    pc.SetBaseAttributeScore("CHA", pc.GetBaseAttributeScore("CHA") - 1);
                    pc.SetBaseAttributeScore("LUS", pc.GetBaseAttributeScore("LUS") + 2);
                    pc.SetBaseAttributeScore("INT", pc.GetBaseAttributeScore("INT") + 1);
                    break;
                case StarSign.Cancer:
                    pc.SetBaseAttributeScore("END", pc.GetBaseAttributeScore("END") - 2);
                    pc.SetBaseAttributeScore("GRE", pc.GetBaseAttributeScore("GRE") + 1);
                    pc.SetBaseAttributeScore("SEL", pc.GetBaseAttributeScore("SEL") + 2);
                    pc.SetBaseAttributeScore("INT", pc.GetBaseAttributeScore("INT") + 3);
                    pc.SetBaseAttributeScore("SOC", pc.GetBaseAttributeScore("SOC") + 1);
                    break;
                case StarSign.Leo:
                    pc.SetBaseAttributeScore("PHY", pc.GetBaseAttributeScore("PHY") + 1);
                    pc.SetBaseAttributeScore("END", pc.GetBaseAttributeScore("END") + 1);
                    pc.SetBaseAttributeScore("CHA", pc.GetBaseAttributeScore("CHA") + 2);
                    pc.SetBaseAttributeScore("SEL", pc.GetBaseAttributeScore("SEL") + 1);
                    pc.SetBaseAttributeScore("BRV", pc.GetBaseAttributeScore("BRV") + 1);
                    pc.SetBaseAttributeScore("FTH", pc.GetBaseAttributeScore("FTH") - 2);
                    pc.SetBaseAttributeScore("SOC", pc.GetBaseAttributeScore("SOC") + 1);
                    break;
                case StarSign.Virgo:
                    pc.SetBaseAttributeScore("AGI", pc.GetBaseAttributeScore("AGI") + 1);
                    pc.SetBaseAttributeScore("CHA", pc.GetBaseAttributeScore("CHA") - 1);
                    pc.SetBaseAttributeScore("GRE", pc.GetBaseAttributeScore("GRE") + 1);
                    pc.SetBaseAttributeScore("LUS", pc.GetBaseAttributeScore("LUS") + 2);
                    pc.SetBaseAttributeScore("INT", pc.GetBaseAttributeScore("INT") + 1);
                    break;
                case StarSign.Libra:
                    pc.SetBaseAttributeScore("PHY", pc.GetBaseAttributeScore("PHY") - 1);
                    pc.SetBaseAttributeScore("AGI", pc.GetBaseAttributeScore("AGI") + 1);
                    pc.SetBaseAttributeScore("END", pc.GetBaseAttributeScore("END") + 1);
                    pc.SetBaseAttributeScore("LUS", pc.GetBaseAttributeScore("LUS") - 1);
                    pc.SetBaseAttributeScore("BRV", pc.GetBaseAttributeScore("BRV") - 2);
                    pc.SetBaseAttributeScore("INT", pc.GetBaseAttributeScore("INT") + 2);
                    pc.SetBaseAttributeScore("SOC", pc.GetBaseAttributeScore("SOC") + 1);
                    break;
                case StarSign.Scorpio:
                    pc.SetBaseAttributeScore("CHA", pc.GetBaseAttributeScore("CHA") - 2);
                    pc.SetBaseAttributeScore("SEL", pc.GetBaseAttributeScore("SEL") + 1);
                    pc.SetBaseAttributeScore("BRV", pc.GetBaseAttributeScore("BRV") + 1);
                    pc.SetBaseAttributeScore("INT", pc.GetBaseAttributeScore("INT") - 2);
                    pc.SetBaseAttributeScore("FTH", pc.GetBaseAttributeScore("FTH") + 1);
                    break;
                case StarSign.Sagittarius:
                    pc.SetBaseAttributeScore("PHY", pc.GetBaseAttributeScore("PHY") + 1);
                    pc.SetBaseAttributeScore("AGI", pc.GetBaseAttributeScore("AGI") + 2);
                    pc.SetBaseAttributeScore("CHA", pc.GetBaseAttributeScore("CHA") + 1);
                    pc.SetBaseAttributeScore("LUS", pc.GetBaseAttributeScore("LUS") + 1);
                    pc.SetBaseAttributeScore("SOC", pc.GetBaseAttributeScore("SOC") + 1);
                    break;
                case StarSign.Capricorn:
                    pc.SetBaseAttributeScore("PHY", pc.GetBaseAttributeScore("PHY") + 1);
                    pc.SetBaseAttributeScore("END", pc.GetBaseAttributeScore("END") - 1);
                    pc.SetBaseAttributeScore("CHA", pc.GetBaseAttributeScore("CHA") - 1);
                    pc.SetBaseAttributeScore("GRE", pc.GetBaseAttributeScore("GRE") - 1);
                    pc.SetBaseAttributeScore("LUS", pc.GetBaseAttributeScore("LUS") - 1);
                    pc.SetBaseAttributeScore("BRV", pc.GetBaseAttributeScore("BRV") + 1);
                    pc.SetBaseAttributeScore("INT", pc.GetBaseAttributeScore("INT") - 1);
                    break;
            }
            if (pc.GetBaseAttributeScore("PHY") < 3)
            {
                pc.SetBaseAttributeScore("PHY", 3);
            }
            else if (pc.GetBaseAttributeScore("PHY") > 18)
            {
                pc.SetBaseAttributeScore("PHY", 18);
            }
            if (pc.GetBaseAttributeScore("AGI") < 3)
            {
                pc.SetBaseAttributeScore("AGI", 3);
            }
            else if (pc.GetBaseAttributeScore("AGI") > 18)
            {
                pc.SetBaseAttributeScore("AGI", 18);
            }
            if (pc.GetBaseAttributeScore("END") < 3)
            {
                pc.SetBaseAttributeScore("END", 3);
            }
            else if (pc.GetBaseAttributeScore("END") > 18)
            {
                pc.SetBaseAttributeScore("END", 18);
            }
            if (pc.GetBaseAttributeScore("BRV") < 3)
            {
                pc.SetBaseAttributeScore("BRV", 3);
            }
            else if (pc.GetBaseAttributeScore("BRV") > 18)
            {
                pc.SetBaseAttributeScore("BRV", 18);
            }
            if (pc.GetBaseAttributeScore("CHA") < 3)
            {
                pc.SetBaseAttributeScore("CHA", 3);
            }
            else if (pc.GetBaseAttributeScore("CHA") > 18)
            {
                pc.SetBaseAttributeScore("CHA", 18);
            }
            if (pc.GetBaseAttributeScore("INT") < 3)
            {
                pc.SetBaseAttributeScore("INT", 3);
            }
            else if (pc.GetBaseAttributeScore("INT") > 18)
            {
                pc.SetBaseAttributeScore("INT", 18);
            }
            if (pc.GetBaseAttributeScore("GRE") < 3)
            {
                pc.SetBaseAttributeScore("GRE", 3);
            }
            else if (pc.GetBaseAttributeScore("GRE") > 18)
            {
                pc.SetBaseAttributeScore("GRE", 18);
            }
            if (pc.GetBaseAttributeScore("SEL") < 3)
            {
                pc.SetBaseAttributeScore("SEL", 3);
            }
            else if (pc.GetBaseAttributeScore("SEL") > 18)
            {
                pc.SetBaseAttributeScore("SEL", 18);
            }
            if (pc.GetBaseAttributeScore("LUS") < 3)
            {
                pc.SetBaseAttributeScore("LUS", 3);
            }
            else if (pc.GetBaseAttributeScore("LUS") > 18)
            {
                pc.SetBaseAttributeScore("LUS", 18);
            }
            if (pc.GetBaseAttributeScore("FTH") < 3)
            {
                pc.SetBaseAttributeScore("FTH", 3);
            }
            else if (pc.GetBaseAttributeScore("FTH") > 18)
            {
                pc.SetBaseAttributeScore("FTH", 18);
            }
            if (pc.GetBaseAttributeScore("PIE") < 3)
            {
                pc.SetBaseAttributeScore("PIE", 3);
            }
            else if (pc.GetBaseAttributeScore("PIE") > 18)
            {
                pc.SetBaseAttributeScore("PIE", 18);
            }
            if (pc.GetBaseAttributeScore("MAN") < 3)
            {
                pc.SetBaseAttributeScore("MAN", 3);
            }
            else if (pc.GetBaseAttributeScore("MAN") > 18)
            {
                pc.SetBaseAttributeScore("MAN", 18);
            }
            if (pc.GetBaseAttributeScore("SOC") < 3)
            {
                pc.SetBaseAttributeScore("SOC", 3);
            }
            else if (pc.GetBaseAttributeScore("SOC") > 18)
            {
                pc.SetBaseAttributeScore("SOC", 18);
            }
        }
        private void RollScores(FWCharacter pc)
        {
            pc.SetBaseAttributeScore("PHY", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("AGI", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("END", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("BRV", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("CHA", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("INT", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("GRE", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("SEL", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("LUS", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("FTH", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("PIE", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("MAN", Diceroller.Instance.RollXdY(3, 6));
            pc.SetBaseAttributeScore("SOC", Diceroller.Instance.RollXdY(3, 6));
        }
    }
}
