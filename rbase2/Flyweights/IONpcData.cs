﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public abstract class IONpcData
    {
        public static  int MAX_STACKED_BEHAVIOR = 5;
        float absorb;
        long aiming_start;
        float aimtime;
        float armor_class;
        float armorClass;
        float backstab_skill;
        private int behavior;
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
        /** the {@link IoNpcData}'s gender. */
        private int gender;
        /** the BaseInteractiveObject associated with this {@link IoNpcData}. */
        private BaseInteractiveObject io;
        float lastmouth;
        float life;
        float look_around_inc;
        long ltemp;
        float mana;
        float maxlife;
        float maxmana;
        private int movemode;
        float moveproblem;
        /** the {@link IoNpcData}'s name. */
        private char[] name;
        /** all IONpcData flags. */
        private long npcFlags;
        // IO_PATHFIND pathfind;
        // EERIE_EXTRA_ROTATE * ex_rotate;
        // D3DCOLOR blood_color;
        char padd;
        private IOPathfind pathfinder;
        // IO_BEHAVIOR_DATA stacked[MAX_STACKED_BEHAVIOR];
        float poisonned;
        float reach;
        private bool reachedtarget; // Is
                                       // target
                                       // in
                                       // REACHZONE ?
        long reachedtime;
        char resist_fire;
        char resist_magic;
        char resist_poison;
        float speakpitch;
        private int splatDamages;
        private int splatTotNb;
        /** the stack of behaviors. */
        private  BehaviourData[] stacked;
    float stare_factor;
        short strike_time;
        private int tactics; // 0=none
        private int targetInfo;
        // ;
        /** the {@link IoNpcData}'s title. */
        private char[] title;
        // 1=side ;
        // 2=side+back
        float tohit;
        short unused;
        // EERIE_3D last_splat_pos;
        float vvpos;

        short walk_start_time;
        /** the IONpcData's weapon. */
        private BaseInteractiveObject weapon;
        private int weaponInHand;
        char[] weaponname = new char[256];
        long weapontype;
        private int xpvalue;
        /**
         * Creates a new instance of {@link IoNpcData}.
         * @ if there is an error defining attributes
         */
        protected IoNpcData() 
        {
            
        name = new char[0];
        stacked = new BehaviourData[MAX_STACKED_BEHAVIOR];
        for (int i = 0; i<MAX_STACKED_BEHAVIOR; i++) {
            if (stacked[i] == null) {
                stacked[i] = new BehaviourData();
    }
}
pathfinder = new IOPathfind();
    }
    /**
     * Adds a behavior flag.
     * @param flag the flag
     */
    public void addBehavior( Behaviour behaviorEnum)
{
    behavior |= behaviorEnum.GetFlag();
}
/**
 * Adds a behavior flag.
 * @param flag the flag
 */
public void addBehavior( long flag)
{
    behavior |= flag;
}
/**
 * Adds an IONpcData flag.
 * @param flag the flag
 */
public  void addNPCFlag( long flag)
{
    npcFlags |= flag;
}
/**
 * Adjusts the IONpcData's life by a specific amount.
 * @param dmg the amount
 */
protected abstract void adjustLife(float dmg);
/**
 * Adjusts the IONpcData's mana by a specific amount.
 * @param dmg the amount
 */
protected abstract void adjustMana(float dmg);
/**
 * Applies extra damage from a poisoned attack.
 * @param srcIoid the source of the damage
 * @param isSpellDamage flag indicating whether the damage is from a spell
 * @ if an error occurs
 */
private void applyPoisonDamage( int srcIoid,
         bool isSpellDamage) 
{
        if (Interactive.GetInstance().hasIO(srcIoid)) {
        BaseInteractiveObject poisonWeaponIO = null;
        BaseInteractiveObject sourceIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(
                srcIoid);
        if (sourceIO.HasIOFlag(IoGlobals.IO_01_PC))
        {
            IoPcData player = sourceIO.PcData;
            if (player.getEquippedItem(
                    EquipmentGlobals.EQUIP_SLOT_WEAPON) > 0
                    && Interactive.GetInstance().hasIO(
                            player.getEquippedItem(
                                    EquipmentGlobals.EQUIP_SLOT_WEAPON)))
            {
                poisonWeaponIO = (BaseInteractiveObject)Interactive.GetInstance()
                        .getIO(player.getEquippedItem(
                                EquipmentGlobals.EQUIP_SLOT_WEAPON));

                if (poisonWeaponIO != null
                        && (poisonWeaponIO.getPoisonLevel() == 0
                                || poisonWeaponIO
                                        .getPoisonCharges() == 0)
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
                poisonWeaponIO =
                        (BaseInteractiveObject)sourceIO.getNPCData().getWeapon();
                if (poisonWeaponIO != null
                        && (poisonWeaponIO.getPoisonLevel() == 0
                                || poisonWeaponIO
                                        .getPoisonCharges() == 0))
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
                && poisonWeaponIO.getPoisonLevel() > 0
                && poisonWeaponIO.getPoisonCharges() > 0)
        {
            // TODO - apply poison damage

            // reduce poison level on attacking weapon
            if (poisonWeaponIO.getPoisonCharges() > 0)
            {
                poisonWeaponIO.setPoisonCharges(
                        poisonWeaponIO.getPoisonCharges() - 1);
            }
        }
        sourceIO = null;
        poisonWeaponIO = null;
    }
}
/**
 * Drains mana from the IONpcData, returning the full amount drained.
 * @param dmg the attempted amount of mana to be drained
 * @return {@link float}
 */
public  float ARX_DAMAGES_DrainMana( float dmg)
{
    float manaDrained = 0;
    if (getBaseMana() >= dmg)
    {
        adjustMana(-dmg);
        manaDrained = dmg;
    }
    else
    {
        manaDrained = getBaseMana();
        adjustMana(-manaDrained);
    }
    return manaDrained;
}
/**
 * Heals the IONpcData's mana.
 * @param dmg the amount of healing
 */
public  void ARX_DAMAGES_HealManaInter( float dmg)
{
    if (getBaseLife() > 0.f)
    {
        if (dmg > 0.f)
        {
            adjustMana(dmg);
        }
    }
}
public  void ARX_NPC_Behaviour_Change( int newBehavior, long p)
{
    if (hasBehavior(Behaviour.BEHAVIOUR_FIGHT)
            && (newBehavior & Behaviour.BEHAVIOUR_FIGHT
                    .GetFlag()) == Behaviour.BEHAVIOUR_FIGHT.GetFlag())
    {
        stopActiveAnimation();
        // ANIM_USE * ause1 = &io->animlayer[1];
        // AcquireLastAnim(io);
        // FinishAnim(io, ause1->cur_anim);
        // ause1->cur_anim = NULL;
    }

    if (hasBehavior(Behaviour.BEHAVIOUR_NONE)
            && (newBehavior & Behaviour.BEHAVIOUR_NONE
                    .GetFlag()) == Behaviour.BEHAVIOUR_NONE.GetFlag())
    {
        stopIdleAnimation();
        // ANIM_USE * ause0 = &io->animlayer[0];
        // AcquireLastAnim(io);
        // FinishAnim(io, ause0->cur_anim);
        // ause0->cur_anim = NULL;
        // ANIM_Set(ause0, io->anims[ANIM_DEFAULT]);
        // ause0->flags &= ~EA_LOOP;

        stopActiveAnimation();
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
        stopIdleAnimation();
        // ANIM_USE * ause0 = &io->animlayer[0];
        // AcquireLastAnim(io);
        // FinishAnim(io, ause0->cur_anim);
        // ANIM_Set(ause0, io->anims[ANIM_DEFAULT]);
        // ause0->altidx_cur = 0;
    }
    clearBehavior();
    behavior = newBehavior;
    behavior_param = p;
}
public void ARX_NPC_Behaviour_Stack()
{
    for (int i = 0; i < MAX_STACKED_BEHAVIOR; i++)
    {
        BehaviourData bd = stacked[i];
        if (!bd.exists())
        {
            bd.setBehaviour(behavior);
            bd.setBehaviorParam(behavior_param);
            bd.setTactics(tactics);
            // set pathfinding information
            // if (io->_npcdata->pathfind.listnb > 0)
            // bd->target = io->_npcdata->pathfind.truetarget;
            // else
            // bd->target = io->targetinfo;

            bd.setMovemode(movemode);
            bd.setExists(true);
            return;
        }
    }
}
public void ARX_NPC_Behaviour_UnStack()
{
    for (int i = 0; i < MAX_STACKED_BEHAVIOR; i++)
    {
        BehaviourData bd = stacked[i];
        if (bd.exists())
        {
            // AcquireLastAnim(io);
            behavior = bd.getBehaviour();
            behavior_param = bd.getBehaviorParam();
            this.tactics = bd.getTactics();
            this.targetInfo = bd.getTarget();
            this.movemode = bd.getMoveMode();
            bd.setExists(false);
            // ARX_NPC_LaunchPathfind(io, bd->target);

            if (this.hasBehavior(Behaviour.BEHAVIOUR_NONE))
            {
                // memcpy(io->animlayer, bd->animlayer,
                // sizeof(ANIM_USE)*MAX_ANIM_LAYERS);
            }
        }
    }
}
public abstract void ARX_NPC_ManagePoison();
/**
 * Revives the IONpcData.
 * @param reposition if <tt>true</tt> IONpcData is moved to their initial position
 * @ if an error occurs
 */
public  void ARX_NPC_Revive( bool reposition)
            
{
    // TODO - check if secondary inventory belongs to the IONpcData
    // and kill it
    // if ((TSecondaryInventory) && (TSecondaryInventory->io == io)) {
    // TSecondaryInventory = NULL;
    // }

    Script.GetInstance().setMainEvent(getIo(), "MAIN");

    getIo().RemoveIOFlag(IoGlobals.IO_07_NO_COLLISIONS);
    restoreLifeToMax();
    Script.GetInstance().resetObject(io, true);
    restoreLifeToMax();

        if (reposition) {
        moveToInitialPosition();
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
/**
 * @param xp
 * @param killerIO
 */
protected abstract void awardXpForNpcDeath( int xp,  BaseInteractiveObject killerIO);
/** Clears all behavior flags that were set. */
public void clearBehavior()
{
    behavior = 0;
}
/** Clears all IONpcData flags that were set. */
public  void clearNPCFlags()
{
    npcFlags = 0;
}
/**
 * Handles a non-living IONpcData being damaged.
 * @param dmg the amount of damage
 * @param srcIoid the source of the damage
 * @param isSpellDamage flag indicating whether the damage is from a spell
 * @ if an error occurs
 */
protected abstract void damageNonLivingNPC(float dmg, int srcIoid,
        bool isSpellDamage) ;
/**
 * Damages an IONpcData.
 * @param dmg the amount of damage
 * @param srcIoid the source of the damage
 * @param isSpellDamage flag indicating whether the damage is from a spell
 * @return {@link float}
 * @ if an error occurs
 */
public  float damageNPC( float dmg,  int srcIoid,
         bool isSpellDamage) 
{
        float damagesdone = 0.f;
        if (io.getShow() > 0
                && !io.HasIOFlag(IoGlobals.IO_08_INVULNERABILITY)) {
        if (getBaseLife() <= 0f)
        {
            damageNonLivingNPC(dmg, srcIoid, isSpellDamage);
        }
        else
        {
            // send OUCH event
            sendOuchEvent(dmg, srcIoid);
            // TODO - remove Confusion spell when hit

            if (dmg >= 0.f)
            {
                this.applyPoisonDamage(srcIoid, isSpellDamage);
                int accepted = ScriptConsts.ACCEPT;
                // if BaseInteractiveObject has a script, send HIT event
                if (io.getScript() != null)
                {
                    accepted = sendHitEvent(dmg, srcIoid, isSpellDamage);
                }
                // if HIT event doesn't handle damage, handle it here
                if (accepted == ScriptConsts.ACCEPT)
                {
                    damagesdone = processDamage(dmg, srcIoid);
                }
            }
        }
    }
        return damagesdone;
}
/**
 * Forces the IONpcData to die.
 * @param killerIO the BaseInteractiveObject that killed the IONpcData
 * @ if an error occurs
 */
public  void forceDeath( BaseInteractiveObject killerIO) 
{
        if (io.getMainevent() == null
                || (io.getMainevent() != null
                        && !io.getMainevent().equalsIgnoreCase("DEAD"))) {
        BaseInteractiveObject oldSender = (BaseInteractiveObject)Script.GetInstance().getEventSender();
        Script.GetInstance().setEventSender(killerIO);

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
        resetBehavior();

        // TODO - kill speeches
        // ARX_SPEECH_ReleaseIOSpeech(io);

        // Kill all Timers...
        Script.GetInstance().timerClearByIO(io);

        if (io.getMainevent() == null
                || (io.getMainevent() != null
                        && !io.getMainevent().equalsIgnoreCase("DEAD")))
        {
            Script.GetInstance().notifyIOEvent(
                    io, ScriptConsts.SM_017_DIE, "");
        }

        if (Interactive.GetInstance().hasIO(io))
        {
            io.setMainevent("DEAD");

            // TODO - kill animations
            // if (EEDistance3D(&io_dead->pos, &ACTIVECAM->pos) > 3200) {
            // io_dead->animlayer[0].ctime = 9999999;
            // io_dead->lastanimtime = 0;
            // }

            // set killer
            String killer = "";

            setWeaponInHand(-1);

            Interactive.GetInstance().ARX_INTERACTIVE_DestroyDynamicInfo(
                    io);

            // set killer name
            if (killerIO != null
                    && killerIO.HasIOFlag(IoGlobals.IO_01_PC))
            {
                killer = "PLAYER";
            }
            else if (killerIO != null
                  && killerIO.HasIOFlag(IoGlobals.IO_03_NPC))
            {
                killer = new String(killerIO.getNPCData().getName());
            }
            int i = Interactive.GetInstance().getMaxIORefId();
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
                    if (Interactive.GetInstance().hasIO(
                            ioo.getTargetinfo()))
                    {
                        if (Interactive.GetInstance().getIO(
                                ioo.getTargetinfo()).Equals(io))
                        {
                            Script.GetInstance().setEventSender(io);
                            Script.GetInstance().stackSendIOScriptEvent(ioo,
                                    0,
                                    new Object[] { "killer", killer },
                                    "onTargetDeath");
                            ioo.setTargetinfo(IoGlobals.TARGET_NONE);
                            ioo.getNPCData().setReachedtarget(false);
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
            adjustLife(-99999);

            if (getWeapon() != null)
            {
                BaseInteractiveObject wpnIO = getWeapon();
                if (Interactive.GetInstance().hasIO(wpnIO))
                {
                    wpnIO.show =IoGlobals.SHOW_FLAG_IN_SCENE);
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
        Script.GetInstance().setEventSender(oldSender);
    }
}
/**
 * Gets the armorClass
 * @return {@link float}
 */
public float getArmorClass()
{
    return armorClass;
}
/**
 * Gets the IONpcData's base life value from the correct attribute.
 * @return {@link float}
 */
public abstract float getBaseLife();
/**
 * Gets the IONpcData's base mana value from the correct attribute.
 * @return {@link float}
 */
public abstract float getBaseMana();
/**
 * Gets the {@link IoNpcData}'s gender.
 * @return int
 */
public  int getGender()
{
    return gender;
}
/**
 * Gets the BaseInteractiveObject associated with this {@link IoNpcData}.
 * @return {@link BaseInteractiveObject}
 */

    public  BaseInteractiveObject getIo()
{
    return io;
}
/**
 * Gets the value for the movemode.
 * @return {@link int}
 */
public int getMovemode()
{
    return movemode;
}
/**
 * Gets the {@link IoNpcData}'s name.
 * @return char[]
 */
public  char[] getName()
{
    return name;
}
public IOPathfind getPathfinding()
{
    return pathfinder;
}
public abstract int getPoisonned();
/**
 * Gets the splatDamages
 * @return {@link int}
 */
public int getSplatDamages()
{
    return splatDamages;
}
/**
 * Gets the splatTotNb
 * @return {@link int}
 */
public int getSplatTotNb()
{
    return splatTotNb;
}
/**
 * Gets the value for the tactics.
 * @return {@link int}
 */
public int getTactics()
{
    return tactics;
}
/**
 * Gets the {@link IoNpcData}'s title.
 * @return char[]
 */
public  char[] getTitle()
{
    return title;
}
/**
 * Gets the IONpcData's weapon.
 * @return {@link BaseInteractiveObject}
 */
public  BaseInteractiveObject getWeapon()
{
    return weapon;
}
/**
 * Gets the value for the weaponInHand.
 * @return {@link int}
 */
public int getWeaponInHand()
{
    return weaponInHand;
}
/**
 * Gets the value for the xpvalue.
 * @return {@link int}
 */
public int getXpvalue()
{
    return xpvalue;
}
public long getXPValue()
{
    return this.xpvalue;
}
/**
 * Determines if the {@link BaseInteractiveObject} has a specific behavior
 * flag.
 * @param flag the flag
 * @return true if the {@link BaseInteractiveObject} has the flag; false
 *         otherwise
 */
public  bool hasBehavior( Behaviour behaviorEnum)
{
    return hasBehavior(behaviorEnum.GetFlag());
}
/**
 * Determines if the {@link BaseInteractiveObject} has a specific behavior
 * flag.
 * @param flag the flag
 * @return true if the {@link BaseInteractiveObject} has the flag; false
 *         otherwise
 */
public  bool hasBehavior( long flag)
{
    return (behavior & flag) == flag;
}
/**
 * Determines if the IONpcData has life remaining.
 * @return <tt>true</tt> if the IONpcData still have some LP/HP remaining;
 *         <tt>false</tt> otherwise
 */
protected abstract bool hasLifeRemaining();
/**
 * Determines if the {@link IoNpcData} has a specific flag.
 * @param flag the flag
 * @return true if the {@link IoNpcData} has the flag; false otherwise
 */
public  bool hasNPCFlag( long flag)
{
    return (npcFlags & flag) == flag;
}
/**
 * Gets the value for the reachedtarget.
 * @return {@link long}
 */
public bool hasReachedtarget()
{
    return reachedtarget;
}
/**
 * Heals an IONpcData for a specific amount.
 * @param healAmt the healing amount
 */
public  void healNPC( float healAmt)
{
    if (getBaseLife() > 0.f)
    {
        if (healAmt > 0.f)
        {
            adjustLife(healAmt);
        }
    }
}
/**
 * Determines if an IONpcData is dead.
 * @return <tt>true</tt> if the IONpcData is dead; <tt>false</tt> otherwise
 */
public bool IsDeadNPC()
{
    bool dead = false;
    if (!hasLifeRemaining())
    {
        dead = true;
    }
    if (!dead
            && io.getMainevent() != null
            && io.getMainevent().equalsIgnoreCase("DEAD"))
    {
        dead = true;
    }
    return dead;
}
/** Moves the IONpcData to their initial position. */
protected abstract void moveToInitialPosition();
/**
 * @param dmg
 * @param srcIoid
 * @return
 * @
 */
private float processDamage( float dmg,  int srcIoid)
            
{
        float damagesdone = Math.min(dmg, getBaseLife());
    adjustLife(-dmg);
        if (getBaseLife() <= 0.f) { // IONpcData is dead
                                    // base life should be 0
        if (Interactive.GetInstance().hasIO(srcIoid))
        {
            int xp = xpvalue;
            BaseInteractiveObject srcIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(srcIoid);
            forceDeath(srcIO);
            if (srcIO.HasIOFlag(IoGlobals.IO_01_PC))
            {
                awardXpForNpcDeath(xp, srcIO);
            }
        }
        else
        {
            forceDeath(null);
        }
    }
        return damagesdone;
}
/**
 * Removes a behavior flag.
 * @param flag the flag
 */
public  void removeBehavior( Behaviour behaviorEnum)
{
    behavior &= ~behaviorEnum.GetFlag();
}
/**
 * Removes a behavior flag.
 * @param flag the flag
 */
public  void removeBehavior( long flag)
{
    behavior &= ~flag;
}
/**
 * Removes an IONpcData flag.
 * @param flag the flag
 */
public  void removeNPCFlag( long flag)
{
    npcFlags &= ~flag;
}
/** Resets the behavior. */
public void resetBehavior()
{
    behavior = Behaviour.BEHAVIOUR_NONE.GetFlag();
    for (int i = 0; i < MAX_STACKED_BEHAVIOR; i++)
    {
        if (stacked[i] == null)
        {
            stacked[i] = new BehaviourData();
        }
        stacked[i].setExists(false);
    }
}
/** Restores the IONpcData to their maximum life. */
protected abstract void restoreLifeToMax();
/**
 * Sends the IONpcData BaseInteractiveObject a 'Hit' event.
 * @param dmg the amount of damage
 * @param srcIoid the source of the damage
 * @param isSpellDamage flag indicating whether the damage is from a spell
 * @ if an error occurs
 */
private int sendHitEvent( float dmg,  int srcIoid,
         bool isSpellDamage) 
{
        if (Interactive.GetInstance().hasIO(srcIoid)) {
        Script.GetInstance().setEventSender(
                Interactive.GetInstance().getIO(srcIoid));
    } else {
        Script.GetInstance().setEventSender(null);
    }

    Object [] params;
        if (Script.GetInstance().getEventSender() != null
                && Script.GetInstance().getEventSender().HasIOFlag(
                        IoGlobals.IO_01_PC)) {
        BaseInteractiveObject plrIO = (BaseInteractiveObject)Script.GetInstance().getEventSender();
        if (isSpellDamage)
        {
                params = new Object[] { "SPELL_DMG", dmg };
        }
        else
        {
            int wpnId = plrIO.PcData.getEquippedItem(
                    EquipmentGlobals.EQUIP_SLOT_WEAPON);
            BaseInteractiveObject wpnIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(wpnId);
            int wpnType = EquipmentGlobals.WEAPON_BARE;
            if (wpnIO != null)
            {
                wpnType = wpnIO.ItemData.getWeaponType();
            }
            switch (wpnType)
            {
                case EquipmentGlobals.WEAPON_BARE:
                    params = new Object[] { "BARE_DMG", dmg };
                    break;
                case EquipmentGlobals.WEAPON_DAGGER:
                    params = new Object[] { "DAGGER_DMG", dmg };
                    break;
                case EquipmentGlobals.WEAPON_1H:
                    params = new Object[] { "1H_DMG", dmg };
                    break;
                case EquipmentGlobals.WEAPON_2H:
                    params = new Object[] { "2H_DMG", dmg };
                    break;
                case EquipmentGlobals.WEAPON_BOW:
                    params = new Object[] { "ARROW_DMG", dmg };
                    break;
                default:
                    params = new Object[] { "DMG", dmg };
                    break;
            }
            wpnIO = null;
        }
        plrIO = null;
    } else {
            params = new Object[] { "DMG", dmg };
    }
        // if player summoned object causing damage,
        // change event sender to player
        if (summonerIsPlayer((BaseInteractiveObject) Script.GetInstance().getEventSender())) {
        BaseInteractiveObject summonerIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(
                Script.GetInstance().getEventSender().getSummoner());
        Script.GetInstance().setEventSender(summonerIO);
        summonerIO = null;
            params = new Object[] { "SUMMONED_DMG", dmg };
    } else {
            params = new Object[] {
                    "SUMMONED_OUCH", 0f,
                    "OUCH", io.getDamageSum() };
    }
        return Script.GetInstance().sendIOScriptEvent(
                io, ScriptConsts.SM_016_HIT, params, null);
}
/**
 * Sends the IONpcData BaseInteractiveObject an 'Ouch' event.
 * @param dmg the amount of damage
 * @param srcIoid the source of the damage
 * @ if an error occurs
 */
private void sendOuchEvent( float dmg,  int srcIoid)
            
{
    io.setDamageSum(io.getDamageSum() + dmg);
        // set the event sender
        if (Interactive.GetInstance().hasIO(srcIoid)) {
        Script.GetInstance().setEventSender(
                Interactive.GetInstance().getIO(srcIoid));
    } else {
        Script.GetInstance().setEventSender(null);
    }
    // check to see if the damage is coming from a summoned object
    Object [] params;
        if (summonerIsPlayer((BaseInteractiveObject) Script.GetInstance().getEventSender())) {
            params = new Object[] {
                    "SUMMONED_OUCH", io.getDamageSum(),
                    "OUCH", 0f };
    } else {
            params = new Object[] {
                    "SUMMONED_OUCH", 0f,
                    "OUCH", io.getDamageSum() };
    }
    Script.GetInstance().sendIOScriptEvent(io,
                ScriptConsts.SM_045_OUCH, params, null);
    io.setDamageSum(0f);
}
/**
 * Sets the armorClass
 * @param val the armorClass to set
 */
public void setArmorClass( float val)
{
    this.armorClass = val;
}
/**
 * Sets the {@link IoNpcData}'s gender.
 * @param val the gender to set
 */
public  void setGender( int val)
{
    gender = val;
    notifyWatchers();
}
/**
 * Sets the BaseInteractiveObject associated with this {@link IoNpcData}.
 * @param newIO the BaseInteractiveObject to set
 */
public  void setIo( BaseInteractiveObject newIO)
{
    this.io = newIO;
}
/**
 * Sets the value of the movemode.
 * @param val the new value to set
 */
public void setMovemode( int val)
{
    this.movemode = val;
}
/**
 * Sets the {@link IoPcData}'s name.
 * @param val the name to set
 */
public  void setName( char[] val)
{
    name = val;
    notifyWatchers();
}
/**
 * Sets the {@link IoPcData}'s name.
 * @param val the name to set
 */
public  void setName( String val)
{
    name = val;
    notifyWatchers();
}
/**
 * Sets the value of the reachedtarget.
 * @param val the new value to set
 */
public void setReachedtarget( bool val)
{
    this.reachedtarget = val;
}
/**
 * Sets the splatDamages
 * @param splatDamages the splatDamages to set
 */
public void setSplatDamages( int val)
{
    this.splatDamages = val;
}
/**
 * Sets the splatTotNb
 * @param splatTotNb the splatTotNb to set
 */
public void setSplatTotNb( int val)
{
    this.splatTotNb = val;
}
/**
 * Sets the value of the tactics.
 * @param val the new value to set
 */
public void setTactics( int val)
{
    this.tactics = val;
}
/**
 * Sets the {@link IoPcData}'s title.
 * @param val the title to set
 */
public  void setTitle( char[] val)
{
    title = val;
    notifyWatchers();
}
/**
 * Sets the {@link IoPcData}'s title.
 * @param val the title to set
 */
public  void setTitle( String val)
{
    title = val;
    notifyWatchers();
}
/**
 * Sets the IONpcData's weapon.
 * @param wpnIO the weapon to set
 */
public  void setWeapon( BaseInteractiveObject wpnIO)
{
    weapon = wpnIO;
    if (weapon != null)
    {
        weaponInHand = weapon.GetRefId();
    }
    else
    {
        weaponInHand = -1;
    }
}
/**
 * Sets the value of the weaponInHand.
 * @param ioid the new value to set
 * @ if an error occurs
 */
public  void setWeaponInHand( int ioid) 
{
        this.weaponInHand = ioid;
        if (Interactive.GetInstance().hasIO(weaponInHand)) {
        weapon = (BaseInteractiveObject)Interactive.GetInstance().getIO(weaponInHand);
    } else {
        weapon = null;
    }
}
/**
 * Sets the value of the xpvalue.
 * @param val the new value to set
 */
public void setXpvalue( int val)
{
    this.xpvalue = val;
}
/** Restores the IONpcData to their maximum life. */
protected abstract void stopActiveAnimation();
/** Restores the IONpcData to their maximum life. */
protected abstract void stopIdleAnimation();
/**
 * Determines if a summoned BaseInteractiveObject's summoner is a IOPcData.
 * @param io the BaseInteractiveObject
 * @return <tt>true</tt> if the summoner is a player; <tt>false</tt>
 *         otherwise
 * @ if an error occurs
 */
private bool summonerIsPlayer(BaseInteractiveObject io) 
{
    bool isPlayer = false;
        if (io != null) {
        int summonerId = io.getSummoner();
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
    }
}
