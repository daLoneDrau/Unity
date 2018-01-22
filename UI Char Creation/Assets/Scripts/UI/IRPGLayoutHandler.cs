using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    interface IRPGLayoutHandler : ILayoutSelfController
    {
        void UpdateRect();
        Vector2 GetPreferredSize();
    }
}
