using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public abstract class BaseInteractiveObject
    {
        /** the names of animations associated with the interactive object. */
        private String[] animationNames;
        /** the animation ids associated with the interactive object. */
        private int[] animations;
        /** the <see cref="BaseInteractiveObject"/>'s armor material. */
        private String armormaterial;
        /** any flags that have been set. */
        private long behaviorFlags = 0;
        private float damageSum;
        /** flags indicating the IO is taking damage of a specific type. */
        private long dmgFlags = 0;
        /** any game flags that have been set. */
        private long gameFlags = 0;
        /** the object's init position. */
        private Vector2 initPosition;
        /** the inventory data. */
        private InventoryData inventory;
        /** any flags that have been set. */
        private long ioFlags = 0;
        /** the list of groups to which the IO belongs. */
        private String[] ioGroups = new String[0];
        /// <summary>
        /// the <see cref="BaseInteractiveObject"/>'s <see cref="IOItemData"/>.
        /// </summary>
        public IOItemData ItemData { get; set; }
        public int Level { get; set; }
        public String Mainevent { get; set; }
        public IONpcData NpcData { get; set; }
        /// <summary>
        /// the number of spells currently on the object.
        /// </summary>
        private int numberOfSpellsOn;
        /// <summary>
        /// overriding script associated with the object.
        /// </summary>
        public Scriptable Overscript { get; set; }
        public IOPcData PcData { get; set; }
        public int PoisonCharges { get; set; }
        public int PoisonLevel { get; set; }
        /// <summary>
        /// the object's position.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// the object's reference id.
        /// </summary>
        private int refId;
        /// <summary>
        /// primary script associated with the object.
        /// </summary>
        public Scriptable Script { get; set; }
        /// <summary>
        /// flag indicating whether the item is loaded by script.
        /// </summary>
        private bool ScriptLoaded { get; set; }
        /// <summary>
        /// the show status (in scene, in inventory).
        /// </summary>
        public int Show { get; set; }
        public int SparkNBlood { get; set; }
        private int sparkNBlood;
        private IOSpellCastData spellcastData;
        /// <summary>
        /// the list of spells currently active on the object.
        /// </summary>
        private int[] spellsOn;
        public int StatCount { get; set; }
        public int StatSent { get; set; }
        public int Summoner { get; set; }
        public Vector3 Target { get; set; }
        public int Targetinfo { get; set; }
        /// <summary>
        /// any type flags that have been set (is the object a goblin, weapon, etc...).
        /// </summary>
        private long typeFlags = 0;
        /// <summary>
        /// the <see cref="BaseInteractiveObject"/>'s weapon material.
        /// </summary>
        public String Weaponmaterial { get; set; }
        /**
         * Creates a new instance of {@link BaseInteractiveObject}.
         * @param id the reference id
         */
        protected BaseInteractiveObject(int id)
        {
            refId = id;
            animations = new int[0];
            animationNames = new String[0];
            spellcastData = new IOSpellCastData();
            Target = new Vector3();
        }
        /**
         * Adds an animation by a given name to the interactive object.
         * @param name the animation's name
         * @param id the animation's reference id
         * @throws PooledException if there is an error with the stringbuilder
         * @throws RPGException if the name is null
         */
        public void addAnimation(String name, int id)
            throws PooledException, RPGException {
        if (name == null) {
            PooledStringBuilder sb =
                    StringBuilderPool.getInstance().getStringBuilder();
        sb.append("ERROR! InteractiveObject.addAnimation() - ");
            sb.append("null value sent in parameters");
            RPGException ex = new RPGException(ErrorMessage.ILLEGAL_OPERATION,
                    sb.toString());
        sb.returnToPool();
            throw ex;
        }
    int index = -1;
        for (int i = 0; i<animationNames.length; i++) {
            if (animationNames[i] == null || animationNames[i] != null
                    && animationNames[i].equals(name)) {
                index = i;
                break;
            }
        }
        if (index == -1) {
            // extend the names array
            index = animations.length;
            String[] tempStr = new String[animations.length + 1];
System.arraycopy(animationNames, 0, tempStr, 0,
                    animationNames.length);
            animationNames = tempStr;
            // extend the animations array
            int[] temp = new int[animations.length + 1];
System.arraycopy(animations, 0, temp, 0, animations.length);
            animations = temp;
        }
        animationNames[index] = name;
        animations[index] = id;
    }
    /**
     * Adds a behavior flag.
     * @param flag the flag
     */
    public void addBehaviorFlag(long flag)
{
    behaviorFlags |= flag;
}
/**
 * Adds a game flag.
 * @param flag the flag
 */
public void addGameFlag(long flag)
{
    gameFlags |= flag;
}

/**
 * Adds the IO to a group.
 * @param group the group
 */
public void addGroup(String group)
{
    bool found = false;
    for (int i = 0; i < ioGroups.length; i++)
    {
        if (group.equalsIgnoreCase(ioGroups[i]))
        {
            found = true;
            break;
        }
    }
    if (!found)
    {
        int index = -1;
        for (int i = 0; i < ioGroups.length; i++)
        {
            if (ioGroups[i] == null)
            {
                index = i;
                break;
            }
        }
        if (index == -1)
        {
            index = ioGroups.length;
            String[] dest = new String[ioGroups.length + 1];
            System.arraycopy(ioGroups, 0, dest, 0, ioGroups.length);
            ioGroups = dest;
            dest = null;
        }
        ioGroups[index] = group;
    }
}
/**
 * Adds a flag.
 * @param flag the flag
 */
public void addIOFlag(long flag)
{
    ioFlags |= flag;
}
/**
 * Adds an active spell on the object.
 * @param spellId the spell's id
 */
public void addSpellOn(int spellId)
{
    if (spellsOn == null)
    {
        spellsOn = new int[0];
    }
    int[] dest = new int[spellsOn.length + 1];
    System.arraycopy(spellsOn, 0, dest, 0, spellsOn.length);
    dest[spellsOn.length] = spellId;
    spellsOn = dest;
    dest = null;
    numberOfSpellsOn++;
}

/**
 * Adds a type flag.
 * @param flag the flag
 * @throws RPGException if an invalid type is set
 */
public void addTypeFlag(int flag) throws RPGException
{
        switch (flag) {
        case EquipmentGlobals.OBJECT_TYPE_DAGGER:
        case EquipmentGlobals.OBJECT_TYPE_1H:
        case EquipmentGlobals.OBJECT_TYPE_2H:
        case EquipmentGlobals.OBJECT_TYPE_BOW:
            clearTypeFlags();
        typeFlags |= EquipmentGlobals.OBJECT_TYPE_WEAPON;
        addIOFlag(IoGlobals.IO_02_ITEM);
        break;
        case EquipmentGlobals.OBJECT_TYPE_SHIELD:
        case EquipmentGlobals.OBJECT_TYPE_ARMOR:
        case EquipmentGlobals.OBJECT_TYPE_HELMET:
        case EquipmentGlobals.OBJECT_TYPE_LEGGINGS:
        case EquipmentGlobals.OBJECT_TYPE_RING:
            addIOFlag(IoGlobals.IO_02_ITEM);
        case EquipmentGlobals.OBJECT_TYPE_FOOD:
        case EquipmentGlobals.OBJECT_TYPE_GOLD:
            clearTypeFlags();
        break;
        case EquipmentGlobals.OBJECT_TYPE_WEAPON:
            throw new RPGException(ErrorMessage.INVALID_DATA_TYPE,
                    "Cannot set weapon type, must specify weapon class");
        default:
            throw new RPGException(ErrorMessage.INVALID_DATA_TYPE,
                    "Invalid object type " + flag);
    }
    typeFlags |= flag;
}
/** Clears all behavior flags that were set. */
public void clearBehaviorFlags()
{
    behaviorFlags = 0;
}
/** Clears all game flags that were set. */
public void clearGameFlags()
{
    gameFlags = 0;
}
/** Clears all flags that were set. */
public void clearIOFlags()
{
    ioFlags = 0;
}
/** Clears all type flags that were set. */
public void clearTypeFlags()
{
    typeFlags = 0;
}
/**
 * {@inheritDoc}
 */
@Override
    public bool equals(Object obj)
{
    bool equals = false;
    if (this == obj)
    {
        equals = true;
    }
    else if (obj != null && obj instanceof BaseInteractiveObject) {
        BaseInteractiveObject other = (BaseInteractiveObject)obj;
        if (this.dmgFlags == other.dmgFlags
                && this.gameFlags == other.gameFlags
                && this.ioFlags == other.ioFlags
                && this.numberOfSpellsOn == other.numberOfSpellsOn
                && this.refId == other.refId)
        {
            equals = true;
        }
    }
    return equals;
}

/**
 * Gets the reference id of the animation associated with a specific name.
 * @param name the name of the animation
 * @return int
 * @throws Exception if the name is null, or no animation by the given name
 *             was ever set on the interactive object
 */
public int getAnimation(String name) throws Exception
{
        if (name == null) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        sb.append("ERROR! InteractiveObject.getAnimation() - ");
        sb.append("null value sent in parameters");
        Exception ex = new Exception(sb.toString());
        sb.returnToPool();
        throw ex;
    }
        int index = -1;
        for (int i = 0; i < animationNames.length; i++) {
        if (animationNames[i] != null && animationNames[i].equals(name))
        {
            index = i;
            break;
        }
    }
        if (index == -1) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        sb.append("ERROR! InteractiveObject.getAnimation() - ");
        sb.append("no animation set for ");
        sb.append(name);
        Exception ex = new Exception(sb.toString());
        sb.returnToPool();
        throw ex;
    }
        return animations [index];
}

