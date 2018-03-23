using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.BarbarianPrince.UI.Controllers
{
    public class DemoController : MonoBehaviour
    {
        private static DemoController instance;
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static DemoController Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject
                    {
                        name = "DemoController"
                    };
                    instance = go.AddComponent<DemoController>();
                }
                return instance;
            }
        }
    }
}
