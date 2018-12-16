using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WoFM.Flyweights;

namespace WoFM.Scriptables.Items
{
    public class ItemBase : WoFMScriptable
    {
        public override int OnInit()
        {
            return base.OnInit();
        }
        public override int OnInView()
        {
            Io.Show = 1;
            return base.OnInView();
        }
        public override int OnOutOfView()
        {
            Io.Show = 0;
            return base.OnOutOfView();
        }
    }
}