/**
 * Gets the {@link BaseInteractiveObject}'s armor material.
 * @return {@link String}
 */
public String getArmormaterial()
{
    return armormaterial;
}

public float getDamageSum()
{
    return damageSum;
}

/**
 * @return the initPosition
 */
public Vector2 getInitPosition()
{
    return initPosition;
}

/**
 * Gets the IO's inventory.
 * @return {@link InventoryData}
 */
public InventoryData getInventory()
{
    return inventory;
}

/**
 * Gets a group to which the IO belongs.
 * @param index the index
 * @return {@link String}
 */
public String getIOGroup(int index)
{
    return ioGroups[index];
}

/**
 * Gets item data for the {@link BaseInteractiveObject}.
 * @return {@link IOItemData}
 */
public IOItemData getItemData()
{
    return itemData;
}

/**
 * Gets the value for the level.
 * @return {@link int}
 */
public int getLevel()
{
    return level;
}

/**
 * Gets the mainevent
 * @return {@link String}
 */
public String getMainevent()
{
    return mainevent;
}
/**
 * Gets IONpcData data for the {@link BaseInteractiveObject}.
 * @return {@link IONpcData}
 */
public IONpcData getNPCData()
{
    return npcData;
}
/**
 * Gets the number of spells on the {@link BaseInteractiveObject}.
 * @return <code>int</code>
 */
