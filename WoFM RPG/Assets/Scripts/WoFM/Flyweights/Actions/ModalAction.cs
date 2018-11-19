using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.UI._2D;
using WoFM.UI.GlobalControllers;
using WoFM.UI.SceneControllers;
using WoFM.UI.Widgets;

namespace WoFM.Flyweights.Actions
{
    public class ModalAction : IGameAction
    {
        private ModalPanelDetails modalPanelDetails;
        private bool resolved = false;
        public ModalAction(ModalPanelDetails mpd)
        {
            modalPanelDetails = mpd;
        }

        public void Execute()
        {
            if (!resolved)
            {
                ModalPanel.Instance.NewChoice(modalPanelDetails);
                resolved = true;
            }
        }
        public bool IsResolved()
        {
            return resolved;
        }
    }
}
