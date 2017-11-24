using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public abstract class IOPcData
    {
        /** the number of bags the player has. */
        private int bags;
        /** the {@link IoPcData}'s gender. */
        private int gender = -1;
        /** the character's gold. */
        private float gold;
        /** interface flags. */
        private int interfaceFlags;
        /** the IO associated with this {@link IoPcData}. */
        private IO io;
        /** the player's key ring. */
        private char[][] keyring;
        /** the {@link IoPcData}'s level. */
        private int level = 0;
        /** the {@link IoPcData}'s name. */
        private char[] name;
        /** the number of keys on the key ring. */
        private int numKeys;
        /** the {@link IoPcData}'s Profession. */
        private int profession = -1;
        /** the {@link IoPcData}'s Race. */
        private int race = -1;
        /** the {@link IoPcData}'s experience points. */
        private int xp;
        /**
         * Creates a new instance of {@link IoPcData}.
         * @ if there is an error defining attributes
         */
        protected IoPcData() 
        {
            
		name = new char[0];
	}
    /**
	 * Adds an interface flag.
	 * @param flag the flag
	 */
    public  void addInterfaceFlag( long flag)
    {
        interfaceFlags |= flag;
    }
    /**
	 * Adds a key to the keyring.
	 * @param key the key
	 */
    public  void addKey( char[] key)
    {
        if (keyring == null)
        {
            keyring = new char[0][];
        }
        char[] keyCopy = new char[key.Length];
        System.arraycopy(key, 0, keyCopy, 0, key.Length);
        for (int i = keyCopy.Length - 1; i >= 0; i--)
        {
            keyCopy[i] = Character.toLowerCase(keyCopy[i]);
        }
        int index = -1;
        for (int i = keyring.Length - 1; i >= 0; i--)
        {
            if (keyring[i] == null)
            {
                index = i;
                break;
            }
            char[] keyRingCopy = new char[keyring[i].Length];
            System.arraycopy(keyring[i], 0, keyRingCopy, 0, keyring[i].Length);
            for (int j = keyRingCopy.Length - 1; j >= 0; j--)
            {
                keyRingCopy[j] = Character.toLowerCase(keyRingCopy[j]);
            }
            if (Arrays.Equals(keyRingCopy, keyCopy))
            {
                index = i;
                break;
            }
        }
        if (index == -1)
        {
            keyring = ArrayUtilities.GetInstance().extendArray(key, keyring);
            numKeys++;
        }
        keyCopy = null;
    }
    /**
	 * Adds a key to the keyring.
	 * @param key the key
	 */
    public  void addKey( String key)
    {
        addKey(key);
    }
    /**
	 * Adjusts the {@link IoPcData}'s gold.
	 * @param val the amount adjusted by
	 */
    public  void adjustGold( float val)
    {
        gold += val;
        if (gold < 0)
        {
            gold = 0;
        }
        notifyWatchers();
    }
    /**
	 * Adjusts the player's life by a specific amount.
	 * @param dmg the amount
	 */
    private  void adjustLife( float dmg)
    {
        String ls = getLifeAttribute();
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
        try
        {
            sb.append("M");
            sb.append(ls);
        }
        catch (PooledException e)
        {
            JOGLErrorHandler.GetInstance().fatalError(e);
        }
        String mls = sb.toString();
        sb.ReturnToPool();
        sb = null;
        setBaseAttributeScore(getLifeAttribute(), getBaseLife() + dmg);
        if (getBaseLife() > getFullAttributeScore(mls))
        {
            // if Hit Points now > max
            setBaseAttributeScore(ls, getFullAttributeScore(mls));
        }
        if (getBaseLife() < 0f)
        {
            // if life now < 0
            setBaseAttributeScore(ls, 0f);
        }
        ls = null;
        mls = null;
    }
    /**
	 * Adjusts the player's mana by a specific amount.
	 * @param dmg the amount
	 */
    protected abstract void adjustMana(float dmg);
    /**
	 * Adjusts the {@link IoPcData}'s experience points.
	 * @param val the amount adjusted by
	 */
    public  void adjustXp( int val)
    {
        xp += val;
        if (xp < 0)
        {
            xp = 0;
        }
        notifyWatchers();
    }
    /**
	 * Damages the player.
	 * @param dmg the damage amount
	 * @param type the type of damage
	 * @param source the source of the damage
	 * @return {@link float}
	 * @ if an error occurs
	 */
    public  float ARX_DAMAGES_DamagePlayer( float dmg,
             long type,
             int source) 
    {
		float damagesdone = 0.f;
        computeFullStats();
		if (!io.HasIOFlag(IoGlobals.PLAYERFLAGS_INVULNERABILITY)
		        && getBaseLife() > 0) {
            if (dmg > getBaseLife())
            {
                damagesdone = getBaseLife();
            }
            else
            {
                damagesdone = dmg;
            }
            io.setDamageSum(io.getDamageSum() + dmg);

            // TODO - add timer for ouch
            // if (ARXTime > inter.iobj[0]->ouch_time + 500) {
            IO oes = (IO)Script.GetInstance().getEventSender();

            if (Interactive.GetInstance().hasIO(source))
            {
                Script.GetInstance()
                        .setEventSender(Interactive
                                .GetInstance().getIO(source));
            }
            else
            {
                Script.GetInstance().setEventSender(null);
            }
            Script.GetInstance().sendIOScriptEvent(io,
                    ScriptConsts.SM_045_OUCH,
                    new Object[] { "OUCH", io.getDamageSum(),
                            "SUMMONED_OUCH", 0f },
                    null);
            Script.GetInstance().setEventSender(oes);
            io.setDamageSum(0);
            // }

            if (dmg > 0.f)
            {
                if (Interactive.GetInstance().hasIO(source))
                {
                    IO poisonWeaponIO = null;
                    IO sourceIO =
                            (IO)Interactive.GetInstance()
                                    .getIO(source);

                    if (sourceIO.HasIOFlag(IoGlobals.IO_03_NPC))
                    {
                        poisonWeaponIO = (IO)sourceIO.getNPCData().getWeapon();
                        if (poisonWeaponIO != null
                                && (poisonWeaponIO.getPoisonLevel() == 0
                                        || poisonWeaponIO
                                                .getPoisonCharges() == 0))
                        {
                            poisonWeaponIO = null;
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
                        // TODO - handle poisoning

                        if (poisonWeaponIO.getPoisonCharges() > 0)
                        {
                            poisonWeaponIO.setPoisonCharges(
                                    poisonWeaponIO.getPoisonCharges() - 1);
                        }
                    }
                }

                bool alive;
                if (getBaseLife() > 0)
                {
                    alive = true;
                }
                else
                {
                    alive = false;
                }
                adjustLife(-dmg);

                if (getBaseLife() <= 0.f)
                {
                    adjustLife(-getBaseLife());
                    if (alive)
                    {
                        // TODO - what is this?
                        // REFUSE_GAME_RETURN = true;
                        becomesDead();

                        // TODO - play fire sounds
                        // if (type & DAMAGE_TYPE_FIRE
                        // || type & DAMAGE_TYPE_FAKEFIRE) {
                        // ARX_SOUND_PlayInterface(SND_PLAYER_DEATH_BY_FIRE);
                        // }

                        Script.GetInstance().sendIOScriptEvent(io,
                                ScriptConsts.SM_017_DIE, null, null);

                        int i = Interactive.GetInstance().getMaxIORefId();
                        for (; i >= 0; i--)
                        {
                            if (!Interactive.GetInstance().hasIO(i))
                            {
                                continue;
                            }
                            IO ioo = (IO)Interactive.GetInstance().getIO(i);
                            // tell all IOs not to target player anymore
                            if (ioo != null
                                    && ioo.HasIOFlag(IoGlobals.IO_03_NPC))
                            {
                                if (ioo.getTargetinfo() == io.GetRefId()
                                        || ioo.getTargetinfo() == IoGlobals.TARGET_PLAYER)
                                {
                                    Script.GetInstance()
                                            .setEventSender(io);
                                    String killer = "";
                                    if (source == io.GetRefId())
                                    {
                                        killer = "PLAYER";
                                    }
                                    else if (source <= -1)
                                    {
                                        killer = "NONE";
                                    }
                                    else if (Interactive.GetInstance()
                                          .hasIO(source))
                                    {
                                        IO sourceIO =
                                                (IO)Interactive
                                                        .GetInstance().getIO(
                                                                source);
                                        if (sourceIO.HasIOFlag(
                                                IoGlobals.IO_03_NPC))
                                        {
                                            killer = new String(
                                                    sourceIO.getNPCData()
                                                            .getName());
                                        }
                                    }
                                    Script.GetInstance().sendIOScriptEvent(ioo,
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
    /**
	 * Drains mana from the IONpcData, returning the full amount drained.
	 * @param dmg the attempted amount of mana to be drained
	 * @return {@link float}
	 */
    public  float ARX_DAMAGES_DrainMana( float dmg)
    {
        float manaDrained = 0;
        if (!io.HasIOFlag(IoGlobals.PLAYERFLAGS_NO_MANA_DRAIN))
        {
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
        }
        return manaDrained;
    }
    /**
	 * Heals the player's mana.
	 * @param dmg the amount of healing
	 */
    public  void ARX_DAMAGES_HealManaPlayer( float dmg)
    {
        if (getBaseLife() > 0.f)
        {
            if (dmg > 0.f)
            {
                adjustMana(dmg);
            }
        }
    }
    /**
	 * Heals the player.
	 * @param dmg the amount of healing
	 */
    public  void ARX_DAMAGES_HealPlayer( float dmg)
    {
        if (getBaseLife() > 0.f)
        {
            if (dmg > 0.f)
            {
                // if (!BLOCK_PLAYER_CONTROLS)
                adjustLife(dmg);
            }
        }
    }
    /**
	 * Gets the type of weapon the player is wielding.
	 * @return {@link long}
	 * @throws PooledException if an error occurs
	 * @ if an error occurs
	 */
    public  long ARX_EQUIPMENT_GetPlayerWeaponType() 
    {
		int type = EquipmentGlobals.WEAPON_BARE;
		int wpnId = getEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
		if (wpnId >= 0
		        && Interactive.GetInstance().hasIO(wpnId)) {
            IO weapon = (IO)Interactive.GetInstance().getIO(wpnId);
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
	 * Determines if the player has an item equipped.
	 * @param itemIO the item
	 * @return <tt>true</tt> if the player has the item equipped; <tt>false</tt>
	 *         otherwise
	 * @throws PooledException if an error occurs
	 * @ if an error occurs
	 */
    public  bool ARX_EQUIPMENT_IsPlayerEquip( IO itemIO)

            
    {
        bool isEquipped = false;
		int i = ProjectConstants.GetInstance().getMaxEquipped() - 1;
		for (; i >= 0; i--) {
            if (this.getEquippedItem(i) >= 0
                    && Interactive.GetInstance().hasIO(getEquippedItem(i)))
            {
                IO toequip = (IO)Interactive.GetInstance().getIO(
                        getEquippedItem(i));
                if (toequip.Equals(itemIO))
                {
                    isEquipped = true;
                    break;
                }
            }
        }
		return isEquipped;
    }
    /** Re-creates the player's appearance. */
    public abstract void ARX_EQUIPMENT_RecreatePlayerMesh();
    /**
	 * Unequips the player's weapon.
	 * @throws PooledException if an error occurs
	 * @ if an error occurs
	 */
    public  void ARX_EQUIPMENT_UnEquipPlayerWeapon()

            
    {
		int wpnId = getEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
		if (wpnId >= 0
		        && Interactive.GetInstance().hasIO(wpnId)) {
            IO weapon = (IO)Interactive.GetInstance().getIO(wpnId);
            weapon.getItemData().ARX_EQUIPMENT_UnEquip(io, false);
        }
        setEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON, -1);
    }
    /**
	 * Called when a player dies.
	 * @
	 */
    public  void becomesDead() 
    {
		int i = ProjectConstants.GetInstance().getMaxSpells() - 1;
		for (; i >= 0; i--) {
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
    public abstract bool canIdentifyEquipment(IOEquipItem equipitem);
    /** Clears all interface flags that were set. */
    public  void clearInterfaceFlags()
    {
        interfaceFlags = 0;
    }
    /**
	 * Gets the player's base life value from the correct attribute.
	 * @return {@link float}
	 */
    protected abstract float getBaseLife();
    /**
	 * Gets the player's base mana value from the correct attribute.
	 * @return {@link float}
	 */
    protected abstract float getBaseMana();
    /**
	 * Gets the {@link IoPcData}'s gender.
	 * @return int
	 */
    public  int getGender()
    {
        return gender;
    }
    /**
	 * Gets the character's gold.
	 * @return <code>float</code>
	 */
    public  float getGold()
    {
        return gold;
    }
    /**
	 * Gets the IO associated with this {@link IoPcData}.
	 * @return {@link IO}
	 */
    public  IO getIo()
    {
        return io;
    }
    /**
	 * Gets a specific key from the keyring.
	 * @param index the key's index
	 * @return {@link String}
	 */
    public  char[] getKey( int index)
    {
        char[] key = null;
        if (keyring != null
                && index >= 0
                && index < keyring.Length)
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
    private int getKeyIndex( char[] key)
    {
        int index = -1;
        if (keyring == null)
        {
            keyring = new char[0][];
        }
        char[] keyCopy = new char[key.Length];
        System.arraycopy(key, 0, keyCopy, 0, key.Length);
        for (int i = keyCopy.Length - 1; i >= 0; i--)
        {
            keyCopy[i] = Character.toLowerCase(keyCopy[i]);
        }
        for (int i = keyring.Length - 1; i >= 0; i--)
        {
            char[] arrCopy = new char[keyring[i].Length];
            System.arraycopy(keyring[i], 0, arrCopy, 0, keyring[i].Length);
            for (int j = arrCopy.Length - 1; j >= 0; j--)
            {
                arrCopy[j] = Character.toLowerCase(arrCopy[j]);
            }
            if (Arrays.Equals(keyCopy, arrCopy))
            {
                index = i;
                break;
            }
            arrCopy = null;
        }
        keyCopy = null;
        return index;
    }
    /**
	 * Gets the {@link IoPcData}'s level.
	 * @return int
	 */
    public  int getLevel()
    {
        return level;
    }
    protected abstract String getLifeAttribute();
    /**
	 * Gets the {@link IoPcData}'s name.
	 * @return char[]
	 */
    public  char[] getName()
    {
        return name;
    }
    /**
	 * Gets the value for the bags.
	 * @return {@link int}
	 */
    public int getNumberOfBags()
    {
        return bags;
    }
    /**
	 * Gets the number of keys on the key ring.
	 * @return {@link int}
	 */
    public  int getNumKeys()
    {
        return numKeys;
    }
    /**
	 * Gets the {@link IoPcData}'s Profession.
	 * @return int
	 */
    public  int getProfession()
    {
        return profession;
    }
    /**
	 * Gets the {@link IoPcData}'s Race.
	 * @return int
	 */
    public  int getRace()
    {
        return race;
    }
    /**
	 * Gets the {@link IoPcData}'s experience points.
	 * @return int
	 */
    public  long getXp()
    {
        return xp;
    }
    /**
	 * Determines if the {@link IoPcData} has a specific flag.
	 * @param flag the flag
	 * @return true if the {@link IoPcData} has the flag; false otherwise
	 */
    public  bool hasInterfaceFlag( long flag)
    {
        return (interfaceFlags & flag) == flag;
    }
    /**
	 * Determines if the IOPcData has a key in their keyring.
	 * @param key the key's name
	 * @return <tt>true</tt> if the IOPcData has the key <tt>false></tt> otherwise
	 */
    public  bool hasKey( char[] key)
    {
        bool hasKey = false;
        if (keyring == null)
        {
            keyring = new char[0][];
        }
        char[] keyCopy = new char[key.Length];
        System.arraycopy(key, 0, keyCopy, 0, key.Length);
        for (int i = keyCopy.Length - 1; i >= 0; i--)
        {
            keyCopy[i] = Character.toLowerCase(keyCopy[i]);
        }
        for (int i = keyring.Length - 1; i >= 0; i--)
        {
            char[] arrCopy = new char[keyring[i].Length];
            System.arraycopy(keyring[i], 0, arrCopy, 0, keyring[i].Length);
            for (int j = arrCopy.Length - 1; j >= 0; j--)
            {
                arrCopy[j] = Character.toLowerCase(arrCopy[j]);
            }
            if (Arrays.Equals(keyCopy, arrCopy))
            {
                hasKey = true;
                break;
            }
        }
        keyCopy = null;
        return hasKey;
    }
    /**
	 * Determines if the IOPcData has a key in their keyring.
	 * @param key the key's name
	 * @return <tt>true</tt> if the IOPcData has the key <tt>false></tt> otherwise
	 */
    public  bool hasKey( String key)
    {
        return hasKey(key);
    }
    /**
	 * Removes an interface flag.
	 * @param flag the flag
	 */
    public  void removeInterfaceFlag( long flag)
    {
        interfaceFlags &= ~flag;
    }
    /**
	 * Removes a key.
	 * @param key the key's id
	 */
    public  void removeKey( char[] key)
    {
        int index = getKeyIndex(key);
        if (index >= 0)
        {
            keyring = ArrayUtilities.GetInstance().removeIndex(index, keyring);
            numKeys--;
        }
    }
    /**
	 * Removes a key.
	 * @param key the key's id
	 */
    public  void removeKey( String key)
    {
        removeKey(key);
    }
    /**
	 * Sets the {@link IoPcData}'s gender.
	 * @param val the gender to set
	 */
    public  void setGender( int val)
    {
        gender = val;
        notifyWatchers();
    }
    /**
	 * Sets the IO associated with the pc data.
	 * @param newIO the IO to set
	 */
    public  void setIo( IO newIO)
    {
        io = newIO;
        if (newIO != null
                && newIO.getPCData() == null)
        {
            newIO.setPCData(this);
        }
    }
    /**
	 * Sets the {@link IoPcData}'s level.
	 * @param val the level to set
	 */
    public  void setLevel( int val)
    {
        level = val;
        notifyWatchers();
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
	 * Sets the {@link IoPcData}'s Profession.
	 * @param val the profession to set
	 */
    public  void setProfession( int val)
    {
        profession = val;
        notifyWatchers();
    }
    /**
	 * Sets the {@link IoPcData}'s Race.
	 * @param val the race to set
	 */
    public  void setRace( int val)
    {
        race = val;
        notifyWatchers();
    }
}
}
