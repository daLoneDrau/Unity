using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    class IOCharacter
    {
        /** the set of attributes defining the IOPcData. */
        private Map<String, Attribute> attributes;
        /**
         * the reference ids of all items equipped by the {@link IOCharacter},
         * indexed by equipment slot.
         */
        private int[] equippedItems;
        /**
         * the list of {@link Watcher}s associated with this {@link IOCharacter}.
         */
        private Watcher[] watchers = new Watcher[0];
        protected IOCharacter() 
        {
            watchers = new Watcher[0];
        defineAttributes();
        initEquippedItems(ProjectConstants.GetInstance().getMaxEquipped());
    }
    /**
     * {@inheritDoc}
     */
    @Override
    public  void addWatcher( Watcher watcher)
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
                watchers = ArrayUtilities.GetInstance().extendArray(
                        watcher, watchers);
            }
        }
    }
    /**
     * Adjusts the attribute modifier for a specific attribute.
     * @param attr the attribute name
     * @param val the modifier
     * @ if the attribute name is missing or incorrect
     */
    public  void adjustAttributeModifier( String attr,
             float val) 
    {
        if (attr == null) {
            throw new RPGException(ErrorMessage.INTERNAL_BAD_ARGUMENT,
                    "Attribute name cannot be null");
        }
        if (!attributes.containsKey(attr)) {
            throw new RPGException(ErrorMessage.INTERNAL_BAD_ARGUMENT,
                    "No such attribute " + attr);
        }
        attributes.get(attr).adjustModifier(val);
        }
    protected abstract void applyRulesModifiers() ;
    protected abstract void applyRulesPercentModifiers();
    /**
     * Gets the total modifier for a specific element type from the equipment
     * the player is wielding.
     * @param elementType the type of element
     * @return {@link float}
     * @ if an error occurs
     */
    public  float ARX_EQUIPMENT_Apply( int elementType)
            
    {
        float toadd = 0;
        int i = ProjectConstants.GetInstance().getMaxEquipped() - 1;
        for (; i >= 0; i--) {
            if (equippedItems[i] >= 0
                    && Interactive.GetInstance().hasIO(equippedItems[i]))
            {
                BaseInteractiveObject toequip =
                        (BaseInteractiveObject)Interactive.GetInstance().getIO(equippedItems[i]);
                if (toequip.HasIOFlag(IoGlobals.IO_02_ITEM)
                        && toequip.ItemData != null
                        && toequip.ItemData.getEquipitem() != null)
                {
                    EquipmentItemModifier element =
                            toequip.ItemData.getEquipitem().getElement(
                                    elementType);
                    if (!element.isPercentage())
                    {
                        toadd += element.getValue();
                    }
                }
                toequip = null;
            }
        }
        return toadd;
    }
    /**
     * Gets the total percentage modifier for a specific element type from the
     * equipment the player is wielding.
     * @param elementType the type of element
     * @param trueval the true value
     * @return {@link float}
     * @ if an error occurs
     */
    public  float ARX_EQUIPMENT_ApplyPercent( int elementType,
             float trueval) 
    {
        float toadd = 0;
        int i = ProjectConstants.GetInstance().getMaxEquipped() - 1;
        for (; i >= 0; i--) {
            if (equippedItems[i] >= 0
                    && Interactive.GetInstance().hasIO(equippedItems[i]))
            {
                BaseInteractiveObject toequip =
                        (BaseInteractiveObject)Interactive.GetInstance().getIO(equippedItems[i]);
                if (toequip.HasIOFlag(IoGlobals.IO_02_ITEM)
                        && toequip.ItemData != null
                        && toequip.ItemData.getEquipitem() != null)
                {
                    EquipmentItemModifier element =
                            toequip.ItemData.getEquipitem().getElement(
                                    elementType);
                    if (element.isPercentage())
                    {
                        toadd += element.getValue();
                    }
                }
                toequip = null;
            }
        }
        return toadd * trueval * MathGlobals.DIV100;
    }
    public abstract void ARX_EQUIPMENT_RecreatePlayerMesh();
    /**
     * Releases an equipped BaseInteractiveObject.
     * @param id the BaseInteractiveObject's reference id
     * @ if an error occurs
     */
    public  void ARX_EQUIPMENT_Release( int id) 
    {
        int i = ProjectConstants.GetInstance().getMaxEquipped() - 1;
        for (; i >= 0; i--) {
            if (equippedItems[i] == id)
            {
                equippedItems[i] = -1;
            }
        }
    }
    /**
     * Removes all the player's equipment.
     * @throws PooledException if an error occurs
     * @ if an error occurs
     */
    public  void ARX_EQUIPMENT_UnEquipAll()
            
    {
        int i = ProjectConstants.GetInstance().getMaxEquipped() - 1;
        for (; i >= 0; i--) {
            if (equippedItems[i] >= 0)
            {
                if (!Interactive.GetInstance().hasIO(equippedItems[i]))
                {
                    throw new RPGException(ErrorMessage.INVALID_DATA_TYPE,
                            "Equipped unregistered item in slot " + i);
                }
                BaseInteractiveObject itemIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(
                        equippedItems[i]);
                if (!itemIO.HasIOFlag(IoGlobals.IO_02_ITEM))
                {
                    throw new RPGException(ErrorMessage.INVALID_DATA_TYPE,
                            "Equipped item without IO_02_ITEM in slot " + i);
                }
                if (itemIO.ItemData == null)
                {
                    throw new RPGException(ErrorMessage.INVALID_DATA_TYPE,
                            "Equipped item with null item data in slot " + i);
                }
                itemIO.ItemData.ARX_EQUIPMENT_UnEquip(getIo(), false);
            }
        }
        computeFullStats();
    }
    public abstract bool calculateBackstab();
    public abstract bool calculateCriticalHit();
    /**
     * Clears the attribute modifier for a specific attribute.
     * @param attr the attribute name
     */
    public  void clearAttributeModifier( String attr)
    {
        attributes.get(attr).clearModifier();
    }
    /** Clears all ability score modifiers. */
    public  void clearModAbilityScores()
    {
        if (attributes != null)
        {
            Iterator<String> iter = attributes.keySet().iterator();
            while (iter.hasNext())
            {
                Attribute attr = attributes.get(iter.next());
                attr.clearModifier();
            }
        }
    }
    /**
     * Compute FULL versions of player stats including equipped items, spells,
     * and any other effect altering them.
     * @ if an error occurs
     */
    public  void computeFullStats() 
    {
        // clear mods
        clearModAbilityScores();
        // apply equipment modifiers
        Object []
        []
        map = getAttributeMap();
        for (int i = map.Length - 1; i >= 0; i--) {
            adjustAttributeModifier((String)map[i][0],
                    ARX_EQUIPMENT_Apply((int)map[i][2]));
        }
        // apply modifiers based on rules
        applyRulesModifiers();
        // apply percent modifiers
        for (int i = map.Length - 1; i >= 0; i--) {
            float percentModifier = ARX_EQUIPMENT_ApplyPercent((int)map[i][2],
                    getBaseAttributeScore((String)map[i][0])
                            + getAttributeModifier((String)map[i][0]));
            adjustAttributeModifier((String)map[i][0], percentModifier);
        }
        // apply percent modifiers based on rules
        applyRulesPercentModifiers();
    }
    /**
     * Defines the IOPcData's attributes.
     * @ if an error occurs
     */
    protected  void defineAttributes() 
    {
        attributes = new HashMap<String, Attribute>();
        Object[][] map = getAttributeMap();
        for (int i = map.Length - 1; i >= 0; i--) {
            attributes.put((String) map[i][0],
                    new Attribute((String) map[i][0], (String) map[i][1]));
        }
map = null;
    }
    /**
     * Gets a specific attribute.
     * @param abbr the attribute's abbreviation
     * @return {@link Attribute}
     */
    protected  Attribute getAttribute( String abbr)
{
    return attributes.get(abbr);
}
protected abstract Object[][] getAttributeMap();
/**
 * Gets the attribute modifier for a specific attribute.
 * @param attr the attribute name
 * @return {@link float}
 */
