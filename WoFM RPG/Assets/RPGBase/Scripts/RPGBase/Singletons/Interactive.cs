using RPGBase.Constants;
using RPGBase.Flyweights;
using System;
using UnityEngine;

namespace RPGBase.Singletons
{
    public class Interactive : MonoBehaviour
    {
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static Interactive Instance { get; protected set; }
        private bool FAST_RELEASE;
        /// <summary>
        /// Adds an interactive object to the scene.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public BaseInteractiveObject AddItem(string item, long flags)
        {
            BaseInteractiveObject io = null;
            // TODO - fix this
            // BaseInteractiveObject io = WebServiceClient.Instance.GetItemByName(item);
            if (io != null)
            {
                // add additional flags, such as GOLD or MOVABLE
                if ((flags & IoGlobals.NO_ON_LOAD) != IoGlobals.NO_ON_LOAD)
                {
                    Script.Instance.SendIOScriptEvent(io, ScriptConsts.SM_041_LOAD, null, "");
                }
                // TODO - remove spellcasting data
                // io->spellcast_data.castingspell = -1;
                // TODO -set texture and position
            }
            return io;
        }
        /// <summary>
        /// Destroys dynamic info for an interactive object.
        /// </summary>
        /// <param name="io">the BaseInteractiveObject</param>
        public void DestroyDynamicInfo(BaseInteractiveObject io)
        {
            if (io != null)
            {
                int n = GetInterNum(io);
                ForceIOLeaveZone(io, 0);
                BaseInteractiveObject[] objs = GetIOs();
                for (int i = objs.Length - 1; i >= 0; i--)
                {
                    BaseInteractiveObject pio = objs[i];
                    if (pio != null
                            && pio.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        bool found = false;
                        IOPcData player = pio.PcData;
                        // check to see if player was equipped with the item
                        int j = ProjectConstants.Instance.GetMaxEquipped() - 1;
                        for (; j >= 0; j--)
                        {
                            if (player.GetEquippedItem(j) == n
                                    && Interactive.Instance.HasIO(n))
                            {
                                // have player unequip
                                io.ItemData.UnEquip(pio, true);
                                player.SetEquippedItem(j, -1);
                                found = true;
                                break;
                            }
                        }
                        player = null;
                        if (found)
                        {
                            break;
                        }
                    }
                }

                Script.Instance.EventStackClearForIo(io);

                if (Interactive.Instance.HasIO(n))
                {
                    int i = ProjectConstants.Instance.GetMaxSpells();
                    for (; i >= 0; i--)
                    {
                        Spell spell = SpellController.Instance.GetSpell(i);
                        if (spell != null)
                        {
                            if (spell.Exists
                                    && spell.Caster == n)
                            {
                                spell.TimeToLive = 0;
                                spell.TurnsToLive = 0;
                            }
                        }
                    }
                }
            }
        }
        public void DestroyIO(BaseInteractiveObject io)
        {
            if (io != null
                    && io.Show != IoGlobals.SHOW_FLAG_DESTROYED)
            {
                ForceIOLeaveZone(io, 0);
                // if interactive object was being dragged
                // if (DRAGINTER == ioo) {
                // set drag object to null
                // Set_DragInter(NULL);
                // }

                // if interactive object was being hovered by mouse
                // if (FlyingOverIO == ioo) {
                // set hovered object to null
                // FlyingOverIO = NULL;
                // }

                // if interactive object was being combined
                // if (COMBINE == ioo) {
                // set combined object to null
                // COMBINE = NULL;
                // }
                if (io.HasIOFlag(IoGlobals.IO_02_ITEM)
                        && io.ItemData.Count > 1)
                {
                    io.ItemData.AdjustCount(-1);
                }
                else
                {
                    // Kill all spells
                    int numm = GetInterNum(io);

                    if (HasIO(numm))
                    {
                        // kill all spells from caster
                        // ARX_SPELLS_FizzleAllSpellsFromCaster(numm);
                    }

                    // Need To Kill timers
                    Script.Instance.TimerClearByIO(io);
                    io.Show = IoGlobals.SHOW_FLAG_DESTROYED;
                    io.RemoveGameFlag(IoGlobals.GFLAG_ISINTREATZONE);

                    if (!FAST_RELEASE)
                    {
                        RemoveFromAllInventories(io);
                    }
                    // unlink from any other IOs
                    // if (ioo->obj) {
                    // EERIE_3DOBJ * eobj = ioo->obj;
                    // while (eobj->nblinked) {
                    // long k = 0;
                    // if ((eobj->linked[k].lgroup != -1)
                    // && eobj->linked[k].obj) {
                    // INTERACTIVE_OBJ * iooo =
                    // (INTERACTIVE_OBJ *)eobj->linked[k].io;

                    // if ((iooo) && ValidIOAddress(iooo)) {
                    // EERIE_LINKEDOBJ_UnLinkObjectFromObject(
                    // ioo->obj, iooo->obj);
                    // ARX_INTERACTIVE_DestroyIO(iooo);
                    // }
                    // }
                    // }
                    // }

                    DestroyDynamicInfo(io);

                    if (io.ScriptLoaded)
                    {
                        int num = GetInterNum(io);
                        ReleaseIO(io);

                        if (HasIO(num))
                        {
                            GetIOs()[num] = null;
                        }
                    }
                }
                print("making call to destroy io now");
                // detach the IO from its parent
                io.transform.parent = null;
                Destroy(io);
            }
        }
        public virtual void ForceIOLeaveZone(BaseInteractiveObject io, long flags) { throw new NotImplementedException(); }
        public int GetInterNum(BaseInteractiveObject io)
        {
            int num = -1;
            if (io != null)
            {
                BaseInteractiveObject[] objs = GetIOs();
                for (int i = objs.Length - 1; i >= 0; i--)
                {
                    if (objs[i].Equals(io))
                    {
                        num = i;
                        break;
                    }
                }
                objs = null;
            }
            return num;
        }
        /// <summary>
        /// Gets a <see cref="BaseInteractiveObject"/> by its reference id.
        /// </summary>
        /// <param name="id">the reference id</param>
        /// <returns><see cref="BaseInteractiveObject"/></returns>
        public BaseInteractiveObject GetIO(int id)
        {
            BaseInteractiveObject io = null;
            if (HasIO(id))
            {
                BaseInteractiveObject[] objs = GetIOs();
                for (int i = objs.Length - 1; i >= 0; i--)
                {
                    if (objs[i] != null
                            && objs[i].RefId == id)
                    {
                        io = objs[i];
                        break;
                    }
                }
                objs = null;
            }
            else
            {
                throw new RPGException(ErrorMessage.INTERNAL_BAD_ARGUMENT, "BaseInteractiveObject does not exist");
            }
            return io;
        }
        /// <summary>
        /// Gets the internal list of IOs.
        /// </summary>
        /// <returns><see cref="BaseInteractiveObject"/>[]</returns>
        protected virtual BaseInteractiveObject[] GetIOs() { throw new NotImplementedException(); }
        /// <summary>
        /// Gets the largest reference Id in use.
        /// </summary>
        /// <returns>int</returns>
        public virtual int GetMaxIORefId() { throw new NotImplementedException(); }
        /// <summary>
        /// Initializes a new interactive object.
        /// </summary>
        protected virtual void NewIO(BaseInteractiveObject io) { throw new NotImplementedException(); }
        /// <summary>
        /// Gets an BaseInteractiveObject's reference id by name.  If the target is "none" or does not exist, -1 is returned.If the target is "self" or "me", -2;
        /// </summary>
        /// <param name="name">the BaseInteractiveObject's name</param>
        /// <returns>int</returns>
        public int GetTargetByNameTarget(string name)
        {
            int ioid = -1;
            if (name != null)
            {
                if (string.Equals(name, "self", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(name, "me", StringComparison.OrdinalIgnoreCase))
                {
                    ioid = -2;
                }
                else if (string.Equals(name, "player", StringComparison.OrdinalIgnoreCase))
                {
                    ioid = ProjectConstants.Instance.GetPlayer();
                }
                else
                {
                    BaseInteractiveObject[] ios = this.GetIOs();
                    for (int i = ios.Length - 1; i >= 0; i--)
                    {
                        BaseInteractiveObject io = ios[i];
                        if (io.HasIOFlag(IoGlobals.IO_03_NPC))
                        {
                            if (string.Equals(name, io.NpcData.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                ioid = io.RefId;
                                break;
                            }
                        }
                        else if (io.HasIOFlag(IoGlobals.IO_02_ITEM))
                        {
                            if (string.Equals(name, io.ItemData.ItemName, StringComparison.OrdinalIgnoreCase))
                            {
                                ioid = io.RefId;
                                break;
                            }
                        }
                        io = null;
                    }
                    ios = null;
                }
            }
            return ioid;
        }
        /// <summary>
        /// Determines if the {@link Interactive} has an interactive object by a specific id.
        /// </summary>
        /// <param name="id">the id</param>
        /// <returns>true if an interactive object by that id has been stored already; false otherwise</returns>
        public bool HasIO(int id)
        {
            bool has = false;
            BaseInteractiveObject[] objs = GetIOs();
            for (int i = objs.Length - 1; i >= 0; i--)
            {
                if (objs[i] != null
                        && id == objs[i].RefId)
                {
                    has = true;
                    break;
                }
            }
            objs = null;
            return has;
        }
        /**
         * Determines if the {@link Interactive} has a specific interactive object.
         * @param io the BaseInteractiveObject
         * @return true if that interactive object has been stored already; false
         *         otherwise
         */
        public bool HasIO(BaseInteractiveObject io)
        {
            bool has = false;
            if (io != null)
            {
                BaseInteractiveObject[] objs = GetIOs();
                for (int i = objs.Length - 1; i >= 0; i--)
                {
                    if (objs[i] != null
                            && io.RefId == objs[i].RefId
                            && io.Equals(objs[i]))
                    {
                        has = true;
                        break;
                    }
                }
                objs = null;
            }
            return has;
        }
        /**
         * Determines if two separate IOs represent the same object. Two objects are
         * considered the same if they are both non-unique items that have the same
         * name. PCs and NPCs will always return <tt>false</tt> when compared.
         * @param io0 the first BaseInteractiveObject
         * @param io1 the second BaseInteractiveObject
         * @return <tt>true</tt> if the IOs represent the same object;
         *         <tt>false</tt> otherwise
         */
        public bool IsSameObject(BaseInteractiveObject io0, BaseInteractiveObject io1)
        {
            bool same = false;
            if (io0 != null
                    && io1 != null)
            {
                if (!io0.HasIOFlag(IoGlobals.IO_13_UNIQUE)
                        && !io1.HasIOFlag(IoGlobals.IO_13_UNIQUE))
                {
                    if (io0.HasIOFlag(IoGlobals.IO_02_ITEM)
                            && io1.HasIOFlag(IoGlobals.IO_02_ITEM)
                            && io0.Overscript == null
                            && io1.Overscript == null
                            && string.Equals(io0.ItemData.ItemName, io1.ItemData.ItemName, StringComparison.OrdinalIgnoreCase))
                    {
                        same = true;
                    }
                }
            }
            return same;
        }
        /**
         * Sets the weapon on an NPC.
         * @param io the BaseInteractiveObject
         * @param temp the temp object
         * @
         */
        public void PrepareSetWeapon(BaseInteractiveObject io, string temp)

        {
            if (io != null
                    && io.HasIOFlag(IoGlobals.IO_03_NPC))
            {
                if (io.NpcData.Weapon != null)
                {
                    BaseInteractiveObject oldWpnIO = (BaseInteractiveObject)io.NpcData.Weapon;
                    // unlink the weapon from the NPC BaseInteractiveObject
                    // EERIE_LINKEDOBJ_UnLinkObjectFromObject(io->obj, ioo->obj);
                    io.NpcData.Weapon = null;
                    ReleaseIO(oldWpnIO);
                    oldWpnIO = null;
                }
                // load BaseInteractiveObject from DB
                BaseInteractiveObject wpnIO = AddItem(temp, IoGlobals.IO_IMMEDIATELOAD);
                if (wpnIO != null)
                {
                    io.NpcData.Weapon = wpnIO;
                    io.Show = IoGlobals.SHOW_FLAG_LINKED;
                    wpnIO.ScriptLoaded = true;
                    // TODO - link item to io
                    // SetWeapon_Back(io);
                }
            }
        }
        /**
         * Releases the BaseInteractiveObject and all resources.
         * @param ioid the BaseInteractiveObject's id
         * @ if an error occurs
         */
        public void ReleaseIO(int ioid)
        {
            if (!HasIO(ioid))
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Invalid BaseInteractiveObject id " + ioid);
            }
            ReleaseIO(GetIO(ioid));
        }
        /**
         * Releases the BaseInteractiveObject and all resources.
         * @param io the BaseInteractiveObject
         */
        public void ReleaseIO(BaseInteractiveObject io)
        {
            if (io != null)
            {
                if (io.Inventory != null)
                {
                    InventoryData inventory = io.Inventory;
                    if (inventory != null)
                    {
                        for (int j = 0; j < inventory.GetNumInventorySlots(); j++)
                        {
                            if (io.Equals(inventory.Slots[j].Io))
                            {
                                inventory.Slots[j].Io = null;
                                inventory.Slots[j].Show = true;
                            }
                        }
                    }
                }
                // release script timers and spells
                // release from groups
                //
                Script.Instance.TimerClearByIO(io);
                // MagicRealmSpells.Instance.removeAllSpellsOn(io);
                Script.Instance.ReleaseScript(io.Script);
                Script.Instance.ReleaseScript(io.Overscript);
                Script.Instance.ReleaseAllGroups(io);
                int id = io.RefId;
                int index = -1;
                BaseInteractiveObject[] objs = GetIOs();
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i] != null
                            && id == objs[i].RefId)
                    {
                        index = i;
                        break;
                    }
                }
                if (index > -1)
                {
                    objs[index] = null;
                }
                objs = null;
                objs = GetIOs();
            }
        }
        /**
         * Removes an item from all available inventories.
         * @param itemIO the item
         * @ if an error occurs
         */
        public void RemoveFromAllInventories(BaseInteractiveObject itemIO)
        {
            if (itemIO != null)
            {
                int i = Interactive.Instance.GetMaxIORefId();
                for (; i >= 0; i--)
                {
                    if (Interactive.Instance.HasIO(i))
                    {
                        BaseInteractiveObject invIo = (BaseInteractiveObject)Interactive.Instance.GetIO(i);
                        InventoryData inventoryData;
                        if (invIo.Inventory != null)
                        {
                            inventoryData = invIo.Inventory;
                            for (int j = inventoryData.GetNumInventorySlots() - 1; j >= 0; j--)
                            {
                                InventorySlot slot = inventoryData.Slots[j];
                                if (slot.Io != null
                                        && slot.Io.Equals(itemIO))
                                {
                                    slot.Io = null;
                                    slot.Show = true;
                                }
                            }
                        }
                        invIo = null;
                        inventoryData = null;
                    }
                }
            }
        }
        public void TeleportBehindTarget(BaseInteractiveObject io)
        {
            // TODO Auto-generated method stub

        }
        // TODO Auto-generated method stub
        /*
        public void Teleport(BaseInteractiveObject io, SimpleVector2 position)
        {
            // TODO Auto-generated method stub

        }
        */
        /**
         * Determines an BaseInteractiveObject's world position and sets the location to a
         *  {@link SimpleVector2} parameter.
         * @param io the BaseInteractiveObject
         * @param pos the parameter
         * @return <tt>true</tt> if the item has a position; <tt>false</tt>
         * otherwise
         * @ if an error occurs
         *//*
        public bool GetItemWorldPosition(BaseInteractiveObject io, SimpleVector2 pos)
        {
            bool hasPosition = false;
            if (io != null)
            {
                if (io.Show != IoGlobals.SHOW_FLAG_IN_SCENE)
                {
                    // TODO if item is being cursor dragged, set its location to
                    // player's

                    BaseInteractiveObject[] ios = this.GetIOs();
                    bool found = false;
                    for (int i = ios.Length - 1; i >= 0; i--)
                    {
                        BaseInteractiveObject iio = ios[i];
                        if (iio == null)
                        {
                            continue;
                        }
                        if (iio.HasIOFlag(IoGlobals.IO_03_NPC))
                        {
                            if (iio.Equals(io))
                            {
                                // teleporting to NPC io
                                pos.set(iio.getPosition());
                                found = true;
                                hasPosition = true;
                                break;
                            }
                            // check to see if NPC has BaseInteractiveObject in inventory
                            if (iio.Inventory.IsInPlayerInventory(io))
                            {
                                // teleporting to NPC io
                                pos.set(iio.getPosition());
                                found = true;
                                hasPosition = true;
                                break;
                            }
                        }
                        else if (iio.HasIOFlag(IoGlobals.IO_01_PC))
                        {
                            if (iio.Equals(io))
                            {
                                // teleporting to PC io
                                pos.set(iio.getPosition());
                                found = true;
                                hasPosition = true;
                                break;
                            }
                            // check to see if PC has BaseInteractiveObject in inventory
                            if (iio.Inventory.IsInPlayerInventory(io))
                            {
                                // teleporting to PC io
                                pos.set(iio.getPosition());
                                found = true;
                                hasPosition = true;
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        // not found as NPC, PC, or item in inventory
                        for (int i = ios.Length - 1; i >= 0; i--)
                        {
                            BaseInteractiveObject iio = ios[i];
                            if (iio.Equals(io))
                            {
                                pos.set(iio.getPosition());
                                hasPosition = true;
                                break;
                            }
                        }
                    }
                }
            }
            return hasPosition;
        }
        */
    }
}