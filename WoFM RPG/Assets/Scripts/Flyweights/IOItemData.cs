using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public abstract class IOItemData
    {
        /// <summary>
        /// the current number in an inventory slot.
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// the item's description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// modifier data for the item.
        /// </summary>
        public IOEquipItem Equipitem { get; set; }
        /// <summary>
        /// dunno?
        /// </summary>
        public char FoodValue { get; set; }
        /// <summary>
        /// the BaseInteractiveObject associated with this data.
        /// </summary>
        public BaseInteractiveObject Io { get; set; }
        /// <summary>
        /// the item's name.
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// the item's light value.
        /// </summary>
        public int LightValue { get; set; }
        /// <summary>
        /// the maximum number of the item the player can own.
        /// </summary>
        public int MaxOwned { get; set; }
        /// <summary>
        /// the item's price.
        /// </summary>
        public float Price { get; set; }
        /// <summary>
        /// the type of ring the item is.
        /// </summary>
        public int RingType { get; set; }
        /// <summary>
        /// the amount of the item that can be stacked in one inventory slot.
        /// </summary>
        public int StackSize { get; set; }
        /// <summary>
        /// dunno?
        /// </summary>
        public char Stealvalue { get; set; }
        /// <summary>
        /// the item's weight.
        /// </summary>
        public float Weight { get; set; }
        /// <summary>
        /// the item's title.
        /// </summary>
        public string Title { get; set; }
        /**
         * Creates a new instance of {@link IOItemData}.
         * @
         */
        public IOItemData()
        {
            Equipitem = new IOEquipItem();
        }
        /**
         * Adjusts the {@link IOItemData}'s Count.
         * @param val the amount adjusted by
         */
        public void AdjustCount(int val)
        {
            if (Count + val < 0)
            {
                throw new RPGException(ErrorMessage.INVALID_PARAM,
                        "Cannot remove that many items");
            }
            if (Count + val > MaxOwned)
            {
                throw new RPGException(ErrorMessage.INVALID_PARAM,
                        "Cannot add that many items");
            }
            Count += val;
        }
        protected abstract float ApplyCriticalModifier();
        public float ComputeDamages(BaseInteractiveObject io_source,
                BaseInteractiveObject io_target, float dmgModifier)
        {
            float damages = 0;
            // send event to target. someone attacked you!
            Script.GetInstance().setEventSender(io_source);
            Script.GetInstance().sendIOScriptEvent(io_target,
                        ScriptConsts.SM_057_AGGRESSION, null, null);
            if (io_source != null
                    && io_target != null)
            {
                if (!io_target.HasIOFlag(IoGlobals.IO_01_PC)
                        && !io_target.HasIOFlag(IoGlobals.IO_03_NPC)
                /* && io_target.HasIOFlag(IoGlobals.fix) */)
                {
                    if (io_source.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        // player fixing the target object
                        // ARX_DAMAGES_DamageFIX(io_target, player.Full_damages, 0,
                        // 0);
                    }
                    else if (io_source.HasIOFlag(IoGlobals.IO_03_NPC))
                    {
                        // IONpcData fixing target
                        // ARX_DAMAGES_DamageFIX(io_target,
                        // io_source->_npcdata->damages, GetInterNum(io_source), 0);
                    }
                    else
                    {
                        // unknown fixing target
                        // ARX_DAMAGES_DamageFIX(io_target, 1,
                        // GetInterNum(io_source), 0);
                    }
                }
                else
                {
                    float attack, ac;
                    float backstab = 1f;
                    // weapon material
                    String wmat = "BARE";
                    // armor material
                    String amat = "FLESH";
                    bool critical = false;
                    if (io_source.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        int wpnId = io_source.GetPCData().getEquippedItem(
                                EquipmentGlobals.EQUIP_SLOT_WEAPON);
                        if (Interactive.GetInstance().hasIO(wpnId))
                        {
                            BaseInteractiveObject io = (BaseInteractiveObject)Interactive.GetInstance().getIO(wpnId);
                            if (io.GetWeaponmaterial() != null
                                    && io.GetWeaponmaterial().Length() > 0)
                            {
                                wmat = io.GetWeaponmaterial();
                            }
                            io = null;
                        }
                        attack = io_source.GetPCData().GetFullDamage();
                        if (io_source.GetPCData().CalculateCriticalHit()
                                && Script.GetInstance().sendIOScriptEvent(
                                        io_source, ScriptConsts.SM_054_CRITICAL,
                                        null, null) != ScriptConsts.REFUSE)
                        {
                            critical = true;
                        }
                        damages = attack * dmgModifier;
                        if (io_source.GetPCData().calculateBackstab()
                                && Script.GetInstance().sendIOScriptEvent(
                                        io_source, ScriptConsts.SM_056_BACKSTAB,
                                        null, null) != ScriptConsts.REFUSE)
                        {
                            backstab = this.GetBackstabModifier();
                        }
                    }
                    else
                    {
                        if (io_source.HasIOFlag(IoGlobals.IO_03_NPC))
                        {
                            int wpnId = io_source.GetNPCData().getEquippedItem(
                                    EquipmentGlobals.EQUIP_SLOT_WEAPON);
                            if (Interactive.GetInstance().hasIO(wpnId))
                            {
                                BaseInteractiveObject io = (BaseInteractiveObject)Interactive.GetInstance().getIO(wpnId);
                                if (io.GetWeaponmaterial() != null
                                        && io.GetWeaponmaterial().Length() > 0)
                                {
                                    wmat = io.GetWeaponmaterial();
                                }
                                io = null;
                            }
                            else
                            {
                                if (io_source.GetWeaponmaterial() != null
                                        && io_source.GetWeaponmaterial()
                                                .Length() > 0)
                                {
                                    wmat = io_source.GetWeaponmaterial();
                                }
                            }
                            attack = io_source.GetNPCData().GetFullDamage();
                            if (io_source.GetNPCData().CalculateCriticalHit()
                                    && Script.GetInstance().sendIOScriptEvent(
                                            io_source,
                                            ScriptConsts.SM_054_CRITICAL,
                                            null, null) != ScriptConsts.REFUSE)
                            {
                                critical = true;
                            }
                            damages = attack * dmgModifier;
                            if (io_source.GetNPCData().calculateBackstab()
                                    && Script.GetInstance().sendIOScriptEvent(
                                            io_source,
                                            ScriptConsts.SM_056_BACKSTAB,
                                            null, null) != ScriptConsts.REFUSE)
                            {
                                backstab = this.GetBackstabModifier();
                            }
                        }
                        else
                        {
                            throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                                    "Compute Damages call made by non-character");
                        }
                    }
                    // calculate how much damage is absorbed by armor
                    float absorb = this.CalculateArmorDeflection();
                    // float absorb;

                    // if (io_target == inter.iobj[0]) {
                    // ac = player.Full_armor_class;
                    // absorb = player.Full_Skill_Defense * DIV2;
                    // } else {
                    // ac = ARX_INTERACTIVE_GetArmorClass(io_target);
                    // absorb = io_target->_npcdata->absorb;
                    // long value = ARX_SPELLS_GetSpellOn(io_target, SPELL_CURSE);
                    // if (value >= 0) {
                    // float modif = (spells[value].caster_level * 0.05f);
                    // ac *= modif;
                    // absorb *= modif;
                    // }
                    // }
                    if (io_target.GetArmormaterial() != null
                            && io.GetArmormaterial().Length() > 0)
                    {
                        amat = io.GetArmormaterial();
                    }
                    if (io_target.HasIOFlag(IoGlobals.IO_03_NPC)
                            || io_target.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        int armrId;
                        if (io_target.HasIOFlag(IoGlobals.IO_03_NPC))
                        {
                            armrId = io_target.GetNPCData().getEquippedItem(
                                    EquipmentGlobals.EQUIP_SLOT_TORSO);
                        }
                        else
                        {
                            armrId = io_target.GetPCData().getEquippedItem(
                                    EquipmentGlobals.EQUIP_SLOT_TORSO);
                        }
                        if (Interactive.GetInstance().hasIO(armrId))
                        {
                            BaseInteractiveObject io = (BaseInteractiveObject)Interactive.GetInstance().getIO(armrId);
                            if (io.GetArmormaterial() != null
                                    && io.GetArmormaterial().Length() > 0)
                            {
                                amat = io.GetArmormaterial();
                            }
                            io = null;
                        }
                    }
                    damages *= backstab;
                    // dmgs -= dmgs * (absorb * DIV100);

                    // TODO - play sound based on the power of the hit
                    if (damages > 0f)
                    {
                        if (critical)
                        {
                            damages = this.ApplyCriticalModifier();
                            // dmgs *= 1.5f;
                        }

                        if (io_target.HasIOFlag(IoGlobals.IO_01_PC))
                        {
                            // TODO - push player when hit
                            // ARX_DAMAGES_SCREEN_SPLATS_Add(&ppos, dmgs);
                            io_target.GetPCData().ARX_DAMAGES_DamagePlayer(damages,
                                    0, io_source.GetRefId());
                            // ARX_DAMAGES_DamagePlayerEquipment(dmgs);
                        }
                        else
                        {
                            // TODO - push IONpcData when hit
                            io_target.GetNPCData().damageNPC(damages,
                                    io_source.GetRefId(), false);
                        }
                    }
                }

            }
            return damages;
        }
        /**
         * Equips the item on a target BaseInteractiveObject.
         * @param target the target BaseInteractiveObject
         * @throws PooledException if an error occurs
         * @ if an error occurs
         */
        public void Equip(BaseInteractiveObject target)
        {
            if (Io == null)
            {
                throw new RPGException(ErrorMessage.INTERNAL_ERROR,
                        "Cannot equip item with no BaseInteractiveObject data");
            }
            if (target != null)
            {
                if (target.HasIOFlag(IoGlobals.IO_01_PC)
                        || target.HasIOFlag(IoGlobals.IO_03_NPC))
                {
                    IOCharacter charData;
                    if (target.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        charData = target.GetPCData();
                    }
                    else
                    {
                        charData = target.GetNPCData();
                    }
                    int validid = -1;
                    int i = Interactive.GetInstance().getMaxIORefId();
                    for (; i >= 0; i--)
                    {
                        if (Interactive.GetInstance().hasIO(i)
                                && Interactive.GetInstance().getIO(i) != null
                                && io.Equals(Interactive.GetInstance().getIO(i)))
                        {
                            validid = i;
                            break;
                        }
                    }
                    if (validid >= 0)
                    {
                        Interactive.GetInstance().RemoveFromAllInventories(io);
                        io.SetShow(IoGlobals.SHOW_FLAG_ON_PLAYER); // on player
                                                                   // handle drag
                                                                   // if (toequip == DRAGINTER)
                                                                   // Set_DragInter(NULL);
                        if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_WEAPON))
                        {
                            EquipWeapon(charData);
                        }
                        else if (io
                              .HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_SHIELD))
                        {
                            EquipShield(charData);
                        }
                        else if (io
                              .HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_RING))
                        {
                            EquipRing(charData);
                        }
                        else if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_ARMOR))
                        {
                            // unequip old armor
                            UnequipItemInSlot(
                                    charData, EquipmentGlobals.EQUIP_SLOT_TORSO);
                            // equip new armor
                            charData.setEquippedItem(
                                    EquipmentGlobals.EQUIP_SLOT_TORSO, validid);
                        }
                        else if (io
                              .HasTypeFlag(
                                      EquipmentGlobals.OBJECT_TYPE_LEGGINGS))
                        {
                            // unequip old leggings
                            UnequipItemInSlot(
                                    charData, EquipmentGlobals.EQUIP_SLOT_LEGGINGS);
                            // equip new leggings
                            charData.setEquippedItem(
                                    EquipmentGlobals.EQUIP_SLOT_LEGGINGS, validid);
                        }
                        else if (io
                              .HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_HELMET))
                        {
                            // unequip old helmet
                            UnequipItemInSlot(
                                    charData, EquipmentGlobals.EQUIP_SLOT_HELMET);
                            // equip new helmet
                            charData.setEquippedItem(
                                    EquipmentGlobals.EQUIP_SLOT_HELMET, validid);
                        }
                        if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_HELMET)
                                || io.HasTypeFlag(
                                        EquipmentGlobals.OBJECT_TYPE_ARMOR)
                                || io.HasTypeFlag(
                                        EquipmentGlobals.OBJECT_TYPE_LEGGINGS))
                        {
                            charData.ARX_EQUIPMENT_RecreatePlayerMesh();
                        }
                        charData.computeFullStats();
                    }
                }
            }
        }
        public  void ARX_EQUIPMENT_ReleaseAll()
        {
            ARX_EQUIPMENT_ReleaseEquipItem();
        }
        /** Releases the {@link IOEquipItem} data from the item. */
        public  void ARX_EQUIPMENT_ReleaseEquipItem()
        {
            if (equipitem != null)
            {
                equipitem = null;
            }
        }
        /**
         * Sets the item's object type.
         * @param flag the type flag
         * @param added if <tt>true</tt>, the type is set; otherwise it is removed
         * @ if an error occurs
         */
        public  void ARX_EQUIPMENT_SetObjectType( int flag,
                 bool added) 
        {
        if (added) {
        io.AddTypeFlag(flag);
    } else {
        io.RemoveTypeFlag(flag);
    }
}
/**
 * Unequips the item from the targeted BaseInteractiveObject.
 * @param target the targeted BaseInteractiveObject
 * @param isDestroyed if<tt>true</tt> the item is destroyed afterwards
 * @throws PooledException if an error occurs
 * @ if an error occurs
 */