public  float getAttributeModifier( String attr)
{
    return attributes.get(attr).getModifier();
}
/**
 * Gets an attribute's display name.
 * @param attr the attribute's abbreviation
 * @return {@link String}
 */
public  String getAttributeName( String attr)
{
    return new String(attributes.get(attr).getDisplayName());
}
/**
 * Gets all attributes.
 * @return {@link Map}<{@link String}, {@link Attribute}>
 */
protected  Map<String, Attribute> getAttributes() {
    return attributes;
}
/**
 * Gets the base attribute score for a specific attribute.
 * @param attr the attribute name
 * @return {@link float}
 */
public  float getBaseAttributeScore( char[] attr)
{
    return getBaseAttributeScore(new String(attr));
}
/**
 * Gets the base attribute score for a specific attribute.
 * @param attr the attribute name
 * @return {@link float}
 */
public  float getBaseAttributeScore( String attr)
{
    return attributes.get(attr).getBase();
}
/**
 * Gets the reference id of the item the {@link IOCharacter} has equipped at
 * a specific equipment slot. -1 is returned if no item is equipped.
 * @param slot the equipment slot
 * @return <code>int</code>
 * @ if the equipment slot was never defined
 */
public  int getEquippedItem( int slot) 
{
        int id = -1;
        if (slot < 0
                || slot >= equippedItems.Length) {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
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
        RPGException ex = new RPGException(
                ErrorMessage.BAD_PARAMETERS, sb.ToString());
        sb.ReturnToPool();
        throw ex;
    }
    id = equippedItems [slot];
        return id;
}
/**
 * Gets the full attribute score for a specific attribute.
 * @param attr the attribute name
 * @return {@link float}
 */