public int getNumberOfSpellsOn()
{
    return numberOfSpellsOn;
}
/**
 * Gets the number of IO groups to which the IO belongs.
 * @return {@link int}
 */
public int getNumIOGroups()
{
    return this.ioGroups.length;
}

/**
 * Gets the overscript.
 * @return {@link Scriptable}
 */
public Scriptable getOverscript()
{
    return overscript;
}

/**
 * Gets item data for the {@link BaseInteractiveObject}.
 * @return {@link IOPcData}
 */
public IOPcData getPCData()
{
    return pcData;
}

public int getPoisonCharges()
{
    return poisonCharges;
}

/**
 * Gets the value for the poisonLevel.
 * @return {@link int}
 */
public int getPoisonLevel()
{
    return poisonLevel;
}

/**
 * Gets the position
 * @return {@link Vector2}
 */
public Vector2 getPosition()
{
    return position;
}

/**
 * Gets the {@link BaseInteractiveObject}'s reference id.
 * @return int
 */
public int getRefId()
{
    return refId;
}

/**
 * Gets the script
 * @return {@link Scriptable}
 */
public Scriptable getScript()
{
    return script;
}

/**
 * Gets the show status.
 * @return <code>int</code>
 */
public int getShow()
{
    return show;
}

public int getSparkNBlood()
{
    return sparkNBlood;
}

/**
 * Gets the spellcast_data
 * @return {@link IOSpellCastData}
 */
public IOSpellCastData getSpellcastData()
{
    return spellcastData;
}

public int getSpellOn(int index)
{
    return spellsOn[index];
}

/**
 * Gets the statCount
 * @return {@link int}
 */
public int getStatCount()
{
    return statCount;
}

/**
 * Gets the statSent
 * @return {@link int}
 */
public int getStatSent()
{
    return statSent;
}

public int getSummoner()
{
    return summoner;
}
/**
 * @return the target
 */
public Vector3 getTarget()
{
    return target;
}

/**
 * Gets the targetinfo
 * @return {@link int}
 */
public int getTargetinfo()
{
    return targetinfo;
}

/**
 * Gets the {@link BaseInteractiveObject}'s weapon material.
 * @return {@link String}
 */
public String getWeaponmaterial()
{
    return weaponmaterial;
}

/**
 * Determines if the {@link BaseInteractiveObject} has a specific behavior
 * flag.
 * @param flag the flag
 * @return true if the {@link BaseInteractiveObject} has the flag; false
 *         otherwise
 */
