using RPGBase.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.UI._2D;
using WoFM.UI.GlobalControllers;
using WoFM.UI.SceneControllers;

namespace WoFM.Flyweights.Actions
{
    public class MessageAction : IGameAction
    {
        private string message;
        private int level;
        private bool resolved = false;
        /// <summary>
        /// Creates a new instance of <see cref="MoveIoSpeedyAction"/>.
        /// </summary>
        /// <param name="i">the IO being moved</param>
        /// <param name="v">the IO's destination</param>
        public MessageAction(string m, int l)
        {
            message = m;
            level = l;
        }
        public void Execute()
        {
            if (!resolved)
            {
                Messages.Instance.SendMessage(message, level);
                resolved = true;
            }
        }
        public bool IsResolved()
        {
            return resolved;
        }
    }
}
