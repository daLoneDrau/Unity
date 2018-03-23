using Assets.Scripts.BarbarianPrince.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Scriptables.Mobs
{
    public class CalArath: MobBase
    {
        public override int OnInit()
        {
            Console.WriteLine("CalArath oninit");
            BPCharacter pc = (BPCharacter)Io.PcData;
            pc.SetBaseAttributeScore("CS", 0);
            pc.SetBaseAttributeScore("EN", 9);
            return base.OnInit();
        }
    }
}