public bool hasBehaviorFlag(long flag)
{
    return (behaviorFlags & flag) == flag;
}

/**
 * Determines if the {@link BaseInteractiveObject} has a specific game flag.
 * @param flag the flag
 * @return true if the {@link BaseInteractiveObject} has the flag; false
 *         otherwise
 */
public bool hasGameFlag(long flag)
{
    return (gameFlags & flag) == flag;
}

/*
 * (non-Javadoc)
 * @see java.lang.Object#hashCode()
 */
@Override
    public int hashCode()
{
    // TODO Auto-generated method stub
    return super.hashCode();
}
/**
 * Determines if the {@link BaseInteractiveObject} has a specific flag.
 * @param flag the flag
 * @return true if the {@link BaseInteractiveObject} has the flag; false
 *         otherwise
 */
public bool hasIOFlag(long flag)
{
    return (ioFlags & flag) == flag;
}
/**
 * Determines if the {@link BaseInteractiveObject} has a specific type flag.
 * @param flag the flag
 * @return true if the {@link BaseInteractiveObject} has the flag; false
 *         otherwise
 */
public bool hasTypeFlag(long flag)
{
    return (typeFlags & flag) == flag;
}
public bool isInGroup(String group)
{
    bool found = false;
    for (int i = 0; i < ioGroups.length; i++)
    {
        if (group.equalsIgnoreCase(ioGroups[i]))
        {
            found = true;
            break;
        }
    }
    return found;
}
/**
 * Gets the flag indicating if the item is loaded by script.
 * @return <code>bool</code>
 */
public bool isScriptLoaded()
{
    return scriptLoaded;
}
/** Removes all active spells. */
public void removeAllSpells()
{
    spellsOn = new int[0];
}
/**
 * Removes a behavior flag.
 * @param flag the flag
 */
public void removeBehaviorFlag(long flag)
{
    behaviorFlags &= ~flag;
}

/**
 * Removes a game flag.
 * @param flag the flag
 */
public void removeGameFlag(long flag)
{
    gameFlags &= ~flag;
}

/**
 * Removes the IO from a group.
 * @param group the group
 */
public void removeGroup(String group)
{
    int index = -1;
    if (group != null)
    {
        for (int i = 0; i < ioGroups.length; i++)
        {
            if (group.equalsIgnoreCase(ioGroups[i]))
            {
                index = i;
                break;
            }
        }
    }
    if (index != -1)
    {
        String[] dest = new String[ioGroups.length - 1];
        if (index == 0)
        {
            System.arraycopy(ioGroups, 1, dest, 0, ioGroups.length - 1);
        }
        else if (index == ioGroups.length - 1)
        {
            System.arraycopy(ioGroups, 0, dest, 0, ioGroups.length - 1);
        }
        else
        {
            // copy up to index
            System.arraycopy(ioGroups, 0, dest, 0, index);
            // copy everything after index
            System.arraycopy(ioGroups, index + 1, dest, index,
                    ioGroups.length - 1 - index);
        }
        ioGroups = dest;
        dest = null;
    }
}
/**
 * Removes a flag.
 * @param flag the flag
 */
public void removeIOFlag(long flag)
{
    ioFlags &= ~flag;
}
/**
 * Removes an active spell.
 * @param spellId the spell's id
 */
public void removeSpellOn(int spellId)
{
    if (numberOfSpellsOn == 1)
    {
        spellsOn = new int[0];
    }
    int index = 0;
    for (; index < numberOfSpellsOn; index++)
    {
        if (spellsOn[index] == spellId)
        {
            break;
        }
    }
    if (index < numberOfSpellsOn)
    {
        numberOfSpellsOn--;
        int[] dest = new int[numberOfSpellsOn];
        if (index == 0)
        {
            // copy everything from after index
            System.arraycopy(spellsOn, 1, dest, 0, numberOfSpellsOn);
        }
        else if (index == numberOfSpellsOn)
        {
            // copy everything from before index
            System.arraycopy(spellsOn, 0, dest, 0, numberOfSpellsOn);
        }
        else
        {
            System.arraycopy(spellsOn, 0, dest, 0, index);
            System.arraycopy(spellsOn, index + 1, dest, index,
                    numberOfSpellsOn - index);
        }
        spellsOn = dest;
        dest = null;
    }
    else
    {
        // spell id was never found
        // nothing to remove
    }
}
/**
 * Removes a type flag.
 * @param flag the flag
 */
public void removeTypeFlag(long flag)
{
    typeFlags &= ~flag;
}
/**
 * Sets the {@link BaseInteractiveObject}'s armor material.
 * @param val the new value
 */
