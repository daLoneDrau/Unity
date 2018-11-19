using RPGBase.Constants;
using RPGBase.Scripts.UI._2D;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.Scriptables.Mobs;
using WoFM.Singletons;
using WoFM.UI.GlobalControllers;
using WoFM.UI.SceneControllers;
using WoFM.UI.Widgets;

namespace WoFM.Scriptables.Triggers
{
    public class Trigger71 : WoFMScriptable
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
                // create orc io
                WoFMInteractiveObject io = GameController.Instance.NewMob("orc71", "orc_0", new Orc71()).GetComponent<WoFMInteractiveObject>();
                // send message
                Messages.Instance.SendMessage(GameController.Instance.GetText(GetLocalStringVariableValue("text")));
                // show modal
                ModalPanel.Instance.NewChoice(new ModalPanelDetails()
                {
                    content = GameController.Instance.GetText(GetLocalStringVariableValue("text")),
                    iconImage = SpriteMap.Instance.GetSprite("icon_orc_face"),
                    button1Details = new EventButtonDetails()
                    {
                        buttonTitle = "Tiptoe past him...",
                        action = () => { FireHearEvent(io); }
                    },
                    button2Details = new EventButtonDetails()
                    {
                        buttonTitle = "Attack!",
                        action = () => { FireWakeEvent(io); }
                    }
                });
                SetLocalVariable("fired", 1);
            }
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// Fires a 'hear' event for the Orc after the modal clears.
        /// </summary>
        /// <param name="io">the Orc's IO</param>
        public void FireHearEvent(WoFMInteractiveObject io)
        {
            io.Script.RemoveDisallowedEvent(ScriptConsts.DISABLE_HEAR);
            Script.Instance.EventSender = ((WoFMInteractive)Interactive.Instance).GetPlayerIO();
            Script.Instance.SendIOScriptEvent(io, ScriptConsts.SM_046_HEAR, null, null);
        }
        /// <summary>
        /// Fires a 'hear' event for the Orc after the modal clears.
        /// </summary>
        /// <param name="io">the Orc's IO</param>
        public void FireWakeEvent(WoFMInteractiveObject io)
        {
            io.Script.RemoveDisallowedEvent(ScriptConsts.DISABLE_HEAR);
            io.Script.SetLocalVariable("force_hear", 1);
            Script.Instance.EventSender = ((WoFMInteractive)Interactive.Instance).GetPlayerIO();
            Script.Instance.SendIOScriptEvent(io, ScriptConsts.SM_046_HEAR, null, null);
        }
    }
}
