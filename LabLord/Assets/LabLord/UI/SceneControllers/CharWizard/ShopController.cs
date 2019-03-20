using LabLord.Constants;
using LabLord.Flyweights;
using LabLord.UI.SceneControllers.LabLord;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Scripts.UI.SimpleJSON;
using RPGBase.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace LabLord.UI.SceneControllers.CharWizard
{
    public class ShopController : Singleton<ShopController>, IWatcher
    {
        #region UI ELEMENTS
        /// <summary>
        /// the pool of button objects.
        /// </summary>
        public SimpleObjectPool ButtonObjectPool;
        /// <summary>
        /// the Buy button.
        /// </summary>
        public Button Buy;
        /// <summary>
        /// the shop's content panel.
        /// </summary>
        public GameObject ContentPanel;
        /// <summary>
        /// the tooltip area for shop items.
        /// </summary>
        public Text ContentTooltipArea;
        /// <summary>
        /// the Cost field.
        /// </summary>
        public Text Cost;
        /// <summary>
        /// the Player's Gold field.
        /// </summary>
        public Text Gold;
        /// <summary>
        /// the toggles for Armour and Weapons.
        /// </summary>
        public GameObject[] ShopToggles;
        #endregion
        /// <summary>
        /// the flag for the armour list display.
        /// </summary>
        private const int ARMOUR_LIST = 1;
        /// <summary>
        /// the flag for the weapons list display.
        /// </summary>
        private const int WEAPONS_LIST = 0;
        /// <summary>
        /// the configuration file.
        /// </summary>
        private JSONNode shopFile;
        /// <summary>
        /// The list of items displayed in the shop.
        /// </summary>
        private JSONArray shopList;
        /// <summary>
        /// the type of item being displayed. default is weapons
        /// </summary>
        private int type = WEAPONS_LIST;
        #region MONOBEHAVIOUR messages
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            //*******************************//
            // LOAD GAME RESOURCES
            //*******************************//
            // load shop file (JSON) version
            string json = System.IO.File.ReadAllText(@"Assets/LabLord/JS/shop.json");
            shopFile = JSON.Parse(json);
        }
        #endregion
        #region LIST SETUP
        /// <summary>
        /// Adds item buttons to the shop.
        /// </summary>
        /// <param name="pc">the player character shopping</param>
        private void AddButtons(LabLordCharacter pc, bool isWeapons)
        {
            // DISABLE BUY BUTTON
            Buy.interactable = false;
            // RESET COST
            Cost.text = "0";
            // REMOVE ALL OLD BUTTONS
            RemoveButtons();
            // RESET SHOP LIST
            if (isWeapons)
            {
                SetWeaponsList(pc);
            }
            else
            {
                SetArmourList(pc);
            }
            // ADD NEW BUTTONS
            for (int i = 0, li = shopList.Count; i < li; i++)
            {
                SetButton(pc, shopList[i], isWeapons);
            }
            ValidateList(pc, 0);
        }
        /// <summary>
        /// Removes all buttons from the item list.
        /// </summary>
        private void RemoveButtons()
        {
            while (ContentPanel.transform.childCount > 0)
            {
                GameObject toRemove = ContentPanel.transform.GetChild(0).gameObject;
                ButtonObjectPool.ReturnObject(toRemove);
            }
        }
        /// <summary>
        /// Sets the list of armours displayed for a character based upon their class.
        /// </summary>
        /// <param name="pc">the player character</param>
        private void SetArmourList(LabLordCharacter pc)
        {
            shopList = new JSONArray();
            JSONArray list = shopFile["armour"].AsArray;
            for (int i = 0, li = list.Count; i < li; i++)
            {
                JSONNode node = list[i];
                JSONArray classes = node["classes"].AsArray;
                if (classes[pc.Clazz].AsInt == 1)
                {
                    shopList.Add(node);
                }
            }
        }
        /// <summary>
        /// Sets up a shop item button.
        /// </summary>
        /// <param name="pc">the player character shopping</param>
        /// <param name="node">the item data object</param>
        /// <param name="isWeapon">flag indicating whether the item is a weapon or not</param>
        private void SetButton(LabLordCharacter pc, JSONNode node, bool isWeapon)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            GameObject newButton = ButtonObjectPool.GetObject();
            newButton.name = node["name"];
            newButton.transform.SetParent(ContentPanel.transform, false);
            // reset local scale - something makes it go out of wack
            newButton.transform.localScale = new Vector3(1, 1, 1);
            newButton.transform.GetChild(0)
                .GetChild(0).GetComponent<Text>().text = node["name"];
            sb.Append((string)node["name"]);
            sb.Append("\r\n");
            sb.Append(LabLordCurrency.ToString(node["cost"].AsInt));
            sb.Append("\r\n");
            if (isWeapon)
            {
                sb.Append("Damage: ");
                sb.Append((string)node["damage"]);
            }
            else
            {
                sb.Append("AC: ");
                sb.Append((string)node["ac"]);
            }
            sb.Append("\r\n");
            sb.Append("Weight: ");
            sb.Append(node["weight"].AsFloat);
            sb.Append(" .lbs");
            sb.Append("\r\n");
            newButton.GetComponent<InteractiveTooltipWidget>().TooltipText = sb.ToString();
            newButton.GetComponent<InteractiveTooltipWidget>().TooltipArea = ContentTooltipArea;
            Toggle toggle = newButton.GetComponent<Toggle>();
            // remove all listeners
            toggle.onValueChanged.RemoveAllListeners();
            toggle.isOn = false;
            // add our listener
            toggle.onValueChanged.AddListener(delegate
            {
                HandleItemSelection(toggle, pc);
            });
            sb.Length = 0;
            sb.ReturnToPool();
        }
        /// <summary>
        /// Sets up the wizard to display shop items.
        /// </summary>
        /// <param name="pc">the player character doing the shopping</param>
        /// <returns></returns>
        public LlStepWizard.Callback Setup(LabLordCharacter pc)
        {
            LlStepWizard.Callback c = () => { };
            // ADD CONTENT TO SHOP
            AddButtons(pc, true);
            // SET CALLBACK - button animation will not work until button is active on screen
            switch (pc.Clazz)
            {
                case LabLordClass.CLASS_ASSASSIN:
                case LabLordClass.CLASS_FIGHTER:
                case LabLordClass.CLASS_MONK:
                case LabLordClass.CLASS_PALADIN:
                case LabLordClass.CLASS_RANGER:
                case LabLordClass.CLASS_THIEF:
                case LabLordClass.CLASS_CLERIC:
                case LabLordClass.CLASS_DRUID:
                    // enable armour button
                    c = () =>
                    {
                        ShopToggles[1].GetComponent<Toggle>().isOn = true;
                        ShopToggles[0].GetComponent<Selectable>().interactable = true;
                        ShopToggles[0].GetComponent<Animator>().Play("Unselected");
                    };
                    break;
                case LabLordClass.CLASS_MAGIC_USER:
                case LabLordClass.CLASS_ILLUSIONIST:
                    // disable armour button
                    c = () =>
                    {
                        ShopToggles[1].GetComponent<Toggle>().isOn = true;
                        ShopToggles[0].GetComponent<Selectable>().interactable = false;
                        ShopToggles[0].GetComponent<Animator>().Play("Disabled");
                    };
                    break;
            }
            // set up shop toggles
            for (int i = ShopToggles.Length - 1; i >= 0; i--)
            {
                Toggle toggle = ShopToggles[i].GetComponent<Toggle>();
                // add our listener
                toggle.onValueChanged.AddListener(delegate
                {
                    SwitchLists(toggle, pc);
                });
            }
            return c;
        }
        /// <summary>
        /// Sets the list of weapons displayed for a character based upon their class.
        /// </summary>
        /// <param name="pc">the player character</param>
        private void SetWeaponsList(LabLordCharacter pc)
        {
            shopList = new JSONArray();
            JSONArray list = shopFile["weapons"].AsArray;
            for (int i = 0, li = list.Count; i < li; i++)
            {
                JSONNode node = list[i];
                JSONArray classes = node["classes"].AsArray, races = node["races"].AsArray;
                if (classes[pc.Clazz].AsInt == 1
                    && races[pc.Race].AsInt == 1)
                {
                    shopList.Add(node);
                }
            }
        }
        /// <summary>
        /// Switches the shop list by item type.
        /// </summary>
        /// <param name="toggle">the item toggle that was pressed</param>
        /// <param name="pc">the player character</param>
        public void SwitchLists(Toggle toggle, LabLordCharacter pc)
        {
            if (toggle.isOn)
            {
                switch (type)
                {
                    case ARMOUR_LIST:
                        if (string.Equals(toggle.gameObject.name, "weapons", StringComparison.OrdinalIgnoreCase))
                        {
                            // switch to weapons list
                            type = WEAPONS_LIST;
                            AddButtons(pc, true);
                        }
                        break;
                    default:
                        if (string.Equals(toggle.gameObject.name, "armor", StringComparison.OrdinalIgnoreCase))
                        {
                            // switch to armour list
                            type = ARMOUR_LIST;
                            AddButtons(pc, false);
                        }
                        break;

                }
            }
        }
        /// <summary>
        /// Validates the list displayed to the user, enabling/disabling items that are unaffordable.
        /// </summary>
        /// <param name="pc">the player character that is shopping</param>
        /// <param name="totalCost">the total cost of items currently selected</param>
        private void ValidateList(LabLordCharacter pc, int totalCost)
        {
            for (int i = 0, li = shopList.Count; i < li; i++)
            {
                JSONNode node = shopList[i];
                Toggle t = ContentPanel.transform.GetChild(i).GetComponent<Toggle>();
                if (t.isOn) // skip items that are already on
                {
                    continue;
                }
                if (totalCost + node["cost"].AsInt <= pc.Gold)
                {
                    //print("set unselected " + node["name"]);
                    t.gameObject.GetComponent<Animator>().Play("Unselected");
                    t.gameObject.GetComponent<Selectable>().interactable = true;
                }
                else
                {
                    // print("set disabled " + node["name"]);
                    t.gameObject.GetComponent<Animator>().Play("Disabled");
                    t.gameObject.GetComponent<Selectable>().interactable = false;
                }
                node = null;
                t = null;
            }
        }
        #endregion
        #region LIST ITEM EVENTS
        /// <summary>
        /// Gets the total cost of all items previously marked, and the cost of the selected item.
        /// </summary>
        /// <param name="toggle">the toggle for the selected item</param>
        /// <returns>int[]</returns>
        private int[] GetTotalShopCosts(Toggle toggle)
        {
            int[] costArr = new int[2];
            int c = 0;
            for (int i = ContentPanel.transform.childCount - 1; i >= 0; i--)
            {
                Toggle t = ContentPanel.transform.GetChild(i).GetComponent<Toggle>();
                if (t.isOn)
                {
                    JSONNode node = shopList[i];
                    if (toggle != null
                        && t.Equals(toggle))
                    {
                        costArr[1] = node["cost"].AsInt;
                        continue;
                    }
                    c += node["cost"].AsInt;
                }
            }
            costArr[0] = c;
            return costArr;
        }
        /// <summary>
        /// Handles the action of a user selecting an item.
        /// </summary>
        /// <param name="toggle">the toggle for the selected item - null if no item was selected</param>
        /// <param name="pc">the player character</param>
        public void HandleItemSelection(Toggle toggle, LabLordCharacter pc)
        {
            int totalCost = 0, itemCost = 0;
            // get the cost of all items previously on the list
            int[] r = GetTotalShopCosts(toggle);
            totalCost = r[0];
            itemCost = r[1];
            r = null;

            if (toggle != null
                && toggle.isOn) // we're here because user selected an item
            {
                if (totalCost + itemCost <= pc.Gold)
                {
                    // user can afford item
                    // print("user can afford " + toggle.gameObject.name);
                    // add item to cost
                    totalCost += itemCost;
                    // allow Selected animation to play
                    toggle.gameObject.GetComponent<Animator>().Play("Selected");
                }
                else
                {
                    // print("user cannot afford " + toggle.gameObject.name);
                    // user cannot afford item
                    // turn off toggle value
                    toggle.isOn = false;
                }
            }
            ValidateList(pc, totalCost);
            // SET COST
            Cost.text = LabLordCurrency.ToString(totalCost);
            // ENABLE/DISABLE BUY BUTTON
            Buy.interactable = totalCost > 0;
        }
        /// <summary>
        /// Purchases goods for the player character.
        /// </summary>
        public void PurchaseGoods()
        {
            LabLordCharacter pc = (LabLordCharacter)LabLordWizardController.Instance.Player.GetComponent<LabLordInteractiveObject>().PcData;
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            // REMOVE PLAYER'S GOLD FIRST - BEFORE TOGGLES TURNED OFF
            pc.AdjustGold(-GetTotalShopCosts(null)[0]);
            // go through each item on the list and give the player the item
            for (int i = 0, li = shopList.Count; i < li; i++)
            {
                JSONNode node = shopList[i];
                Toggle t = ContentPanel.transform.GetChild(i).GetComponent<Toggle>();
                if (t.isOn) // ONLY ADD SELECTED ITEMS
                {
                    // CREATE NEW INSTANCE OF ITEM
                    sb.Append("LabLord.Scriptables.Items.");
                    switch (type)
                    {
                        case ARMOUR_LIST:
                            sb.Append("Armour.");
                            break;
                        default:
                            sb.Append("Weapons.");
                            break;
                    }
                    sb.Append((string)node["script"]);
                    LabLordScriptable scriptable = Activator.CreateInstance(Type.GetType(sb.ToString())) as LabLordScriptable;
                    sb.Length = 0;
                    GameObject io = LabLordWizardController.Instance.NewItem(scriptable);
                    // GIVE ITEM TO PC
                    pc.Io.Inventory.CanBePutInInventory(io.GetComponent<BaseInteractiveObject>());
                    io.transform.parent = pc.Io.gameObject.transform;

                    // TURN OFF LIST TOGGLE
                    t.isOn = false;
                }
            }
            // RESET COST
            Cost.text = "0";
            // RESET ITEM LIST
            HandleItemSelection(null, pc);
            sb.ReturnToPool();
            sb = null;
        }
        #endregion
        public void WatchUpdated(Watchable data)
        {
            LabLordCharacter pc = (LabLordCharacter)data;
            Gold.text = LabLordCurrency.ToString((int)pc.Gold);
        }
    }
}