public void ARX_EQUIPMENT_UnEquip( BaseInteractiveObject target,
         bool isDestroyed) 
{
        if (target != null) {
        if (target.HasIOFlag(IoGlobals.IO_01_PC))
        {
            int i = ProjectConstants.GetInstance().getMaxEquipped() - 1;
            for (; i >= 0; i--)
            {
                IoPcData player = target.GetPCData();
                int itemRefId = player.getEquippedItem(i);
                if (itemRefId >= 0
                        && Interactive.GetInstance().hasIO(itemRefId)
                        && Interactive.GetInstance().getIO(
                                itemRefId).Equals(io))
                {
                    // EERIE_LINKEDOBJ_UnLinkObjectFromObject(
                    // target->obj, tounequip->obj);
                    player.ARX_EQUIPMENT_Release(itemRefId);
                    // target->bbox1.x = 9999;
                    // target->bbox2.x = -9999;

                    if (!isDestroyed)
                    {
                        // if (DRAGINTER == null) {
                        // ARX_SOUND_PlayInterface(SND_INVSTD);
                        // Set_DragInter(tounequip);
                        // } else
                        if (!target.getInventory().CanBePutInInventory(
                                io))
                        {
                            target.getInventory().PutInFrontOfPlayer(
                                    io, true);
                        }
                    }
                    // send event from this item to target to unequip
                    Script.GetInstance().setEventSender(io);
                    Script.GetInstance().sendIOScriptEvent(target,
                            ScriptConsts.SM_007_EQUIPOUT, null, null);
                    // send event from target to this item to unequip
                    Script.GetInstance().setEventSender(target);
                    Script.GetInstance().sendIOScriptEvent(io,
                            ScriptConsts.SM_007_EQUIPOUT, null, null);
                }
            }
            if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_HELMET)
                    || io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_ARMOR)
                    || io.HasTypeFlag(
                            EquipmentGlobals.OBJECT_TYPE_LEGGINGS))
            {
                target.GetPCData().ARX_EQUIPMENT_RecreatePlayerMesh();
            }
        }
    }
}
protected abstract float CalculateArmorDeflection();
/**
 * Equips a ring on a character.
 * @param charData the character data
 * @ if an error occurs
 */