public void setArmormaterial(String val)
{
    armormaterial = val;
}

/**
 * Sets the value of the damageSum.
 * @param damageSum the new value to set
 */
public void setDamageSum(float damageSum)
{
    this.damageSum = damageSum;
}

/**
 * @param initPosition the initPosition to set
 */
public void setInitPosition(Vector2 initPosition)
{
    this.initPosition = initPosition;
}

/**
 * Sets the IO's inventory.
 * @param val the inventory to set
 */
public void setInventory(InventoryData val)
{
    this.inventory = val;
    inventory.setIo(this);
}

/**
 * Sets {@link IOItemData} data for the {@link BaseInteractiveObject}.
 * @param data the new {@link IOItemData}
 */
public void setItemData(IOItemData data)
{
    itemData = data;
    if (itemData != null)
    {
        if (itemData.getIo() == null)
        {
            itemData.setIo(this);
        }
        else if (itemData.getIo().refId != refId)
        {
            itemData.setIo(this);
        }
    }
}

/**
 * Sets the value of the level.
 * @param level the new value to set
 */
public void setLevel(int level)
{
    this.level = level;
}

/**
 * Sets the mainevent
 * @param mainevent the mainevent to set
 */
public void setMainevent(String mainevent)
{
    this.mainevent = mainevent;
}

/**
 * Sets IONpcData data for the {@link BaseInteractiveObject}.
 * @param data the new item data
 */
public void setNPCData(IONpcData data)
{
    npcData = data;
    if (npcData != null)
    {
        if (npcData.getIo() == null)
        {
            npcData.setIo(this);
        }
        else if (npcData.getIo().refId != refId)
        {
            npcData.setIo(this);
        }
    }
}

/**
 * Sets the overscript.
 * @param overscript the overscript to set
 */
public void setOverscript(Scriptable overscript)
{
    this.overscript = overscript;
}

/**
 * Sets item data for the {@link BaseInteractiveObject}.
 * @param data the new pc data
 */
public void setPCData(IOPcData data)
{
    pcData = data;
    if (pcData != null)
    {
        if (pcData.getIo() == null)
        {
            pcData.setIo(this);
        }
        else if (pcData.getIo().refId != refId)
        {
            pcData.setIo(this);
        }
    }
}

/**
 * Sets the value of the poisonCharges.
 * @param poisonCharges the new value to set
 */
public void setPoisonCharges(int poisonCharges)
{
    this.poisonCharges = poisonCharges;
}

/**
 * Sets the value of the poisonLevel.
 * @param poisonLevel the new value to set
 */
public void setPoisonLevel(int poisonLevel)
{
    this.poisonLevel = poisonLevel;
}
/**
 * Sets the position.
 * @param val the position to set
 */
public void setPosition(Vector2 val)
{
    this.position = val;
}
/**
 * Sets the position.
 * @param val the position to set
 * @throws RPGException 
 */
public void setPosition(SimplePoint val) throws RPGException
{
        this.position = new Vector2(val);
    }
    /**
     * Sets the script
     * @param script the script to set
     */
    public void setScript(Scriptable val)
{
    this.script = val;
    val.setIO(this);
}

/**
 * Sets the flag indicating if the item is loaded by script.
 * @param val the flag to set
 */
public void setScriptLoaded(bool val)
{
    this.scriptLoaded = val;
}

/**
 * Sets the show status.
 * @param val the show status to set
 */
public void setShow(int val)
{
    this.show = val;
}

public void setSparkNBlood(int val)
{
    sparkNBlood = val;
}
/**
 * Sets the statCount
 * @param val the statCount to set
 */
public void setStatCount(int val)
{
    this.statCount = val;
}
/**
 * Sets the statSent
 * @param val the statSent to set
 */
public void setStatSent(int val)
{
    this.statSent = val;
}
/**
 * Sets the value of the summoner.
 * @param summoner the new value to set
 */
public void setSummoner(int summoner)
{
    this.summoner = summoner;
}
/**
 * @param target the target to set
 */
public void setTarget(Vector3 target)
{
    this.target = target;
}
/**
 * Sets the targetinfo
 * @param targetinfo the targetinfo to set
 */
public void setTargetinfo(int targetinfo)
{
    this.targetinfo = targetinfo;
}
/**
 * Sets the {@link BaseInteractiveObject}'s weapon material.
 * @param val the new value
 */
public void setWeaponmaterial(String val)
{
    weaponmaterial = val;
}
    }
}
