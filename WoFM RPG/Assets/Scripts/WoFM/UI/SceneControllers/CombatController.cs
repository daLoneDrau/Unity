using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Scripts.UI._2D;
using RPGBase.Singletons;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WoFM.Flyweights;
using WoFM.Singletons;
using WoFM.UI._2D;
using WoFM.UI.GlobalControllers;
using WoFM.UI.Tooltips;

namespace WoFM.UI.SceneControllers
{
    public class CombatController : Singleton<CombatController>, IWatcher
    {
        const int TOP = 0;
        const int BOTTOM = 1;
        /// <summary>
        /// the number of animations left to complete before ending the round.
        /// </summary>
        private int animationsToComplete;
        private WoFMAnimationUtility animationUtility;
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
        /// <summary>
        /// the canvas instance the controller is associated with.
        /// </summary>
        private Canvas myCanvas;
        /// <summary>
        /// the list of enemy IOs by ref id.
        /// </summary>
        private int[] enemies = new int[0] { };
        /// <summary>
        /// flag indicating the round has started.
        /// </summary>
        private bool roundStarted;
        /// <summary>
        /// the current round number.
        /// </summary>
        private int roundNumber;
        #region UI elements
        /// <summary>
        /// the list of enemy UI elements.
        /// </summary>
        public GameObject[] Enemies;
        /// <summary>
        /// the Hero's UI element.
        /// </summary>
        public GameObject Hero;
        /// <summary>
        /// the Hero's UI element.
        /// </summary>
        public GameObject HeroStats;
        #region Buttons
        public Button AttackButton;
        public Button DefendButton;
        public Button StrongAttackButton;
        public Button StrongDefendButton;
        #endregion
        /// <summary>
        /// the field displaying the enemy's name.
        /// </summary>
        public Text EnemyName;
        /// <summary>
        /// The Enemy Stamina gauges.
        /// </summary>
        public GameObject EnemyStats;
        /// <summary>
        /// the animation controlling the marquee.
        /// </summary>
        public Animator MarqueeAnimation;
        /// <summary>
        /// The animator controlling the slash effect.
        /// </summary>
        public Animator SlashAnimation;
        /// <summary>
        /// the slash element.
        /// </summary>
        public GameObject Weapon;
        /// <summary>
        /// the wound element.
        /// </summary>
        public GameObject Wound;
        /// <summary>
        /// The text displayed showing the character's options.
        /// </summary>
        public Text RoundNotices;
        /// <summary>
        /// the element displaying the marquee.
        /// </summary>
        public Text Marquee;
        /// <summary>
        /// The message played upon victory.
        /// </summary>
        public string VictoryMessage { get; set; }
        #endregion
        /// <summary>
        /// Guarantee this will be always a singleton only - can't use the constructor!  Use this for initialization.
        /// </summary>
        protected CombatController() { }
        #region Combat Sequence
        /// <summary>
        /// Adds enemies to Combat.
        /// </summary>
        /// <param name="list">the list of enemy reference ids being added</param>
        public void AddEnemies(params int[] list)
        {
            for (int i = 0, li = list.Length; i < li; i++)
            {
                enemies = ArrayUtilities.Instance.ExtendArray(list[i], enemies);
                Interactive.Instance.GetIO(list[i]).NpcData.AddWatcher(this);
            }
        }
        /// <summary>
        /// Checks to see if any enemies died this round. Dead enemies are removed at the end of each round.
        /// </summary>
        /// <returns></returns>
        private bool NeedDeathAnimations()
        {
            bool aDeathOccurred = false;
            for (int i = enemies.Length - 1; i >= 0; i--)
            {
                BaseInteractiveObject io = Interactive.Instance.GetIO(enemies[i]);
                if (io.NpcData.IsDeadNPC())
                {
                    Debug.Log(io + " is dead");
                    // add an extra animation to play the death
                    animationsToComplete++;
                    roundStarted = true;
                    // play death animation
                    PlayDeath(io.RefId);
                    aDeathOccurred = true;
                }
            }
            return aDeathOccurred;
        }
        private void DisableButtons()
        {
            AttackButton.interactable = false;
            DefendButton.interactable = false;
            StrongAttackButton.interactable = false;
            StrongDefendButton.interactable = false;
        }
        /// <summary>
        /// Performs the main combat sequence:
        /// <list type="number">
        /// <item><description>Options panels are de-activated.</description></item>
        /// <item><description>The Marquee is reset and animated.</description></item>
        /// <item><description>Round actions are decided.</description></item>
        /// <item><description>Selected Options panel is re-activated.</description></item>
        /// </list>
        /// After the sequence, processing stops until the player chooses an action.
        /// </summary>
        /// <returns></returns>
        public IEnumerator CombatSequence()
        {
            // de-activate all buttons
            DisableButtons();
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
                    RoundNotices.text = "Player wins initiative. Attack!";
                    AttackButton.interactable = true;
                    StrongAttackButton.interactable = true;
                    break;
                case WoFMCombat.CREATURE_WOUNDS_PLAYER:
                    sb.Append(Interactive.Instance.GetIO(enemies[0]).NpcData.Name);
                    sb.Append(" wins initiative! Defend!");
                    RoundNotices.text = sb.ToString();
                    DefendButton.interactable = true;
                    StrongDefendButton.interactable = true;
                    break;
                default:
                    sb.Append(Interactive.Instance.GetIO(enemies[0]).NpcData.Name);
                    sb.Append(" eyes you warily... Choose: Strike or Protect yourself.");
                    RoundNotices.text = sb.ToString();
                    AttackButton.interactable = true;
                    DefendButton.interactable = true;
                    break;
            }
            sb.ReturnToPool();
            pc = null;
            npc = null;
            sb = null;
        }
        /// <summary>
        /// Finishes the combat round.
        /// </summary>
        /// <param name="caller"></param>
        public void FinishRound(string caller)
        {
            print("***************FINISHROUND - PLAYED ANIMATION " + caller);
            animationsToComplete--;
            // check to see if all animations have completed since the round began
            if (animationsToComplete <= 0
                && roundStarted)
            {
                roundStarted = false;
                // TODO - check to see if combat is over
                bool combatOver = NeedDeathAnimations();
                if (NeedDeathAnimations())
                {
                    print("\tneed death animations");
                    if (animationsToComplete < 0)
                    {
                        animationsToComplete = 0;
                    }
                    print("\thave " + animationsToComplete + " animations left. still in round " + (roundNumber - 1));
                }
                else
                {
                    print("\tDo not need death animations");
                    if (CombatWon())
                    {
                        print("\tCombat was won!");
                        // play You Won!
                        StartCoroutine(Victory());
                    }
                    else if (CombatLost())
                    {

                    }
                    else
                    {
                        // go to next round
                        print("\tSTART ROUND " + roundNumber);
                        StartCoroutine(CombatSequence());
                    }
                }
            }
            else
            {
                if (animationsToComplete < 0)
                {
                    animationsToComplete = 0;
                }
                print("\thave " + animationsToComplete + " animations left. still in round " + (roundNumber - 1));
            }
        }
        private bool CombatWon()
        {
            return enemies.Length == 0;
        }
        private bool CombatLost()
        {
            WoFMInteractiveObject pio = ((WoFMInteractive)Interactive.Instance).GetPlayerIO();
            return pio.PcData.IsDead();
        }
        /// <summary>
        /// Sets the enemies for display.
        /// </summary>
        private void SetupEnemies()
        {
            print("SetupEnemies: " + enemies.Length + " enemies");
            for (int i = 0, li = Enemies.Length; i < li; i++)
            {
                if (i < enemies.Length
                    && enemies.Length > 0)
                {
                    WoFMInteractiveObject io = (WoFMInteractiveObject)Interactive.Instance.GetIO(enemies[i]);
                    Enemies[i].transform.GetChild(0).GetComponent<Image>().sprite = io.Sprite;
                    if (i == 0)
                    {
                        EnemyName.text = io.NpcData.Name;
                    }
                    io = null;
                }
                else
                {
                    Enemies[i].SetActive(false);
                }
            }
        }
        /// <summary>
        /// Starts a round of combat.  Variables are reset, gauges are re-drawn and the combat sequence starts.
        /// </summary>
        public void StartCombat()
        {
            print("starting combat");
            print(GameSceneController.Instance.CONTROLS_FROZEN);
            // set global combat variables
            Script.Instance.SetGlobalVariable("COMBAT_ROUNDS", 0);
            Script.Instance.SetGlobalVariable("COMBAT_ON", 1);
            VictoryMessage = null;
            // clear 
            Messages.Instance.Clear();
            roundNumber = 1;
            gameObject.SetActive(true);
            SetupEnemies();
            WoFMInteractiveObject pio = ((WoFMInteractive)Interactive.Instance).GetPlayerIO();
            Hero.transform.GetChild(0).GetComponent<Image>().sprite = pio.Sprite;
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
            print("**********************ATTACK");
            roundStarted = true;
            // set # of animations to 1 to start (for the player's swing)
            animationsToComplete = 1;
            DisableButtons();
            WoFMInteractiveObject io = (WoFMInteractiveObject)Interactive.Instance.GetIO(enemies[0]);
            print("\tDAMAGE ENEMY");
            // get damage from player's current weapon
            float damage = Combat.Instance.ComputeDamages(
                ((WoFMInteractive)Interactive.Instance).GetPlayerIO(), // source
                null, // weapon - not needed
                Interactive.Instance.GetIO(enemies[0]), // target
                0); // no flags since luck wasn't used
            // need 1 animation for swing
        }
        public void Defend()
        {
            roundStarted = true;
            DisableButtons();
            WoFMInteractiveObject io = (WoFMInteractiveObject)Interactive.Instance.GetIO(enemies[0]);
            // get damage from player's current weapon
            float damage = Combat.Instance.ComputeDamages(
                Interactive.Instance.GetIO(enemies[0]), // source
                null, // weapon - not needed
                ((WoFMInteractive)Interactive.Instance).GetPlayerIO(), // target
                0); // no flags since luck wasn't used
            // need 1 animation for swing
            animationsToComplete = 1;
            print("damaged for " + damage);
        }
        public void ToughDefend()
        {
            roundStarted = true;
            DisableButtons();
            print("defendefen");
        }
        public void WildAttack()
        {
            roundStarted = true;
            DisableButtons();
            print("attakattak");
        }
        #endregion
        #region Combat Animations
        /*
        /// <summary>
        /// Checks to see if an enemy's skill bar needs to be updated.
        /// </summary>
        /// <param name="ioData"></param>
        private void CheckForEnemySkillUpdate(IONpcData ioData)
        {
            // print("checking enemy stam");
            int index = -1;
            for (int i = enemies.Length - 1; i >= 0; i--)
            {
                if (enemies[i] == ioData.Io.RefId)
                {
                    index = i;
                    break;
                }
            }
            // print("found enemy at index " + index);
            RectTransform rt = EnemyGauges[(index * 2) + 1].GetComponent<RectTransform>();
            float currentBarLen = rt.anchorMax.x - MinX;
            float realPercent = ioData.GetFullAttributeScore("SKL") / 24f;
            // print("real percent is " + realPercent);
            if (realPercent > 1)
            {
                realPercent = 1f;
            }
            float realBarLen = realPercent * barLen;
            if (currentBarLen != realBarLen)
            {
                // add an extra animation to update the gauge
                animationsToComplete++;
                StartCoroutine(UpdateGuage(EnemyGauges[index * 2 + 1], realPercent));
            }
        }
        /// <summary>
        /// Checks to see if an enemy's stamina bar needs to be updated.
        /// </summary>
        /// <param name="ioData"></param>
        private void CheckForEnemyStaminaUpdate(IONpcData ioData)
        {
            // print("checking enemy stam");
            int index = -1;
            for (int i = enemies.Length - 1; i >= 0; i--)
            {
                if (enemies[i] == ioData.Io.RefId)
                {
                    index = i;
                    break;
                }
            }
            // print("found enemy at index " + index);
            RectTransform rt = EnemyGauges[index * 2].GetComponent<RectTransform>();
            float currentBarLen = rt.anchorMax.x - MinX;
            float realPercent = ioData.Life / 24f;
            // print("real percent is " + realPercent);
            if (realPercent > 1)
            {
                realPercent = 1f;
            }
            float realBarLen = realPercent * barLen;
            if (currentBarLen != realBarLen)
            {
                // add an extra animation to update the gauge
                animationsToComplete++;
                StartCoroutine(UpdateGuage(EnemyGauges[index * 2], realPercent));
            }
        }
        private void CheckForPlayerLuckUpdate(IOPcData ioData)
        {
            RectTransform rt = PlayerGauges[2].GetComponent<RectTransform>();
            float currentBarLen = rt.anchorMax.x - MinX;
            float realPercent = ioData.GetFullAttributeScore("LUK") / 24f;
            if (realPercent > 1)
            {
                realPercent = 1f;
            }
            float realBarLen = realPercent * barLen;
            if (currentBarLen != realBarLen)
            {
                // add an extra animation to update the gauge
                animationsToComplete++;
                StartCoroutine(UpdateGuage(PlayerGauges[2], realPercent));
            }
        }
        private void CheckForPlayerSkillUpdate(IOPcData ioData)
        {
            RectTransform rt = PlayerGauges[1].GetComponent<RectTransform>();
            float currentBarLen = rt.anchorMax.x - MinX;
            float realPercent = ioData.GetFullAttributeScore("SKL") / 24f;
            if (realPercent > 1)
            {
                realPercent = 1f;
            }
            float realBarLen = realPercent * barLen;
            if (currentBarLen != realBarLen)
            {
                // add an extra animation to update the gauge
                animationsToComplete++;
                StartCoroutine(UpdateGuage(PlayerGauges[1], realPercent));
            }
        }
        private void CheckForPlayerStaminaUpdate(IOPcData ioData)
        {
            RectTransform rt = PlayerGauges[0].GetComponent<RectTransform>();
            float currentBarLen = rt.anchorMax.x - MinX;
            float realPercent = ioData.Life / 24f;
            if (realPercent > 1)
            {
                realPercent = 1f;
            }
            float realBarLen = realPercent * barLen;
            if (currentBarLen != realBarLen)
            {
                // add an extra animation to update the gauge
                animationsToComplete++;
                StartCoroutine(UpdateGuage(PlayerGauges[0], realPercent));
            }
        }
        */
        /// <summary>
        /// Plays the death animation.
        /// </summary>
        /// <param name="ioid"></param>
        public void PlayDeath(int ioid)
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
                print("found dead enemy at " + index);
                StartCoroutine(FlashDeadEnemy(Enemies[index].transform.GetChild(1).GetComponent<Image>()));
            }
        }
        private IEnumerator Victory()
        {
            Marquee.text = "You Won!";
            MarqueeAnimation.Play("GrowingMarquee");
            yield return new WaitForSeconds(2f);
            // clean up data
            // de-activate all buttons
            DisableButtons();
            animationsToComplete = 0;
            Script.Instance.SetGlobalVariable("COMBAT_ON", 0);
            gameObject.SetActive(false);
            if (VictoryMessage != null)
            {
                Messages.Instance.SendMessage(VictoryMessage);
            }
        }
        private IEnumerator FlashDeadEnemy(Image image)
        {
            int flashes = 4;
            float time = 2f;
            float segment = time / (float)flashes;
            for (int i = flashes; i > 0; i--)
            {
                float waitTime = segment * ((float)(flashes - i + 1) / flashes);

                // turn alpha on
                image.color = new Color(1, 1, 1, 1);
                yield return new WaitForSeconds(segment - waitTime);

                // turn alpha off
                image.color = new Color(1, 1, 1, 0);
                yield return new WaitForSeconds(waitTime);

                // at i = 16, off for 1/16, on for 15/16
                // at i = 15, off for 2/16, on for 14/16
                // at i = 1, off for 16/16, on for 0/16
            }
            // after animation loop, continue processing
            int index = -1;
            for (int i = enemies.Length - 1; i >= 0; i--)
            {
                print("checking enemy " + i);
                BaseInteractiveObject io = Interactive.Instance.GetIO(enemies[i]);
                if (io.NpcData.IsDeadNPC())
                {
                    print("io is dead");
                    index = i;
                    break;
                }
            }
            if (index >= 0)
            {
                // add action to destroy enemy IO from game
                Interactive.Instance.DestroyIO(Interactive.Instance.GetIO(enemies[index]));

                print("Removing enemy " + index);
                enemies = ArrayUtilities.Instance.RemoveIndex(index, enemies);

                SetupEnemies();
            }
            FinishRound("Death");
        }
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
                // target is enemy
                RectTransform targetTransform = Enemies[index].transform.GetChild(0).GetComponent<RectTransform>();
                // attacker is PC
                RectTransform attackerTransform = Hero.transform.GetChild(0).GetComponent<RectTransform>();
                int orientation = BOTTOM;
                // animated object is weapon
                RectTransform weaponTransform = Weapon.GetComponent<RectTransform>();
                // get attacker's weapon
                WoFMInteractiveObject pc = ((WoFMInteractive)Interactive.Instance).GetPlayerIO();
                WoFMInteractiveObject wpnIo = (WoFMInteractiveObject)Interactive.Instance.GetIO(pc.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON));
                WoFMItemData weapon = (WoFMItemData)wpnIo.ItemData;

                // Play Stab first
                print("going to play stab");
                PlayStab(targetTransform,   // target sprite
                    attackerTransform,      // attacker sprite
                    weaponTransform,        // weapon sprite
                    weapon,                   // weapon data
                    orientation);           // orientation relative to attacker
                // TODO - change animation based on hit strength
            }
            else
            {
                PlayCombatAnimation(Hero.GetComponent<RectTransform>(), Weapon.transform, "Hit Right");
                // TODO - change animation based on hit strength
            }
        }
        public const int STAB = 0;
        private void OrientUiElementOnObject(RectTransform obj, RectTransform element, int orientation, float spriteRotation)
        {
            // get parent's height
            float objectHeight = obj.sizeDelta.y;
            // get slash height
            float elementHeight = element.sizeDelta.y;
            // get object's position within canvas space
            Vector3 canvasPosition = myCanvas.transform.InverseTransformPoint(obj.position);
            switch (orientation)
            {
                case TOP: // element is above object
                    canvasPosition.y += objectHeight / 2;
                    canvasPosition.y += elementHeight / 2;
                    // orient element from top to bottom
                    element.Rotate(0f, 0f, 0f - spriteRotation);
                    break;
                case BOTTOM: // element is below object
                    canvasPosition.y -= objectHeight / 2;
                    canvasPosition.y -= elementHeight / 2;
                    // orient element from bottom to top
                    element.Rotate(0f, 0f, 180f - spriteRotation);
                    break;
                default: // element is centered on object
                    // no rotations done
                    break;
            }
            element.localPosition = canvasPosition;
        }
        private void PlayStab(RectTransform target, RectTransform attacker, RectTransform animationObject, WoFMItemData weapon, int orientation)
        {
            print("playstab");
            animationObject.SetAsLastSibling();
            // position weapon sprite
            OrientUiElementOnObject(attacker, animationObject, orientation, weapon.SpriteRotation);

            Vector3 endPosition;
            // set weapon sprite
            animationObject.GetComponent<Image>().sprite = SpriteMap.Instance.GetSprite(weapon.Sprite);
            // get end position for animation
            switch (orientation)
            {
                case TOP:
                    // sprite appears above attacker and moves up
                    endPosition = new Vector3();
                    endPosition = new Vector3(animationObject.localPosition.x, animationObject.localPosition.y + 20f, 0f);
                    break;
                default: // BOTTOM
                    // sprite appears below attacker and moves down
                    endPosition = new Vector3(animationObject.localPosition.x, animationObject.localPosition.y - 20f, 0f);
                    break;
            }
            // sprite is positioned and rotated correctly.  Play the animation
            FinishDelegate d = new FinishDelegate(FinishAttackAnimation(target, animationObject.transform as RectTransform, Wound.transform as RectTransform));
            StartCoroutine(animationUtility.MoveObjectInStraightLine(animationObject, endPosition, .01f, true));
            // StartCoroutine(StabAnimation(animationObject, endPosition, .01f));
        }
        public delegate IEnumerator FinishDelegate(RectTransform rt0, RectTransform rt1, RectTransform rt2);
        private IEnumerator FinishAttackAnimation(RectTransform target, RectTransform weaponObject, RectTransform woundObject)
        {
            // position wound sprite
            OrientUiElementOnObject(target, woundObject, -1, 0f);
            // set wound sprite
            woundObject.GetComponent<Image>().sprite = SpriteMap.Instance.GetSprite("slash_left");
            // fade wound and weapon
            float fadeSpeed = 0.5f;
            Image wpnImage = weaponObject.gameObject.GetComponent<Image>(), woundImage = woundObject.gameObject.GetComponent<Image>();
            while (wpnImage.color.a > 0.4f)
            {
                float newA = wpnImage.color.a * fadeSpeed;
                wpnImage.color = new Color(wpnImage.color.r, wpnImage.color.g, wpnImage.color.b, newA);
                woundImage.color = new Color(woundImage.color.r, woundImage.color.g, woundImage.color.b, newA);
                yield return null;
            }
            wpnImage.color = new Color(wpnImage.color.r, wpnImage.color.g, wpnImage.color.b, 0f);
            woundImage.color = new Color(woundImage.color.r, woundImage.color.g, woundImage.color.b, 0f);
            // play wound and fade from view
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
                //SlashAnimation.Play(animationclip);
            }
        }
        #endregion
        public void WatchUpdated(Watchable data)
        {
            if (gameObject.activeInHierarchy)
            {
                if (data is IOPcData)
                {
                    print("\tpc has been updated");
                    UpdateHeroStats((IOPcData)data);
                }
                else if (data is IONpcData)
                {
                    print("\tan enemy has been updated");
                    UpdateEnemyStats((IONpcData)data);
                }
            }
        }
        private void UpdateEnemyStats(IONpcData npc)
        {
            this.EnemyStats.transform.GetChild(0).GetComponent<Text>().text = ((int)npc.GetFullAttributeScore("SKL")).ToString();
            this.EnemyStats.transform.GetChild(1).GetComponent<Text>().text = ((int)npc.Life).ToString();
        }
        private void UpdateHeroStats(IOPcData npc)
        {
            this.HeroStats.transform.GetChild(0).GetComponent<Text>().text = ((int)npc.GetFullAttributeScore("SKL")).ToString();
            this.HeroStats.transform.GetChild(1).GetComponent<Text>().text = ((int)npc.Life).ToString();
            this.HeroStats.transform.GetChild(2).GetComponent<Text>().text = ((int)npc.GetFullAttributeScore("LUK")).ToString();
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
            FinishRound(gauge.name);
        }
        #region MonoBehavior
        public void Awake()
        {
            print(Instance); // call the instance before disabling the game object
            animationUtility = gameObject.AddComponent<WoFMAnimationUtility>() as WoFMAnimationUtility;
            gameObject.SetActive(false);
            DisableButtons();
            RoundNotices.text = "";

            // store reciprical of moveTime to use multiplaction instead of division
            inverseMoveTime = 1f / moveTime;
            barLen = MaxX - MinX;

            myCanvas = transform.root.GetComponent<Canvas>();
        }
        public void OnEnable()
        {
            transform.SetAsLastSibling();
        }
        #endregion
    }
}