private void EquipRing( IOCharacter charData) 
{
    // check left and right finger
    // to see if it can be equipped
    bool canEquip = true;
        int ioid = charData.getEquippedItem(
                EquipmentGlobals.EQUIP_SLOT_RING_LEFT);
        if (Interactive.GetInstance().hasIO(ioid)) {
        BaseInteractiveObject oldRing = (BaseInteractiveObject)Interactive.GetInstance().getIO(ioid);
        if (oldRing.ItemData.getRingType() == ringType)
        {
            // already wearing that type
            // of ring on left finger
            canEquip = false;
        }
        oldRing = null;
    }
        if (canEquip) {
        ioid = charData.getEquippedItem(
                EquipmentGlobals.EQUIP_SLOT_RING_RIGHT);
        if (Interactive.GetInstance().hasIO(ioid))
        {
            BaseInteractiveObject oldRing = (BaseInteractiveObject)Interactive.GetInstance().getIO(ioid);
            if (oldRing.ItemData.getRingType() == ringType)
            {
                // already wearing that type
                // of ring on right finger
                canEquip = false;
            }
            oldRing = null;
        }
    }
        if (canEquip) {
        int equipSlot = -1;
        if (charData.getEquippedItem(
                EquipmentGlobals.EQUIP_SLOT_RING_LEFT) < 0)
        {
            equipSlot = EquipmentGlobals.EQUIP_SLOT_RING_LEFT;
        }
        if (charData.getEquippedItem(
                EquipmentGlobals.EQUIP_SLOT_RING_RIGHT) < 0)
        {
            equipSlot = EquipmentGlobals.EQUIP_SLOT_RING_RIGHT;
        }
        if (equipSlot == -1)
        {
            if (!charData.getIo().getInventory().isLeftRing())
            {
                ioid = charData.getEquippedItem(
                        EquipmentGlobals.EQUIP_SLOT_RING_RIGHT);
                if (Interactive.GetInstance().hasIO(ioid))
                {
                    BaseInteractiveObject oldIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(ioid);
                    if (oldIO.HasIOFlag(IoGlobals.IO_02_ITEM))
                    {
                        UnequipItemInSlot(charData,
                                EquipmentGlobals.EQUIP_SLOT_RING_RIGHT);
                    }
                    oldIO = null;
                }
                equipSlot =
                        EquipmentGlobals.EQUIP_SLOT_RING_RIGHT;
            }
            else
            {
                ioid = charData.getEquippedItem(
                        EquipmentGlobals.EQUIP_SLOT_RING_LEFT);
                if (Interactive.GetInstance().hasIO(ioid))
                {
                    BaseInteractiveObject oldIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(ioid);
                    if (oldIO.HasIOFlag(IoGlobals.IO_02_ITEM))
                    {
                        UnequipItemInSlot(charData,
                                EquipmentGlobals.EQUIP_SLOT_RING_LEFT);
                    }
                    oldIO = null;
                }
                equipSlot = EquipmentGlobals.EQUIP_SLOT_RING_LEFT;
            }
            charData.getIo().getInventory().setLeftRing(
                    !charData.getIo().getInventory().isLeftRing());
        }
        charData.setEquippedItem(equipSlot, io.GetRefId());
    }
}
/**
 * Equips a shield on a character.
 * @param charData the character data
 * @ if an error occurs
 */
