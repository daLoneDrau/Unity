using LabLord.Constants;
using LabLord.Flyweights;
using LabLord.Singletons;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabLord.Scriptables.Items.Weapons
{
    public class Dart : WeaponBase
    {
        private void Enchant()
        {
            Io.ItemData.Price = GetLocalIntVariableValue("tmp");
            SetLocalVariable("tmp", 0);
            SetLocalVariable("reagent", "none");
        }
        public int OnBreak()
        {
            // PLAY "broken_weapon"
            Interactive.Instance.DestroyIO(Io);
            return ScriptConsts.ACCEPT;
        }
        public override int OnCombine()
        {
            LabLordInteractiveObject playerIO =
                ((LabLordInteractive)Interactive.Instance).GetPlayerIO();
            LabLordInteractiveObject itemIO = (LabLordInteractiveObject)
                Interactive.Instance.GetIO(
                        GetLocalIntVariableValue("combined_with"));
            if (itemIO != null)
            {
                string mixedWith = itemIO.ItemData.ItemName;
                string myName = Io.ItemData.ItemName;
                // if combining with a green potion
                if (string.Equals(mixedWith, "Green Potion", StringComparison.OrdinalIgnoreCase))
                {
                    if (GetLocalIntVariableValue("poisonable") == 0)
                    {
                        // send message that the weapon can't be poisoned
                    }
                    else
                    {
                        if (playerIO.PcData.GetFullAttributeScore("SK") < 30)
                        {
                            // send message player isn't skilled enough
                        }
                        else
                        {
                            // send event to potion to empty itself
                            Script.Instance.StackSendIOScriptEvent(
                                    itemIO,
                                    0,
                                    null,
                                    "Empty");
                            int tmp = (int)
                                    playerIO.PcData.GetFullAttributeScore(
                                            "SK");
                            tmp -= 27; // change temp value to 3 to 73
                            tmp /= 3; // change temo value to 1 to 24
                            Io.PoisonLevel = tmp;
                            Io.PoisonCharges = tmp;
                            // send message to player that weapon is poisoned
                        }
                    }
                }
                else if (string.Equals(mixedWith, "egg", StringComparison.OrdinalIgnoreCase))
                {
                    // send message that you cannot combine
                }
                else if (GetLocalIntVariableValue("enchanted") == 1)
                {
                    // send message that you cannot combine
                }
                else if (string.Equals(mixedWith, "dragon egg", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(mixedWith, "Meteor Sabre", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(mixedWith, "Meteor Zweihander", StringComparison.OrdinalIgnoreCase))
                    {
                        SetLocalVariable("reagent", "egg");
                        // show halo graphic
                        ReagentMixed();
                    }
                    else
                    {
                        // send message to player that you can't combine
                    }
                }
                else if (string.Equals(mixedWith, "Meteor Sabre", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(mixedWith, "Meteor Zweihander", StringComparison.OrdinalIgnoreCase))
                {
                    // send message to player that you can't combine
                }
                else if (string.Equals(mixedWith, "garlic", StringComparison.OrdinalIgnoreCase))
                {
                    SetLocalVariable("reagent", "garlic");
                    // show halo graphic
                    ReagentMixed();
                }
                else if (string.Equals(mixedWith, "bone powder", StringComparison.OrdinalIgnoreCase)
                      || string.Equals(mixedWith, "dragon bone powder", StringComparison.OrdinalIgnoreCase)
                      || string.Equals(mixedWith, "golem heart", StringComparison.OrdinalIgnoreCase)
                      || string.Equals(mixedWith, "amikar rock", StringComparison.OrdinalIgnoreCase))
                {
                    SetLocalVariable("reagent", mixedWith);
                    // show halo graphic
                    ReagentMixed();
                }
            }
            return base.OnCombine();
        }
        public int OnDurability()
        {
            // SET_DURABILITY -c ~^$PARAM1~
            return ScriptConsts.ACCEPT;
        }
        public override int OnEquip()
        {
            // play sound file "equip_nosword"
            // PLAY "equip_nosword"
            return base.OnEquip();
        }
        public override int OnInit()
        {
            // set local variables
            Io.ItemData.ItemName = "Dart";
            SetLocalVariable("shop_listing", "Dart");
            Io.ItemData.Description = "A projectile weapon.";
            Io.ItemData.MaxOwned = 99;
            // apply modifiers
            /*
            Io.ItemData.Equipitem.GetElementModifier(LabLordGlobals.EQUIP_ELEMENT_DAMAGE).Value = 2f;
            if (GameController.Instance.GOD_MODE)
            {
                Io.ItemData.Equipitem.GetElementModifier(LabLordGlobals.EQUIP_ELEMENT_DAMAGE).Value = 99f;
            }
            */
            // 5sp
            Io.ItemData.Price = 50f;

            Io.ItemData.StackSize = 1;
            Io.ItemData.Title = "Dart";
            Io.ItemData.SetObjectType(EquipmentGlobals.OBJECT_TYPE_BOW);
            Io.ItemData.Weight = 0.5f;
            
            Script.Instance.AddToGroup(Io, "ARMORY");
            // Damage is 1D4
            SetLocalVariable("DMG", new int[] { 1, 4 });

            SetLocalVariable("reagent", "none");
            SetLocalVariable("poisonable", 1);
            return base.OnInit();
        }
        public override int OnInventoryUse()
        {
            int fighting = Script.Instance.GetGlobalIntVariableValue(
                    "FIGHTING");
            if (fighting == 0)
            {
                // player isn't fighting already
                // check to see if player is strong enough to use?
                // if player isn't strong enough to wield
                // send a message
                // else if player isn't skilled enough to wield
                // send a message
                // else
                // have player equip the item
                Io.ItemData.Equip(((LabLordInteractive)Interactive.Instance).GetPlayerIO());
            }
            return base.OnInventoryUse();
        }
        public int OnRepaired()
        {
            // if (super.getLocalIntVariableValue("repair_check_durability") == 1) {
            // IF (^DURABILITY == ^MAXDURABILITY) {
            // SPEAK -p [player_weapon_already_repaired] NOP
            // ACCEPT
            // }
            // SENDEVENT REPAIR ^SENDER ""
            // ACCEPT
            // }
            // SET §tmp ~^MAXDURABILITY~
            // REPAIR SELF ~^PLAYER_SKILL_OBJECT_KNOWLEDGE~
            // if (^DURABILITY < §tmp) {
            // SPEAK -p [player_weapon_repaired_partially] NOP
            // ACCEPT
            // }
            // SPEAK -p [player_weapon_repaired_in_full] NOP
            // UNSET §tmp
            // ACCEPT
            return ScriptConsts.ACCEPT;
        }
        public int OnSpellCast()
        {
            /*
            if ("ENCHANT WEAPON".equalsIgnoreCase(
                    GetLocalStringVariableValue("spell_cast")))
            {
                if (super.getLocalIntVariableValue("enchanted") == 1)
                {
                    // send message cannot enchant
                    // SPEAK -p [player_no] NOP
                }
                else
                {
                    if ("none".equalsIgnoreCase(
                            GetLocalStringVariableValue("reagent")))
                    {
                        // send message cannot enchant
                        // SPEAK -p [player_wrong] NOP
                    }
                    else
                    {
                        // play spell sound
                        // PLAY -o "Magic_Spell_Enchant_Weapon"
                        // enchanting with dragon egg
                        if ("egg".equalsIgnoreCase(
                                GetLocalStringVariableValue("reagent")))
                        {
                            if (super.getLocalIntVariableValue(
                                    "caster_skill_level") < 8)
                            {
                                // send message not skilled enough
                                // SPEAK -p [player_not_skilled_enough] NOP
                            }
                            else
                            {
                                super.setLocalVariable("enchanted", 1);
                                if (Script.getInstance().getGlobalIntVariableValue(
                                        "need_superweapon") == 1)
                                {
                                    // update quest book
                                    // QUEST [system_Quest_log_final_meeting]
                                    // HEROSAY [system_questbook_updated]
                                    // play sound for system alerts
                                    // PLAY SYSTEM
                                }
                                if (Script.getInstance().getGlobalIntVariableValue(
                                        "superweapon") < 2)
                                {
                                    Script.getInstance().setGlobalVariable(
                                            "weapon_enchanted", 2);
                                    Script.getInstance().setGlobalVariable(
                                                                    "need_superweapon", 2);
                                    Script.getInstance().setGlobalVariable(
                                                                    "superweapon", 2);
                                }
                                super.setLocalVariable("reagent", "none");
                                String myName = new String(
                                        super.getIO().getItemData().getItemName());
                                if ("Meteor Sabre".equalsIgnoreCase(myName))
                                {
                                    // replace me with an enchanted Meteor Sabre
                                    // REPLACEME "SABRE_METEOR_ENCHANT"
                                }
                                else if ("Meteor Zweihander"
                                      .equalsIgnoreCase(myName))
                                {
                                    // replace me with an enchanted Meteor Sabre
                                    // REPLACEME "SWORD_2HANDED_METEOR_ENCHANT"
                                }
                            }
                        }
                        else
                        {
                            super.setLocalVariable("enchanted", 1);
                            super.getIO().getItemData().setItemName("Axe");
                            // SETNAME [description_axe]
                            if ("garlic".equalsIgnoreCase(
                                    GetLocalStringVariableValue("reagent")))
                            {
                                // play halo effect
                                // if player casting skill < 50,
                                // set affect DEX by 1, else by 2
                                // HALO -ocs 0 1 0 30
                                // IF (^PLAYER_SKILL_CASTING < 50) {
                                // SETEQUIP DEXTERITY 1
                                // }
                                // IF (^PLAYER_SKILL_CASTING > 50) {
                                // SETEQUIP DEXTERITY 2
                                // }
                                int tmp = super.getIO().getItemData().getPrice();
                                tmp *= 1.5f;
                                super.setLocalVariable("tmp", tmp);

                                enchant();
                            }
                            else if ("bone powder".equalsIgnoreCase(
                                  GetLocalStringVariableValue("reagent")))
                            {
                                // play halo effect
                                // if player casting skill < 50,
                                // set affect STR by 2, else by 3
                                // HALO -ocs 1 0.5 0 30
                                // IF (^PLAYER_SKILL_CASTING < 50) {
                                // SETEQUIP STRENGTH 2
                                // }
                                // IF (^PLAYER_SKILL_CASTING > 50) {
                                // SETEQUIP STRENGTH 3
                                // }
                                int tmp = super.getIO().getItemData().getPrice();
                                tmp *= 3f;
                                super.setLocalVariable("tmp", tmp);

                                enchant();
                            }
                            else if ("dragon bone powder".equalsIgnoreCase(
                                  GetLocalStringVariableValue("reagent")))
                            {
                                // play halo effect
                                // set affect STR by 1
                                // HALO -ocs 1 0 0 30
                                // SETEQUIP STRENGTH 1
                                int tmp = super.getIO().getItemData().getPrice();
                                tmp *= 1.5f;
                                super.setLocalVariable("tmp", tmp);

                                enchant();
                            }
                        }
                    }
                }
            }
            */
            return ScriptConsts.ACCEPT;
        }
        private void ReagentMixed()
        {
            LabLordInteractiveObject itemIO = (LabLordInteractiveObject)
                Interactive.Instance.GetIO(
                        GetLocalIntVariableValue("combined_with"));
            Interactive.Instance.DestroyIO(itemIO);
            // kill the haloe
            // TIMERoff 1 1 HALO -f
            // SPEAK -p [Player_off_interesting] NOP
        }
    }
}
