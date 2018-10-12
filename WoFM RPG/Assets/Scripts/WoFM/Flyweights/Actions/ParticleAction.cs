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
        /// the reference id of the IO associated with the particle system
        /// </summary>
        private int ioid = -1;
        /// <summary>
        /// the particle emitter that plays due to the action.
        /// </summary>
        private MethodInfo method;
        /// <summary>
        /// the location where the particle system emits.
        /// </summary>
        private Vector3 position;
        /// <summary>
        /// Creates a new instance of <see cref="ParticleAction"/>.
        /// </summary>
        /// <param name="m">the particle emitter that plays due to the action</param>
        /// <param name="v">the location where the particles emit</param>
        public ParticleAction(MethodInfo m, Vector3 v)
        {
            method = m;
            position = v;
        }
        /// <summary>
        /// Creates a new instance of <see cref="ParticleAction"/>.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="id"></param>
        public ParticleAction(MethodInfo m, int id)
        {
            method = m;
            ioid = id;
        }
        public void Execute()
        {
            if (ioid == -1)
            {
                method.Invoke(Particles.Instance, new object[] { position });
            }
            else
            {
                method.Invoke(Particles.Instance, new object[] { ioid });
            }
        }
        public bool IsResolved()
        {
            return true;
        }
    }
}