private void EquipShield( IOCharacter charData) 
{
    // unequip old shield
    UnequipItemInSlot(charData, EquipmentGlobals.EQUIP_SLOT_SHIELD);
    // equip new shield
    charData.setEquippedItem(
                EquipmentGlobals.EQUIP_SLOT_SHIELD, io.GetRefId());
        // TODO - attach new shield to mesh
        // EERIE_LINKEDOBJ_LinkObjectToObject(target->obj,
        // io->obj, "SHIELD_ATTACH", "SHIELD_ATTACH", io);
        int wpnID =
                charData.getEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
        if (wpnID >= 0) {
        if (Interactive.GetInstance().hasIO(wpnID))
        {
            BaseInteractiveObject wpn = (BaseInteractiveObject)Interactive.GetInstance().getIO(wpnID);
            if (wpn.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_2H)
                    || wpn.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW))
            {
                // unequip old weapon
                UnequipItemInSlot(
                        charData, EquipmentGlobals.EQUIP_SLOT_WEAPON);
            }
        }
    }
}
/**
 * Equips a weapon for a character.
 * @param charData the character data
 * @ if an error occurs
 */
private void EquipWeapon( IOCharacter charData) 
{
    // unequip old weapon
    UnequipItemInSlot(charData, EquipmentGlobals.EQUIP_SLOT_WEAPON);
    // equip new weapon
    charData.setEquippedItem(
                EquipmentGlobals.EQUIP_SLOT_WEAPON, io.GetRefId());
        // attach it to player mesh
        if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW)) {
        // EERIE_LINKEDOBJ_LinkObjectToObject(
        // target->obj, io->obj,
        // "WEAPON_ATTACH", "TEST", io);
    } else {
        // EERIE_LINKEDOBJ_LinkObjectToObject(
        // target->obj,
        // io->obj,
        // "WEAPON_ATTACH", "PRIMARY_ATTACH", io); //
    }
        if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_2H)
                || io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW)) {
        // for bows or 2-handed swords, unequip old shield
        UnequipItemInSlot(charData, EquipmentGlobals.EQUIP_SLOT_SHIELD);
    }
}
protected abstract float GetBackstabModifier();
/**
 * Gets the current number in an inventory slot.
 * @return {@link short}
 */
