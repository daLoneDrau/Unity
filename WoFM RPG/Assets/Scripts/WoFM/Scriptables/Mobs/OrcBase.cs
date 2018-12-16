using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.UI.GlobalControllers;

namespace WoFM.Scriptables.Mobs
{
    /// <summary>
    /// $: GLOBAL TEXT £: LOCAL TEXT #: GLOBAL LONG §: LOCAL LONG &: GLOBAL FLOAT @: LOCAL FLOAT
    /// 
    /// based on goblin_base.asl
    /// @author drau
    /// </summary>
    public class OrcBase : MobBase
    {
        public const int REFLECTION_MODE_NOTHING = 0;
        public const int REFLECTION_MODE_NORMAL = 1;
        public const int REFLECTION_MODE_THREATENED = 2;
        public const int REFLECTION_MODE_SEARCHING = 3;
        public const int FIGHT_MODE_NONE = 0;
        public const int FIGHT_MODE_FIGHT = 1;
        public const int FIGHT_MODE_FLEE = 2;
        /// <summary>
        /// 
        /// </summary>
        private void AttackPlayerAfterOuch()
        {
            SetLocalVariable("ignorefailure", 0);
            if (Script.Instance.GetGlobalIntVariableValue("PLAYERSPELL_INVISIBILITY") == 1)
            {
                LookForSuite();
            }
            else
            {
                // turn off the 'lookfor' timer
                Script.Instance.TimerClearByNameAndIO("lookfor", Io);
                // turn off the 'heard' timer
                Script.Instance.TimerClearByNameAndIO("heard", Io);
                SetLocalVariable("panicmode", 1);
                SetLocalVariable("looking_for", 0);
                SetLocalVariable("enemy", 1);
                // turn off hearing - io is in combat
                AssignDisallowedEvent(ScriptConsts.DISABLE_HEAR);
                if (GetLocalIntVariableValue("noise_heard") < 2)
                {
                    SetLocalVariable("noise_heard", 2);
                }

                CallForHelp();
                if (GetLocalIntVariableValue("player_enemy_send") == 0)
                {
                    SetLocalVariable("player_enemy_send", 1);
                    // send event to all members of group that the player attacked
                    Script.Instance.StackSendGroupScriptEvent(
                            GetLocalStringVariableValue("friend"),
                            0, null, "onPlayerEnemy");
                    Debug.Log("PLAYER_ENEMY sent");
                }
                // kill all local timers
                Script.Instance.TimerClearAllLocalsForIO(Io);
                // clear the quiet timer
                Script.Instance.TimerClearByNameAndIO("quiet", Io);
                // SET_NPC_STAT BACKSTAB 0
                if (Io.NpcData.Life < GetLocalIntVariableValue("cowardice"))
                {
                    // flee();
                }
                else if (GetLocalIntVariableValue("fighting_mode") == 2)
                {
                    // running away
                }
                else
                {
                    if (GetLocalIntVariableValue("fighting_mode") == 1)
                    {
                        SetLocalVariable("reflection_mode", 2);
                    }
                    else
                    {
                        if (GetLocalStringVariableValue(
                                "attached_object") != "NONE")
                        {
                            // DETACH ~£attached_object~ SELF
                            // OBJECT_HIDE ~£attached_object~ ON
                            // SET £attached_object "NONE"
                            SetLocalVariable("attached_object", "NONE");
                        }

                        SaveBehavior();
                        // reset misc reflection timer
                        Script.Instance.StackSendIOScriptEvent(Io, 0, null, "onMiscReflection");
                        if (GetLocalIntVariableValue("fighting_mode") == 3
                                && Io.NpcData.Life < GetLocalIntVariableValue("cowardice"))
                        {
                            // flee();
                        }
                        else if (GetLocalIntVariableValue("tactic") == 2)
                        {
                            // flee(); // coward
                        }
                        else
                        {
                            if (GetLocalIntVariableValue("spotted") == 0)
                            {
                                SetLocalVariable("spotted", 1);
                                // hail aggressively
                                // SPEAK -a ~£hail~ NOP
                            }
                            SetLocalVariable("reflection_mode", 2);
                            SetLocalVariable("fighting_mode", 1);
                            if (GetLocalIntVariableValue("tactic") == 0)
                            {
                                Io.NpcData.ChangeBehavior(RPGBase.Constants.Behaviour.BEHAVIOUR_FIGHT.GetFlag() + RPGBase.Constants.Behaviour.BEHAVIOUR_MOVE_TO.GetFlag(),
                                        0);
                            }
                            else if (GetLocalIntVariableValue("tactic") == 1)
                            {
                                Io.NpcData.ChangeBehavior(RPGBase.Constants.Behaviour.BEHAVIOUR_FIGHT.GetFlag() + RPGBase.Constants.Behaviour.BEHAVIOUR_SNEAK.GetFlag() + RPGBase.Constants.Behaviour.BEHAVIOUR_MOVE_TO.GetFlag(),
                                        0);
                            }
                            else if (GetLocalIntVariableValue("tactic") == 3)
                            {
                                Io.NpcData.ChangeBehavior(RPGBase.Constants.Behaviour.BEHAVIOUR_MAGIC.GetFlag() + RPGBase.Constants.Behaviour.BEHAVIOUR_MOVE_TO.GetFlag(),
                                        0);
                            }
                            // set pathfinding to target player
                            // SETTARGET -a PLAYER
                            Debug.Log("WEAPON ON");
                            // set weapon in hand
                            // TODO - set weapon
                            Io.NpcData.Movemode = IoGlobals.RUNMODE;
                        }
                    }
                }
            }
        }
        private void CallForHelp()
        {
            if (!string.Equals(GetLocalStringVariableValue("friend"), "NONE", StringComparison.OrdinalIgnoreCase))
            {
                if (GameSceneController.Instance.CONTROLS_FROZEN)
                {
                    long tmp = Script.Instance.GetGlobalLongVariableValue("COMBAT_ROUNDS");
                    tmp -= GetLocalLongVariableValue("last_call_help");
                    if (tmp > 4)
                    {
                        // don't call for help too often...
                        Debug.Log("CALL FOR HELP !!!");
                        if (GetLocalIntVariableValue("fighting_mode") == 2)
                        {
                            Script.Instance.StackSendGroupScriptEvent(GetLocalStringVariableValue("friend"), 0, null, "callHelp");
                            // TODO -
                            // also send call to everyone within 1200 unit radius
                        }
                        else
                        {
                            Script.Instance.StackSendGroupScriptEvent(GetLocalStringVariableValue("friend"), 0, null, "callHelp");
                            // TODO -
                            // also send call to everyone within 600 unit radius
                        }
                        SetLocalVariable("last_call_help", Script.Instance.GetGlobalLongVariableValue("COMBAT_ROUNDS"));
                    }
                }
            }
        }
        private void Flee()
        {
            if (GetLocalIntVariableValue("fighting_mode") != 2)
            {
                // change to fleeing
                SetLocalVariable("fighting_mode", 2);
                Debug.Log("Fleeing");
                // kill local timers
                Script.Instance.TimerClearAllLocalsForIO(Io);
                // turn off hearing and collisions - io is fleeing
                AssignDisallowedEvent(ScriptConsts.DISABLE_HEAR);

                AssignDisallowedEvent(ScriptConsts.DISABLE_COLLIDE_NPC);

                SetLocalVariable("reflection_mode", 0);
                if (!string.Equals(GetLocalStringVariableValue("helping_buddy"), "NOBUDDY", StringComparison.OrdinalIgnoreCase))
                {
                    Io.NpcData.ChangeBehavior(RPGBase.Constants.Behaviour.BEHAVIOUR_FLEE.GetFlag(), 1000);
                    // set pathfinding to target player
                    // SETTARGET PLAYER
                    Io.NpcData.Movemode = IoGlobals.RUNMODE;
                    if (Script.Instance.IsIOInGroup(Io, "UNDEAD"))
                    {
                        Io.NpcData.Movemode = IoGlobals.WALKMODE;
                    }
                }
                else
                {
                    Script.Instance.StackSendIOScriptEvent(Io,
                        0,
                        new object[] { "flee_marker", GetLocalStringVariableValue("flee_marker") },
                        "Panic");
                    Io.NpcData.ChangeBehavior(RPGBase.Constants.Behaviour.BEHAVIOUR_MOVE_TO.GetFlag(), 0);
                    // set pathfinding to target helping buddy
                    // SETTARGET -a £helping_buddy
                    Io.NpcData.Movemode = IoGlobals.RUNMODE;
                    if (Script.Instance.IsIOInGroup(Io, "UNDEAD"))
                    {
                        Io.NpcData.Movemode = IoGlobals.WALKMODE;
                    }
                }

                CallForHelp();
                // Create a Type array.
                Type[] typeArray = new Type[4];
                typeArray.SetValue(typeof(BaseInteractiveObject), 0);
                typeArray.SetValue(typeof(int), 1);
                typeArray.SetValue(typeof(object[]), 2);
                typeArray.SetValue(typeof(string), 3);
                ScriptTimerInitializationParameters timerParams = new ScriptTimerInitializationParameters
                {
                    Name = "coward",
                    Script = this,
                    Io = Io,
                    Milliseconds = 2000,
                    StartTime = (long)RPGTime.Instance.GetGameTime(false),
                    RepeatTimes = 1,
                    Obj = Script.Instance,
                    Method = Script.Instance.GetType().GetMethod("StackSendIOScriptEvent", typeArray),
                    Args = new object[] {
                        Io,
                        0,
                        new object[] {
                            "speak_duration", 5,
                            "speak_tone", "aggressive",
                            "speak_text", GetLocalStringVariableValue("help")
                        },
                        "SpeakNoRepeat"
                    }
                };
                Script.Instance.StartTimer(timerParams);
                timerParams.Clear();

                timerParams.Name = "home";
                timerParams.Script = this;
                timerParams.Io = Io;
                timerParams.Milliseconds = 30000;
                timerParams.StartTime = (long)RPGTime.Instance.GetGameTime(false);
                timerParams.RepeatTimes = 1;
                timerParams.Obj = this;
                timerParams.Method = GetType().GetMethod("GoHome");
                timerParams.RepeatTimes = 1;
                Script.Instance.StartTimer(timerParams);
            }
        }
        private void LookFor()
        {
            if (GameSceneController.Instance.CONTROLS_FROZEN)
            {
                Script.Instance.TimerClearByNameAndIO("lookfor", Io);
                if (GetLocalIntVariableValue("confused") == 1
                        || Script.Instance.GetGlobalLongVariableValue("PLAYERSPELL_INVISIBILITY") == 1)
                {
                    LookForSuite();
                }
                else if (Script.Instance.GetGlobalLongVariableValue("DIST_PLAYER") < 500)
                {
                    PlayerDetected();
                }
            }
        }
        private void LookForSuite()
        {
            if (GameSceneController.Instance.CONTROLS_FROZEN)
            {
                if (GetLocalIntVariableValue("looking_for") > 2)
                {
                    PlayerDetected();
                }
                else
                {
                    if (GetLocalIntVariableValue("fighting_mode") <= 1)
                    {
                        Io.NpcData.ChangeBehavior(RPGBase.Constants.Behaviour.BEHAVIOUR_LOOK_FOR.GetFlag(), 500);
                        // SETTARGET -a PLAYER
                        Io.NpcData.Movemode = IoGlobals.WALKMODE;
                        SetLocalVariable("looking_for", 2);
                        SetLocalVariable("fighting_mode", 0);
                        RemoveDisallowedEvent(ScriptConsts.DISABLE_HEAR);
                        SetLocalVariable("reflection_mode", 3);
                        int timerNum = Script.Instance.TimerGetFree();
                        if (timerNum >= 0)
                        {
                            // after 30 seconds, go home
                            ScriptTimer timer = Script.Instance.GetScriptTimer(timerNum);
                            timer.Script = this;
                            timer.Exists = true;
                            timer.Io = Io;
                            timer.Msecs = 18000;
                            timer.Name = "home";
                            // invoke method go home
                            timer.Action = new ScriptTimerAction(this, // object
                                        GetType().GetMethod("GoHome"), // method
                                        null);
                            timer.Times = 1;
                            timer.ClearFlags();
                        }
                    }
                }
            }
        }
        public int MiscReflection()
        {
            if (Io.PoisonLevel > 0)
            {
                if (GetLocalIntVariableValue("enemy") != 1)
                {
                    SetLocalVariable("enemy", 1);
                    OuchSuite();
                }
            }
            else if (GetLocalIntVariableValue("reflection_mode") > 0
                  && Script.Instance.GetGlobalIntVariableValue("SHUT_UP") != 1)
            {
                int tmp;
                if (GetLocalIntVariableValue("reflection_mode") == 2)
                {
                    // in fighting mode -> more reflections - roll 1d10 + 3
                    tmp = Diceroller.Instance.RolldXPlusY(10, 3);
                }
                else
                {
                    // not in fighting mode -> more reflections - roll 1d32 + 5
                    tmp = Diceroller.Instance.RolldXPlusY(32, 5);
                }
                if (GetLocalStringVariableValue("type").Contains("undead"))
                {
                    tmp /= 2;
                }
                // set next reflection timer
                // Create a Type array.
                Type[] typeArray = new Type[4];
                typeArray.SetValue(typeof(BaseInteractiveObject), 0);
                typeArray.SetValue(typeof(int), 1);
                typeArray.SetValue(typeof(object[]), 2);
                typeArray.SetValue(typeof(string), 3);
                ScriptTimerInitializationParameters timerParams = new ScriptTimerInitializationParameters
                {
                    Name = "misc_reflection",
                    Script = this,
                    FlagValues = 1,
                    Io = Io,
                    Milliseconds = tmp,
                    StartTime = (long)RPGTime.Instance.GameTime,
                    RepeatTimes = 0,
                    Obj = Script.Instance,
                    Method = Script.Instance.GetType().GetMethod("StackSendIOScriptEvent", typeArray),
                    Args = new object[] {
                            Io,
                            0,
                            null,
                            "MiscReflection"
                        }
                };
                Script.Instance.StartTimer(timerParams);

                if (GetLocalIntVariableValue("reflection_mode") == 1)
                {
                    if (GetLocalStringVariableValue("misc") != null)
                    {
                        if (GetLocalIntVariableValue("short_reflections") == 1)
                        {
                            if (Diceroller.Instance.RolldX(2) == 1)
                            {
                                // SENDEVENT SPEAK_NO_REPEAT SELF "6 N
                                // [Human_male_misc_short]" ACCEPT
                            }
                        }
                        else
                        {
                            // SENDEVENT SPEAK_NO_REPEAT SELF "10 N £misc"
                        }
                    }
                }
                else if (GetLocalIntVariableValue("reflection_mode") == 2
                      && GetLocalStringVariableValue("threat") != null)
                {
                    // SENDEVENT SPEAK_NO_REPEAT SELF "3 A £threat"
                }
                else if (GetLocalStringVariableValue("search") != null)
                {
                    // SENDEVENT SPEAK_NO_REPEAT SELF "3 N £search"
                }
            }
            return ScriptConsts.ACCEPT;
        }
        public override int OnInit()
        {
            SetLocalVariable("voice", "");
            // turn off hearing
            AssignDisallowedEvent(ScriptConsts.DISABLE_HEAR);
            // 1 : PLAYER_ENEMY event already sent by this NPC
            SetLocalVariable("player_enemy_send", 0);
            // to avoid to many CALL_FOR_HELP events
            SetLocalVariable("last_call_help", 0);
            // name of attached object (if one)
            SetLocalVariable("attached_object", "NONE");
            // if 1 : must have a SPECIAL_ATTACK in
            // the code (ratmen & mummies for instance)
            SetLocalVariable("special_attack", 0);
            // meynier... tu dors...
            SetLocalVariable("sleeping", 0);
            // when = 1, attack mice
            SetLocalVariable("care_about_mice", 0);

            // in order to restore the main behavior after a look_for or a help
            SetLocalVariable("main_behavior_stacked", 0);
            // 0: nothing, 1: normal, 2: threat, 3: search
            SetLocalVariable("reflection_mode", REFLECTION_MODE_NOTHING);
            // used for various reasons,
            // 1 indicates that the NPC currently sees he player.
            SetLocalVariable("player_in_sight", 0);

            // if a npc hears a sound more than 3 times, he detects the player
            SetLocalVariable("noise_heard", 0);
            // 1 = the NPC is about to look for the player 2=looking for him
            SetLocalVariable("looking_for", 0);
            // 0 = NO 1 = Fighting 2 = Fleeing
            SetLocalVariable("fighting_mode", FIGHT_MODE_NONE);
            SetLocalVariable("last_heard", "NOHEAR");
            SetLocalVariable("snd_tim", 0);
            SetLocalVariable("ouch_tim", 0);
            // used for chats to save current reflection_mode
            SetLocalVariable("saved_reflection", 0);
            // to stop looping anims if ATTACK_PLAYER called
            SetLocalVariable("frozen", 0);
            // defines if the NPC has already said "I'll get you" to the player
            SetLocalVariable("spotted", 0);
            // defines the current dialogue position
            SetLocalVariable("chatpos", 0);
            // current target
            SetLocalVariable("targ", 0);
            // this stores the name of the current attacked mice, so that the NPC
            // doesn't attack another one until this one is dead.
            SetLocalVariable("targeted_mice", "NOMOUSE");
            // this stores the name of the current NPC that his being helped.
            SetLocalVariable("helping_target", "NOFRIEND");
            // this might change, but it currently defines the ONLY
            // key that the NPC carries with them
            SetLocalVariable("key_carried", "NOKEY");
            // this is used to check what door is dealing the npc with right now.
            SetLocalVariable("targeted_door", "NODOOR");
            // set backstab stat to 1
            // SET_NPC_STAT BACKSTAB 1
            // only for spell casters
            SetLocalVariable("spell_ready", 1);
            // friend to run to in case of trouble
            SetLocalVariable("helping_buddy", "NOBUDDY");
            // last time someone spoke
            SetLocalVariable("last_reflection", 0);
            // go back to this marker if combat finished
            SetLocalVariable("init_marker", "NONE");
            SetLocalVariable("friend", "NONE");
            // set detection value, from -1 (off) to 100
            // SETDETECT 40
            // the number of attempt at passing a locked door
            SetLocalVariable("door_locked_attempt", 0);
            // set the radius for physics
            // PHYSICAL RADIUS 30
            // set material
            // SET_MATERIAL FLESH
            // SET_ARMOR_MATERIAL LEATHER
            // SET_STEP_MATERIAL Foot_bare
            // SET_BLOOD 0.9 0.1 0.1
            // inventory created as part of IO
            // SETIRCOLOR 0.8 0.0 0.0
            // stats are set during serialization
            // SET_NPC_STAT RESISTMAGIC 10
            // SET_NPC_STAT RESISTPOISON 10
            // SET_NPC_STAT RESISTFIRE 1
            // defines if the NPC is enemy to the player at the moment
            SetLocalVariable("enemy", 0);
            // when = 0, the NPC is not sure if he saw the
            // player "did that thing move over there ?"
            SetLocalVariable("panicmode", 0);
            // 0 = normal 1 = sneak 2 = rabit 3 = caster
            SetLocalVariable("tactic", 0);
            // used to restore previous tactic after a repel undead
            SetLocalVariable("current_tactic", 0);
            // if life < cowardice, NPC flees
            SetLocalVariable("cowardice", 8);
            // level of magic needed to confuse this npc
            SetLocalVariable("confusability", 3);
            // if damage < pain , no hit anim. pain threshold will be 1
            SetLocalVariable("pain", 1);
            // new set the value for the npc heals himself
            SetLocalVariable("low_life_alert", 1);
            SetLocalVariable("friend", "goblin");
            SetLocalVariable("type", "goblin_base");

            return base.OnInit();
        }
        public override int OnInitEnd()
        {
            Debug.Log("onInitEnd OrcScript");
            if (GetLocalIntVariableValue("enemy") == 1)
            {
                // turn hearing back on
                RemoveDisallowedEvent(ScriptConsts.DISABLE_HEAR);
            }
            if (!string.Equals("none", GetLocalStringVariableValue("friend"), StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log(Io);
                Debug.Log(GetLocalStringVariableValue("friend"));
                Script.Instance.AddToGroup(Io, GetLocalStringVariableValue("friend"));
            }
            int scale = 95;
            scale += Diceroller.Instance.RolldX(10);
            SetLocalVariable("scale", scale);
            // SETSCALE §scale
            if (GetLocalIntVariableValue("care_about_mice") == 1)
            {
                Script.Instance.AddToGroup(Io, "MICECARE");
            }
            if (!string.Equals("goblin_base", GetLocalStringVariableValue("type"), StringComparison.OrdinalIgnoreCase))
            {
                // TODO - fix set weapon to work
                // Interactive.Instance.PrepareSetWeapon(Io, "Orc Cleaver");
                // TODO set additional stats

                // TODO load animations

                // TODO set reflection texts

                // set reflection timer
                // Create a Type array.
                Type[] typeArray = new Type[4];
                typeArray.SetValue(typeof(BaseInteractiveObject), 0);
                typeArray.SetValue(typeof(int), 1);
                typeArray.SetValue(typeof(object[]), 2);
                typeArray.SetValue(typeof(string), 3);
                ScriptTimerInitializationParameters timerParams = new ScriptTimerInitializationParameters
                {
                    Name = "misc_reflection",
                    Script = this,
                    FlagValues = 1,
                    Io = Io,
                    Milliseconds = 10000,
                    StartTime = (long)RPGTime.Instance.GetGameTime(false),
                    RepeatTimes = 0,
                    Obj = Script.Instance,
                    Method = Script.Instance.GetType().GetMethod("StackSendIOScriptEvent", typeArray),
                    Args = new object[] {
                        Io,
                        0,
                        null,
                        "MiscReflection"
                    }
                };

                Script.Instance.StartTimer(timerParams);
            }
            return base.OnInitEnd();
        }
        public override int OnOuch()
        {
            OuchStart();
            OuchSuite();
            return ScriptConsts.ACCEPT;
        }
        /*
         * (non-Javadoc)
         * @see com.dalonedrow.rpg.base.flyweights.Scriptable#onInit()
         */
        public override int OnSpellcast()
        {
            // SHUT_UP global var means a cinematic is playing - no spells cast
            // if caster is not PC - no spells cast
            if (Script.Instance.GetGlobalIntVariableValue("SHUT_UP") != 0
                    && Script.Instance.EventSender.HasIOFlag(IoGlobals.IO_01_PC))
            {
                /*
                 * IF (§casting_lvl != 0) { IF (^$PARAM1 == NEGATE_MAGIC) { SET
                 * #NEGATE ^#PARAM2 ACCEPT } } IF ( SELF ISGROUP UNDEAD ) { IF
                 * (^$PARAM1 == REPEL_UNDEAD) { SET #REPEL ^#PARAM2 IF (£type ==
                 * "undead_lich") { IF (^#PARAM2 < 6) { HERO_SAY -d
                 * "pas assez fort, mon fils" ACCEPT } } GOTO REPEL } } IF (^$PARAM1
                 * == CONFUSE) { IF (^#PARAM2 < §confusability) ACCEPT SENDEVENT
                 * UNDETECTPLAYER SELF "" SET §confused 1 ACCEPT } IF (§enemy == 0)
                 * ACCEPT IF (£type == "human_ylside") ACCEPT IF (£type ==
                 * "undead_lich") ACCEPT IF (^$PARAM1 == HARM) { IF (^PLAYER_LIFE <
                 * 20) ACCEPT GOTO NO_PAIN_REPEL } IF (^$PARAM1 == LIFE_DRAIN) { IF
                 * (^PLAYER_LIFE < 20) ACCEPT GOTO NO_PAIN_REPEL } IF (^$PARAM1 ==
                 * MANA_DRAIN) { IF (§casting_lvl == 0) ACCEPT IF (^PLAYER_LIFE <
                 * 20) ACCEPT GOTO NO_PAIN_REPEL } ACCEPT
                 */
            }
            return base.OnSpellcast();
        }
        private void OuchStart()
        {
            Debug.Log("OUCH ");
            float ouchDmg = GetLocalFloatVariableValue("SUMMONED_OUCH")
                    + GetLocalFloatVariableValue("OUCH");
            Debug.Log(ouchDmg);
            int painThreshold = GetLocalIntVariableValue("pain");
            Debug.Log(" PAIN THRESHOLD ");
            Debug.Log(painThreshold);
            if (ouchDmg < painThreshold)
            {
                Debug.Log("Damage is below pain threshold");
                if (Script.Instance.GetGlobalIntVariableValue("PLAYERCASTING") == 0)
                {
                    // Script.Instance.forceAnimation(HIT_SHORT);
                }
                if (GetLocalIntVariableValue("enemy") == 0)
                {
                    // clear all speech
                }
            }
            else
            {
                Debug.Log("Damage is above pain threshold");
                // damage is above pain threshold
                long tmp = Script.Instance.GetGlobalIntVariableValue("COMBAT_ROUNDS");
                Debug.Log("COMBAT_ROUNDS::" + tmp);
                if (HasLocalVariable("ouch_time"))
                {
                    tmp -= GetLocalLongVariableValue("ouch_time");
                }
                if (tmp > 4)
                {
                    Debug.Log("been more than 4 rounds since last ouch");
                    // been more than 4 seconds since last recorded ouch?
                    // force hit animation
                    // Script.Instance.forceAnimation(HIT);
                    // set current time as last ouch
                    SetLocalVariable("ouch_time", Script.Instance.GetGlobalIntVariableValue("COMBAT_ROUNDS"));
                }
                tmp = painThreshold;
                tmp *= 3;
                if (ouchDmg >= tmp)
                {
                    Debug.Log("Damage was greater than 3x pain threshold");
                    if (Diceroller.Instance.RolldX(2) == 2)
                    {
                        // speak angrily "ouch_strong"
                    }
                }
                else
                {
                    tmp = painThreshold;
                    tmp *= 2;
                    if (ouchDmg >= tmp)
                    {
                        Debug.Log("Damage was greater than 2x pain threshold");
                        if (Diceroller.Instance.RolldX(2) == 2)
                        {
                            // speak angrily "ouch_medium"
                        }
                    }
                    else
                    {
                        Debug.Log("Damage was greater than pain threshold");
                        if (Diceroller.Instance.RolldX(2) == 2)
                        {
                            // speak angrily "ouch"
                        }
                    }
                }
            }
        }
        private void OuchSuite()
        {
            if (GameSceneController.Instance.CONTROLS_FROZEN)
            {
                // don't react to aggression
            }
            else
            {
                if (Script.Instance.EventSender.HasIOFlag(IoGlobals.IO_01_PC))
                {
                    if (GetLocalIntVariableValue("player_in_sight") == 0)
                    {
                        // player not in sight
                        SetLocalVariable("enemy", 1);
                        // LOOK FOR ATTACKER
                    }
                    // turn off aggression - io is in combat
                    AssignDisallowedEvent(ScriptConsts.DISABLE_AGGRESSION);
                    SetLocalVariable("spotted", 1);
                    AttackPlayerAfterOuch();
                }
            }
        }
        public int PlayerDetected()
        {
            // if player is invisible ignore the event
            // IF (^PLAYERSPELL_INVISIBILITY == 1) ACCEPT

            // if IO is confused, ignore the event
            if (GetLocalIntVariableValue("confused") != 1)
            {
                SetLocalVariable("player_in_sight", 1);
                if (GameSceneController.Instance.CONTROLS_FROZEN)
                {
                    // SET_NPC_STAT BACKSTAB 0
                    if (GetLocalIntVariableValue("enemy") != 0
                            || GetLocalIntVariableValue("fighting_mode") != 2
                            || GetLocalIntVariableValue("sleeping") != 1)
                    {
                        if (GetLocalIntVariableValue("panicmode") > 0)
                        {
                            // attackPlayer();
                        } // ELSE IF (^DIST_PLAYER < 600) GOTO ATTACK_PLAYER
                        else
                        {
                            // set reflection timer
                            // Create a Type array.
                            Type[] typeArray = new Type[4];
                            typeArray.SetValue(typeof(BaseInteractiveObject), 0);
                            typeArray.SetValue(typeof(int), 1);
                            typeArray.SetValue(typeof(object[]), 2);
                            typeArray.SetValue(typeof(string), 3);
                            ScriptTimerInitializationParameters timerParams = new ScriptTimerInitializationParameters
                            {
                                Name = "doubting",
                                Script = this,
                                FlagValues = 0,
                                Io = Io,
                                Milliseconds = 3000,
                                StartTime = (long)RPGTime.Instance.GetGameTime(false),
                                RepeatTimes = 1,
                                Obj = Script.Instance,
                                Method = Script.Instance.GetType().GetMethod("StackSendIOScriptEvent", typeArray),
                                Args = new object[] {
                                    Io,
                                    0,
                                    null,
                                    "AttackPlayer"
                                }
                            };
                            Script.Instance.StartTimer(timerParams);
                            // set panic mode to 2 to start doubting
                            SetLocalVariable("panicmode", 2);

                            SetLocalVariable("noise_heard", 2);

                            SetLocalVariable("looking_for", 0);
                            // speak who goes there
                            // SPEAK -a ~£whogoesthere~ NOP
                            SetLocalVariable("reflection_mode", 0);
                            Script.Instance.TimerClearByNameAndIO("quiet", Io);
                            SaveBehavior();
                            Io.NpcData.AddBehavior(RPGBase.Constants.Behaviour.BEHAVIOUR_MOVE_TO);
                            // SETTARGET PLAYER
                            Io.NpcData.Movemode = IoGlobals.WALKMODE;
                        }
                    }
                }
            }
            return ScriptConsts.ACCEPT;
        }
        private void SaveBehavior()
        {
            Script.Instance.TimerClearByNameAndIO("colplayer", Io);
            if (GetLocalIntVariableValue("main_behavior_stacked") == 0)
            {
                if (GetLocalIntVariableValue("frozen") == 1)
                {
                    // frozen anim -> wake up !
                    SetLocalVariable("frozen", 0);
                    // PLAYANIM NONE
                    // PLAYANIM -2 NONE
                    // PHYSICAL ON
                    // COLLISION ON
                    Io.NpcData.ChangeBehavior(
                            RPGBase.Constants.Behaviour.BEHAVIOUR_FRIENDLY.GetFlag(), 0);
                    // SETTARGET PLAYER
                }
                SetLocalVariable("main_behavior_stacked", 1);
                Debug.Log("stack");
                Io.NpcData.StackBehavior();
            }
            else
            {
                // CLEAR_MICE_TARGET
            }
        }
    }
}
