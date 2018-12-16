using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.UI.GlobalControllers;

namespace WoFM.UI._2D
{
    public class FreezeThaw:MonoBehaviour
    {
        public void Freeze()
        {
            GameSceneController.Instance.CONTROLS_FROZEN = true;
        }
        public void Thaw()
        {
            GameSceneController.Instance.CONTROLS_FROZEN = false;
        }
    }
}