public  int getCount()
{
    return Count;
}
/**
 * Gets the item's description.
 * @return {@link char[]}
 */
public  char[] getDescription()
{
    return description;
}
/**
 * Gets the list of equipment item modifiers.
 * @return {@link EquipmentItemModifier}[]
 */
public  IOEquipItem getEquipitem()
{
    return equipitem;
}
/**
 * Gets the value for the foodValue.
 * @return {@link char}
 */
public char getFoodValue()
{
    return foodValue;
}
/**
 * Gets the BaseInteractiveObject associated with this data.
 * @return {@link BaseInteractiveObject}
 */
public BaseInteractiveObject getIo()
{
    return io;
}
/**
 * Gets the item's name.
 * @return <code>char</code>[]
 */
public  char[] getItemName()
{
    return itemName;
}
/**
 * Gets the value for the lightValue.
 * @return {@link int}
 */
public int getLightValue()
{
    return lightValue;
}
/**
 * Gets the maximum number of the item the player can own.
 * @return {@link int}
 */
public  int getMaxOwned()
{
    return MaxOwned;
}
/**
 * Gets the item's price.
 * @return {@link float}
 */
public  float getPrice()
{
    return price;
}
/**
 * Gets the type of ring the item is.
 * @return {@link int}
 */
public int getRingType()
{
    return ringType;
}
/**
 * Gets the value for the stackSize.
 * @return {@link int}
 */
