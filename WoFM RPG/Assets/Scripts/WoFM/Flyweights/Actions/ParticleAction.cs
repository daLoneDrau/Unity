using RPGBase.Constants;
using RPGBase.Scripts.UI._2D;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using WoFM.Singletons;
using WoFM.UI.GlobalControllers;
using WoFM.UI.SceneControllers;

namespace WoFM.Flyweights.Actions
{
    public class ParticleAction : IGameAction
    {
        /// <summary>
        /// the particle emitter that plays due to the action.
        /// </summary>
        private MethodInfo method;
        /// <summary>
        /// the list of parameters applied when calling the particle emitter.
        /// </summary>
        private object[] parameters;
        /// <summary>
        /// Creates a new instance of <see cref="ParticleAction"/>.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="args"></param>
        public ParticleAction(MethodInfo m, params object[] args)
        {
            method = m;
            parameters = args;
        }
        public void Execute()
        {
            method.Invoke(Particles.Instance, parameters);
        }
        public bool IsResolved()
        {
            return true;
        }
    }
}
