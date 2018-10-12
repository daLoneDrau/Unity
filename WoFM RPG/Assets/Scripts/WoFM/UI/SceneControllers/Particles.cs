﻿using RPGBase.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;

namespace WoFM.UI.SceneControllers
{
    public class Particles : Singleton<Particles>
    {
        /// <summary>
        /// plays a *bonk* message that rises above the player unit.
        /// </summary>
        public ParticleSystem bonker;
        /// <summary>
        /// plays a *locked* particle that appears above the assigned unit.
        /// </summary>
        public ParticleSystem locked;
        /// <summary>
        /// plays a *z* series that spirals above the assigned unit.
        /// </summary>
        public ParticleSystem snorer;
        /// <summary>
        /// plays a *help* message that shouts into the screen.
        /// </summary>
        public ParticleSystem helper;
        /// <summary>
        /// plays a *snort* message to indicate a snoring unit has woken up.
        /// </summary>
        public ParticleSystem snorter;
        /// <summary>
        /// Co-routine to move the particle animator off-screen once it is finished.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FinishParticles(ParticleSystem particles)
        {
            // loop until particle is gone
            while (particles.IsAlive(true))
            {
                yield return null;
            }
            // move back off-screen
            particles.transform.position = new Vector3(-1, 0, 0);
        }
        /// <summary>
        /// Plays the *bonk* animation above a specific location.
        /// </summary>
        /// <param name="location">the location where the animation plays above</param>
        public void PlayBonk(Vector3 location)
        {
            bonker.transform.position = location + new Vector3(0, .625f, 0);
            bonker.Play();
            StartCoroutine(FinishParticles(bonker));
        }
        public void PlayBonkAboveIo(int ioid)
        {
            WoFMInteractiveObject io = (WoFMInteractiveObject)Interactive.Instance.GetIO(ioid);
            bonker.transform.position = io.transform.position + new Vector3(0, .625f, 0);
            bonker.Play();
            StartCoroutine(FinishParticles(bonker));
        }
        /// <summary>
        /// Plays the *locked* animation above a specific location.
        /// </summary>
        /// <param name="location">the location where the animation plays above</param>
        public void PlayLocked(Vector3 location)
        {
            locked.transform.position = location + new Vector3(0, .625f, 0);
            locked.Play();
            StartCoroutine(FinishParticles(locked));
        }
    }
}
