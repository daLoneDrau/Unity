using RPGBase.Constants;
using RPGBase.Singletons;
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
        private string description;
        /// <summary>
        /// the item's description.
        /// </summary>
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                if (description == null)
                {
                    throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Description cannot be null");
                }
            }
        }
        /// <summary>
        /// modifier data for the item.
        /// </summary>
        public IOEquipItem Equipitem { get; set; }
        /// <summary>
        /// dunno?
        /// </summary>
        public char FoodValue { get; set; }
        private BaseInteractiveObject io;
        /// <summary>
        /// the BaseInteractiveObject associated with this data.
        /// </summary>
        public BaseInteractiveObject Io
        {
            get { return io; }
            set
            {
                io = value;
                if (value != null
                        && value.ItemData == null)
                {
                    value.ItemData = this;
                }
            }
        }
        private string itemName;
        /// <summary>
        /// the item's name.
        /// </summary>
        public string ItemName
        {
            get { return itemName; }
            set
            {
                itemName = value;
                if (itemName == null)
                {
                    throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Item name cannot be null");
                }
            }
        }
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
        /// <summary>
        /// Creates a new instance of <see cref="IOItemData"/>.
        /// </summary>
        public IOItemData()
        {
            Equipitem = new IOEquipItem();
        }
        /// <summary>
        /// Adjusts the <see cref="IOItemData"/>'s count.
        /// </summary>
        /// <param name="val">the amount adjusted by</param>
        public void AdjustCount(int val)
        {
            if (Count + val < 0)
            {
                throw new RPGException(ErrorMessage.INVALID_PARAM, "Cannot remove that many items");
            }
            if (Count + val > MaxOwned)
            {
                throw new RPGException(ErrorMessage.INVALID_PARAM, "Cannot add that many items");
            }
            Count += val;
        }
        protected abstract float ApplyCriticalModifier();
        public float ComputeDamages(BaseInteractiveObject io_source, BaseInteractiveObject io_target, float dmgModifier)
        {
            float damages = 0;
            // send event to target. someone attacked you!
            Script.Instance.EventSender = io_source;
            Script.Instance.SendIOScriptEvent(io_target, ScriptConsts.SM_057_AGGRESSION, null, null);
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
                        int wpnId = io_source.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
                        if (Interactive.Instance.HasIO(wpnId))
                        {
                            BaseInteractiveObject io = (BaseInteractiveObject)Interactive.Instance.GetIO(wpnId);
                            if (io.Weaponmaterial != null
                                    && io.Weaponmaterial.Length > 0)
                            {
                                wmat = io.Weaponmaterial;
                            }
                            io = null;
                        }
                        attack = io_source.PcData.GetFullDamage();
                        if (io_source.PcData.CalculateCriticalHit()
                                && Script.Instance.SendIOScriptEvent(io_source, ScriptConsts.SM_054_CRITICAL, null, null) != ScriptConsts.REFUSE)
                        {
                            critical = true;
                        }
                        damages = attack * dmgModifier;
                        if (io_source.PcData.CalculateBackstab()
                                && Script.Instance.SendIOScriptEvent(io_source, ScriptConsts.SM_056_BACKSTAB, null, null) != ScriptConsts.REFUSE)
                        {
                            backstab = this.GetBackstabModifier();
                        }
                    }
                    else
                    {
                        if (io_source.HasIOFlag(IoGlobals.IO_03_NPC))
                        {
                            int wpnId = io_source.NpcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
                            if (Interactive.Instance.HasIO(wpnId))
                            {
                                BaseInteractiveObject io = (BaseInteractiveObject)Interactive.Instance.GetIO(wpnId);
                                if (io.Weaponmaterial != null
                                        && io.Weaponmaterial.Length > 0)
                                {
                                    wmat = io.Weaponmaterial;
                                }
                                io = null;
                            }
                            else
                            {
                                if (io_source.Weaponmaterial != null
                                        && io_source.Weaponmaterial.Length > 0)
                                {
                                    wmat = io_source.Weaponmaterial;
                                }
                            }
                            attack = io_source.NpcData.GetFullDamage();
                            if (io_source.NpcData.CalculateCriticalHit()
                                    && Script.Instance.SendIOScriptEvent(io_source, ScriptConsts.SM_054_CRITICAL, null, null) != ScriptConsts.REFUSE)
                            {
                                critical = true;
                            }
                            damages = attack * dmgModifier;
                            if (io_source.NpcData.CalculateBackstab()
                                    && Script.Instance.SendIOScriptEvent(io_source, ScriptConsts.SM_056_BACKSTAB, null, null) != ScriptConsts.REFUSE)
                            {
                                backstab = this.GetBackstabModifier();
                            }
                        }
                        else
                        {
                            throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Compute Damages call made by non-character");
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
                    if (io_target.Armormaterial != null
                            && io.Armormaterial.Length > 0)
                    {
                        amat = io.Armormaterial;
                    }
                    if (io_target.HasIOFlag(IoGlobals.IO_03_NPC)
                            || io_target.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        int armrId;
                        if (io_target.HasIOFlag(IoGlobals.IO_03_NPC))
                        {
                            armrId = io_target.NpcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO);
                        }
                        else
                        {
                            armrId = io_target.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO);
                        }
                        if (Interactive.Instance.HasIO(armrId))
                        {
                            BaseInteractiveObject io = (BaseInteractiveObject)Interactive.Instance.GetIO(armrId);
                            if (io.Armormaterial != null
                                    && io.Armormaterial.Length > 0)
                            {
                                amat = io.Armormaterial;
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
                            io_target.PcData.DamagePlayer(damages, 0, io_source.RefId);
                            // ARX_DAMAGES_DamagePlayerEquipment(dmgs);
                        }
                        else
                        {
                            // TODO - push IONpcData when hit
                            io_target.NpcData.DamageNPC(damages, io_source.RefId, false);
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
        public virtual void Equip(BaseInteractiveObject target)
        {
            if (Io == null)
            {
                throw new RPGException(ErrorMessage.INTERNAL_ERROR, "Cannot equip item with no BaseInteractiveObject data");
            }
            if (target != null)
            {
                if (target.HasIOFlag(IoGlobals.IO_01_PC)
                        || target.HasIOFlag(IoGlobals.IO_03_NPC))
                {
                    IOCharacter charData;
                    if (target.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        charData = target.PcData;
                    }
                    else
                    {
                        charData = target.NpcData;
                    }
                    int validid = -1;
                    int i = Interactive.Instance.GetMaxIORefId();
                    for (; i >= 0; i--)
                    {
                        if (Interactive.Instance.HasIO(i)
                                && Interactive.Instance.GetIO(i) != null
                                && io.Equals(Interactive.Instance.GetIO(i)))
                        {
                            validid = i;
                            break;
                        }
                    }
                    if (validid >= 0)
                    {
                        Interactive.Instance.RemoveFromAllInventories(io);
                        io.Show = IoGlobals.SHOW_FLAG_ON_PLAYER; // on player
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
                            UnequipItemInSlot(charData, EquipmentGlobals.EQUIP_SLOT_TORSO);
                            // equip new armor
                            charData.SetEquippedItem(EquipmentGlobals.EQUIP_SLOT_TORSO, validid);
                        }
                        else if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_LEGGINGS))
                        {
                            // unequip old leggings
                            UnequipItemInSlot(charData, EquipmentGlobals.EQUIP_SLOT_LEGGINGS);
                            // equip new leggings
                            charData.SetEquippedItem(EquipmentGlobals.EQUIP_SLOT_LEGGINGS, validid);
                        }
                        else if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_HELMET))
                        {
                            // unequip old helmet
                            UnequipItemInSlot(charData, EquipmentGlobals.EQUIP_SLOT_HELMET);
                            // equip new helmet
                            charData.SetEquippedItem(EquipmentGlobals.EQUIP_SLOT_HELMET, validid);
                        }
                        if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_HELMET)
                                || io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_ARMOR)
                                || io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_LEGGINGS))
                        {
                            charData.RecreatePlayerMesh();
                        }
                        charData.ComputeFullStats();
                    }
                }
            }
        }
        public void ReleaseAll()
        {
            ReleaseEquipItem();
        }
        /** Releases the {@link IOEquipItem} data from the item. */
        public void ReleaseEquipItem()
        {
            if (Equipitem != null)
            {
                Equipitem = null;
            }
        }
        /// <summary>
        /// Sets the item's object type.
        /// </summary>
        /// <param name="flag">the type flag</param>
        /// <param name="added">if <tt>true</tt>, the type is set; otherwise it is removed</param>
        public void SetObjectType(int flag, bool added = true)
        {
            if (added)
            {
                io.AddTypeFlag(flag);
            }
            else
            {
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
        public void UnEquip(BaseInteractiveObject target, bool isDestroyed)
        {
            if (target != null)
            {
                if (target.HasIOFlag(IoGlobals.IO_01_PC))
                {
                    int i = ProjectConstants.Instance.GetMaxEquipped() - 1;
                    for (; i >= 0; i--)
                    {
                        IOPcData player = target.PcData;
                        int itemRefId = player.GetEquippedItem(i);
                        if (itemRefId >= 0
                                && Interactive.Instance.HasIO(itemRefId)
                                && Interactive.Instance.GetIO(
                                        itemRefId).Equals(io))
                        {
                            // EERIE_LINKEDOBJ_UnLinkObjectFromObject(
                            // target->obj, tounequip->obj);
                            player.ReleaseEquipment(itemRefId);
                            // target->bbox1.x = 9999;
                            // target->bbox2.x = -9999;

                            if (!isDestroyed)
                            {
                                // if (DRAGINTER == null) {
                                // ARX_SOUND_PlayInterface(SND_INVSTD);
                                // Set_DragInter(tounequip);
                                // } else
                                if (!target.Inventory.CanBePutInInventory(io))
                                {
                                    target.Inventory.PutInFrontOfPlayer(io, true);
                                }
                            }
                            // send event from this item to target to unequip
                            Script.Instance.EventSender = io;
                            Script.Instance.SendIOScriptEvent(target, ScriptConsts.SM_007_EQUIPOUT, null, null);
                            // send event from target to this item to unequip
                            Script.Instance.EventSender = target;
                            Script.Instance.SendIOScriptEvent(io, ScriptConsts.SM_007_EQUIPOUT, null, null);
                        }
                    }
                    if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_HELMET)
                            || io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_ARMOR)
                            || io.HasTypeFlag(
                                    EquipmentGlobals.OBJECT_TYPE_LEGGINGS))
                    {
                        target.PcData.RecreatePlayerMesh();
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
        private void EquipRing(IOCharacter charData)
        {
            // check left and right finger
            // to see if it can be equipped
            bool canEquip = true;
            int ioid = charData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_RING_LEFT);
            if (Interactive.Instance.HasIO(ioid))
            {
                BaseInteractiveObject oldRing = (BaseInteractiveObject)Interactive.Instance.GetIO(ioid);
                if (oldRing.ItemData.RingType == RingType)
                {
                    // already wearing that type
                    // of ring on left finger
                    canEquip = false;
                }
                oldRing = null;
            }
            if (canEquip)
            {
                ioid = charData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_RING_RIGHT);
                if (Interactive.Instance.HasIO(ioid))
                {
                    BaseInteractiveObject oldRing = (BaseInteractiveObject)Interactive.Instance.GetIO(ioid);
                    if (oldRing.ItemData.RingType == RingType)
                    {
                        // already wearing that type
                        // of ring on right finger
                        canEquip = false;
                    }
                    oldRing = null;
                }
            }
            if (canEquip)
            {
                int equipSlot = -1;
                if (charData.GetEquippedItem(
                        EquipmentGlobals.EQUIP_SLOT_RING_LEFT) < 0)
                {
                    equipSlot = EquipmentGlobals.EQUIP_SLOT_RING_LEFT;
                }
                if (charData.GetEquippedItem(
                        EquipmentGlobals.EQUIP_SLOT_RING_RIGHT) < 0)
                {
                    equipSlot = EquipmentGlobals.EQUIP_SLOT_RING_RIGHT;
                }
                if (equipSlot == -1)
                {
                    if (!charData.GetIo().Inventory.LeftRing)
                    {
                        ioid = charData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_RING_RIGHT);
                        if (Interactive.Instance.HasIO(ioid))
                        {
                            BaseInteractiveObject oldIO = (BaseInteractiveObject)Interactive.Instance.GetIO(ioid);
                            if (oldIO.HasIOFlag(IoGlobals.IO_02_ITEM))
                            {
                                UnequipItemInSlot(charData, EquipmentGlobals.EQUIP_SLOT_RING_RIGHT);
                            }
                            oldIO = null;
                        }
                        equipSlot = EquipmentGlobals.EQUIP_SLOT_RING_RIGHT;
                    }
                    else
                    {
                        ioid = charData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_RING_LEFT);
                        if (Interactive.Instance.HasIO(ioid))
                        {
                            BaseInteractiveObject oldIO = (BaseInteractiveObject)Interactive.Instance.GetIO(ioid);
                            if (oldIO.HasIOFlag(IoGlobals.IO_02_ITEM))
                            {
                                UnequipItemInSlot(charData, EquipmentGlobals.EQUIP_SLOT_RING_LEFT);
                            }
                            oldIO = null;
                        }
                        equipSlot = EquipmentGlobals.EQUIP_SLOT_RING_LEFT;
                    }
                    charData.GetIo().Inventory.LeftRing = !charData.GetIo().Inventory.LeftRing;
                }
                charData.SetEquippedItem(equipSlot, io.RefId);
            }
        }
        /**
         * Equips a shield on a character.
         * @param charData the character data
         * @ if an error occurs
         */
        private void EquipShield(IOCharacter charData)
        {
            // unequip old shield
            UnequipItemInSlot(charData, EquipmentGlobals.EQUIP_SLOT_SHIELD);
            // equip new shield
            charData.SetEquippedItem(EquipmentGlobals.EQUIP_SLOT_SHIELD, io.RefId);
            // TODO - attach new shield to mesh
            // EERIE_LINKEDOBJ_LinkObjectToObject(target->obj,
            // io->obj, "SHIELD_ATTACH", "SHIELD_ATTACH", io);
            int wpnID =
                    charData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
            if (wpnID >= 0)
            {
                if (Interactive.Instance.HasIO(wpnID))
                {
                    BaseInteractiveObject wpn = (BaseInteractiveObject)Interactive.Instance.GetIO(wpnID);
                    if (wpn.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_2H)
                            || wpn.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW))
                    {
                        // unequip old weapon
                        UnequipItemInSlot(charData, EquipmentGlobals.EQUIP_SLOT_WEAPON);
                    }
                }
            }
        }
        /**
         * Equips a weapon for a character.
         * @param charData the character data
         * @ if an error occurs
         */
        private void EquipWeapon(IOCharacter charData)
        {
            // unequip old weapon
            UnequipItemInSlot(charData, EquipmentGlobals.EQUIP_SLOT_WEAPON);
            // equip new weapon
            charData.SetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON, io.RefId);
            // attach it to player mesh
            if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW))
            {
                // EERIE_LINKEDOBJ_LinkObjectToObject(
                // target->obj, io->obj,
                // "WEAPON_ATTACH", "TEST", io);
            }
            else
            {
                // EERIE_LINKEDOBJ_LinkObjectToObject(
                // target->obj,
                // io->obj,
                // "WEAPON_ATTACH", "PRIMARY_ATTACH", io); //
            }
            if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_2H)
                    || io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW))
            {
                // for bows or 2-handed swords, unequip old shield
                UnequipItemInSlot(charData, EquipmentGlobals.EQUIP_SLOT_SHIELD);
            }
        }
        protected abstract float GetBackstabModifier();
        /**
         * Gets the type of weapon an item is.
         * @return {@link int}
         */
        public int GetWeaponType()
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
        private void UnequipItemInSlot(IOCharacter player, int slot)
        {
            if (player.GetEquippedItem(slot) >= 0)
            {
                int slotioid = player.GetEquippedItem(slot);
                if (Interactive.Instance.HasIO(slotioid))
                {
                    BaseInteractiveObject equipIO = (BaseInteractiveObject)Interactive.Instance.GetIO(slotioid);
                    if (equipIO.HasIOFlag(IoGlobals.IO_02_ITEM)
                            && equipIO.ItemData != null)
                    {
                        equipIO.ItemData.UnEquip(player.GetIo(), false);
                    }
                    equipIO = null;
                }
            }
        }
    }
}
