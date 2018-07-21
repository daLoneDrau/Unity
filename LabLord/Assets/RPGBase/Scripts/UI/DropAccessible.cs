using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public interface IDropAccessible
    {
        void HandleDrop();
        GameObject GetGameObject();
    }
}
