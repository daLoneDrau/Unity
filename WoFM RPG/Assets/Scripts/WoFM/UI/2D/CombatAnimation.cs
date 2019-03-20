using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.UI.SceneControllers;

namespace WoFM.UI._2D
{
    public class CombatAnimation:MonoBehaviour
    {
        public void AnimationFinished()
        {
            print("*****************AnimationFinished " + gameObject.name);
            CombatController.Instance.FinishRound(gameObject.name);
        }
    }
}
