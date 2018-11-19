using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections;
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
    public class WaitAction : IGameAction
    {
        private float time;
        public bool Resolved { private get; set; }
        public WaitAction(float t)
        {
            time = t;
        }
        public void Execute()
        {
            if (!Resolved)
            {
                Debug.Log("waiting ");
                GameSceneController.Instance.WaitForSomeTime(time, this);
            }
        }
        public bool IsResolved()
        {
            return Resolved;
        }
    }
}
