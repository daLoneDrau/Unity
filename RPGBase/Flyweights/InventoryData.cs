﻿using System;
using RPGBase.Constants;
using RPGBase.Singletons;

namespace RPGBase.Flyweights
{
    public abstract class InventoryData
    {
        /// <summary>
        /// the <see cref="BaseInteractiveObject"/> associated with this <see cref="InventoryData"/>.
        /// </summary>
        public BaseInteractiveObject Io { get; set; }
        /// <summary>
        /// flag indicating the left ring needs to be replaced.
        /// </summary>
        private bool LeftRing { get; set; } = false;
        /// <summary>
        /// the inventory Slots.
        /// </summary>
        private InventorySlot[] Slots { get; set; }
        /**
         * Sends messages between an item and its owner that it is now in inventory.
         * @param invOwnerIO the owner
         * @param itemIO the item
         * @ if an error occurs
         */
        public void DeclareInInventory(BaseInteractiveObject invOwnerIO, BaseInteractiveObject itemIO)
        {
            if (itemIO != null)
            {
                // TODO - handle ignition
                // if (io->ignition > 0) {
                // if (ValidDynLight(io->ignit_light))
                // DynLight[io->ignit_light].exist = 0;

                // io->ignit_light = -1;

                // if (io->ignit_sound != ARX_SOUND_INVALID_RESOURCE) {
                // ARX_SOUND_Stop(io->ignit_sound);
                // io->ignit_sound = ARX_SOUND_INVALID_RESOURCE;
                // }

                // io->ignition = 0;
                // }

                // send event from item to inventory owner
                Script.GetInstance().SetEventSender(itemIO);
                Script.GetInstance().SendIOScriptEvent(invOwnerIO, ScriptConsts.SM_002_INVENTORYIN, new Object[0], null);
                // send event from inventory owner to item
                Script.GetInstance().SetEventSender(invOwnerIO);
                Script.GetInstance().SendIOScriptEvent(itemIO, ScriptConsts.SM_002_INVENTORYIN, new Object[0], null);
                // clear global event sender
                Script.GetInstance().SetEventSender(null);
            }
        }
        /**
         * Action when a player attempts to identify an item.
         * @param playerIO the player's {@link BaseInteractiveObject}
         * @param itemIO the itme's {@link BaseInteractiveObject}
         * @ if an error occurs
         */
        public void IdentifyIO(BaseInteractiveObject playerIO, BaseInteractiveObject itemIO)
        {
            if (playerIO != null
                    && playerIO.HasIOFlag(IoGlobals.IO_01_PC)
                    && playerIO.PcData != null
                    && itemIO != null
                    && itemIO.HasIOFlag(IoGlobals.IO_02_ITEM)
                    && itemIO.ItemData != null
                    && itemIO.ItemData.getEquipitem() != null)
            {
                if (playerIO.PcData.CanIdentifyEquipment(itemIO.ItemData.getEquipitem()))
                {
                    Script.GetInstance().SendIOScriptEvent(itemIO, ScriptConsts.SM_69_IDENTIFY, null, "");
                }
            }
        }
        /**
         * Determines if an item can be put in inventory.
         * @param itemIO the item
         * @return <tt>true</tt> if the item can be put in inventory; <tt>false</tt>
         *         otherwise
         * @ if an error occurs
         */
        public bool CanBePutInInventory(BaseInteractiveObject itemIO)
        {
            bool can = false;
            if (itemIO != null
                    && !itemIO.HasIOFlag(IoGlobals.IO_15_MOVABLE))
            {
                if (itemIO.HasIOFlag(IoGlobals.IO_10_GOLD)
                        && Io.HasIOFlag(IoGlobals.IO_01_PC))
                {
                    Io.PcData.AdjustGold(itemIO.ItemData.getPrice());
                    if (itemIO.ScriptLoaded)
                    {
                        Interactive.GetInstance().RemoveFromAllInventories(itemIO);
                        Interactive.GetInstance().releaseIO(itemIO);
                    }
                    else
                    {
                        itemIO.Show = IoGlobals.SHOW_FLAG_KILLED;
                        itemIO.RemoveGameFlag(IoGlobals.GFLAG_ISINTREATZONE);
                    }
                    can = true;
                }
                if (!can)
                {
                    // first try to stack
                    for (int i = Slots.Length - 1; i >= 0; i--)
                    {
                        BaseInteractiveObject slotIO = (BaseInteractiveObject)Slots[i].Io;
                        if (slotIO != null
                                && slotIO.ItemData.getStackSize() > 1
                                && Interactive.GetInstance().isSameObject(itemIO, slotIO))
                        {
                            // found a matching item - try to stack
                            int slotCount = slotIO.ItemData.getCount();
                            int itemCount = itemIO.ItemData.getCount();
                            int slotMaxStack = slotIO.ItemData.getStackSize();
                            if (slotCount < slotMaxStack)
                            {
                                // there's room to stack more - stack it
                                slotIO.ItemData.adjustCount(itemCount);
                                // check to see if too many are stacked
                                slotCount = slotIO.ItemData.getCount();
                                if (slotCount > slotMaxStack)
                                {
                                    // remove excess from stack
                                    // and put it back into item io
                                    itemIO.ItemData.setCount(slotCount - slotMaxStack);
                                    slotIO.ItemData.setCount(slotMaxStack);
                                }
                                else
                                {
                                    // no excess. remove count from item io
                                    itemIO.ItemData.setCount(0);
                                }
                                // was item count set to 0? release the BaseInteractiveObject
                                if (itemIO.ItemData.getCount() == 0)
                                {
                                    if (itemIO.ScriptLoaded)
                                    {
                                        int inner = Interactive.GetInstance().getMaxIORefId();
                                        for (; inner >= 0; inner--)
                                        {
                                            if (Interactive.GetInstance().hasIO(inner))
                                            {
                                                BaseInteractiveObject innerIO = Interactive.GetInstance().getIO(inner);
                                                if (innerIO.Equals(itemIO))
                                                {
                                                    Interactive.GetInstance().releaseIO(innerIO);
                                                    innerIO = null;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        itemIO.Show = IoGlobals.SHOW_FLAG_KILLED;
                                    }
                                }
                                // declare item in inventory
                                DeclareInInventory(Io, slotIO);
                                can = true;
                                break;
                            }
                        }
                    }
                }
                // cant stack the item? find an empty slot
                if (!can)
                {
                    // find an empty slot for the item
                    for (int i = Slots.Length - 1; i >= 0; i--)
                    {
                        // got an empty slot - add it
                        if (Slots[i].Io == null)
                        {
                            Slots[i].Io = itemIO;
                            Slots[i].Show = true;
                            DeclareInInventory(Io, itemIO);
                            can = true;
                            break;
                        }
                    }
                }
            }
            return can;
        }
        /**
         * UNTESTED DO NOT USE Replaces an item in an BaseInteractiveObject's inventory.
         * @param itemIO the item being added
         * @param old the item being replaced
         * @ if an error occurs
         */
        public void CheckForInventoryReplaceMe(BaseInteractiveObject itemIO, BaseInteractiveObject old)
        {
            if (itemIO != null
                    && old != null)
            {
                bool handled = false;
                if (Io.HasIOFlag(IoGlobals.IO_01_PC))
                {
                    if (IsInPlayerInventory(old))
                    {
                        if (CanBePutInInventory(itemIO))
                        {
                            handled = true;
                        }
                        else
                        {
                            PutInFrontOfPlayer(itemIO, true);
                            handled = true;
                        }
                    }
                }
                if (!handled)
                {
                    int i = Interactive.GetInstance().getMaxIORefId();
                    for (; i >= 0; i--)
                    {
                        BaseInteractiveObject io = (BaseInteractiveObject)Interactive.GetInstance().getIO(i);
                        if (io != null
                                && io.Inventory != null)
                        {
                            InventoryData id = io.Inventory;
                            if (id.IsInPlayerInventory(old))
                            {
                                if (CanBePutInInventory(itemIO))
                                {
                                    handled = true;
                                    break;
                                }
                                else
                                {
                                    this.PutInFrontOfPlayer(itemIO, true);
                                    handled = true;
                                    break;
                                }
                            }
                            id = null;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Removes all items from inventory.
        /// </summary>
        public void CleanInventory()
        {
            for (int i = Slots.Length - 1; i >= 0; i--)
            {
                Slots[i].Io = null;
                Slots[i].Show = true;
            }
        }
        /**
         * Forces all items to be set at a specific level.
         * @param level the level
         */
        public void ForcePlayerInventoryObjectLevel(int level)
        {
            for (int i = Slots.Length - 1; i >= 0; i--)
            {
                if (Slots[i].Io != null)
                {
                    Slots[i].Io.Level = level;
                }
            }
        }
        /**
         * Gets the number of inventory Slots.
         * @return <code>int</code>
         */
        public int GetNumInventorySlots()
        {
            return Slots.Length;
        }
        /**
         * Determines if an item is in inventory.
         * @param io the item
         * @return <tt>true</tt> if the item is in inventory; <tt>false</tt>
         *         otherwise
         */
        public bool IsInPlayerInventory(BaseInteractiveObject io)
        {
            bool f = false;
            for (int i = Slots.Length - 1; i >= 0; i--)
            {
                BaseInteractiveObject ioo = (BaseInteractiveObject)Slots[i].Io;
                if (ioo != null
                        && ioo.Equals(io))
                {
                    f = true;
                    break;
                }
            }
            return f;
        }
        /**
         * Puts an item in front of the player.
         * @param itemIO the item
         * @param doNotApplyPhysics if <tt>true</tt>, do not apply physics
         */
        public abstract void PutInFrontOfPlayer(BaseInteractiveObject itemIO, bool doNotApplyPhysics);
        /**
         * Replaces an item in all inventories.
         * @param oldItemIO the old item being replaced
         * @param newItemIO the new item
         * @ if an error occurs
         */
        public void ReplaceInAllInventories(BaseInteractiveObject oldItemIO, BaseInteractiveObject newItemIO)
        {
            if (oldItemIO != null
                    && !oldItemIO.HasIOFlag(IoGlobals.IO_15_MOVABLE)
                    && newItemIO != null
                    && !newItemIO.HasIOFlag(IoGlobals.IO_15_MOVABLE))
            {
                int oldIORefId = Interactive.GetInstance().GetInterNum(oldItemIO);
                int newIORefId = Interactive.GetInstance().GetInterNum(newItemIO);
                int i = Interactive.GetInstance().getMaxIORefId();
                for (; i >= 0; i--)
                {
                    if (i == oldIORefId
                            || i == newIORefId
                            || !Interactive.GetInstance().hasIO(i))
                    {
                        continue;
                    }
                    BaseInteractiveObject invOwner = (BaseInteractiveObject)Interactive.GetInstance().getIO(i);
                    if (invOwner.Inventory != null)
                    {
                        InventoryData inv = invOwner.Inventory;
                        for (int j = inv.Slots.Length - 1; j >= 0; j--)
                        {
                            if (inv.Slots[j].Io.Equals(oldItemIO))
                            {
                                inv.Slots[j].Io = newItemIO;
                            }
                        }
                    }
                }
            }
        }
        /**
         * Sends a scripted command to an item in inventory.
         * @param itemName the item name
         * @param message the message
         * @ if an error occurs
         */
        public void SendInventoryObjectCommand(String itemName,
                 int message)
        {
            if (itemName != null
                    && itemName.Length > 0)
            {
                for (int i = Slots.Length - 1; i >= 0; i--)
                {
                    BaseInteractiveObject slotIO = (BaseInteractiveObject)Slots[i].Io;
                    if (slotIO != null
                            && slotIO.HasGameFlag(IoGlobals.GFLAG_INTERACTIVITY)
                            && slotIO.ItemData != null)
                    {
                        String ioName =
                                new String(slotIO.ItemData.getItemName());
                        if (string.Equals(itemName, ioName, StringComparison.OrdinalIgnoreCase))
                        {
                            Script.GetInstance().SendIOScriptEvent(slotIO, message, null, "");
                            slotIO = null;
                            break;
                        }
                        ioName = null;
                    }
                    slotIO = null;
                }
            }
        }
    }
}
