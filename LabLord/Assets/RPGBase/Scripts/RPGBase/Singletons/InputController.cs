using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Singletons
{
    public class InputController : Singleton<InputController>
    {
        public bool CONTROLS_FROZEN = false;
    }
}