public int getStackSize()
{
    return stackSize;
}
/**
 * Gets the value for the stealvalue.
 * @return {@link char}
 */
public char getStealvalue()
{
    return stealvalue;
}
/**
 * Gets the type of weapon an item is.
 * @return {@link int}
 */
public  int getWeaponType()
{
    int type = EquipmentGlobals.WEAPON_BARE;
    if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_DAGGER))
    {
        type = EquipmentGlobals.WEAPON_DAGGER;
    }
    else if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_1H))
    {
        type = EquipmentGlobals.WEAPON_1H;
    }
    else if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_2H))
    {
        type = EquipmentGlobals.WEAPON_2H;
    }
    else if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW))
    {
        type = EquipmentGlobals.WEAPON_BOW;
    }
    return type;
}
/**
 * Gets the item's weight.
 * @return {@link float}
 */
public  float getWeight()
{
    return weight;
}
/**
 * Sets the current number in an inventory slot.
 * @param val the new value to set
 */
public  void setCount( int val)
{
    Count = val;
}
/**
 * Sets the {@link IOItemData}'s description.
 * @param val the name to set
 * @ if the parameter is null
 */
public  void setDescription( char[] val) 
{
        if (val == null) {
        throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                "Description cannot be null");
    }
    description = val;
}
/**
 * Sets the {@link IOItemData}'s description.
 * @param val the name to set
 * @ if the parameter is null
 */
