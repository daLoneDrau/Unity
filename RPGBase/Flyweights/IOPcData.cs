using RPGBase.Constants;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public abstract class IOPcData : IOCharacter
    {
        /// <summary>
        /// the number of bags the player has.
        /// </summary>
        private int bags;
        private int gender = -1;
        /// <summary>
        /// the <see cref="IoPcData"/>'s gender.
        /// </summary>
        private int Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                NotifyWatchers();
            }
        }
        /// <summary>
        /// the character's gold.
        /// </summary>
        private float gold;
        public float Gold { get { return gold; } }
        /// <summary>
        /// interface flags.
        /// </summary>
        private long interfaceFlags;
        /// <summary>
        /// the <see cref="BaseInteractiveObject"/> associated with this <see cref="IoPcData"/>.
        /// </summary>
        private BaseInteractiveObject io;
        public BaseInteractiveObject Io
        {
            get { return io; }
            set
            {
                io = value;
                if (value != null
                        && value.PcData == null)
                {
                    value.PcData = this;
                }
            }
        }
        /// <summary>
        /// the player's key ring.
        /// </summary>
        private List<string> keyring = new List<string>();
        private int level = 0;
        /// <summary>
        /// the <see cref="IoPcData"/>'s level.
        /// </summary>
        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                NotifyWatchers();
            }
        }
        private string name;
        /// <summary>
        /// the <see cref="IoPcData"/>'s name.
        /// </summary>
        private string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyWatchers();
            }
        }
        /** the number of keys on the key ring. */
        private int numKeys;
        private int profession = -1;
        /// <summary>
        /// the <see cref="IoPcData"/>'s profession.
        /// </summary>
        private int Profession
        {
            get { return profession; }
            set
            {
                profession = value;
                NotifyWatchers();
            }
        }
        private int race = -1;
        /// <summary>
        /// the <see cref="IoPcData"/>'s race.
        /// </summary>
        private int Race
        {
            get { return race; }
            set
            {
                race = value;
                NotifyWatchers();
            }
        }
        /// <summary>
        /// the <see cref="IoPcData"/>'s experience points.
        /// </summary>
        private int xp;
        public int Xp { get { return xp; } }
        /// <summary>
        /// Creates a new instance of <see cref="IoPcData"/>.
        /// </summary>
        protected IOPcData()
        {
            Name = "";
        }
        /// <summary>
        /// Adds an interface flag.
        /// </summary>
        /// <param name="flag">the flag being added</param>
        public void AddInterfaceFlag(long flag)
        {
            interfaceFlags |= flag;
        }
        /// <summary>
        /// Adds a key to the keyring.
        /// </summary>
        /// <param name="key">the key</param>
        public void AddKey(string key)
        {
            if (key != null
                && !keyring.Contains(key))
            {
                string keyCopy = new string(key.ToCharArray());
                keyring.Add(key);
                keyCopy = null;
            }
        }
        /// <summary>
        /// Adjusts the <see cref="IoPcData"/>'s gold.
        /// </summary>
        /// <param name="val">the amount adjusted by</param>
        public void AdjustGold(float val)
        {
            gold += val;
            if (gold < 0)
            {
                gold = 0;
            }
            NotifyWatchers();
        }
        /// <summary>
        /// Adjusts the player's life by a specific amount.
        /// </summary>
        /// <param name="dmg">the amount</param>
        private void AdjustLife(float dmg)
        {
            String ls = GetLifeAttribute();
            PooledStringBuilder sb = StringBuilderPool.GetInstance().GetStringBuilder();
            sb.Append("M");
            sb.Append(ls);
            String mls = sb.ToString();
            sb.ReturnToPool();
            sb = null;
            SetBaseAttributeScore(GetLifeAttribute(), GetBaseLife() + dmg);
            if (GetBaseLife() > GetFullAttributeScore(mls))
            {
                // if Hit Points now > max
                SetBaseAttributeScore(ls, GetFullAttributeScore(mls));
            }
            if (GetBaseLife() < 0f)
            {
                // if life now < 0
                SetBaseAttributeScore(ls, 0f);
            }
            ls = null;
            mls = null;
        }
        /// <summary>
        /// Adjusts the player's mana by a specific amount.
        /// </summary>
        /// <param name="dmg">the amount</param>
        protected abstract void AdjustMana(float dmg);
        /// <summary>
        /// Adjusts the player's experience points by a specific amount.
        /// </summary>
        /// <param name="dmg">the amount</param>
        public void AdjustXp(int val)
        {
            xp += val;
            if (xp < 0)
            {
                xp = 0;
            }
            NotifyWatchers();
        }
        /// <summary>
        /// Called when a player dies.
        /// </summary>
        public void BecomesDead()
        {
            int i = ProjectConstants.GetInstance().GetMaxSpells() - 1;
            for (; i >= 0; i--)
            {
                Spell spell = SpellController.GetInstance().getSpell(i);
                if (spell.exists()
                        && spell.getCaster() == io.GetRefId())
                {
                    spell.setTimeToLive(0);
                    spell.setTurnsToLive(0);
                }
            }
        }
        /**
         * Determines if a IOPcData can identify a piece of equipment.
         * @param equipitem
         * @return
         */
        public abstract bool CanIdentifyEquipment(IOEquipItem equipitem);
        /** Clears all interface flags that were set. */
        public void ClearInterfaceFlags()
        {
            interfaceFlags = 0;
        }
        /// <summary>
        /// Damages the player.
        /// </summary>
        /// <param name="dmg">the damage amount</param>
        /// <param name="type">the type of damage</param>
        /// <param name="source">the source of the damage</param>
        /// <returns>the total damage done</returns>
        public float DamagePlayer(float dmg, long type, int source)
        {
            float damagesdone = 0f;
            ComputeFullStats();
            if (!io.HasIOFlag(IoGlobals.PLAYERFLAGS_INVULNERABILITY)
                    && GetBaseLife() > 0)
            {
                if (dmg > GetBaseLife())
                {
                    damagesdone = GetBaseLife();
                }
                else
                {
                    damagesdone = dmg;
                }
                io.DamageSum += dmg;

                // TODO - add timer for ouch
                // if (ARXTime > inter.iobj[0]->ouch_time + 500) {
                BaseInteractiveObject oes = (BaseInteractiveObject)Script.GetInstance().GetEventSender();

                if (Interactive.GetInstance().hasIO(source))
                {
                    Script.GetInstance().SetEventSender(Interactive.GetInstance().getIO(source));
                }
                else
                {
                    Script.GetInstance().SetEventSender(null);
                }
                Script.GetInstance().SendIOScriptEvent(io,
                        ScriptConsts.SM_045_OUCH,
                        new Object[] { "OUCH", io.DamageSum, "SUMMONED_OUCH", 0f },
                        null);
                Script.GetInstance().SetEventSender(oes);
                io.DamageSum = 0;
                // }

                if (dmg > 0f)
                {
                    if (Interactive.GetInstance().hasIO(source))
                    {
                        BaseInteractiveObject poisonWeaponIO = null;
                        BaseInteractiveObject sourceIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(source);

                        if (sourceIO.HasIOFlag(IoGlobals.IO_03_NPC))
                        {
                            poisonWeaponIO = (BaseInteractiveObject)sourceIO.NpcData.getWeapon();
                            if (poisonWeaponIO != null
                                    && (poisonWeaponIO.PoisonLevel == 0
                                            || poisonWeaponIO.PoisonCharges == 0))
                            {
                                poisonWeaponIO = null;
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
                            // TODO - handle poisoning

                            if (poisonWeaponIO.PoisonCharges > 0)
                            {
                                poisonWeaponIO.PoisonCharges--;
                            }
                        }
                    }

                    bool alive;
                    if (GetBaseLife() > 0)
                    {
                        alive = true;
                    }
                    else
                    {
                        alive = false;
                    }
                    AdjustLife(-dmg);

                    if (GetBaseLife() <= 0f)
                    {
                        AdjustLife(-GetBaseLife());
                        if (alive)
                        {
                            // TODO - what is this?
                            // REFUSE_GAME_RETURN = true;
                            BecomesDead();

                            // TODO - play fire sounds
                            // if (type & DAMAGE_TYPE_FIRE
                            // || type & DAMAGE_TYPE_FAKEFIRE) {
                            // ARX_SOUND_PlayInterface(SND_PLAYER_DEATH_BY_FIRE);
                            // }

                            Script.GetInstance().SendIOScriptEvent(io, ScriptConsts.SM_017_DIE, null, null);

                            int i = Interactive.GetInstance().getMaxIORefId();
                            for (; i >= 0; i--)
                            {
                                if (!Interactive.GetInstance().hasIO(i))
                                {
                                    continue;
                                }
                                BaseInteractiveObject ioo = (BaseInteractiveObject)Interactive.GetInstance().getIO(i);
                                // tell all IOs not to target player anymore
                                if (ioo != null
                                        && ioo.HasIOFlag(IoGlobals.IO_03_NPC))
                                {
                                    if (ioo.Targetinfo == io.GetRefId()
                                            || ioo.Targetinfo == IoGlobals.TARGET_PLAYER)
                                    {
                                        Script.GetInstance().SetEventSender(io);
                                        String killer = "";
                                        if (source == io.GetRefId())
                                        {
                                            killer = "PLAYER";
                                        }
                                        else if (source <= -1)
                                        {
                                            killer = "NONE";
                                        }
                                        else if (Interactive.GetInstance().hasIO(source))
                                        {
                                            BaseInteractiveObject sourceIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(source);
                                            if (sourceIO.HasIOFlag(IoGlobals.IO_03_NPC))
                                            {
                                                killer = new String(
                                                        sourceIO.NpcData.getName());
                                            }
                                        }
                                        Script.GetInstance().SendIOScriptEvent(ioo,
                                                0,
                                                new Object[] { "tmp_int1", source },
                                                "TargetDeath");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return damagesdone;
        }
        /// <summary>
        /// Drains mana from the IONpcData, returning the full amount drained.
        /// </summary>
        /// <param name="dmg">the attempted amount of mana to be drained</param>
        /// <returns><see cref="float"/></returns>
        public float DrainMana(float dmg)
        {
            float manaDrained = 0;
            if (!io.HasIOFlag(IoGlobals.PLAYERFLAGS_NO_MANA_DRAIN))
            {
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
            }
            return manaDrained;
        }
        /**
         * Gets the player's base life value from the correct attribute.
         * @return {@link float}
         */
        protected abstract float GetBaseLife();
        /**
         * Gets the player's base mana value from the correct attribute.
         * @return {@link float}
         */
        protected abstract float GetBaseMana();
        /**
         * Gets the BaseInteractiveObject associated with this {@link IoPcData}.
         * @return {@link BaseInteractiveObject}
         */
        protected override BaseInteractiveObject GetIo()
        {
            return io;
        }
        /**
         * Gets a specific key from the keyring.
         * @param index the key's index
         * @return {@link String}
         */
        public string GetKey(int index)
        {
            string key = null;
            if (keyring != null
                    && index >= 0
                    && index < keyring.Count)
            {
                key = keyring[index];
            }
            return key;
        }
        /**
         * Gets the index of a specific key.
         * @param key the key's id.
         * @return {@link int}
         */
        private int GetKeyIndex(string key)
        {
            if (keyring == null)
            {
                keyring = new List<string>();
            }
            return keyring.IndexOf(key);
        }
        protected abstract String GetLifeAttribute();
        /**
         * Gets the value for the bags.
         * @return {@link int}
         */
        public int GetNumberOfBags()
        {
            return bags;
        }
        /// <summary>
        /// Gets the type of weapon the player is wielding.
        /// </summary>
        /// <returns>the player's weapon type</returns>
        public int GetPlayerWeaponType()
        {
            int type = EquipmentGlobals.WEAPON_BARE;
            int wpnId = GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
            if (wpnId >= 0
                    && Interactive.GetInstance().hasIO(wpnId))
            {
                BaseInteractiveObject weapon = (BaseInteractiveObject)Interactive.GetInstance().getIO(wpnId);
                if (weapon.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_DAGGER))
                {
                    type = EquipmentGlobals.WEAPON_DAGGER;
                }
                if (weapon.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_1H))
                {
                    type = EquipmentGlobals.WEAPON_1H;
                }
                if (weapon.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_2H))
                {
                    type = EquipmentGlobals.WEAPON_2H;
                }
                if (weapon.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW))
                {
                    type = EquipmentGlobals.WEAPON_BOW;
                }
                weapon = null;
            }
            return type;
        }
        /**
         * Determines if the {@link IoPcData} has a specific flag.
         * @param flag the flag
         * @return true if the {@link IoPcData} has the flag; false otherwise
         */
        public bool HasInterfaceFlag(long flag)
        {
            return (interfaceFlags & flag) == flag;
        }
        /**
         * Determines if the IOPcData has a key in their keyring.
         * @param key the key's name
         * @return <tt>true</tt> if the IOPcData has the key <tt>false></tt> otherwise
         */
        public bool HasKey(string key)
        {
            if (keyring == null)
            {
                keyring = new List<string>();
            }
            return keyring.Contains(key);
        }
        /// <summary>
        /// Heals the player's mana.
        /// </summary>
        /// <param name="dmg">the amount of healing</param>
        public void HealManaPlayer(float dmg)
        {
            if (GetBaseLife() > 0f)
            {
                if (dmg > 0f)
                {
                    AdjustMana(dmg);
                }
            }
        }
        /// <summary>
        /// Heals the player.
        /// </summary>
        /// <param name="dmg">the amount of healing</param>
        public void HealPlayer(float dmg)
        {
            if (GetBaseLife() > 0f)
            {
                if (dmg > 0f)
                {
                    // if (!BLOCK_PLAYER_CONTROLS)
                    AdjustLife(dmg);
                }
            }
        }
        /// <summary>
        /// Determines if the player has an item equipped.
        /// </summary>
        /// <param name="itemIO">the item</param>
        /// <returns><tt>true</tt> if the player has the item equipped; <tt>false</tt> otherwise</returns>
        public bool IsPlayerEquip(BaseInteractiveObject itemIO)
        {
            bool isEquipped = false;
            int i = ProjectConstants.GetInstance().GetMaxEquipped() - 1;
            for (; i >= 0; i--)
            {
                if (this.GetEquippedItem(i) >= 0
                        && Interactive.GetInstance().hasIO(GetEquippedItem(i)))
                {
                    BaseInteractiveObject toequip = (BaseInteractiveObject)Interactive.GetInstance().getIO(GetEquippedItem(i));
                    if (toequip.Equals(itemIO))
                    {
                        isEquipped = true;
                        break;
                    }
                }
            }
            return isEquipped;
        }
        /**
         * Removes an interface flag.
         * @param flag the flag
         */
        public void RemoveInterfaceFlag(long flag)
        {
            interfaceFlags &= ~flag;
        }
        /**
         * Removes a key.
         * @param key the key's id
         */
        public void RemoveKey(string key)
        {
            if (keyring == null)
            {
                keyring = new List<string>();
            }
            keyring.Remove(key);
        }
        /// <summary>
        /// Unequips the player's weapon.
        /// </summary>
        public void UnEquipPlayerWeapon()
        {
            int wpnId = GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
            if (wpnId >= 0
                    && Interactive.GetInstance().hasIO(wpnId))
            {
                BaseInteractiveObject weapon = (BaseInteractiveObject)Interactive.GetInstance().getIO(wpnId);
                weapon.ItemData.ARX_EQUIPMENT_UnEquip(io, false);
            }
            SetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON, -1);
        }
    }
}
