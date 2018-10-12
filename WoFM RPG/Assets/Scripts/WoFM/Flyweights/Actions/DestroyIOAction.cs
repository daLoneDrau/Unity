using RPGBase.Constants;
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
    public class DestroyIOAction : IGameAction
    {
        private int ioid;
        private bool resolved = false;
        /// <summary>
        /// Creates a new instance of <see cref="DestroyIOAction"/>.
        /// </summary>
        /// <param name="id">the IO being destroyed</param>
        public DestroyIOAction(int id)
        {
            ioid = id;
        }
        public void Execute()
        {
            if (!resolved)
            {
                // start to remove
                Interactive.Instance.DestroyIO(Interactive.Instance.GetIO(ioid));
                resolved = true;
            }
        }
        public bool IsResolved()
        {
            return resolved;
        }
    }
}