public  void setDescription( String val) 
{
        if (val == null) {
        throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                "Description cannot be null");
    }
    description = val;
}
/**
 * Sets the equipitem
 * @param val the equipitem to set
 */
public void setEquipitem( IOEquipItem val)
{
    this.equipitem = val;
}
/**
 * Sets the value of the foodValue.
 * @param foodValue the new value to set
 */
public void setFoodValue( char foodValue)
{
    this.foodValue = foodValue;
}
/**
 * Sets the the BaseInteractiveObject associated with this data.
 * @param val the new value to set
 */
public void setIo( BaseInteractiveObject val)
{
    io = val;
    if (val != null
            && val.ItemData == null)
    {
        val.setItemData(this);
    }
}
/**
 * Sets the item's name.
 * @param val the name to set
 * @ if the parameter is null
 */
public  void setItemName( char[] val) 
{
        if (val == null) {
        throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                "Item name cannot be null");
    }
        this.itemName = val;
}
/**
 * Sets the item's name.
 * @param val the name to set
 * @ if the parameter is null
 */
public  void setItemName( String val) 
{
        if (val == null) {
        throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                "Item name cannot be null");
    }
        this.itemName = val;
}
/**
 * Sets the value of the lightValue.
 * @param lightValue the new value to set
 */
public void setLightValue( int lightValue)
{
    this.lightValue = lightValue;
}
/**
 * Sets the maximum number of the item the player can own.
 * @param val the new value
 */
public  void setMaxOwned( int val)
{
    this.MaxOwned = val;
}
/**
 * Sets the item's price.
 * @param val the price to set
 */
public  void setPrice( float val)
{
    price = val;
}
/**
 * Sets the type of ring the item is.
 * @param val the new value to set
 */
public void setRingType( int val)
{
    this.ringType = val;
}
/**
 * Sets the amount of the item that can be stacked in one inventory slot.
 * @param val the value to set
 */
public void setStackSize( int val)
{
    this.stackSize = val;
}
/**
 * Sets the value of the stealvalue.
 * @param stealvalue the new value to set
 */
public void setStealvalue( char stealvalue)
{
    this.stealvalue = stealvalue;
}
/**
 * Sets the item's weight.
 * @param f the weight to set
 */
public  void setWeight( float f)
{
    weight = f;
}
private void UnequipItemInSlot( IOCharacter player,  int slot)
            
{
        if (player.getEquippedItem(slot) >= 0) {
        int slotioid = player.getEquippedItem(slot);
        if (Interactive.GetInstance().hasIO(slotioid))
        {
            BaseInteractiveObject equipIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(slotioid);
            if (equipIO.HasIOFlag(IoGlobals.IO_02_ITEM)
                    && equipIO.ItemData != null)
            {
                equipIO.ItemData.ARX_EQUIPMENT_UnEquip(
                        player.getIo(), false);
            }
            equipIO = null;
        }
    }
}
public String getTitle()
{
    return new String(title);
}
public void setTitle(String val)
{
    title = val;
}
    }
}
