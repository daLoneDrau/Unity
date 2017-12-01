using RPGBase.Flyweights;

namespace RPGBase.Singletons
{
    public class Interactive
    {
        /// <summary>
        /// the singleton instance.
        /// </summary>
        private static Interactive instance;
        /// <summary>
        /// Gets the one and only instance of <see cref="Interactive"/>.
        /// </summary>
        /// <returns><see cref="Interactive"/></returns>
        public static Interactive GetInstance()
        {
            if (Interactive.instance == null)
            {
                Interactive.instance = new Interactive();
            }
            return Interactive.instance;
        }
        private bool FAST_RELEASE;
        /**
         * Adds an interactive object to the scene.
         * @param item
         * @param flags
         * @return
         * @throws RPGException
         */
        public BaseInteractiveObject AddItem(string item, long flags)
        {
            FFInteractiveObject io = ((FFWebServiceClient)
                WebServiceClient.getInstance()).getItemByName(item);
            if (io != null)
            {
                // add additional flags, such as GOLD or MOVABLE
                if ((flags & IoGlobals.NO_ON_LOAD) != IoGlobals.NO_ON_LOAD)
                {
                    Script.getInstance().sendIOScriptEvent(io, ScriptConsts.SM_41_LOAD, null, "");
                }
                // TODO - remove spellcasting data
                // io->spellcast_data.castingspell = -1;
                // TODO -set texture and position
            }
            return io;
        }
        /**
         * Destroys dynamic info for an interactive object.
         * @param io the BaseInteractiveObject
         * @throws PooledException if an error occurs
         * @throws RPGException if an error occurs
         */
        public void ARX_INTERACTIVE_DestroyDynamicInfo(BaseInteractiveObject io)
            throws RPGException
        {
        if (io != null) {
            int n = GetInterNum(io);
            ARX_INTERACTIVE_ForceIOLeaveZone(io, 0);
        BaseInteractiveObject[] objs = getIOs();
            for (int i = objs.length - 1; i >= 0; i--) {
                BaseInteractiveObject pio = objs[i];
                if (pio != null
                        && pio.hasIOFlag(IoGlobals.IO_01_PC)) {
                    boolean found = false;
        IoPcData player = pio.getPCData();
        // check to see if player was equipped with the item
        int j = ProjectConstants.getInstance().getMaxEquipped() - 1;
                    for (; j >= 0; j--) {
                        if (player.getEquippedItem(j) == n
                                && Interactive.getInstance().hasIO(n)) {
                            // have player unequip
                            io.getItemData().ARX_EQUIPMENT_UnEquip(pio, true);
        player.setEquippedItem(j, -1);
                            found = true;
                            break;
                        }
}
player = null;
                    if (found) {
                        break;
                    }
                }
            }

            Script.getInstance().eventStackClearForIo(io);

            if (Interactive.getInstance().hasIO(n)) {
                int i = ProjectConstants.getInstance().getMaxSpells();
                for (; i >= 0; i--) {
                    Spell spell = SpellController.getInstance().getSpell(i);
                    if (spell != null) {
                        if (spell.exists()
                                && spell.getCaster() == n) {
                            spell.setTimeToLive(0);
                            spell.setTurnsToLive(0);
                        }
                    }
                }
            }
        }
    }
    public void ARX_INTERACTIVE_DestroyIO(BaseInteractiveObject io)
            throws RPGException
{
        if (io != null
                && io.getShow() != IoGlobals.SHOW_FLAG_DESTROYED) {
        ARX_INTERACTIVE_ForceIOLeaveZone(io, 0);
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
        if (io.hasIOFlag(IoGlobals.IO_02_ITEM)
                && io.getItemData().getCount() > 1)
        {
            io.getItemData().adjustCount(-1);
        }
        else
        {
            // Kill all spells
            int numm = GetInterNum(io);

            if (hasIO(numm))
            {
                // kill all spells from caster
                // ARX_SPELLS_FizzleAllSpellsFromCaster(numm);
            }

            // Need To Kill timers
            Script.getInstance().timerClearByIO(io);
            io.setShow(IoGlobals.SHOW_FLAG_DESTROYED);
            io.removeGameFlag(IoGlobals.GFLAG_ISINTREATZONE);

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

            ARX_INTERACTIVE_DestroyDynamicInfo(io);

            if (io.isScriptLoaded())
            {
                int num = GetInterNum(io);
                releaseIO(io);

                if (hasIO(num))
                {
                    getIOs()[num] = null;
                }
            }
        }
    }
}
public abstract void ARX_INTERACTIVE_ForceIOLeaveZone(BaseInteractiveObject io,
        long flags);
public int GetInterNum(BaseInteractiveObject io)
{
    int num = -1;
    if (io != null)
    {
        BaseInteractiveObject[] objs = getIOs();
        for (int i = objs.length - 1; i >= 0; i--)
        {
            if (objs[i].equals(io))
            {
                num = i;
                break;
            }
        }
        objs = null;
    }
    return num;
}
/**
 * Gets a {@link BaseInteractiveObject} by its reference id.
 * @param id the reference id
 * @return {@link BaseInteractiveObject}
 * @throws RPGException if the object does not exist
 */
public BaseInteractiveObject getIO(int id) throws RPGException
{
    BaseInteractiveObject io = null;
        if (hasIO(id)) {
        BaseInteractiveObject[] objs = getIOs();
        for (int i = objs.length - 1; i >= 0; i--)
        {
            if (objs[i] != null
                    && objs[i].getRefId() == id)
            {
                io = objs[i];
                break;
            }
        }
        objs = null;
    } else {
        throw new RPGException(
                ErrorMessage.INTERNAL_BAD_ARGUMENT, "BaseInteractiveObject does not exist");
    }
        return io;
}
/**
 * Gets the internal list of IOs.
 * @return {@link BaseInteractiveObject}[]
 */
protected abstract BaseInteractiveObject[] getIOs();
/**
 * Gets the largest reference Id in use.
 * @return {@link int}
 */
public abstract int getMaxIORefId();
/**
 * Gets a new interactive object.
 * @return {@link BaseInteractiveObject}
 */
protected abstract BaseInteractiveObject getNewIO();
/**
 * Gets an BaseInteractiveObject's reference id by name.  If the target is "none" or does not
 * exist, -1 is returned.  If the target is "self" or "me", -2; 
 * @param name the BaseInteractiveObject's name
 * @return {@link int}
 * @throws RPGException if an error occurs
 */
public int getTargetByNameTarget(String name)
            throws RPGException
{
        int ioid = -1;
        if (name != null) {
        if (name.equalsIgnoreCase("self")
                || name.equalsIgnoreCase("me"))
        {
            ioid = -2;
        }
        else if (name.equalsIgnoreCase("player"))
        {
            ioid = ProjectConstants.getInstance().getPlayer();
        }
        else
        {
            BaseInteractiveObject[] ios = this.getIOs();
            for (int i = ios.length - 1; i >= 0; i--)
            {
                BaseInteractiveObject io = ios[i];
                if (io.hasIOFlag(IoGlobals.IO_03_NPC))
                {
                    if (name.equalsIgnoreCase(
                            new String(io.getNPCData().getName())))
                    {
                        ioid = io.getRefId();
                        break;
                    }
                }
                else if (io.hasIOFlag(IoGlobals.IO_02_ITEM))
                {
                    if (name.equalsIgnoreCase(
                            new String(io.getItemData().getItemName())))
                    {
                        ioid = io.getRefId();
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
/**
 * Determines if the {@link Interactive} has an interactive object by a
 * specific id.
 * @param id the id
 * @return true if an interactive object by that id has been stored already;
 *         false otherwise
 */
public boolean hasIO(int id)
{
    boolean has = false;
    BaseInteractiveObject[] objs = getIOs();
    for (int i = objs.length - 1; i >= 0; i--)
    {
        if (objs[i] != null
                && id == objs[i].getRefId())
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
public boolean hasIO(BaseInteractiveObject io)
{
    boolean has = false;
    if (io != null)
    {
        BaseInteractiveObject[] objs = getIOs();
        for (int i = objs.length - 1; i >= 0; i--)
        {
            if (objs[i] != null
                    && io.getRefId() == objs[i].getRefId()
                    && io.equals(objs[i]))
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
public boolean isSameObject(BaseInteractiveObject io0, BaseInteractiveObject io1)
{
    boolean same = false;
    if (io0 != null
            && io1 != null)
    {
        if (!io0.hasIOFlag(IoGlobals.IO_13_UNIQUE)
                && !io1.hasIOFlag(IoGlobals.IO_13_UNIQUE))
        {
            if (io0.hasIOFlag(IoGlobals.IO_02_ITEM)
                    && io1.hasIOFlag(IoGlobals.IO_02_ITEM)
                    && io0.getOverscript() == null
                    && io1.getOverscript() == null)
            {
                String n0 = new String(io0.getItemData().getItemName());
                String n1 = new String(io1.getItemData().getItemName());
                if (n0.equalsIgnoreCase(n1))
                {
                    same = true;
                }
            }
        }
    }
    return same;
}
/**
 * Sets the weapon on an NPC.
 * @param io the BaseInteractiveObject
 * @param temp the temp object
 * @throws RPGException
 */
public void prepareSetWeapon(BaseInteractiveObject io, String temp)
            throws RPGException
{
        if (io != null
                && io.hasIOFlag(IoGlobals.IO_03_NPC)) {
        if (io.getNPCData().getWeapon() != null)
        {
            BaseInteractiveObject oldWpnIO = (BaseInteractiveObject)io.getNPCData().getWeapon();
            // unlink the weapon from the NPC BaseInteractiveObject
            // EERIE_LINKEDOBJ_UnLinkObjectFromObject(io->obj, ioo->obj);
            io.getNPCData().setWeapon(null);
            releaseIO(oldWpnIO);
            oldWpnIO = null;
        }
        // load BaseInteractiveObject from DB
        BaseInteractiveObject wpnIO = AddItem(temp, IoGlobals.IO_IMMEDIATELOAD);
        if (wpnIO != null)
        {
            io.getNPCData().setWeapon(wpnIO);
            io.setShow(IoGlobals.SHOW_FLAG_LINKED);
            wpnIO.setScriptLoaded(true);
            // TODO - link item to io
            // SetWeapon_Back(io);
        }
    }
}
/**
 * Releases the BaseInteractiveObject and all resources.
 * @param ioid the BaseInteractiveObject's id
 * @throws RPGException if an error occurs
 */
public void releaseIO(int ioid) throws RPGException
{
        if (!hasIO(ioid)) {
        throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                "Invalid BaseInteractiveObject id " + ioid);
    }
    releaseIO(getIO(ioid));
}
/**
 * Releases the BaseInteractiveObject and all resources.
 * @param io the BaseInteractiveObject
 */
public void releaseIO(BaseInteractiveObject io)
{
    if (io != null)
    {
        if (io.getInventory() != null)
        {
            InventoryData inventory = io.getInventory();
            if (inventory != null)
            {
                for (int j = 0; j < inventory.getNumInventorySlots(); j++)
                {
                    if (io.equals(inventory.getSlot(j).getIo()))
                    {
                        inventory.getSlot(j).setIo(null);
                        inventory.getSlot(j).setShow(true);
                    }
                }
            }
        }
        // release script timers and spells
        // release from groups
        //
        Script.getInstance().timerClearByIO(io);
        // MagicRealmSpells.getInstance().removeAllSpellsOn(io);
        Script.getInstance().releaseScript(io.getScript());
        Script.getInstance().releaseScript(io.getOverscript());
        Script.getInstance().releaseAllGroups(io);
        int id = io.getRefId();
        int index = -1;
        BaseInteractiveObject[] objs = getIOs();
        for (int i = 0; i < objs.length; i++)
        {
            if (objs[i] != null
                    && id == objs[i].getRefId())
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
        objs = getIOs();
    }
}
/**
 * Removes an item from all available inventories.
 * @param itemIO the item
 * @throws RPGException if an error occurs
 */
public void RemoveFromAllInventories(BaseInteractiveObject itemIO)
            throws RPGException
{
        if (itemIO != null) {
        int i = Interactive.getInstance().getMaxIORefId();
        for (; i >= 0; i--)
        {
            if (Interactive.getInstance().hasIO(i))
            {
                BaseInteractiveObject invIo = (BaseInteractiveObject)Interactive.getInstance().getIO(i);
                InventoryData<BaseInteractiveObject, InventorySlot<BaseInteractiveObject>> inventoryData;
                if (invIo.getInventory() != null)
                {
                    inventoryData = invIo.getInventory();
                    for (int j =
                            inventoryData.getNumInventorySlots()
                                    - 1;
                            j >= 0; j--)
                    {
                        InventorySlot<BaseInteractiveObject> slot = inventoryData.getSlot(j);
                        if (slot.getIo() != null
                                && slot.getIo().equals(itemIO))
                        {
                            slot.setIo(null);
                            slot.setShow(true);
                        }
                    }
                }
                invIo = null;
                inventoryData = null;
            }
        }
    }
}
public void ARX_INTERACTIVE_TeleportBehindTarget(BaseInteractiveObject io)
{
    // TODO Auto-generated method stub

}
public void ARX_INTERACTIVE_Teleport(BaseInteractiveObject io, SimpleVector2 position)
{
    // TODO Auto-generated method stub

}
/**
 * Determines an BaseInteractiveObject's world position and sets the location to a
 *  {@link SimpleVector2} parameter.
 * @param io the BaseInteractiveObject
 * @param pos the parameter
 * @return <tt>true</tt> if the item has a position; <tt>false</tt>
 * otherwise
 * @throws RPGException if an error occurs
 */
public boolean GetItemWorldPosition(BaseInteractiveObject io, SimpleVector2 pos)
            throws RPGException
{
    boolean hasPosition = false;
        if (io != null) {
        if (io.getShow() != IoGlobals.SHOW_FLAG_IN_SCENE)
        {
            // TODO if item is being cursor dragged, set its location to
            // player's

            BaseInteractiveObject[] ios = this.getIOs();
            boolean found = false;
            for (int i = ios.length - 1; i >= 0; i--)
            {
                BaseInteractiveObject iio = ios[i];
                if (iio == null)
                {
                    continue;
                }
                if (iio.hasIOFlag(IoGlobals.IO_03_NPC))
                {
                    if (iio.equals(io))
                    {
                        // teleporting to NPC io
                        pos.set(iio.getPosition());
                        found = true;
                        hasPosition = true;
                        break;
                    }
                    // check to see if NPC has BaseInteractiveObject in inventory
                    if (iio.getInventory().IsInPlayerInventory(io))
                    {
                        // teleporting to NPC io
                        pos.set(iio.getPosition());
                        found = true;
                        hasPosition = true;
                        break;
                    }
                }
                else if (iio.hasIOFlag(IoGlobals.IO_01_PC))
                {
                    if (iio.equals(io))
                    {
                        // teleporting to PC io
                        pos.set(iio.getPosition());
                        found = true;
                        hasPosition = true;
                        break;
                    }
                    // check to see if PC has BaseInteractiveObject in inventory
                    if (iio.getInventory().IsInPlayerInventory(io))
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
                for (int i = ios.length - 1; i >= 0; i--)
                {
                    BaseInteractiveObject iio = ios[i];
                    if (iio.equals(io))
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
    }
}