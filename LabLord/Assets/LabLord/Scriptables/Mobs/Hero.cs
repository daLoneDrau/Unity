using RPGBase.Constants;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LabLord.Flyweights;
using LabLord.Constants;
using LabLord.UI.SceneControllers;

namespace LabLord.Scriptables.Mobs
{
    public class Hero : MobBase
    {
        public override int OnInit()
        {
            Console.WriteLine("Hero oninit");
            Debug.Log("Hero ONINIT");
            LabLordCharacter pc = (LabLordCharacter)Io.PcData;
            // roll stats
            OnCharWizardStepOne();
            // equip sword
            //BPServiceClient.Instance.GetItemByName("Bonebiter");
            return base.OnInit();
        }
        public override int OnCharWizardStepOne()
        {
            LabLordCharacter pc = (LabLordCharacter)Io.PcData;
            do
            {
                pc.SetBaseAttributeScore("STR", Diceroller.Instance.RollXdY(3, 6));
                pc.SetBaseAttributeScore("DEX", Diceroller.Instance.RollXdY(3, 6));
                pc.SetBaseAttributeScore("CON", Diceroller.Instance.RollXdY(3, 6));
                pc.SetBaseAttributeScore("INT", Diceroller.Instance.RollXdY(3, 6));
                pc.SetBaseAttributeScore("WIS", Diceroller.Instance.RollXdY(3, 6));
                pc.SetBaseAttributeScore("CHA", Diceroller.Instance.RollXdY(3, 6));
            } while (CharBuilderController.Instance.GetValidRaces() == 0);

            /*
            pc.ComputeFullStats();
            Debug.Log("HEAL");
            pc.HealPlayer(999, true);
            */
            pc = null;
            CharBuilderController.Instance.EnableRaces();
            return base.OnCharWizardStepOne();
        }
    }
}
