using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using WoFM.Flyweights;
using WoFM.Singletons;
using WoFM.UI.GlobalControllers;

namespace WoFM.UI.SceneControllers
{
    public class CombatController : Singleton<CombatController>, IWatcher
    {
        /// <summary>
        /// the animation controlling the marquee.
        /// </summary>
        public Animator MarqueeAnimation;
        public Text Marquee;
        /// <summary>
        /// the list of enemies.
        /// </summary>
        private int[] enemies = new int[0] { };
        public GameObject[] Enemies;
        /// <summary>
        /// Guarantee this will be always a singleton only - can't use the constructor!  Use this for initialization.
        /// </summary>
        protected CombatController() { }
        public void AddEnemies(params int[] list)
        {
            for (int i = 0, li = list.Length; i < li; i++)
            {
                enemies = ArrayUtilities.Instance.ExtendArray(list[i], enemies);
            }
        }
        public void StartCombat()
        {
            print("starting combat");
            Marquee.text = "Round 1";
            gameObject.SetActive(true);
            MarqueeAnimation.Play("ShrinkingMarquee");
            for (int i = 0, li = Enemies.Length; i < li; i++)
            {
                if (i < enemies.Length)
                {
                    WoFMInteractiveObject io = (WoFMInteractiveObject)Interactive.Instance.GetIO(enemies[i]);
                    Enemies[i].transform.GetChild(1).GetComponent<Image>().sprite = io.Sprite;
                }
                else
                {
                    Enemies[i].SetActive(false);
                }
            }
        }
        public void CombatSequence()
        {
            WoFMInteractiveObject pc = ((WoFMInteractive)Interactive.Instance).GetPlayerIO();
            pc.PcData.ComputeFullStats();
            int playerAttackStrength = Diceroller.Instance.RollXdY(2, 6) + (int)pc.PcData.GetFullAttributeScore("SKL");

            WoFMInteractiveObject npc = (WoFMInteractiveObject)Interactive.Instance.GetIO(enemies[0]);
            npc.NpcData.ComputeFullStats();
            int creatureAttackStrength = Diceroller.Instance.RollXdY(2, 6) + (int)npc.NpcData.GetFullAttributeScore("SKL");

            if (playerAttackStrength > creatureAttackStrength)
            {
                // player wounds creature.  show buttons for regular attack or wild attack
            }
            else if (playerAttackStrength < creatureAttackStrength)
            {
                // creature wounds player.  show buttons for regular defense or hard defense
            }
            else
            {
                // both dodge. show buttons for regular attack, regular defense
            }
        }
        public void WatchUpdated(Watchable data)
        {
            throw new NotImplementedException();
        }
        #region MonoBehavior
        public void Awake()
        {
            print(Instance); // call the instance before disabling the game object
            gameObject.SetActive(false);
        }
        public void OnEnable()
        {
            transform.SetAsLastSibling();
        }
        #endregion
    }
}
