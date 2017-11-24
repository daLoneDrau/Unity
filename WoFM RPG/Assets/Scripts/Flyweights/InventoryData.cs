using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    class InventoryData
    {
        /** the IO associated with this {@link InventoryData}. */
        private IO io;
        /** the number of inventory slots. */
        private int numInventorySlots;
        /** the inventory slots. */
        private SLOT[] slots;
        /**
         * Sends messages between an item and its owner that it is now in inventory.
         * @param invOwnerIO the owner
         * @param itemIO the item
         * @ if an error occurs
         */
        public  void ARX_INVENTORY_Declare_InventoryIn(
                 IO invOwnerIO,
                 IO itemIO) 
        {
		if (itemIO != null) {
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
			Script.GetInstance().setEventSender(itemIO);
        Script.GetInstance().sendIOScriptEvent(invOwnerIO,
                ScriptConsts.SM_002_INVENTORYIN, new Object[0], null);
			// send event from inventory owner to item
			Script.GetInstance().setEventSender(invOwnerIO);
        Script.GetInstance().sendIOScriptEvent(itemIO,
                ScriptConsts.SM_002_INVENTORYIN, new Object[0], null);
			// clear global event sender
			Script.GetInstance().setEventSender(null);
    }
}
/**
 * Action when a player attempts to identify an item.
 * @param playerIO the player's {@link IO}
 * @param itemIO the itme's {@link IO}
 * @ if an error occurs
 */
public  void ARX_INVENTORY_IdentifyIO( IO playerIO,
         IO itemIO) 
{
		if (playerIO != null
				&& playerIO.HasIOFlag(IoGlobals.IO_01_PC)
				&& playerIO.getPCData() != null
				&& itemIO != null
				&& itemIO.HasIOFlag(IoGlobals.IO_02_ITEM)
				&& itemIO.getItemData() != null
				&& itemIO.getItemData().getEquipitem() != null) {
        if (playerIO.getPCData().canIdentifyEquipment(
                itemIO.getItemData().getEquipitem()))
        {
            Script.GetInstance().sendIOScriptEvent(
                    itemIO, ScriptConsts.SM_69_IDENTIFY, null, "");
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
public  bool CanBePutInInventory( IO itemIO)

            
{
    bool can = false;
		if (itemIO != null
				&& !itemIO.HasIOFlag(IoGlobals.IO_15_MOVABLE)) {
        if (itemIO.HasIOFlag(IoGlobals.IO_10_GOLD)
                && io.HasIOFlag(IoGlobals.IO_01_PC))
        {
            io.getPCData().adjustGold(itemIO.getItemData().getPrice());
            if (itemIO.isScriptLoaded())
            {
                Interactive.GetInstance().RemoveFromAllInventories(itemIO);
                Interactive.GetInstance().releaseIO(itemIO);
            }
            else
            {
                itemIO.setShow(IoGlobals.SHOW_FLAG_KILLED);
                itemIO.RemoveGameFlag(IoGlobals.GFLAG_ISINTREATZONE);
            }
            can = true;
        }
        if (!can)
        {
            // first try to stack
            for (int i = numInventorySlots - 1; i >= 0; i--)
            {
                IO slotIO = (IO)slots[i].getIo();
                if (slotIO != null
                        && slotIO.getItemData().getStackSize() > 1
                        && Interactive.GetInstance().isSameObject(itemIO,
                                slotIO))
                {
                    // found a matching item - try to stack
                    int slotCount = slotIO.getItemData().getCount();
                    int itemCount = itemIO.getItemData().getCount();
                    int slotMaxStack =
                            slotIO.getItemData().getStackSize();
                    if (slotCount < slotMaxStack)
                    {
                        // there's room to stack more - stack it
                        slotIO.getItemData().adjustCount(itemCount);
                        // check to see if too many are stacked
                        slotCount = slotIO.getItemData().getCount();
                        if (slotCount > slotMaxStack)
                        {
                            // remove excess from stack
                            // and put it back into item io
                            itemIO.getItemData().setCount(
                                    slotCount - slotMaxStack);
                            slotIO.getItemData().setCount(slotMaxStack);
                        }
                        else
                        {
                            // no excess. remove count from item io
                            itemIO.getItemData().setCount(0);
                        }
                        // was item count set to 0? release the IO
                        if (itemIO.getItemData().getCount() == 0)
                        {
                            if (itemIO.isScriptLoaded())
                            {
                                int inner = Interactive.GetInstance()
                                        .getMaxIORefId();
                                for (; inner >= 0; inner--)
                                {
                                    if (Interactive.GetInstance().hasIO(
                                            inner))
                                    {
                                        IO innerIO = (IO)
                                                Interactive.GetInstance()
                                                .getIO(inner);
                                        if (innerIO.Equals(itemIO))
                                        {
                                            Interactive.GetInstance()
                                            .releaseIO(innerIO);
                                            innerIO = null;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                itemIO.setShow(IoGlobals.SHOW_FLAG_KILLED);
                            }
                        }
                        // declare item in inventory
                        ARX_INVENTORY_Declare_InventoryIn(io, slotIO);
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
            for (int i = numInventorySlots - 1; i >= 0; i--)
            {
                // got an empty slot - add it
                if (slots[i].getIo() == null)
                {
                    slots[i].setIo(itemIO);
                    slots[i].setShow(true);
                    ARX_INVENTORY_Declare_InventoryIn(io, itemIO);
                    can = true;
                    break;
                }
            }
        }
    }
		return can;
}
/**
 * UNTESTED DO NOT USE Replaces an item in an IO's inventory.
 * @param itemIO the item being added
 * @param old the item being replaced
 * @ if an error occurs
 */
public  void CheckForInventoryReplaceMe( IO itemIO,  IO old)

            
{
		if (itemIO != null
				&& old != null) {
        bool handled = false;
        if (io.HasIOFlag(IoGlobals.IO_01_PC))
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
                IO io = (IO)Interactive.GetInstance().getIO(i);
                if (io != null
                        && io.getInventory() != null)
                {
                    InventoryData id = io.getInventory();
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
/** Removes all items from inventory. */
public  void CleanInventory()
{
    for (int i = numInventorySlots - 1; i >= 0; i--)
    {
        slots[i].setIo(null);
        slots[i].setShow(true);
    }
}
/**
 * Forces all items to be set at a specific level.
 * @param level the level
 */
public  void ForcePlayerInventoryObjectLevel( int level)
{
    for (int i = numInventorySlots - 1; i >= 0; i--)
    {
        if (slots[i].getIo() != null)
        {
            slots[i].getIo().setLevel(level);
        }
    }
}
/**
 * Gets the IO associated with this {@link InventoryData}.
 * @return {@link IO}
 */
public  IO getIo()
{
    return io;
}
/**
 * Gets the number of inventory slots.
 * @return <code>int</code>
 */
public  int getNumInventorySlots()
{
    return numInventorySlots;
}
/**
 * Gets the inventory slot at the specific index.
 * @param index the slot index
 * @return {@link SLOT}
 */
public  SLOT getSlot( int index)
{
    return slots[index];
}
/**
 * Determines if an item is in inventory.
 * @param io the item
 * @return <tt>true</tt> if the item is in inventory; <tt>false</tt>
 *         otherwise
 */
public  bool IsInPlayerInventory( IO io)
{
    bool is = false;
    for (int i = numInventorySlots - 1; i >= 0; i--)
    {
        IO ioo = (IO)slots[i].getIo();
        if (ioo != null
                && ioo.Equals(io))
        {

                is = true;
            break;
        }
    }
    return is;
}
/**
 * Puts an item in front of the player.
 * @param itemIO the item
 * @param doNotApplyPhysics if <tt>true</tt>, do not apply physics
 */
public abstract void PutInFrontOfPlayer(IO itemIO,
        bool doNotApplyPhysics);
/**
 * Replaces an item in all inventories.
 * @param oldItemIO the old item being replaced
 * @param newItemIO the new item
 * @ if an error occurs
 */
public  void ReplaceInAllInventories( IO oldItemIO,
         IO newItemIO)

                    
{
		if (oldItemIO != null
				&& !oldItemIO.HasIOFlag(IoGlobals.IO_15_MOVABLE)
				&& newItemIO != null
						&& !newItemIO.HasIOFlag(IoGlobals.IO_15_MOVABLE)) {
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
            IO invOwner = (IO)Interactive.GetInstance().getIO(i);
            if (invOwner.getInventory() != null)
            {
                InventoryData inv = invOwner.getInventory();
                for (int j = inv.numInventorySlots - 1; j >= 0; j--)
                {
                    if (inv.slots[j].getIo().Equals(oldItemIO))
                    {
                        inv.slots[j].setIo(newItemIO);
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
public  void SendInventoryObjectCommand( String itemName,
         int message) 
{
		if (itemName != null
				&& itemName.Length() > 0) {
        for (int i = numInventorySlots - 1; i >= 0; i--)
        {
            IO slotIO = (IO)slots[i].getIo();
            if (slotIO != null
                    && slotIO.HasGameFlag(IoGlobals.GFLAG_INTERACTIVITY)
                    && slotIO.getItemData() != null)
            {
                String ioName =
                        new String(slotIO.getItemData().getItemName());
                if (itemName.equalsIgnoreCase(ioName))
                {
                    Script.GetInstance().sendIOScriptEvent(
                            slotIO, message, null, "");
                    slotIO = null;
                    break;
                }
                ioName = null;
            }
            slotIO = null;
        }
    }
}
/**
 * Sets the IO associated with the inventory.
 * @param newIO the IO to set
 */
public  void setIo( IO newIO)
{
    this.io = newIO;
}
/**
 * Sets the inventory slots.
 * @param val the inventory slots
 */
protected  void setSlots( SLOT[] val)
{
    slots = val;
    numInventorySlots = slots.Length;
}
/** flag indicating the left ring needs to be replaced. */
private bool leftRing = false;
/**
 * Sets the value of the flag indicating whether the left ring is the next
 * one that needs to be switched.
 * @param flag the new value to set
 */
public void setLeftRing( bool flag)
{
    this.leftRing = flag;
}
/**
 * Gets the flag indicating whether the left ring is the next one that needs
 * to be switched.
 * @return <tt>true</tt> if the left ring should be switched; <tt>false</tt>
 * otherwise
 */
public bool isLeftRing()
{
    return leftRing;
}
    }
}
