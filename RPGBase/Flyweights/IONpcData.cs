using RPGBase.Constants;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public abstract class IONpcData : IOCharacter
    {
        public static int MAX_STACKED_BEHAVIOR = 5;
        public float Absorb { get; set; }
        long aiming_start;
        float aimtime;
        float armor_class;
        public float ArmorClass { get; set; }
        float backstab_skill;
        private long behavior;
        float behavior_param;
        float climb_count;
        long collid_state;
        long collid_time;
        float critical;
        long cut;
        short cuts;
        float damages;
        long detect;
        float fDetect;
        long fightdecision;
        /** the <see cref="IONpcData"/>'s gender. */
        private int gender;
        /// <summary>
        /// the <see cref="BaseInteractiveObject"/> associated with this <see cref="IONpcData"/>.
        /// </summary>
        private BaseInteractiveObject io;
        public BaseInteractiveObject Io
        {
            get { return io; }
            set
            {
                io = value;
                if (value != null
                        && value.NpcData == null)
                {
                    value.NpcData = this;
                }
            }
        }
        float lastmouth;
        float life;
        float look_around_inc;
        long ltemp;
        float mana;
        float maxlife;
        float maxmana;
        public int Movemode { get; set; }
        float moveproblem;
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyWatchers();
            }
        }
        /** all IONpcData flags. */
        private long npcFlags;
        // IO_PATHFIND pathfind;
        // EERIE_EXTRA_ROTATE * ex_rotate;
        // D3DCOLOR blood_color;
        char padd;
        //private IOPathfind pathfinder;
        // IO_BEHAVIOR_DATA stacked[MAX_STACKED_BEHAVIOR];
        float poisonned;
        float reach;
        public bool Reachedtarget { get; set; } // Is
                                                // target
                                                // in
                                                // REACHZONE ?
        long reachedtime;
        char resist_fire;
        char resist_magic;
        char resist_poison;
        float speakpitch;
        public int SplatDamages { get; set; }
        public int SplatTotNb { get; set; }
        /** the stack of behaviors. */
        private BehaviourData[] stacked;
        float stare_factor;
        short strike_time;
        public int Tactics; // 0=none
        private int targetInfo;
        // ;
        /** the <see cref="IONpcData"/>'s title. */
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyWatchers();
            }
        }
        // 1=side ;
        // 2=side+back
        float tohit;
        short unused;
        // EERIE_3D last_splat_pos;
        float vvpos;

        short walk_start_time;
        /** the IONpcData's weapon. */
        private BaseInteractiveObject weapon;
        public BaseInteractiveObject Weapon
        {
            get { return weapon; }
            set
            {
                weapon = value;
                if (weapon != null)
                {
                    weaponInHand = weapon.GetRefId();
                }
                else
                {
                    weaponInHand = -1;
                }
            }
        }
        private int weaponInHand;
        /// <summary>
        /// the reference ID for the weapon in NPC's hand.
        /// </summary>
        public int WeaponInHand
        {
            get { return weaponInHand; }
            set
            {
                weaponInHand = value;
                if (Interactive.GetInstance().hasIO(weaponInHand))
                {
                    weapon = (BaseInteractiveObject)Interactive.GetInstance().getIO(weaponInHand);
                }
                else
                {
                    weapon = null;
                }
            }
        }
        char[] weaponname = new char[256];
        long weapontype;
        private int Xpvalue { get; set; }
        /// <summary>
        /// Creates a new instance of <see cref="IONpcData"/>.
        /// </summary>
        protected IONpcData()
        {

            name = "";
            stacked = new BehaviourData[MAX_STACKED_BEHAVIOR];
            for (int i = 0; i < MAX_STACKED_BEHAVIOR; i++)
            {
                if (stacked[i] == null)
                {
                    stacked[i] = new BehaviourData();
                }
            }
            //pathfinder = new IOPathfind();
        }
        /// <summary>
        /// Adds an behavior flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        public void AddBehavior(Behaviour behaviorEnum)
        {
            behavior |= behaviorEnum.GetFlag();
        }
        /// <summary>
        /// Adds an behavior flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        public void AddBehavior(long flag)
        {
            behavior |= flag;
        }
        /// <summary>
        /// Adds an IONpcData flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        public void AddNPCFlag(long flag)
        {
            npcFlags |= flag;
        }
        /// <summary>
        /// Applies extra damage from a poisoned attack.
        /// </summary>
        /// <param name="srcIoid">the source of the damage</param>
        /// <param name="isSpellDamage">flag indicating whether the damage is from a spell</param>
        private void ApplyPoisonDamage(int srcIoid, bool isSpellDamage)
        {
            if (Interactive.GetInstance().hasIO(srcIoid))
            {
                BaseInteractiveObject poisonWeaponIO = null;
                BaseInteractiveObject sourceIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(
                        srcIoid);
                if (sourceIO.HasIOFlag(IoGlobals.IO_01_PC))
                {
                    IOPcData player = sourceIO.PcData;
                    if (player.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON) > 0
                            && Interactive.GetInstance().hasIO(player.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON)))
                    {
                        poisonWeaponIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(player.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON));

                        if (poisonWeaponIO != null
                                && (poisonWeaponIO.PoisonLevel == 0
                                        || poisonWeaponIO.PoisonCharges == 0)
                                || isSpellDamage)
                        {
                            poisonWeaponIO = null;
                        }
                    }
                }
                else
                {
                    if (sourceIO.HasIOFlag(IoGlobals.IO_03_NPC))
                    {
                        poisonWeaponIO = (BaseInteractiveObject)sourceIO.NpcData.Weapon;
                        if (poisonWeaponIO != null
                                && (poisonWeaponIO.PoisonLevel == 0
                                        || poisonWeaponIO.PoisonCharges == 0))
                        {
                            poisonWeaponIO = null;
                        }
                    }
                }
                if (poisonWeaponIO == null)
                {
                    poisonWeaponIO = sourceIO;
                }
                if (poisonWeaponIO != null
                        && poisonWeaponIO.PoisonLevel > 0
                        && poisonWeaponIO.PoisonCharges > 0)
                {
                    // TODO - apply poison damage

                    // reduce poison level on attacking weapon
                    if (poisonWeaponIO.PoisonCharges > 0)
                    {
                        poisonWeaponIO.PoisonCharges--;
                    }
                }
                sourceIO = null;
                poisonWeaponIO = null;
            }
        }
        protected abstract void AwardXpForNpcDeath(int xp, BaseInteractiveObject killerIO);
        public void ChangeBehavior(int newBehavior, long p)
        {
            if (HasBehavior(Behaviour.BEHAVIOUR_FIGHT)
                    && (newBehavior & Behaviour.BEHAVIOUR_FIGHT.GetFlag()) == Behaviour.BEHAVIOUR_FIGHT.GetFlag())
            {
                StopActiveAnimation();
                // ANIM_USE * ause1 = &io->animlayer[1];
                // AcquireLastAnim(io);
                // FinishAnim(io, ause1->cur_anim);
                // ause1->cur_anim = NULL;
            }

            if (HasBehavior(Behaviour.BEHAVIOUR_NONE)
                    && (newBehavior & Behaviour.BEHAVIOUR_NONE.GetFlag()) == Behaviour.BEHAVIOUR_NONE.GetFlag())
            {
                StopIdleAnimation();
                // ANIM_USE * ause0 = &io->animlayer[0];
                // AcquireLastAnim(io);
                // FinishAnim(io, ause0->cur_anim);
                // ause0->cur_anim = NULL;
                // ANIM_Set(ause0, io->anims[ANIM_DEFAULT]);
                // ause0->flags &= ~EA_LOOP;

                StopActiveAnimation();
                // ANIM_USE * ause1 = &io->animlayer[1];
                // AcquireLastAnim(io);
                // FinishAnim(io, ause1->cur_anim);
                // ause1->cur_anim = NULL;
                // ause1->flags &= ~EA_LOOP;

                // stop whatever animation this is
                // ANIM_USE * ause2 = &io->animlayer[2];
                // AcquireLastAnim(io);
                // FinishAnim(io, ause2->cur_anim);
                // ause2->cur_anim = NULL;
                // ause2->flags &= ~EA_LOOP;
            }

            if ((newBehavior & Behaviour.BEHAVIOUR_FRIENDLY
                    .GetFlag()) == Behaviour.BEHAVIOUR_FRIENDLY.GetFlag())
            {
                StopIdleAnimation();
                // ANIM_USE * ause0 = &io->animlayer[0];
                // AcquireLastAnim(io);
                // FinishAnim(io, ause0->cur_anim);
                // ANIM_Set(ause0, io->anims[ANIM_DEFAULT]);
                // ause0->altidx_cur = 0;
            }
            ClearBehavior();
            behavior = newBehavior;
            behavior_param = p;
        }
        /// <summary>
        /// Clears all behavior flags that were set.
        /// </summary>
        public void ClearBehavior()
        {
            behavior = 0;
        }
        /// <summary>
        /// Clears all IONpcData flags that were set.
        /// </summary>
        public void ClearNPCFlags()
        {
            npcFlags = 0;
        }
        /// <summary>
        /// Handles a non-living IONpcData being damaged.
        /// </summary>
        /// <param name="dmg">the amount of damage</param>
        /// <param name="srcIoid">the source of the damage</param>
        /// <param name="isSpellDamage">flag indicating whether the damage is from a spell</param>
        protected abstract void DamageNonLivingNPC(float dmg, int srcIoid, bool isSpellDamage);
        /// <summary>
        /// Damages an IONpcData.
        /// </summary>
        /// <param name="dmg">the amount of damage</param>
        /// <param name="srcIoid">the source of the damage</param>
        /// <param name="isSpellDamage">flag indicating whether the damage is from a spell</param>
        /// <returns></returns>
        public float DamageNPC(float dmg, int srcIoid, bool isSpellDamage)
        {
            float damagesdone = 0f;
            if (io.Show > 0
                    && !io.HasIOFlag(IoGlobals.IO_08_INVULNERABILITY))
            {
                if (GetBaseLife() <= 0f)
                {
                    damageNonLivingNPC(dmg, srcIoid, isSpellDamage);
                }
                else
                {
                    // send OUCH event
                    SendOuchEvent(dmg, srcIoid);
                    // TODO - remove Confusion spell when hit

                    if (dmg >= 0f)
                    {
                        this.applyPoisonDamage(srcIoid, isSpellDamage);
                        int accepted = ScriptConsts.ACCEPT;
                        // if BaseInteractiveObject has a script, send HIT event
                        if (io.Script != null)
                        {
                            accepted = SendHitEvent(dmg, srcIoid, isSpellDamage);
                        }
                        // if HIT event doesn't handle damage, handle it here
                        if (accepted == ScriptConsts.ACCEPT)
                        {
                            damagesdone = ProcessDamage(dmg, srcIoid);
                        }
                    }
                }
            }
            return damagesdone;
        }
        public override float DrainMana(float dmg)
        {
            float manaDrained = 0;
            if (GetBaseMana() >= dmg)
            {
                AdjustMana(-dmg);
                manaDrained = dmg;
            }
            else
            {
                manaDrained = GetBaseMana();
                AdjustMana(-manaDrained);
            }
            return manaDrained;
        }
        /// <summary>
        /// Forces the IONpcData to die.
        /// </summary>
        /// <param name="killerIO">the BaseInteractiveObject that killed the IONpcData</param>
        public void ForceDeath(BaseInteractiveObject killerIO)
        {
            if (io.Mainevent == null
                    || (io.Mainevent != null
                            && !string.Equals(io.Mainevent, "DEAD", StringComparison.OrdinalIgnoreCase)))
            {
                BaseInteractiveObject oldSender = (BaseInteractiveObject)Script.GetInstance().GetEventSender();
                Script.GetInstance().SetEventSender(killerIO);

                // TODO - reset drag BaseInteractiveObject
                // if (io == DRAGINTER)
                // Set_DragInter(NULL);

                // TODO - reset flying over (with mouse) BaseInteractiveObject
                // if (io == FlyingOverIO)
                // FlyingOverIO = NULL;

                // TODO - reset camera 1 when pointing to BaseInteractiveObject
                // if ((MasterCamera.exist & 1) && (MasterCamera.io == io))
                // MasterCamera.exist = 0;

                // TODO - reset camera 2 when pointing to BaseInteractiveObject
                // if ((MasterCamera.exist & 2) && (MasterCamera.want_io == io))
                // MasterCamera.exist = 0;

                // TODO - kill dynamic lighting for BaseInteractiveObject
                // if (ValidDynLight(io->dynlight))
                // DynLight[io->dynlight].exist = 0;

                // io->dynlight = -1;

                // if (ValidDynLight(io->halo.dynlight))
                // DynLight[io->halo.dynlight].exist = 0;

                // io->halo.dynlight = -1;

                // reset all behaviors
                ResetBehavior();

                // TODO - kill speeches
                // ARX_SPEECH_ReleaseIOSpeech(io);

                // Kill all Timers...
                Script.GetInstance().TimerClearByIO(io);

                if (io.Mainevent == null
                        || (io.Mainevent != null
                                && !string.Equals(io.Mainevent, "DEAD", StringComparison.OrdinalIgnoreCase)))
                {
                    Script.GetInstance().NotifyIOEvent(io, ScriptConsts.SM_017_DIE, "");
                }

                if (Interactive.GetInstance().hasIO(io))
                {
                    io.Mainevent = "DEAD";

                    // TODO - kill animations
                    // if (EEDistance3D(&io_dead->pos, &ACTIVECAM->pos) > 3200) {
                    // io_dead->animlayer[0].ctime = 9999999;
                    // io_dead->lastanimtime = 0;
                    // }

                    // set killer
                    String killer = "";

                    WeaponInHand = -1;

                    Interactive.GetInstance().DestroyDynamicInfo(io);

                    // set killer name
                    if (killerIO != null
                            && killerIO.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        killer = "PLAYER";
                    }
                    else if (killerIO != null
                          && killerIO.HasIOFlag(IoGlobals.IO_03_NPC))
                    {
                        killer = killerIO.NpcData.Name;
                    }
                    int i = Interactive.GetInstance().GetMaxIORefId();
                    for (; i >= 0; i--)
                    {
                        if (!Interactive.GetInstance().hasIO(i))
                        {
                            continue;
                        }
                        BaseInteractiveObject ioo = (BaseInteractiveObject)Interactive.GetInstance().getIO(i);
                        if (ioo == null)
                        {
                            continue;
                        }
                        if (ioo.Equals(io))
                        {
                            continue;
                        }
                        if (ioo.HasIOFlag(IoGlobals.IO_03_NPC))
                        {
                            if (Interactive.GetInstance().hasIO(ioo.Targetinfo))
                            {
                                if (Interactive.GetInstance().getIO(ioo.Targetinfo).Equals(io))
                                {
                                    Script.GetInstance().SetEventSender(io);
                                    Script.GetInstance().StackSendIOScriptEvent(ioo,
                                            0,
                                            new Object[] { "killer", killer },
                                            "onTargetDeath");
                                    ioo.Targetinfo = IoGlobals.TARGET_NONE;
                                    ioo.NpcData.Reachedtarget = false;
                                }
                            }
                            // TODO - handle pathfinding target cleanup
                            // if (ValidIONum(ioo->_npcdata->pathfind.truetarget)) {
                            // if (inter.iobj[ioo->_npcdata->pathfind.truetarget] ==
                            // io_dead) {
                            // EVENT_SENDER = io_dead;
                            // Stack_SendIOScriptEvent(inter.iobj[i], 0, killer,
                            // "TARGET_DEATH");
                            // ioo->_npcdata->pathfind.truetarget = TARGET_NONE;
                            // ioo->_npcdata->reachedtarget = 0;
                            // }
                            // }
                        }
                    }

                    // TODO - kill animations
                    // IO_UnlinkAllLinkedObjects(io_dead);
                    // io_dead->animlayer[1].cur_anim = NULL;
                    // io_dead->animlayer[2].cur_anim = NULL;
                    // io_dead->animlayer[3].cur_anim = NULL;

                    // reduce life to 0
                    AdjustLife(-99999);

                    if (Weapon != null)
                    {
                        BaseInteractiveObject wpnIO = Weapon;
                        if (Interactive.GetInstance().hasIO(wpnIO))
                        {
                            wpnIO.Show = IoGlobals.SHOW_FLAG_IN_SCENE);
                            wpnIO.AddIOFlag(IoGlobals.IO_07_NO_COLLISIONS);
                            // TODO - reset positioning and velocity
                            // ioo->pos.x =
                            // ioo->obj->vertexlist3[ioo->obj->origin].v.x;
                            // ioo->pos.y =
                            // ioo->obj->vertexlist3[ioo->obj->origin].v.y;
                            // ioo->pos.z =
                            // ioo->obj->vertexlist3[ioo->obj->origin].v.z;
                            // ioo->velocity.x = 0.f;
                            // ioo->velocity.y = 13.f;
                            // ioo->velocity.z = 0.f;
                            // ioo->stopped = 0;
                        }
                    }
                }
                Script.GetInstance().SetEventSender(oldSender);
            }
        }
        /// <summary>
        /// Gets the IONpcData's base life value from the correct attribute.
        /// </summary>
        /// <returns></returns>
        public abstract float GetBaseLife();
        /// <summary>
        /// Gets the IONpcData's base mana value from the correct attribute.
        /// </summary>
        /// <returns></returns>
        public abstract float GetBaseMana();
        /*
        public IOPathfind getPathfinding()
        {
            return pathfinder;
        }
        */
        public abstract int getPoisonned();
        /// <summary>
        /// Determines if the <see cref="BaseInteractiveObject"/> has a specific behavior flag.
        /// </summary>
        /// <param name="behaviorEnum">the flag</param>
        /// <returns>true if the <see cref="BaseInteractiveObject"/> has the flag; false otherwise</returns>
        public bool HasBehavior(Behaviour behaviorEnum)
        {
            return HasBehavior(behaviorEnum.GetFlag());
        }
        /// <summary>
        /// Determines if the <see cref="BaseInteractiveObject"/> has a specific behavior flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        /// <returns></returns>
        public bool HasBehavior(long flag)
        {
            return (behavior & flag) == flag;
        }
        /// <summary>
        /// Determines if the IONpcData has life remaining.
        /// </summary>
        /// <returns><tt>true</tt> if the IONpcData still have some LP/HP remaining; <tt>false</tt> otherwise</returns>
        protected abstract bool HasLifeRemaining();
        /// <summary>
        /// Determines if the <see cref="IONpcData"/> has a specific flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        /// <returns>true if the <see cref="IONpcData"/> has the flag; false otherwise</returns>
        public bool HasNPCFlag(long flag)
        {
            return (npcFlags & flag) == flag;
        }
        /// <summary>
        /// Heals an <see cref="IONpcData"/> for a specific amount.
        /// </summary>
        /// <param name="healAmt">the healing amount</param>
        public void HealNPC(float healAmt)
        {
            if (GetBaseLife() > 0f)
            {
                if (healAmt > 0f)
                {
                    AdjustLife(healAmt);
                }
            }
        }
        /// <summary>
        /// Determines if an IONpcData is dead.
        /// </summary>
        /// <returns><tt>true</tt> if the IONpcData is dead; <tt>false</tt> otherwise</returns>
        public bool IsDeadNPC()
        {
            bool dead = false;
            if (!hasLifeRemaining())
            {
                dead = true;
            }
            if (!dead
                    && io.Mainevent != null
                    && string.Equals(io.Mainevent, "DEAD", StringComparison.OrdinalIgnoreCase))
            {
                dead = true;
            }
            return dead;
        }
        public abstract void ManagePoison();
        /// <summary>
        /// Moves the IONpcData to their initial position.
        /// </summary>
        protected abstract void MoveToInitialPosition();
        private float ProcessDamage(float dmg, int srcIoid)
        {
            float damagesdone = Math.Min(dmg, GetBaseLife());
            AdjustLife(-dmg);
            if (GetBaseLife() <= 0f)
            { // IONpcData is dead
              // base life should be 0
                if (Interactive.GetInstance().hasIO(srcIoid))
                {
                    int xp = Xpvalue;
                    BaseInteractiveObject srcIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(srcIoid);
                    ForceDeath(srcIO);
                    if (srcIO.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        awardXpForNpcDeath(xp, srcIO);
                    }
                }
                else
                {
                    ForceDeath(null);
                }
            }
            return damagesdone;
        }
        /// <summary>
        /// Removes an behavior flag.
        /// </summary>
        /// <param name="flag">the <see cref="Behaviour"/></param>
        public void RemoveBehavior(Behaviour behaviorEnum)
        {
            behavior &= ~behaviorEnum.GetFlag();
        }
        /// <summary>
        /// Removes an behavior flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        public void RemoveBehavior(long flag)
        {
            behavior &= ~flag;
        }
        /// <summary>
        /// Removes an IONpcData flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        public void RemoveNPCFlag(long flag)
        {
            npcFlags &= ~flag;
        }
        /// <summary>
        /// Resets the behavior.
        /// </summary>
        public void ResetBehavior()
        {
            behavior = Behaviour.BEHAVIOUR_NONE.GetFlag();
            for (int i = 0; i < MAX_STACKED_BEHAVIOR; i++)
            {
                if (stacked[i] == null)
                {
                    stacked[i] = new BehaviourData();
                }
                stacked[i].Exists = false;
            }
        }
        /** Restores the IONpcData to their maximum life. */
        protected abstract void RestoreLifeToMax();
        /// <summary>
        /// Revives the IONpcData.
        /// </summary>
        /// <param name="reposition">if <tt>true</tt> IONpcData is moved to their initial position</param>
        public void ReviveNpc(bool reposition)
        {
            // TODO - check if secondary inventory belongs to the IONpcData
            // and kill it
            // if ((TSecondaryInventory) && (TSecondaryInventory->io == io)) {
            // TSecondaryInventory = NULL;
            // }

            Script.GetInstance().SetMainEvent(Io, "MAIN");

            Io.RemoveIOFlag(IoGlobals.IO_07_NO_COLLISIONS);
            RestoreLifeToMax();
            Script.GetInstance().ResetObject(io, true);
            RestoreLifeToMax();

            if (reposition)
            {
                MoveToInitialPosition();
            }
            // reset texture - later
            // long goretex = -1;

            // for (long i = 0; i < io->obj->nbmaps; i++) {
            // if (io->obj->texturecontainer
            // && io->obj->texturecontainer[i]
            // && (IsIn(io->obj->texturecontainer[i]->m_strName, "GORE"))) {
            // goretex = i;
            // break;
            // }
            // }

            // for (long ll = 0; ll < io->obj->nbfaces; ll++) {
            // if (io->obj->facelist[ll].texid != goretex) {
            // io->obj->facelist[ll].facetype &= ~POLY_HIDE;
            // } else {
            // io->obj->facelist[ll].facetype |= POLY_HIDE;
            // }
            // }

            cuts = 0;
        }
        /// <summary>
        /// Sends the IONpcData BaseInteractiveObject a 'Hit' event.
        /// </summary>
        /// <param name="dmg">the amount of damage</param>
        /// <param name="srcIoid">the source of the damage</param>
        /// <param name="isSpellDamage">flag indicating whether the damage is from a spell</param>
        /// <returns></returns>
        private int SendHitEvent(float dmg, int srcIoid, bool isSpellDamage)
        {
            if (Interactive.GetInstance().hasIO(srcIoid))
            {
                Script.GetInstance().SetEventSender(Interactive.GetInstance().getIO(srcIoid));
            }
            else
            {
                Script.GetInstance().SetEventSender(null);
            }

            Object[] p;
            if (Script.GetInstance().GetEventSender() != null
                    && Script.GetInstance().GetEventSender().HasIOFlag(IoGlobals.IO_01_PC))
            {
                BaseInteractiveObject plrIO = (BaseInteractiveObject)Script.GetInstance().GetEventSender();
                if (isSpellDamage)
                {
                    p = new Object[] { "SPELL_DMG", dmg };
                }
                else
                {
                    int wpnId = plrIO.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
                    BaseInteractiveObject wpnIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(wpnId);
                    int wpnType = EquipmentGlobals.WEAPON_BARE;
                    if (wpnIO != null)
                    {
                        wpnType = wpnIO.ItemData.GetWeaponType();
                    }
                    switch (wpnType)
                    {
                        case EquipmentGlobals.WEAPON_BARE:
                            p = new Object[] { "BARE_DMG", dmg };
                            break;
                        case EquipmentGlobals.WEAPON_DAGGER:
                            p = new Object[] { "DAGGER_DMG", dmg };
                            break;
                        case EquipmentGlobals.WEAPON_1H:
                            p = new Object[] { "1H_DMG", dmg };
                            break;
                        case EquipmentGlobals.WEAPON_2H:
                            p = new Object[] { "2H_DMG", dmg };
                            break;
                        case EquipmentGlobals.WEAPON_BOW:
                            p = new Object[] { "ARROW_DMG", dmg };
                            break;
                        default:
                            p = new Object[] { "DMG", dmg };
                            break;
                    }
                    wpnIO = null;
                }
                plrIO = null;
            }
            else
            {
                p = new Object[] { "DMG", dmg };
            }
            // if player summoned object causing damage,
            // change event sender to player
            if (SummonerIsPlayer((BaseInteractiveObject)Script.GetInstance().GetEventSender()))
            {
                BaseInteractiveObject summonerIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(Script.GetInstance().GetEventSender().getSummoner());
                Script.GetInstance().SetEventSender(summonerIO);
                summonerIO = null;
                p = new Object[] { "SUMMONED_DMG", dmg };
            }
            else
            {
                p = new Object[] {
                    "SUMMONED_OUCH", 0f,
                    "OUCH", io.DamageSum };
            }
            return Script.GetInstance().SendIOScriptEvent(io, ScriptConsts.SM_016_HIT, p, null);
        }
        /// <summary>
        /// Sends the IONpcData BaseInteractiveObject an 'Ouch' event.
        /// </summary>
        /// <param name="dmg">the amount of damage</param>
        /// <param name="srcIoid">the source of the damage</param>
        private void SendOuchEvent(float dmg, int srcIoid)

        {
            io.DamageSum += dmg;
            // set the event sender
            if (Interactive.GetInstance().hasIO(srcIoid))
            {
                Script.GetInstance().SetEventSender(Interactive.GetInstance().getIO(srcIoid));
            }
            else
            {
                Script.GetInstance().SetEventSender(null);
            }
            // check to see if the damage is coming from a summoned object
            Object[] p;
            if (SummonerIsPlayer((BaseInteractiveObject)Script.GetInstance().GetEventSender()))
            {
                p = new Object[] {
                    "SUMMONED_OUCH", io.DamageSum,
                    "OUCH", 0f };
            }
            else
            {
                p = new Object[] {
                    "SUMMONED_OUCH", 0f,
                    "OUCH", io.DamageSum };
            }
            Script.GetInstance().SendIOScriptEvent(io, ScriptConsts.SM_045_OUCH, p, null);
            io.DamageSum = 0f;
        }
        public void StackBehavior()
        {
            for (int i = 0; i < MAX_STACKED_BEHAVIOR; i++)
            {
                BehaviourData bd = stacked[i];
                if (!bd.Exists)
                {
                    bd.Behaviour = behavior;
                    bd.BehaviorParam = behavior_param;
                    bd.Tactics = Tactics;
                    // set pathfinding information
                    // if (io->_npcdata->pathfind.listnb > 0)
                    // bd->target = io->_npcdata->pathfind.truetarget;
                    // else
                    // bd->target = io->targetinfo;

                    bd.MoveMode = Movemode;
                    bd.Exists = true;
                    break;
                }
            }
        }
        /** Restores the IONpcData to their maximum life. */
        protected abstract void StopActiveAnimation();
        /** Restores the IONpcData to their maximum life. */
        protected abstract void StopIdleAnimation();
        /// <summary>
        /// Determines if a summoned BaseInteractiveObject's summoner is a IOPcData.
        /// </summary>
        /// <param name="io">the <see cref="BaseInteractiveObject"/></param>
        /// <returns>true if the summoner is a player; false otherwise</returns>
        private bool SummonerIsPlayer(BaseInteractiveObject io)
        {
            bool isPlayer = false;
            if (io != null)
            {
                int summonerId = io.Summoner;
                if (Interactive.GetInstance().hasIO(summonerId))
                {
                    BaseInteractiveObject summoner = (BaseInteractiveObject)Interactive.GetInstance().getIO(summonerId);
                    if (summoner.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        isPlayer = true;
                    }
                    summoner = null;
                }
            }
            return isPlayer;
        }
        public void UnstackBehavior()
        {
            for (int i = 0; i < MAX_STACKED_BEHAVIOR; i++)
            {
                BehaviourData bd = stacked[i];
                if (bd.Exists)
                {
                    // AcquireLastAnim(io);
                    behavior = bd.Behaviour;
                    behavior_param = bd.BehaviorParam;
                    this.Tactics = bd.Tactics;
                    this.targetInfo = bd.Target;
                    this.Movemode = bd.MoveMode;
                    bd.Exists = false;
                    // ARX_NPC_LaunchPathfind(io, bd->target);

                    if (this.HasBehavior(Behaviour.BEHAVIOUR_NONE))
                    {
                        // memcpy(io->animlayer, bd->animlayer,
                        // sizeof(ANIM_USE)*MAX_ANIM_LAYERS);
                    }
                }
            }
        }
    }
}
