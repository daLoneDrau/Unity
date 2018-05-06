using Assets.Scripts.FantasyWargaming.Flyweights;
using RPGBase.Constants;
using RPGBase.Singletons;
using System;

namespace Assets.Scripts.FantasyWargaming.Scriptables.Items
{
    public class WeaponBase : FWScriptable
    {
        /// <summary>
        /// Enchants the item.
        /// </summary>
        private void Enchant()
        {
            Io.ItemData.Price = GetLocalIntVariableValue("tmp");
            SetLocalVariable("tmp", 0);
            SetLocalVariable("reagent", "none");
        }
        /// <summary>
        /// Break the item.
        /// </summary>
        /// <returns></returns>
        public int OnBreak()
        {
            // PLAY "broken_weapon"
            Interactive.Instance.DestroyIO(Io);
            return ScriptConsts.ACCEPT;
        }
        public override int OnEquip()
        {
            // play sound file "equip_sword"
            // PLAY "equip_sword"
            return base.OnEquip();
        }
        public override int OnInit()
        {
            Console.WriteLine("Wepon ONINIT");
            // set local variables
            SetLocalVariable("reagent", "none");
            SetLocalVariable("poisonable", 1);
            return base.OnInit();
        }
        public override int OnInventoryUse()
        {
            int fighting = Script.Instance.GetGlobalIntVariableValue("FIGHTING");
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
                /*
                Io.ItemData.Equip(
                        ((FFController) ProjectConstants.getInstance())
                                .getPlayerIO());
                                */
            }
            return base.OnInventoryUse();
        }
    }
}
