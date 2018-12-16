using RPGBase.Constants;
using RPGBase.Graph;
using RPGBase.Scripts.UI._2D;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.Flyweights.Actions;
using WoFM.Scriptables.Items;
using WoFM.Singletons;
using WoFM.UI.GlobalControllers;
using WoFM.UI.SceneControllers;
using WoFM.UI.Widgets;

namespace WoFM.Scriptables.Mobs
{
    public class Orc71 : OrcBase
    {
        /// <summary>
        /// the time given to show the orc 'waking up'.
        /// </summary>
        private float WAKE_UP_TIME = 2f;
        public override int OnHear()
        {
            Debug.Log("I HEARD SOMETHING");
            if (!HasLocalVariable("tested_luck"))
            {
                SetLocalVariable("tested_luck", 0);
            }
            if (GetLocalIntVariableValue("tested_luck") != 1)
            {
                SetLocalVariable("tested_luck", 1);
                WoFMInteractiveObject srcIo = (WoFMInteractiveObject)Script.Instance.EventSender;
                if (srcIo.HasIOFlag(IoGlobals.IO_01_PC))
                {
                    int forceHear = 0;
                    if (HasLocalVariable("force_hear"))
                    {
                        forceHear = GetLocalIntVariableValue("force_hear");
                    }
                    if (!((WoFMCharacter)srcIo.PcData).TestYourLuck() || forceHear == 1)
                    {
                        // WAKE UP
                        // 1. wait 5 seconds
                        GameSceneController.Instance.AddMustCompleteAction(new WaitAction(WAKE_UP_TIME));
                        // 2. stop particles.
                        GameSceneController.Instance.AddMustCompleteAction(new ParticleAction(Particles.Instance.GetType().GetMethod("StopSnoringAboveIo")));
                        // 2. play snort
                        GameSceneController.Instance.AddMustCompleteAction(new ParticleAction(Particles.Instance.GetType().GetMethod("PlaySnortAboveIo"), Io.RefId));
                        // 3. wait again
                        GameSceneController.Instance.AddMustCompleteAction(new WaitAction(WAKE_UP_TIME / 2));
                        // 4. add move to pc
                        WeightedGraphEdge[] path = WorldController.Instance.GetLandPath(Io.Position, srcIo.LastPositionHeld);
                        for (int i = path.Length - 1; i > 0; i--)
                        {
                            Vector2 node = WorldController.Instance.GetNodeCoordinatesFromId(path[i].To);
                            GameSceneController.Instance.AddMustCompleteAction(new MoveIoSpeedyAction((WoFMInteractiveObject)Io, node));
                        }
                        // 5. send message about waking up
                        GameSceneController.Instance.AddMustCompleteAction(new MessageAction(GameController.Instance.GetText("orc_71_aggression"), Messages.WARN));
                        // 6. show modal
                        GameSceneController.Instance.AddMustCompleteAction(new ModalAction(new ModalPanelDetails()
                        {
                            content = GameController.Instance.GetText("orc_71_aggression"),
                            iconImage = SpriteMap.Instance.GetSprite("icon_combat"),
                            button1Details = new EventButtonDetails()
                            {
                                buttonTitle = "To Battle!",
                                action = StartCombat
                            }
                        }));
                    }
                }
            }
            return base.OnHear();
        }
        private void StartCombat()
        {
            Debug.Log("STARTING COMBAT");
            CombatController.Instance.AddEnemies(Io.RefId);
            CombatController.Instance.StartCombat();
        }
        public override int OnInit()
        {
            base.OnInit();
            // initialize stats
            Io.NpcData.SetBaseAttributeScore("SKL", 6);
            Io.NpcData.SetBaseAttributeScore("MSK", 6);
            Io.NpcData.SetBaseAttributeScore("STM", 5);
            Io.NpcData.SetBaseAttributeScore("MSTM", 5);
            Io.NpcData.Name = "Orc";
            // heal from god
            Io.NpcData.HealNPC(5, true);
            // equip weapon
            WoFMInteractiveObject wpnIo = GameController.Instance.NewItem("Orc Cleaver", new OrcCleaver()).GetComponent<WoFMInteractiveObject>();
            wpnIo.ItemData.Equip(Io);
            SetLocalVariable("asleep", 1);
            // turn off hearing initially - this will be turned back on after modal plays
            AssignDisallowedEvent(ScriptConsts.DISABLE_HEAR);
            // teleport orc into scene
            GameSceneController.Instance.AddMustCompleteAction(new TeleportAction((WoFMInteractiveObject)Io, new Vector2(636 - GameController.MAP_X_OFFSET, GameController.MAP_Y_OFFSET - 1338)));
            // start particles
            Particles.Instance.PlaySnoreAboveIo(Io.RefId);
            return ScriptConsts.ACCEPT;
        }
        public override int OnOutOfView()
        {
            return base.OnOutOfView();
        }
        public override int OnInView()
        {
            Io.Show = 1;
            return base.OnInView();
        }
    }
}
