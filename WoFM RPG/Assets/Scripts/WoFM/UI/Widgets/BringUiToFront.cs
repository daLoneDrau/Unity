using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WoFM.UI.Widgets
{
    /// <summary>
    /// Class definition for UI entities that must be displayed above all others.
    /// </summary>
    public class BringUiToFront : MonoBehaviour
    {
        void OnEnable()
        {
            transform.SetAsLastSibling();
        }
    }
}
