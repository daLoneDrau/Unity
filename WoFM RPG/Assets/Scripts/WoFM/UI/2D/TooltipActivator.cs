using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.UI.GlobalControllers;

namespace WoFM.UI._2D
{
    public class TooltipActivator : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="Animator"/> that controls the tooltip display.
        /// </summary>
        public Animator Animator;
        public void HideTooltip()
        {
            print("Hide tooltip");
            Animator.Play("Hide Tooltip");
        }
        public void ShowTooltip()
        {
            if (!GameSceneController.Instance.CONTROLS_FROZEN)
            {
                print("Show tooltip");
                Animator.Play("Show Tooltip");
            }
        }
    }
}
