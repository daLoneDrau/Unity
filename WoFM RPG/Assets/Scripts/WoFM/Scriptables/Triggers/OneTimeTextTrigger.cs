using RPGBase.Constants;
using RPGBase.Scripts.UI._2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.UI.GlobalControllers;
using WoFM.UI.SceneControllers;
using WoFM.UI.Widgets;

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
                if (HasLocalVariable("needs_modal")
                    && GetLocalIntVariableValue("needs_modal") == 1)
                {
                    ModalPanelDetails mpd = new ModalPanelDetails()
                    {
                        content = GameController.Instance.GetText(GetLocalStringVariableValue("text")),
                        button1Details = new EventButtonDetails()
                        {
                            buttonTitle = "Okay"
                        }
                    };
                    if (HasLocalVariable("modal_sprite")) {
                        mpd.iconImage = SpriteMap.Instance.GetSprite(GetLocalStringVariableValue("modal_sprite"));
                    }
                    ModalPanel.Instance.NewChoice(mpd);
                }
            }
            return ScriptConsts.ACCEPT;
        }
    }
}
