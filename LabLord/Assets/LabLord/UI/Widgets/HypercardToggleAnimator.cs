using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace LabLord.UI.Widgets
{
    public class HypercardToggleAnimator : MonoBehaviour
    {
        private Toggle toggle;
        private Animator animator;
        void Awake()
        {
            //Fetch the Toggle GameObject
            animator = GetComponent<Animator>();
            toggle = GetComponent<Toggle>();
            //Add listener for when the state of the Toggle changes, and output the state
            toggle.onValueChanged.AddListener(delegate {
                ToggleValueChanged(toggle);
            });
        }
        //Output the new state of the Toggle into Text when the user uses the Toggle
        void ToggleValueChanged(Toggle change)
        {
            if (toggle.isOn)
            {
                animator.Play("Selected");
            }
            else
            {
                animator.Play("Unselected");
            }
        }
    }
}
