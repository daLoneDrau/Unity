using System;
using System.Collections.Generic;
using RPGBase.Constants;
using UnityEngine;

namespace RPGBase.Flyweights
{
    public class BaseInteractiveObject : MonoBehaviour
    {
        /// <summary>
        /// the <see cref="BaseInteractiveObject"/>'s armor material.
        /// </summary>
        public String Armormaterial { get; set; }
        /// <summary>
        /// any flags that have been set.
        /// </summary>
        private long behaviorFlags;
        public float DamageSum { get; set; }
        /// <summary>
        /// flags indicating the BaseInteractiveObject is taking damage of a specific type.
        /// </summary>
        private long dmgFlags;
        /// <summary>
        /// any game flags that have been set.
        /// </summary>
        private long gameFlags;
        /** the object's init position. */
        //private Vector2 InitPosition { get; set; }
        /// <summary>
        /// the inventory data.
        /// </summary>
        private InventoryData inventory;
        /// <summary>
        /// the <see cref="InventoryData"/> property.
        /// </summary>
        public InventoryData Inventory
        {
            get { return inventory; }
            set
            {
                inventory = value;
                inventory.Io = this;
            }
        }
        /// <summary>
        /// any flags that have been set.
        /// </summary>
        private long ioFlags;
        /// <summary>
        /// the list of groups to which the BaseInteractiveObject belongs.
        /// </summary>
        private List<string> ioGroups;
        /// <summary>
        /// the <see cref="BaseInteractiveObject"/>'s <see cref="IOItemData"/>.
        /// </summary>
        private IOItemData itemData;
        /// <summary>
        /// the <see cref="IOItemData"/> property.
        /// </summary>
        public IOItemData ItemData
        {
            get { return itemData; }
            set
            {
                itemData = value;
                if (itemData != null)
                {
                    if (itemData.Io == null)
                    {
                        itemData.Io = this;
                    }
                    else if (itemData.Io.RefId != RefId)
                    {
                        itemData.Io = this;
                    }
                }
            }
        }
        public int Level { get; set; }
        public String Mainevent { get; set; }
        private IONpcData npcData;
        /// <summary>
        /// the <see cref="IONpcData"/> property.
        /// </summary>
        public IONpcData NpcData
        {
            get { return npcData; }
            set
            {
                npcData = value;
                if (npcData != null)
                {
                    if (npcData.Io == null)
                    {
                        npcData.Io = this;
                    }
                    else if (npcData.Io.RefId != RefId)
                    {
                        npcData.Io = this;
                    }
                }
            }
        }
        /// <summary>
        /// the number of spells currently on the object.
        /// </summary>
        private int numberOfSpellsOn;
        /// <summary>
        /// overriding script associated with the object.
        /// </summary>
        private Scriptable overscript;
        /// <summary>
        /// the <see cref="Scriptable"/> property.
        /// </summary>
        public Scriptable Overscript
        {
            get { return overscript; }
            set
            {
                overscript = value;
                overscript.Io = this;
            }
        }
        private IOPcData pcData;
        /// <summary>
        /// the <see cref="IOPcData"/> property.
        /// </summary>
        public IOPcData PcData
        {
            get { return pcData; }
            set
            {
                pcData = value;
                if (pcData != null)
                {
                    if (pcData.Io == null)
                    {
                        pcData.Io = this;
                    }
                    else if (pcData.Io.RefId != RefId)
                    {
                        pcData.Io = this;
                    }
                }
            }
        }
        public int PoisonCharges { get; set; }
        public int PoisonLevel { get; set; }
        /// <summary>
        /// the object's position.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// the object's reference id.
        /// </summary>
        public int RefId { get; set; }
        /// <summary>
        /// primary script associated with the object.
        /// </summary>
        private Scriptable script;
        /// the <see cref="Scriptable"/> property.
        /// </summary>
        public Scriptable Script
        {
            get { return script; }
            set
            {
                script = value;
                script.Io = this;
            }
        }
        /// <summary>
        /// flag indicating whether the item is loaded by script.
        /// </summary>
        public bool ScriptLoaded { get; set; }
        /// <summary>
        /// the show status (in scene, in inventory).
        /// </summary>
        public int Show { get; set; }
        public int SparkNBlood { get; set; }
        private IOSpellCastData spellcastData;
        /// <summary>
        /// the list of spells currently active on the object.
        /// </summary>
        private List<int> spellsOn = new List<int>();
        public Sprite Sprite { get; set; }
        public int StatCount { get; set; }
        public int StatSent { get; set; }
        public int Summoner { get; set; }
        //public Vector3 Target { get; set; }
        public int Targetinfo { get; set; }
        /// <summary>
        /// any type flags that have been set (is the object a goblin, weapon, etc...).
        /// </summary>
        private long typeFlags;
        /// <summary>
        /// the <see cref="BaseInteractiveObject"/>'s weapon material.
        /// </summary>
        public String Weaponmaterial { get; set; }
        /// <summary>
        /// Creates a new instance of <see cref="BaseInteractiveObject"/>.
        /// </summary>
        /// <param name="id">the reference id</param>
        protected BaseInteractiveObject(int id)
        {
            RefId = id;
            spellcastData = new IOSpellCastData();
            // TODO - handle Target
            //Target = new Vector3();
        }
        public virtual void Awake()
        {
            print("BaseInteractiveObject awake");
            ioGroups = new List<string>();
            spellcastData = new IOSpellCastData();
            // TODO - handle Target
            //Target = new Vector3();
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
         * Adds the BaseInteractiveObject to a group.
         * @param group the group
         */
        public void AddGroup(String group)
        {
            if (!ioGroups.Contains(group))
            {
                ioGroups.Add(group);
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
            spellsOn.Add(spellId);
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
        public override bool Equals(System.Object obj)
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
                        && this.RefId == other.RefId)
                {
                    equals = true;
                }
            }
            return equals;
        }
        /**
         * Gets a group to which the BaseInteractiveObject belongs.
         * @param index the index
         * @return {@link String}
         */
        public String GetIOGroup(int index)
        {
            return ioGroups[index];
        }
        /**
         * Gets the number of spells on the <see cref="BaseInteractiveObject"/>.
         * @return <code>int</code>
         */
        public int GetNumberOfSpellsOn()
        {
            return spellsOn.Count;
        }
        /**
         * Gets the number of BaseInteractiveObject groups to which the BaseInteractiveObject belongs.
         * @return {@link int}
         */
        public int GetNumIOGroups()
        {
            return this.ioGroups.Count;
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
         * Determines if the <see cref="BaseInteractiveObject"/> has a specific behavior
         * flag.
         * @param flag the flag
         * @return true if the <see cref="BaseInteractiveObject"/> has the flag; false
         *         otherwise
         */
        public bool HasBehaviorFlag(long flag)
        {
            return (behaviorFlags & flag) == flag;
        }
        /**
         * Determines if the <see cref="BaseInteractiveObject"/> has a specific game flag.
         * @param flag the flag
         * @return true if the <see cref="BaseInteractiveObject"/> has the flag; false
         *         otherwise
         */
        public bool HasGameFlag(long flag)
        {
            return (gameFlags & flag) == flag;
        }
        /**
         * Determines if the <see cref="BaseInteractiveObject"/> has a specific flag.
         * @param flag the flag
         * @return true if the <see cref="BaseInteractiveObject"/> has the flag; false
         *         otherwise
         */
        public bool HasIOFlag(long flag)
        {
            return (ioFlags & flag) == flag;
        }
        /**
         * Determines if the <see cref="BaseInteractiveObject"/> has a specific type flag.
         * @param flag the flag
         * @return true if the <see cref="BaseInteractiveObject"/> has the flag; false
         *         otherwise
         */
        public bool HasTypeFlag(long flag)
        {
            return (typeFlags & flag) == flag;
        }
        public bool IsInGroup(String group)
        {
            return ioGroups.Contains(group);
        }
        /** Removes all active spells. */
        public void RemoveAllSpells()
        {
            spellsOn.Clear();
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
         * Removes the BaseInteractiveObject from a group.
         * @param group the group
         */
        public void RemoveGroup(String group)
        {
            ioGroups.Remove(group);
        }
        /**
         * Removes a flag.
         * @param flag the flag
         */
        public void RemoveIOFlag(long flag)
        {
            ioFlags &= ~flag;
        }
        /**
         * Removes an active spell.
         * @param spellId the spell's id
         */
        public void RemoveSpellOn(int spellId)
        {
            spellsOn.Remove(spellId);
        }
        /**
         * Removes a type flag.
         * @param flag the flag
         */
        public void RemoveTypeFlag(long flag)
        {
            typeFlags &= ~flag;
        }
    }
}
