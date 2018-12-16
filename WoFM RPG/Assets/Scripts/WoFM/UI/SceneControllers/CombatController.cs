using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using WoFM.Flyweights;
using WoFM.Singletons;
using WoFM.UI.GlobalControllers;
using WoFM.UI.Tooltips;

namespace WoFM.UI.SceneControllers
{
    public class CombatController : Singleton<CombatController>, IWatcher
    {
        /// <summary>
        /// the length of a full gauge bar.
        /// </summary>
        private float barLen;
        /// <summary>
        /// used to make movement calculations more efficient.
        /// </summary>
        private float inverseMoveTime;
        /// <summary>
        /// the minimum x-anchor position of a gauge
        /// </summary>
        public float MinX;
        /// <summary>
        /// the maximum x-anchor position of a gauge
        /// </summary>
        public float MaxX;
        /// <summary>
        /// the time it will take the object to perform a move, in seconds.
        /// </summary>
        public float moveTime;
        private int roundNumber;
        public GameObject AttackPanel;
        public GameObject DefensePanel;
        public GameObject NeutralPanel;
        /// <summary>
        /// The Stamina gauge.
        /// </summary>
        public GameObject PlayerGauge;
        /// <summary>
        /// The Stamina gauge.
        /// </summary>
        public GameObject[] EnemyGauges;
        /// <summary>
        /// the animation controlling the marquee.
        /// </summary>
        public Animator MarqueeAnimation;
        public Animator SlashAnimation;
        public GameObject Slash;
        /// <summary>
        /// The text displayed showing the character's options.
        /// </summary>
        public Text RoundMessages;
        public Text Marquee;
        /// <summary>
        /// the list of enemies.
        /// </summary>
        private int[] enemies = new int[0] { };
        public GameObject[] Enemies;
        public GameObject Hero;
        private bool roundStarted;
        /// <summary>
        /// Guarantee this will be always a singleton only - can't use the constructor!  Use this for initialization.
        /// </summary>
        protected CombatController() { }
        private int animationsToComplete;
        #region Combat Setup
        public void AddEnemies(params int[] list)
        {
            for (int i = 0, li = list.Length; i < li; i++)
            {
                enemies = ArrayUtilities.Instance.ExtendArray(list[i], enemies);
                Interactive.Instance.GetIO(list[i]).NpcData.AddWatcher(this);
            }
        }
        public void StartCombat()
        {
            print("starting combat");
            // set global combat variables
            Script.Instance.SetGlobalVariable("COMBAT_ROUNDS", 0);
            Script.Instance.SetGlobalVariable("COMBAT_ON", 1);
            // clear 
            Messages.Instance.Clear();
            roundNumber = 1;
            gameObject.SetActive(true);
            for (int i = 0, li = Enemies.Length; i < li; i++)
            {
                if (i < enemies.Length)
                {
                    WoFMInteractiveObject io = (WoFMInteractiveObject)Interactive.Instance.GetIO(enemies[i]);
                    Enemies[i].transform.GetChild(1).GetComponent<Image>().sprite = io.Sprite;
                    Enemies[i].GetComponent<InteractiveTooltipWidgetStatbar>().IoId = io.RefId;
                    io = null;
                }
                else
                {
                    Enemies[i].SetActive(false);
                }
            }
            WoFMInteractiveObject pio = ((WoFMInteractive)Interactive.Instance).GetPlayerIO();
            Hero.transform.GetChild(1).GetComponent<Image>().sprite = pio.Sprite;
            Hero.GetComponent<InteractiveTooltipWidgetStatbar>().IoId = pio.RefId;
            pio.PcData.ComputeFullStats();
            pio.PcData.NotifyWatchers();
            for (int i = enemies.Length - 1; i >= 0; i--)
            {
                WoFMInteractiveObject io = (WoFMInteractiveObject)Interactive.Instance.GetIO(enemies[i]);
                io.NpcData.ComputeFullStats();
                io.NpcData.NotifyWatchers();
                io = null;
            }
            StartCoroutine(CombatSequence());
        }
        #endregion
        #region Combat Options
        public void Attack()
        {
            roundStarted = true;
            AttackPanel.SetActive(false);
            DefensePanel.SetActive(false);
            NeutralPanel.SetActive(false);
            WoFMInteractiveObject io = (WoFMInteractiveObject)Interactive.Instance.GetIO(enemies[0]);
            // get damage from player's current weapon
            float damage = Combat.Instance.ComputeDamages(
                ((WoFMInteractive)Interactive.Instance).GetPlayerIO(), // source
                null, // weapon - not needed
                Interactive.Instance.GetIO(enemies[0]), // target
                0); // no flags since luck wasn't used
            // need 1 animation for swing
            animationsToComplete = 1;
            if (damage > 0)
            {
                // add an extra animation to update the gauge
                animationsToComplete++;
            }
            print("damaged for " + damage);
        }
        public void Defend()
        {
            roundStarted = true;
            AttackPanel.SetActive(false);
            DefensePanel.SetActive(false);
            NeutralPanel.SetActive(false);
            WoFMInteractiveObject io = (WoFMInteractiveObject)Interactive.Instance.GetIO(enemies[0]);
            // get damage from player's current weapon
            float damage = Combat.Instance.ComputeDamages(
                Interactive.Instance.GetIO(enemies[0]), // source
                null, // weapon - not needed
                ((WoFMInteractive)Interactive.Instance).GetPlayerIO(), // target
                0); // no flags since luck wasn't used
            // need 1 animation for swing
            animationsToComplete = 1;
            if (damage > 0)
            {
                // add an extra animation to update the gauge
                animationsToComplete++;
            }
            print("damaged for " + damage);
        }
        public void ToughDefend()
        {
            roundStarted = true;
            AttackPanel.SetActive(false);
            DefensePanel.SetActive(false);
            NeutralPanel.SetActive(false);
            print("defendefen");
        }
        public void WildAttack()
        {
            roundStarted = true;
            AttackPanel.SetActive(false);
            DefensePanel.SetActive(false);
            NeutralPanel.SetActive(false);
            print("attakattak");
        }
        #endregion
        #region Combat Animations
        public void PlayHit(int ioid, float damages)
        {
            int index = -1;
            for (int i = enemies.Length - 1; i >= 0; i--)
            {
                if (enemies[i] == ioid)
                {
                    index = i;
                    break;
                }
            }
            if (index > -1)
            {
                PlayCombatAnimation(Enemies[index].GetComponent<RectTransform>(), Slash.transform, "Hit Left");
                // TODO - change animation based on hit strength
            }
            else
            {
                PlayCombatAnimation(Hero.GetComponent<RectTransform>(), Slash.transform, "Hit Right");
                // TODO - change animation based on hit strength
            }
        }
        /// <summary>
        /// Checks to see if an enemy's stamina bar needs to be updated.
        /// </summary>
        /// <param name="ioData"></param>
        private void CheckForEnemyStaminaUpdate(IONpcData ioData)
        {
            print("checking enemy stam");
            int index = -1;
            for (int i = enemies.Length - 1; i >= 0; i--)
            {
                if (enemies[i] == ioData.Io.RefId)
                {
                    index = i;
                    break;
                }
            }
            print("found enemy at index " + index);
            RectTransform rt = EnemyGauges[index].GetComponent<RectTransform>();
            float currentBarLen = rt.anchorMax.x - MinX;
            float realPercent = ioData.Life / 24f;
            print("real percent is " + realPercent);
            if (realPercent > 1)
            {
                realPercent = 1f;
            }
            float realBarLen = realPercent * barLen;
            if (currentBarLen != realBarLen)
            {
                print("need to update stam");
                StartCoroutine(UpdateGuage(EnemyGauges[index], realPercent));
            }
        }
        /// <summary>
        /// Plays a combat animation over the target.
        /// </summary>
        /// <param name="target">the target transform</param>
        /// <param name="animationObject">the animation's container object</param>
        /// <param name="animationclip">the name of the animation clip</param>
        private void PlayCombatAnimation(RectTransform target, Transform animationObject, string animationclip)
        {
            animationObject.SetAsLastSibling();
            if (SlashAnimation != null)
            {
                SlashAnimation.StopPlayback();
            }
            gameObject.SetActive(true);
            // get parent's height
            float parentHeight = target.sizeDelta.y;
            // get Slash height
            float myheight = animationObject.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta.y;

            // assume orientation is above.
            Canvas c = transform.root.GetComponent<Canvas>();
            Vector3 cPos = c.transform.InverseTransformPoint(target.position);

            // inverse parent position + 1/2 parent height + 1/2 tooltip height. puts tooltip frame at top of parent
            // cPos.y += (parentHeight / 2) + (myheight / 2);

            // pivot for Slash object is now at bottom of parent container
            cPos.y -= parentHeight / 2;
            cPos.y += myheight / 2;
            animationObject.localPosition = cPos;
            if (SlashAnimation != null)
            {
                SlashAnimation.Play(animationclip);
            }
        }
        #endregion
        public IEnumerator CombatSequence()
        {
            // de-activate all panels;
            AttackPanel.SetActive(false);
            DefensePanel.SetActive(false);
            NeutralPanel.SetActive(false);
            animationsToComplete = 0;

            // play marquee
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            // display current round
            sb.Append("Round ");
            sb.Append(roundNumber);
            Marquee.text = sb.ToString();
            sb.Length = 0;
            MarqueeAnimation.Play("ShrinkingMarquee");
            // increment round number after setting global variable
            Script.Instance.SetGlobalVariable("COMBAT_ROUNDS", roundNumber++);

            // wait for marquee to finish
            yield return new WaitForSeconds(1.4f);

            // calculate round actions
            WoFMInteractiveObject pc = ((WoFMInteractive)Interactive.Instance).GetPlayerIO();

            WoFMInteractiveObject npc = (WoFMInteractiveObject)Interactive.Instance.GetIO(enemies[0]);

            // activate action panels and wait for input
            switch (((WoFMCombat)Combat.Instance).StrikeCheck(pc, npc))
            {
                case WoFMCombat.PLAYER_WOUNDS_CREATURE:
                    Messages.Instance.SendMessage("Player wins initiative. Attack!");
                    AttackPanel.SetActive(true);
                    break;
                case WoFMCombat.CREATURE_WOUNDS_PLAYER:
                    sb.Append(Interactive.Instance.GetIO(enemies[0]).NpcData.Name);
                    sb.Append(" wins initiative! Defend!");
                    Messages.Instance.SendMessage(sb.ToString());
                    DefensePanel.SetActive(true);
                    break;
                default:
                    sb.Append(Interactive.Instance.GetIO(enemies[0]).NpcData.Name);
                    sb.Append(" eyes you warily... Choose: Attack or Defend.");
                    Messages.Instance.SendMessage(sb.ToString());
                    NeutralPanel.SetActive(true);
                    break;
            }
            sb.ReturnToPool();
            pc = null;
            npc = null;
            sb = null;
        }
        public void FinishRound()
        {
            animationsToComplete--;
            if (animationsToComplete <= 0
                && roundStarted)
            {
                roundStarted = false;
                // TODO - check to see if combat is over
                bool combatOver = false;
                if (!combatOver)
                {
                    // go to next round
                    StartCoroutine(CombatSequence());
                }
            }
            else
            {
                print("have " + animationsToComplete + " left");
            }
        }
        public void WatchUpdated(Watchable data)
        {
            if (gameObject.activeInHierarchy)
            {
                if (data is IOPcData)
                {
                    CheckForPlayerStaminaUpdate((IOPcData)data);
                }
                else if (data is IONpcData)
                {
                    print("an enemy has been updated");
                    CheckForEnemyStaminaUpdate((IONpcData)data);
                }
            }
        }
        private void CheckForPlayerStaminaUpdate(IOPcData ioData)
        {
            RectTransform rt = PlayerGauge.GetComponent<RectTransform>();
            float currentBarLen = rt.anchorMax.x - MinX;
            float realPercent = ioData.Life / 24f;
            if (realPercent > 1)
            {
                realPercent = 1f;
            }
            float realBarLen = realPercent * barLen;
            if (currentBarLen != realBarLen)
            {
                StartCoroutine(UpdateGuage(PlayerGauge, realPercent));
            }
        }
        private IEnumerator UpdateGuage(GameObject gauge, float val)
        {
            float realVal = barLen * val;
            realVal += MinX;
            RectTransform rt = gauge.GetComponent<RectTransform>();
            float remaining = realVal - rt.anchorMax.x;
            // while remaining distance still not 0
            while (Mathf.Abs(remaining) > float.Epsilon)
            {
                // find a position proportionally closer to the end based on the move time.
                Vector2 newPosition = Vector2.MoveTowards(new Vector2(rt.anchorMax.x, 1), new Vector2(realVal, 1), inverseMoveTime * Time.deltaTime);
                // update the gauge
                if (newPosition.x > MaxX)
                {
                    rt.anchorMax = new Vector2(MaxX, 1);
                    break;
                }
                else if (newPosition.x < MinX)
                {
                    rt.anchorMax = new Vector2(MinX, 1);
                    break;
                }
                else
                {
                    rt.anchorMax = new Vector2(newPosition.x, 1);
                }
                // re-calculate remaining distance
                remaining = realVal - rt.anchorMax.x;
                // wait one frame before re-evaluating loop condition
                yield return null;
            }
            print("update loop done");
            FinishRound();
        }
        #region MonoBehavior
        public void Awake()
        {
            print(Instance); // call the instance before disabling the game object
            gameObject.SetActive(false);
            AttackPanel.SetActive(false);
            DefensePanel.SetActive(false);
            NeutralPanel.SetActive(false);
            RoundMessages.text = "";

            // store reciprical of moveTime to use multiplaction instead of division
            inverseMoveTime = 1f / moveTime;
            barLen = MaxX - MinX;
        }
        public void OnEnable()
        {
            transform.SetAsLastSibling();
        }
        #endregion
    }
}
