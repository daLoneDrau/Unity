using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGBase.Constants;
using RPGBase.Pooled;

namespace RPGBase.Flyweights
{
    public abstract class BaseInteractiveObject
    {
        /** the <see cref="BaseInteractiveObject"/>'s armor material. */
        private String Armormaterial { get; set; }
        /** any flags that have been set. */
        private long behaviorFlags = 0;
        private float DamageSum { get; set; }
        /** flags indicating the IO is taking damage of a specific type. */
        private long dmgFlags = 0;
        /** any game flags that have been set. */
        private long gameFlags = 0;
        /** the object's init position. */
        private Vector2 InitPosition { get; set; }
        /** the inventory data. */
        private InventoryData inventory { get; set; }
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
        private readonly int refId;
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
            spellcastData = new IOSpellCastData();
            Target = new Vector3();
        }
        /**
         * Adds a behavior flag.
         * @param flag the flag
         */
        public void AddBehaviorFlag(long flag)
        {
            behaviorFlags |= flag;
        }
        /**
         * Adds a game flag.
         * @param flag the flag
         */
        public void AddGameFlag(long flag)
        {
            gameFlags |= flag;
        }
        /**
         * Adds the IO to a group.
         * @param group the group
         */
        public void AddGroup(String group)
        {
            bool found = false;
            for (int i = 0; i < ioGroups.Length; i++)
            {
                if (String.Equals(group, ioGroups[i], StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                int index = -1;
                for (int i = 0; i < ioGroups.Length; i++)
                {
                    if (ioGroups[i] == null)
                    {
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                {
                    // extend the arrays
                    index = ioGroups.Length;
                    Array.Resize(ref ioGroups, index + 1);
                }
                ioGroups[index] = group;
            }
        }
        /**
         * Adds a flag.
         * @param flag the flag
         */
        public void AddIOFlag(long flag)
        {
            ioFlags |= flag;
        }
        /**
         * Adds an active spell on the object.
         * @param spellId the spell's id
         */
        public void AddSpellOn(int spellId)
        {
            if (spellsOn == null)
            {
                spellsOn = new int[0];
            }
            Array.Resize(ref spellsOn, spellsOn.Length + 1);
            spellsOn[spellsOn.Length] = spellId;
            numberOfSpellsOn++;
        }
        /**
         * Adds a type flag.
         * @param flag the flag
         * @ if an invalid type is set
         */
        public void AddTypeFlag(int flag)
        {
            switch (flag)
            {
                case EquipmentGlobals.OBJECT_TYPE_DAGGER:
                case EquipmentGlobals.OBJECT_TYPE_1H:
                case EquipmentGlobals.OBJECT_TYPE_2H:
                case EquipmentGlobals.OBJECT_TYPE_BOW:
                    ClearTypeFlags();
                    typeFlags |= EquipmentGlobals.OBJECT_TYPE_WEAPON;
                    AddIOFlag(IoGlobals.IO_02_ITEM);
                    break;
                case EquipmentGlobals.OBJECT_TYPE_SHIELD:
                case EquipmentGlobals.OBJECT_TYPE_ARMOR:
                case EquipmentGlobals.OBJECT_TYPE_HELMET:
                case EquipmentGlobals.OBJECT_TYPE_LEGGINGS:
                case EquipmentGlobals.OBJECT_TYPE_RING:
                    ClearTypeFlags();
                    AddIOFlag(IoGlobals.IO_02_ITEM);
                    break;
                case EquipmentGlobals.OBJECT_TYPE_FOOD:
                case EquipmentGlobals.OBJECT_TYPE_GOLD:
                    ClearTypeFlags();
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
        public void ClearBehaviorFlags()
        {
            behaviorFlags = 0;
        }
        /** Clears all game flags that were set. */
        public void ClearGameFlags()
        {
            gameFlags = 0;
        }
        /** Clears all flags that were set. */
        public void ClearIOFlags()
        {
            ioFlags = 0;
        }
        /** Clears all type flags that were set. */
        public void ClearTypeFlags()
        {
            typeFlags = 0;
        }
        public bool Equals(System.Object obj)
        {
            bool equals = false;
            if (this == obj)
            {
                equals = true;
            }
            else if (obj != null && obj is BaseInteractiveObject)
            {
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
         * Gets a group to which the IO belongs.
         * @param index the index
         * @return {@link String}
         */
        public String GetIOGroup(int index)
        {
            return ioGroups[index];
        }
        /**
         * Gets the number of spells on the {@link BaseInteractiveObject}.
         * @return <code>int</code>
         */
        public int GetNumberOfSpellsOn()
        {
            return numberOfSpellsOn;
        }
        /**
         * Gets the number of IO groups to which the IO belongs.
         * @return {@link int}
         */
        public int GetNumIOGroups()
        {
            return this.ioGroups.Length;
        }
        /**
         * Gets the {@link BaseInteractiveObject}'s reference id.
         * @return int
         */
        public int GetRefId()
        {
            return refId;
        }
        /**
         * Gets the spellcast_data
         * @return {@link IOSpellCastData}
         */
        public IOSpellCastData GetSpellcastData()
        {
            return spellcastData;
        }
        public int GetSpellOn(int index)
        {
            return spellsOn[index];
        }
        /**
         * Determines if the {@link BaseInteractiveObject} has a specific behavior
         * flag.
         * @param flag the flag
         * @return true if the {@link BaseInteractiveObject} has the flag; false
         *         otherwise
         */
        public bool HasBehaviorFlag(long flag)
        {
            return (behaviorFlags & flag) == flag;
        }
        /**
         * Determines if the {@link BaseInteractiveObject} has a specific game flag.
         * @param flag the flag
         * @return true if the {@link BaseInteractiveObject} has the flag; false
         *         otherwise
         */
        public bool HasGameFlag(long flag)
        {
            return (gameFlags & flag) == flag;
        }
        /**
         * Determines if the {@link BaseInteractiveObject} has a specific flag.
         * @param flag the flag
         * @return true if the {@link BaseInteractiveObject} has the flag; false
         *         otherwise
         */
        public bool HasIOFlag(long flag)
        {
            return (ioFlags & flag) == flag;
        }
        /**
         * Determines if the {@link BaseInteractiveObject} has a specific type flag.
         * @param flag the flag
         * @return true if the {@link BaseInteractiveObject} has the flag; false
         *         otherwise
         */
        public bool HasTypeFlag(long flag)
        {
            return (typeFlags & flag) == flag;
        }
        public bool IsInGroup(String group)
        {
            bool found = false;
            for (int i = 0; i < ioGroups.Length; i++)
            {
                if (String.Equals(group, ioGroups[i], StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }
        /** Removes all active spells. */
        public void RemoveAllSpells()
        {
            spellsOn = new int[0];
        }
        /**
         * Removes a behavior flag.
         * @param flag the flag
         */
        public void RemoveBehaviorFlag(long flag)
        {
            behaviorFlags &= ~flag;
        }

        /**
         * Removes a game flag.
         * @param flag the flag
         */
        public void RemoveGameFlag(long flag)
        {
            gameFlags &= ~flag;
        }

        /**
         * Removes the IO from a group.
         * @param group the group
         */
        public void RemoveGroup(String group)
        {
            int index = -1;
            if (group != null)
            {
                for (int i = 0; i < ioGroups.Length; i++)
                {
                    if (String.Equals(group, ioGroups[i], StringComparison.OrdinalIgnoreCase))
                    {
                        index = i;
                        break;
                    }
                }
            }
            if (index != -1)
            {
                String[] dest = new String[ioGroups.Length - 1];
                if (index == 0)
                {
                    System.arraycopy(ioGroups, 1, dest, 0, ioGroups.Length - 1);
                }
                else if (index == ioGroups.Length - 1)
                {
                    System.arraycopy(ioGroups, 0, dest, 0, ioGroups.Length - 1);
                }
                else
                {
                    // copy up to index
                    System.arraycopy(ioGroups, 0, dest, 0, index);
                    // copy everything after index
                    System.arraycopy(ioGroups, index + 1, dest, index,
                            ioGroups.Length - 1 - index);
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
         * @ 
         */
        public void setPosition(SimplePoint val)
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
