using LabLord.Constants;
using LabLord.Flyweights;
using LabLord.Singletons;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using RPGBase.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace LabLord.UI.SceneControllers.LabLord
{
    public class LabLordInventoryController : Singleton<LabLordInventoryController>, IWatcher
    {
        #region UI ELEMENTS
        /// <summary>
        /// the list of Inventory Slots.
        /// </summary>
        public GameObject[] InventorySlots;
        /// <summary>
        /// the Equip/Uneqip button.
        /// </summary>
        public Button Equip;
        #endregion
        #region MonoBehaviour messages
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        void Start()
        {
            LabLordCharacter pc = (LabLordCharacter)LabLordWizardController.Instance.Player.GetComponent<LabLordInteractiveObject>().PcData;
            pc.AddWatcher(this);

            for (int i = InventorySlots.Length - 1; i >= 0; i--)
            {
                // remove markers
                InventorySlots[i].transform.GetChild(0)
                    .GetComponent<Text>().text = "";
                Toggle t = InventorySlots[i].GetComponent<Toggle>();
                // remove all listeners
                t.onValueChanged.RemoveAllListeners();
                // turn off the toggle
                t.isOn = false;
                t.interactable = false;
                // add our listener
                t.onValueChanged.AddListener(delegate
                {
                    HandleItemSelection(t, pc);
                });
            }
        }
        #endregion
        #region SHEET ADJUSTMENTS
        /// <summary>
        /// Handles the user clicking the Equip/Unequip button.
        /// </summary>
        public void EquipItem()
        {
            LabLordCharacter pc = (LabLordCharacter)LabLordWizardController.Instance.Player.GetComponent<LabLordInteractiveObject>().PcData;
            int ioid = -1;
            for (int i = InventorySlots.Length - 1; i >= 0; i--)
            {
                Toggle t = InventorySlots[i].GetComponent<Toggle>();
                if (t.isOn)
                {
                    ioid = Int32.Parse(InventorySlots[i].GetComponent<InteractiveTooltipWidget>().TooltipText);
                    // turn off toggle
                    t.isOn = false;
                    break;
                }
            }
            if (ioid >= 0)
            {
                LabLordInteractiveObject io = (LabLordInteractiveObject)Interactive.Instance.GetIO(ioid);
                if (pc.IsPlayerEquip(io))
                {
                    io.ItemData.UnEquip(pc.Io, false);
                }
                else
                {
                    print("Pressed equip");
                    io.ItemData.Equip(pc.Io);
                }
            }
            else
            {
                print("How the HELL did we get here?!?!?");
            }
        }
        /// <summary>
        /// Gets the tooltip associated with the selected Inventory Slot.
        /// </summary>
        /// <param name="ioidString">the Reference id of the IO in the inventory slot</param>
        /// <returns></returns>
        public string GetTooltipText(string ioidString)
        {
            string s = "";
            int ioid = Int32.Parse(ioidString);
            LabLordInteractiveObject io = (LabLordInteractiveObject)Interactive.Instance.GetIO(ioid);
            if (io.HasIOFlag(IoGlobals.IO_02_ITEM))
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                sb.Append("<b>");
                sb.Append(io.ItemData.Title);
                sb.Append("</b>\r\n");
                if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_ARMOR))
                {
                    sb.Append("Armour\r\n");
                    sb.Append("Armour Class: ");
                    int acBase = 9 + (int)io.ItemData.Equipitem.GetElementModifier(LabLordGlobals.EQUIP_ELEMENT_AC).Value;
                    sb.Append(acBase);
                    sb.Append("\r\n");
                }
                else if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_SHIELD))
                {
                    sb.Append("Shield\r\n");
                    sb.Append("Armour Class: ");
                    int acBase = (int)io.ItemData.Equipitem.GetElementModifier(LabLordGlobals.EQUIP_ELEMENT_AC).Value;
                    sb.Append(Math.Abs(acBase));
                    if (acBase < 0)
                    {
                        sb.Append(" less\r\n");
                    }
                }
                else if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_HELMET))
                {
                    sb.Append("Helmet\r\n");
                }
                else if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_WEAPON))
                {
                    if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_BOW))
                    {
                        sb.Append("Bow\r\n");
                    }
                    else if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_1H))
                    {
                        sb.Append("One-handed Weapon\r\n");
                    }
                    else if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_2H))
                    {
                        sb.Append("Two-handed Weapon\r\n");
                    }
                    else if (io.HasTypeFlag(EquipmentGlobals.OBJECT_TYPE_DAGGER))
                    {
                        sb.Append("Dagger\r\n");
                    }
                    sb.Append("Damage: ");
                    int[] dmg = io.Script.GetLocalIntArrayVariableValue("DMG");
                    sb.Append(dmg[0]);
                    sb.Append("D");
                    sb.Append(dmg[1]);
                    sb.Append("\r\n");
                }
                sb.Append("Weight: ");
                sb.Append(io.ItemData.Weight);
                sb.Append(" .lbs");
                sb.Append("\r\n");
                sb.Append(io.ItemData.Description);
                s = sb.ToString();
                sb.ReturnToPool();
            }
            return s;
        }
        /// <summary>
        /// Handles the action of a user selecting an item.
        /// </summary>
        /// <param name="toggle">the toggle for the selected item - null if no item was selected</param>
        /// <param name="pc">the player character</param>
        public void HandleItemSelection(Toggle toggle, LabLordCharacter pc)
        {
            if (toggle.isOn)
            {
                // allow Selected animation to play
                toggle.gameObject.GetComponent<Animator>().Play("Selected");
                // turn on Equip/Unequip button
                int ioid = Int32.Parse(toggle.gameObject.GetComponent<InteractiveTooltipWidget>().TooltipText);
                // check to see if equipped
                if (pc.IsPlayerEquip(Interactive.Instance.GetIO(ioid)))
                {
                    Equip.transform.GetChild(0)
                        .GetChild(0)
                        .GetComponent<Text>().text = "Unequip";
                }
                else
                {
                    Equip.transform.GetChild(0)
                        .GetChild(0)
                        .GetComponent<Text>().text = "Equip";
                }
                // turn on equip/unequip button
                Equip.interactable = true;
            }
            else
            {
                Equip.transform.GetChild(0)
                    .GetChild(0)
                    .GetComponent<Text>().text = "Equip";
                // turn off equip/unequip button
                Equip.interactable = false;
                // show Unselected animation
                toggle.gameObject.GetComponent<Animator>().Play("Unselected");
            }
        }
        /// <summary>
        /// Sets the onscreen inventory.
        /// </summary>
        /// <param name="pc"></param>
        private void SetInventory(LabLordCharacter pc)
        {
            // clear off markers and equipment
            for (int i = pc.Io.Inventory.Slots.Length - 1; i >= 0; i--)
            {
                Text onScreen = InventorySlots[i].transform.GetChild(1)
                        .transform.GetChild(0)
                        .GetComponent<Text>();
                Text marker = InventorySlots[i].transform.GetChild(0)
                    .GetComponent<Text>();
                onScreen.text = "";
                marker.text = "";

                onScreen = null;
                marker = null;
            }
            int offset = 0;
            // set equipped items first
            for (int slot = 0, li = ProjectConstants.Instance.GetMaxEquipped(); slot < li; slot++)
            {
                int ioid = pc.GetEquippedItem(slot);
                if (ioid >= 0)
                {
                    LabLordInteractiveObject io = (LabLordInteractiveObject)Interactive.Instance.GetIO(ioid);
                    Toggle toggle = InventorySlots[offset].GetComponent<Toggle>();
                    InteractiveTooltipWidget tooltip = InventorySlots[offset].GetComponent<InteractiveTooltipWidget>();
                    Text onScreen = InventorySlots[offset].transform.GetChild(1)
                            .transform.GetChild(0)
                            .GetComponent<Text>();
                    Text marker = InventorySlots[offset].transform.GetChild(0)
                        .GetComponent<Text>();

                    toggle.interactable = true;
                    tooltip.TooltipText = ioid.ToString();
                    onScreen.text = io.ItemData.Title;
                    marker.text = "<b>E</b>";

                    toggle = null;
                    tooltip = null;
                    onScreen = null;
                    marker = null;
                    offset++;
                }
            }
            // set unequipped items
            for (int i = 0, li = pc.Io.Inventory.Slots.Length; i < li; i++)
            {
                InventorySlot pcSlot = pc.Io.Inventory.Slots[i];
                if (pcSlot.Io != null
                    && pcSlot.Show)
                {
                    Toggle toggle = InventorySlots[offset].GetComponent<Toggle>();
                    InteractiveTooltipWidget tooltip = InventorySlots[offset].GetComponent<InteractiveTooltipWidget>();
                    Text onScreen = InventorySlots[offset].transform.GetChild(1)
                            .transform.GetChild(0)
                            .GetComponent<Text>();

                    toggle.interactable = true;
                    tooltip.TooltipText = pcSlot.Io.RefId.ToString();
                    onScreen.text = pcSlot.Io.ItemData.Title;
                    offset++;

                    toggle = null;
                    tooltip = null;
                    onScreen = null;
                }
                pcSlot = null;
            }
        }
        #endregion
        public void WatchUpdated(Watchable data)
        {
            LabLordCharacter pc = (LabLordCharacter)data;
            SetInventory(pc);
        }
    }
}