public  float getFullAttributeScore( String attr)
{
    return attributes.get(attr).getFull();
}
public abstract float getFullDamage();
/**
 * Gets the BaseInteractiveObject associated with this {@link IOCharacter}.
 * @return {@link BaseInteractiveObject}
 */
public abstract BaseInteractiveObject getIo();
public abstract float getMaxLife();
/**
 * Initializes the items the {@link IOCharacter} has equipped.
 * @param total the total number of equipment slots
 */
private void initEquippedItems( int total)
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
@Override
    public  void notifyWatchers()
{
    for (int i = watchers.Length - 1; i >= 0; i--)
    {
        watchers[i].watchUpdated(this);
    }
}
/**
 * {@inheritDoc}
 */
@Override
    public  void removeWatcher( Watcher watcher)
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
        watchers =
                ArrayUtilities.GetInstance().removeIndex(index, watchers);
    }
}
/**
 * Sets the base attribute score for a specific attribute.
 * @param attr the attribute name
 * @param val the new base attribute score
 */
public  void setBaseAttributeScore( String attr,
         float val)
{
    attributes.get(attr).setBase(val);
}
/**
 * Sets the reference id of the item the {@link IOCharacter} has equipped at
 * a specific equipment slot.
 * @param slot the equipment slot
 * @param item the item being equipped
 * @ if the equipment slot was never defined
 */
public  void setEquippedItem( int slot,
         BaseInteractiveObject item)
            
{
        if (slot < 0
                || slot >= equippedItems.Length) {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
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
        RPGException ex = new RPGException(
                ErrorMessage.BAD_PARAMETERS, sb.ToString());
        sb.ReturnToPool();
        throw ex;
    }
        if (item == null) {
        equippedItems[slot] = -1;
    } else {
        equippedItems[slot] = item.GetRefId();
    }
}
/**
 * Sets the reference id of the item the {@link IOCharacter} has equipped at
 * a specific equipment slot.
 * @param slot the equipment slot
 * @param id the item's reference id
 * @ if the equipment slot was never defined
 */
public  void setEquippedItem( int slot,  int id)
            
{
        if (slot < 0
                || slot >= equippedItems.Length) {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
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
        RPGException ex = new RPGException(
                ErrorMessage.BAD_PARAMETERS, sb.ToString());
        sb.ReturnToPool();
        throw ex;
    }
    equippedItems [slot] = id;
}
    }
}
