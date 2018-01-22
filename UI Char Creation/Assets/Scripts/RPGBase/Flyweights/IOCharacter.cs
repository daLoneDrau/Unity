using RPGBase.Constants;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public abstract class IOCharacter : Watchable
    {
        /// <summary>
        /// the index in the attribute map indicating the attribute's abbreviation.
        /// </summary>
        private const int ATTR_MAP_ABBR = 0;
        /// <summary>
        /// the index in the attribute map indicating the attribute's name.
        /// </summary>
        private const int ATTR_MAP_NAME = 1;
        /// <summary>
        /// the index in the attribute map indicating the attribute's modifier code.
        /// </summary>
        private const int ATTR_MAP_MOD_CODE = 2;
        /// <summary>
        /// the set of attributes defining the <see cref="IOCharacter"/>.
        /// </summary>
        private Dictionary<string, Attribute> attributes;
        /// <summary>
        /// the reference ids of all items equipped by the <see cref="IOCharacter"/> indexed by equipment slot.
        /// </summary>
        private int[] equippedItems;
        private int gender = -1;
        /// <summary>
        /// the <see cref="IOCharacter"/>'s gender.
        /// </summary>
        public int Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                NotifyWatchers();
            }
        }
        public float Life { get; private set; }
        /// <summary>
        /// the list of <see cref="Watcher"/>s associated with this <see cref="IOCharacter"/>.
        /// </summary>
        private Watcher[] watchers = new Watcher[0];
        /// <summary>
        /// Creates a new instance of <see cref="IOCharacter"/>.
        /// </summary>
        protected IOCharacter()
        {
            watchers = new Watcher[0];
            DefineAttributes();
            InitEquippedItems(ProjectConstants.Instance.GetMaxEquipped());
        }
        /**
         * {@inheritDoc}
         */
        public override void AddWatcher(Watcher watcher)
        {
            if (watcher != null)
            {
                int index = -1;
                for (int i = watchers.Length - 1; i >= 0; i--)
                {
                    if (watchers[i] == null)
                    {
                        index = i;
                        break;
                    }
                }
                if (index > -1)
                {
                    watchers[index] = watcher;
                }
                else
                {
                    watchers = ArrayUtilities.Instance.ExtendArray(watcher, watchers);
                }
            }
        }
        /// <summary>
        /// Adjusts the attribute modifier for a specific attribute.
        /// </summary>
        /// <param name="attr">the attribute name</param>
        /// <param name="val">the modifier amount</param>
        public void AdjustAttributeModifier(String attr, float val)
        {
            if (attr == null)
            {
                throw new RPGException(ErrorMessage.INTERNAL_BAD_ARGUMENT, "Attribute name cannot be null");
            }
            if (!attributes.ContainsKey(attr))
            {
                throw new RPGException(ErrorMessage.INTERNAL_BAD_ARGUMENT, "No such attribute " + attr);
            }
            attributes[attr].AdjustModifier(val);
        }
        /// <summary>
        /// Adjusts the <see cref="IOCharacter"/>'s life by a specific amount.
        /// </summary>
        /// <param name="dmg">the amount</param>
        protected void AdjustLife(float dmg)
        {
            String ls = GetLifeAttribute();
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("M");
            sb.Append(ls);
            String mls = sb.ToString();
            sb.ReturnToPool();
            sb = null;
            Life += dmg;
            if (Life > GetFullAttributeScore(mls))
            {
                // if Hit Points now > max
                Life = GetFullAttributeScore(mls);
            }
            if (Life < 0f)
            {
                // if life now < 0
                Life = 0f;
            }
            ls = null;
            mls = null;
        }
        /// <summary>
        /// Adjusts the <see cref="IOCharacter"/>'s mana by a specific amount.
        /// </summary>
        /// <param name="dmg">the amount</param>
        protected abstract void AdjustMana(float dmg);
        /// <summary>
        /// Gets the total modifier for a specific element type from the equipment the player is wielding.
        /// </summary>
        /// <param name="elementType">the type of element</param>
        /// <returns>float</returns>
        public float ApplyEquipmentModifiers(int elementType)
        {
            float toadd = 0;
            for (int i = ProjectConstants.Instance.GetMaxEquipped() - 1; i >= 0; i--)
            {
                if (equippedItems[i] >= 0
                        && Interactive.Instance.HasIO(equippedItems[i]))
                {
                    BaseInteractiveObject toequip = (BaseInteractiveObject)Interactive.Instance.GetIO(equippedItems[i]);
                    if (toequip.HasIOFlag(IoGlobals.IO_02_ITEM)
                            && toequip.ItemData != null
                            && toequip.ItemData.Equipitem != null)
                    {
                        EquipmentItemModifier element = toequip.ItemData.Equipitem.GetElementModifier(elementType);
                        if (!element.Percent)
                        {
                            toadd += element.Value;
                        }
                    }
                    toequip = null;
                }
            }
            return toadd;
        }
        protected abstract void ApplyRulesModifiers();
        protected abstract void ApplyRulesPercentModifiers();
        /// <summary>
        /// Gets the total percentage modifier for a specific element type from the equipment the player is wielding.
        /// </summary>
        /// <param name="elementType">the type of element</param>
        /// <param name="trueval">the true value being modified</param>
        /// <returns>float</returns>
        public float ApplyEquipmentPercentModifiers(int elementType, float trueval)
        {
            float toadd = 0;
            int i = ProjectConstants.Instance.GetMaxEquipped() - 1;
            for (; i >= 0; i--)
            {
                if (equippedItems[i] >= 0
                        && Interactive.Instance.HasIO(equippedItems[i]))
                {
                    BaseInteractiveObject toequip = (BaseInteractiveObject)Interactive.Instance.GetIO(equippedItems[i]);
                    if (toequip.HasIOFlag(IoGlobals.IO_02_ITEM)
                            && toequip.ItemData != null
                            && toequip.ItemData.Equipitem != null)
                    {
                        EquipmentItemModifier element = toequip.ItemData.Equipitem.GetElementModifier(elementType);
                        if (element.Percent)
                        {
                            toadd += element.Value;
                        }
                    }
                    toequip = null;
                }
            }
            return toadd * trueval * MathGlobals.DIV100;
        }
        public abstract bool CalculateBackstab();
        public abstract bool CalculateCriticalHit();
        /// <summary>
        /// Clears the attribute modifier for a specific attribute.
        /// </summary>
        /// <param name="attr">the attribute name</param>
        public void ClearAttributeModifier(string attr)
        {
            attributes[attr].ClearModifier();
        }
        /// <summary>
        /// Clears all ability score modifiers.
        /// </summary>
        public void ClearModAbilityScores()
        {
            if (attributes != null)
            {
                foreach (KeyValuePair<string, Attribute> entry in attributes)
                {
                    entry.Value.ClearModifier();
                }
            }
        }
        /// <summary>
        /// Compute FULL versions of player stats including equipped items, spells, and any other effect altering them.
        /// </summary>
        public void ComputeFullStats()
        {
            // clear mods
            ClearModAbilityScores();
            // apply equipment modifiers
            Object[][] map = GetAttributeMap();
            for (int i = map.Length - 1; i >= 0; i--)
            {
                AdjustAttributeModifier((string)map[i][ATTR_MAP_ABBR], ApplyEquipmentModifiers((int)map[i][ATTR_MAP_MOD_CODE]));
            }
            // apply modifiers based on rules
            ApplyRulesModifiers();
            // apply percent modifiers
            for (int i = map.Length - 1; i >= 0; i--)
            {
                float percentModifier = this.ApplyEquipmentPercentModifiers((int)map[i][ATTR_MAP_MOD_CODE], GetBaseAttributeScore((String)map[i][ATTR_MAP_ABBR]) + GetAttributeModifier((String)map[i][ATTR_MAP_ABBR]));
                AdjustAttributeModifier((String)map[i][ATTR_MAP_ABBR], percentModifier);
            }
            // apply percent modifiers based on rules
            ApplyRulesPercentModifiers();
        }
        /// <summary>
        /// Defines the <see cref="IOCharacter"/>'s attributes.
        /// </summary>
        protected void DefineAttributes()
        {
            attributes = new Dictionary<string, Attribute>();
            Object[][] map = GetAttributeMap();
            for (int i = map.Length - 1; i >= 0; i--)
            {
                attributes.Add((string)map[i][ATTR_MAP_ABBR], new Attribute((string)map[i][ATTR_MAP_ABBR], (string)map[i][ATTR_MAP_NAME]));
            }
            map = null;
        }
        /// <summary>
        /// Drains mana from the <see cref="IOCharacter"/>, returning the full amount drained.
        /// </summary>
        /// <param name="dmg">the attempted amount of mana to be drained</param>
        /// <returns></returns>
        public abstract float DrainMana(float dmg);
        /// <summary>
        /// Gets a specific attribute.
        /// </summary>
        /// <param name="abbr">the attribute's abbreviation</param>
        /// <returns><see cref="Attribute"/></returns>
        protected Attribute GetAttribute(String abbr)
        {
            return attributes[abbr];
        }
        /// <summary>
        /// Gets the initial attribute map.
        /// </summary>
        /// <returns></returns>
        protected abstract Object[][] GetAttributeMap();
        /// <summary>
        /// Gets the attribute modifier for a specific attribute.
        /// </summary>
        /// <param name="attr">the attribute name</param>
        /// <returns>float</returns>
        public float GetAttributeModifier(String attr)
        {
            return attributes[attr].Modifier;
        }
        /// <summary>
        /// Gets an attribute's display name.
        /// </summary>
        /// <param name="attr">the attribute's abbreviation</param>
        /// <returns><see cref="string"/></returns>
        public String GetAttributeName(String attr)
        {
            return attributes[attr].DisplayName;
        }
        /// <summary>
        /// Gets all attributes.
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, Attribute> GetAttributes()
        {
            return attributes;
        }
        /// <summary>
        /// Gets the base attribute score for a specific attribute.
        /// </summary>
        /// <param name="attr">the attribute name</param>
        /// <returns>float</returns>
        public float GetBaseAttributeScore(string attr)
        {
            return attributes[attr].BaseVal;
        }
        /// <summary>
        /// Gets the <see cref="IOCharacter"/>'s base mana value from the correct attribute.
        /// </summary>
        /// <returns></returns>
        protected abstract float GetBaseMana();
        /// <summary>
        /// Gets the reference id of the item the <see cref="IOCharacter"/> has equipped at a specific equipment slot. -1 is returned if no item is equipped.
        /// </summary>
        /// <param name="slot">the equipment slot</param>
        /// <returns><see cref="int"/></returns>
        public int GetEquippedItem(int slot)
        {
            int id = -1;
            if (slot < 0
                    || slot >= equippedItems.Length)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Error - equipment slot ");
                    sb.Append(slot);
                    sb.Append(" is outside array bounds.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.BAD_PARAMETERS, sb.ToString());
                sb.ReturnToPool();
                throw ex;
            }
            id = equippedItems[slot];
            return id;
        }
        /// <summary>
        /// Gets the full attribute score for a specific attribute.
        /// </summary>
        /// <param name="attr">the attribute name</param>
        /// <returns><see cref="float"/></returns>
        public float GetFullAttributeScore(string attr)
        {
            return attributes[attr].Full;
        }
        public abstract float GetFullDamage();
        public abstract BaseInteractiveObject GetIo();
        protected abstract String GetLifeAttribute();
        public abstract float GetMaxLife();
        /// <summary>
        /// Heals the IONpcData's mana.
        /// </summary>
        /// <param name="dmg">the amount of healing</param>
        public void HealMana(float dmg)
        {
            if (GetBaseMana() > 0f)
            {
                if (dmg > 0f)
                {
                    AdjustMana(dmg);
                }
            }
        }
        /// <summary>
        /// Initializes the items the <see cref="IOCharacter"/> has equipped.
        /// </summary>
        /// <param name="total">the total number of equipment slots</param>
        private void InitEquippedItems(int total)
        {
            equippedItems = new int[total];
            for (int i = 0; i < equippedItems.Length; i++)
            {
                equippedItems[i] = -1;
            }
        }
        /**
         * {@inheritDoc}
         */
        public override void NotifyWatchers()
        {
            for (int i = watchers.Length - 1; i >= 0; i--)
            {
                watchers[i].WatchUpdated(this);
            }
        }
        public abstract void RecreatePlayerMesh();
        /// <summary>
        /// Releases an equipped <see cref="BaseInteractiveObject"/>.
        /// </summary>
        /// <param name="id">the <see cref="BaseInteractiveObject"/>'s reference id</param>
        public void ReleaseEquipment(int id)
        {
            for (int i = ProjectConstants.Instance.GetMaxEquipped() - 1; i >= 0; i--)
            {
                if (equippedItems[i] == id)
                {
                    equippedItems[i] = -1;
                }
            }
        }
        /**
         * {@inheritDoc}
         */
        public override void RemoveWatcher(Watcher watcher)
        {
            int index = -1;
            for (int i = watchers.Length - 1; i >= 0; i--)
            {
                if (watchers[i].Equals(watcher))
                {
                    index = i;
                    break;
                }
            }
            if (index > -1)
            {
                watchers = ArrayUtilities.Instance.RemoveIndex(index, watchers);
            }
        }
        /// <summary>
        /// Sets the base attribute score for a specific attribute.
        /// </summary>
        /// <param name="attr">the attribute name</param>
        /// <param name="val">the new base attribute score</param>
        public void SetBaseAttributeScore(string attr, float val)
        {
            attributes[attr].BaseVal = val;
            NotifyWatchers();
        }
        /// <summary>
        /// Sets the reference id of the item the <see cref="IOCharacter"/> has equipped at a specific equipment slot.
        /// </summary>
        /// <param name="slot">the equipment slot</param>
        /// <param name="item">the item being equipped</param>
        public void SetEquippedItem(int slot, BaseInteractiveObject item)
        {
            if (slot < 0
                    || slot >= equippedItems.Length)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Error - equipment slot ");
                    sb.Append(slot);
                    sb.Append(" is outside array bounds.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.BAD_PARAMETERS, sb.ToString());
                sb.ReturnToPool();
                throw ex;
            }
            if (item == null)
            {
                equippedItems[slot] = -1;
            }
            else
            {
                equippedItems[slot] = item.RefId;
            }
        }
        /// <summary>
        /// Sets the reference id of the item the <see cref="IOCharacter"/> has equipped at a specific equipment slot.
        /// </summary>
        /// <param name="slot">the equipment slot</param>
        /// <param name="id">the item's reference id</param>
        public void SetEquippedItem(int slot, int id)
        {
            if (slot < 0
                    || slot >= equippedItems.Length)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Error - equipment slot ");
                    sb.Append(slot);
                    sb.Append(" is outside array bounds.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.BAD_PARAMETERS, sb.ToString());
                sb.ReturnToPool();
                throw ex;
            }
            equippedItems[slot] = id;
        }
        /// <summary>
        /// Removes all the player's equipment.
        /// </summary>
        public void UnEquipAll()
        {
            int i = ProjectConstants.Instance.GetMaxEquipped() - 1;
            for (; i >= 0; i--)
            {
                if (equippedItems[i] >= 0)
                {
                    if (!Interactive.Instance.HasIO(equippedItems[i]))
                    {
                        throw new RPGException(ErrorMessage.INVALID_DATA_TYPE, "Equipped unregistered item in slot " + i);
                    }
                    BaseInteractiveObject itemIO = (BaseInteractiveObject)Interactive.Instance.GetIO(equippedItems[i]);
                    if (!itemIO.HasIOFlag(IoGlobals.IO_02_ITEM))
                    {
                        throw new RPGException(ErrorMessage.INVALID_DATA_TYPE, "Equipped item without IO_02_ITEM in slot " + i);
                    }
                    if (itemIO.ItemData == null)
                    {
                        throw new RPGException(ErrorMessage.INVALID_DATA_TYPE, "Equipped item with null item data in slot " + i);
                    }
                    itemIO.ItemData.UnEquip(GetIo(), false);
                }
            }
            ComputeFullStats();
        }
    }
}
