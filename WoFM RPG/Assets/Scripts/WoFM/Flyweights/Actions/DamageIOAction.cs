using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.UI._2D;
using WoFM.UI.GlobalControllers;
using WoFM.UI.SceneControllers;

namespace WoFM.Flyweights.Actions
{
    public class DamageIOAction : IGameAction
    {
        private int ioid;
        private bool resolved = false;
        private int damages;
        private int type;
        private int sourceIoid;
        public DamageIOAction(int id, int v1, int v2, int v3)
        {
            ioid = id;
            this.damages = v1;
            this.type = v2;
            this.sourceIoid = v3;
        }

        public void Execute()
        {
            if (!resolved)
            {
                Debug.Log("damage action!");
                // start to remove
                BaseInteractiveObject io = Interactive.Instance.GetIO(ioid);
                if (io != null)
                {
                    Debug.Log("io not null");
                    if (io.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        Debug.Log("damage player");
                        io.PcData.DamagePlayer(damages, type, sourceIoid);
                    }
                }
                resolved = true;
            }
        }
        public bool IsResolved()
        {
            return resolved;
        }
    }
}
