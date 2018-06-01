using Assets.Scripts.Blueholme.Flyweights;
using RPGBase.Constants;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Blueholme.Scriptables.Mobs
{
    public class FighterBase : HeroBase
    {
        /// <summary>
        /// On participating in a combat round.
        /// </summary>
        /// <returns></returns>
        public override int OnCombatFlurry()
        {
            /*
            // i am a fighter.  who are my opponents?
            int[] opponents = GetLocalIntArrayVariableValue("opponents_party");
            List<BHInteractiveObject> opponentIos = new List<BHInteractiveObject>();
            // get list of all living opponent
            for (int i = opponents.Length - 1; i >= 0; i--)
            {
                if (Interactive.Instance.HasIO(opponents[i]))
                {
                    BHInteractiveObject io = (BHInteractiveObject)Interactive.Instance.GetIO(opponents[i]);
                    if (io.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        // opponent is another PC
                        if (!io.PcData.IsDead())
                        {
                            opponentIos.Add(io);
                        }
                    }
                    else if (io.HasIOFlag(IoGlobals.IO_03_NPC))
                    {

                    }
                }
            }
            // go through list of opponents finding the best one to target.
            int recommendedTarget = -1;
            if (!HasLocalVariable("last_target"))
            {
                SetLocalVariable("last_target", -1);
            }
            else
            {
                recommendedTarget = GetLocalIntVariableValue("last_target");
            }
            if (recommendedTarget>=0
                && IsIoInList(opponentIos, recommendedTarget))
            {
                SetLocalVariable("last_target", recommendedTarget);
            }


            BHInteractiveObject wpnIo = null;
            int wpnId = io.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON);
            if (Interactive.Instance.HasIO(wpnId))
            {
                wpnIo = (BHInteractiveObject)Interactive.Instance.GetIO(wpnId);
            }
            // TODO check to see if target is dead
            int targId = io.Script.GetLocalIntVariableValue("target_practice");
            if (Interactive.Instance.HasIO(targId)
                && !IsIoDead(Interactive.Instance.GetIO(targId)))
            {
                // IO can strike.  check to see if attack is allowed
                int lastRoundAttack = io.Script.GetLocalIntVariableValue("last_round_attack");
                int wpnSpd = GetWeaponSpeed(io.PcData);
                // HVY weapons strike every other round
                if (wpnSpd == BHGlobals.HEAVY_WEAPON)
                {
                    if (Round - lastRoundAttack == 2)
                    {
                        Debug.Log("`shiudl strike");
                        StrikeCheck(io, wpnIo, 0, targId);
                        io.Script.SetLocalVariable("last_round_attack", Round);
                    }
                    else
                    {
                        Debug.Log("`no strike");
                        PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                        sb.Append(io.PcData.Name);
                        sb.Append(" readies ");
                        sb.Append(Gender.GENDER_POSSESSIVE[io.PcData.Gender]);
                        sb.Append(" weapon.\n");
                        Messages.Instance.Add(sb.ToString());
                        sb.ReturnToPool();
                    }
                }
                else
                {
                    Debug.Log("`shiudl strike");
                    StrikeCheck(io, wpnIo, 0, targId);
                    io.Script.SetLocalVariable("last_round_attack", Round);
                }
                if (IsIoDead(Interactive.Instance.GetIO(targId)))
                {
                    BaseInteractiveObject targIo = Interactive.Instance.GetIO(targId);
                    PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                    if (targIo.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        sb.Append(targIo.PcData.Name);
                    }
                    sb.Append(" dies!\n");
                    Messages.Instance.Add(sb.ToString());
                    sb.ReturnToPool();
                }
            }
            */
            return ScriptConsts.ACCEPT;
        }
        private bool IsIoInList(List<BHInteractiveObject> ioList, int refId)
        {
            bool f = false;
            for (int i = ioList.Count - 1; i >= 0; i--)
            {
                BHInteractiveObject io = ioList[i];
                if (io.RefId == refId)
                {
                    f = true;
                    break;
                }
            }
            return f;
        }
    }
}
