using RPGBase.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.UI.GlobalControllers;
using WoFM.UI.SceneControllers;

namespace WoFM.Scriptables.Triggers
{
    public class OneTimeTextTrigger : WoFMScriptable
    {
        public override int OnEnterTrigger()
        {
            if (HasLocalVariable("fired")
                && GetLocalIntVariableValue("fired") == 1)
            {
                // script already fired. do nothing
            }
            else
            {
                Messages.Instance.SendMessage(GameController.Instance.GetText(GetLocalStringVariableValue("text")));
                SetLocalVariable("fired", 1);
            }
            return ScriptConsts.ACCEPT;
        }
    }
}
