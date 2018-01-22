using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    interface IRPGLayoutHandler : ILayoutSelfController
    {
        void Configure();
        Vector2 GetPreferredSize();
        void Resize();
        void PlaceChildren();
    }
}
