using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WoFM.UI._2D
{
    public class Blocker : MonoBehaviour
    {
        public const int VOID = 0;
        public const int WALL = 1;
        public int Type { get; set; }
    }
}
