using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.UI.SceneControllers;

namespace WoFM.Singletons
{
    public class WoFMCombat : Combat
    {
        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject
                {
                    name = "WoFMCombat"
                };
                Instance = go.AddComponent<WoFMCombat>();
                DontDestroyOnLoad(go);
            }
        }
        public const int USE_LUCK = 1;
        public const int PLAYER_WOUNDS_CREATURE = 0;
        public const int CREATURE_WOUNDS_PLAYER = 1;
        public const int AVOIDED_EACH_OTHERS_BLOWS = 2;

        public override float ComputeDamages(BaseInteractiveObject srcIo, BaseInteractiveObject wpnIo, BaseInteractiveObject targetIo, long flags)
        {
            bool stop = false;
            ComputeDamagesParameters parameters = new ComputeDamagesParameters();
            // set source as event sender and send target message that someone is being aggressive to us
            Script.Instance.EventSender = srcIo;
            Script.Instance.SendIOScriptEvent(targetIo, ScriptConsts.SM_057_AGGRESSION, null, "");
            // if source or target is null, stop processing
            if (srcIo == null
                || targetIo == null)
            {
                stop = true;
                print("need to stop because null");
            }

            // check for fixable target
            if (!stop)
            {
                // if target is a fixable item, fix and stop processing
                if (!targetIo.HasIOFlag(IoGlobals.IO_01_PC)
                    && !targetIo.HasIOFlag(IoGlobals.IO_03_NPC))
                {
                    // not targeting a PC or NPC
                    if (targetIo.HasIOFlag(IoGlobals.IO_17_FIX))
                    {
                        FixTarget(srcIo, targetIo, flags);
                    }
                    stop = true;
                }
            }
            if (!stop)
            {
                // player attack
                if (srcIo.HasIOFlag(IoGlobals.IO_01_PC))
                {
                    DoPCAttackDamages(srcIo, targetIo, flags, ref parameters);
                }
                else if (srcIo.HasIOFlag(IoGlobals.IO_03_NPC)) // NPC attack
                {
                    DoNPCAttackDamages(srcIo, targetIo, flags, ref parameters);
                }
                else
                {
                    stop = true;
                }
            }

            if (!stop)
            {
                DoApplyArmorToDamage(srcIo, targetIo, ref parameters);
            }

            if (!stop)
            {
                // do final calculation of attack hitting
                bool hits = true;
                if (hits)
                {
                    // play sound of weapon hitting flesh
                    // ARX_SOUND_PlayCollision("FLESH", wmat, power, 1.f, &pos, io_source);
                    if (parameters.damages > 0f)
                    {
                        // TODO - apply critical modifier to damage

                        if (targetIo.HasIOFlag(IoGlobals.IO_01_PC))
                        {
                            // show effects to player on screen
                            // push/shake player's avatar, show blood spatter
                            CombatController.Instance.PlayHit(targetIo.RefId, parameters.damages);
                            // damage PC
                            targetIo.PcData.DamagePlayer(parameters.damages, 0, srcIo.RefId);
                            // ARX_DAMAGES_DamagePlayerEquipment(dmgs);
                        }
                        else
                        {
                            // show effects to NPC on screen
                            // push/shake NPC's avatar, show blood spatter
                            CombatController.Instance.PlayHit(targetIo.RefId, parameters.damages);
                            // damage NPC
                            targetIo.NpcData.DamageNPC(
                                parameters.damages, // source
                                srcIo.RefId, // source
                                false);
                        }
                    }
                }
            }

            return parameters.damages;
        }
        #region Process Compute Damages
        private void DoApplyArmorToDamage(BaseInteractiveObject srcIo, BaseInteractiveObject targetIo, ref ComputeDamagesParameters parameters)
        {
            float absorb = 0;
            // get target's armor modifier
            if (targetIo.HasIOFlag(IoGlobals.IO_01_PC))
            {
                if (targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_SHIELD) >= 0
                    && Interactive.Instance.HasIO(targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_SHIELD)))
                {
                    // player equipped with a shield.
                    // roll a 6 to absorb 1 damage
                    if (Diceroller.Instance.RolldX(6) == 6)
                    {
                        absorb = 1f;
                    }
                }
            }
            else
            {
                // TODO - figure out NPC armor
            }
            // get armor material
            if (targetIo.Armormaterial != null)
            {
                parameters.amat = targetIo.Armormaterial;
            }
            if (targetIo.HasIOFlag(IoGlobals.IO_01_PC))
            {
                if (targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_SHIELD) >= 0
                    && Interactive.Instance.HasIO(targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_SHIELD)))
                {
                    WoFMInteractiveObject io = (WoFMInteractiveObject)Interactive.Instance.GetIO(targetIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_SHIELD));
                    if (io.Armormaterial != null)
                    {
                        parameters.amat = io.Armormaterial;
                    }
                    io = null;
                }
            }
            parameters.damages -= absorb;

            // play collision sound
            // ARX_SOUND_PlayCollision(amat, wmat, power, 1.f, &pos, io_source);
        }
        private void DoNPCAttackDamages(BaseInteractiveObject srcIo, BaseInteractiveObject targetIo, long flags, ref ComputeDamagesParameters parameters)
        {
            // get weapon material
            if (srcIo.Weaponmaterial != null)
            {
                parameters.wmat = srcIo.Weaponmaterial;
            }
            if (srcIo.NpcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON) >= 0
                && Interactive.Instance.HasIO(srcIo.NpcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON)))
            {
                WoFMInteractiveObject io = (WoFMInteractiveObject)Interactive.Instance.GetIO(srcIo.NpcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON));
                if (io.Weaponmaterial != null)
                {
                    parameters.wmat = io.Weaponmaterial;
                }
                io = null;
            }

            float attack = srcIo.NpcData.GetFullAttributeScore("DMG");

            bool sufferingFromCurse = false;
            if (sufferingFromCurse)
            {
                attack *= 0.5f;
            }
            // TODO - calculate damages
            parameters.damages = attack;
            // TODO - calculate critical hit
            bool critical = false;
            bool criticalCalculationResult = false;
            if (criticalCalculationResult
                && Script.Instance.SendIOScriptEvent(srcIo, ScriptConsts.SM_054_CRITICAL, null, "") != ScriptConsts.REFUSE)
            {
                critical = true;
            }

            // TODO - calculate backstab modifier
            bool backstabCalculationResult = false;
            if (backstabCalculationResult
                && Script.Instance.SendIOScriptEvent(srcIo, ScriptConsts.SM_056_BACKSTAB, null, "") != ScriptConsts.REFUSE)
            {
                parameters.backstab = 1.5f;
            }
        }
        private void DoPCAttackDamages(BaseInteractiveObject srcIo, BaseInteractiveObject targetIo, long flags, ref ComputeDamagesParameters parameters)
        {
            // re-compute PC stats
            srcIo.PcData.ComputeFullStats();

            // get weapon material
            if (srcIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON) >= 0
                && Interactive.Instance.HasIO(srcIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON)))
            {
                WoFMInteractiveObject io = (WoFMInteractiveObject)Interactive.Instance.GetIO(srcIo.PcData.GetEquippedItem(EquipmentGlobals.EQUIP_SLOT_WEAPON));
                if (io.Weaponmaterial != null)
                {
                    parameters.wmat = io.Weaponmaterial;
                }
                io = null;
            }

            float attack = srcIo.PcData.GetFullAttributeScore("DMG");

            // TODO - calculate critical hit
            bool critical = false;
            bool criticalCalculationResult = false;
            if (criticalCalculationResult
                && Script.Instance.SendIOScriptEvent(srcIo, ScriptConsts.SM_054_CRITICAL, null, "") != ScriptConsts.REFUSE)
            {
                critical = true;
            }

            // TODO - calculate damages
            parameters.damages = attack;
            if ((flags & USE_LUCK) == USE_LUCK)
            {
                parameters.damages++;
            }

            // TODO - calculate backstab modifier
            if (targetIo.NpcData.HasNPCFlag(IoGlobals.NPCFLAG_BACKSTAB))
            {
                bool backstabCalculationResult = false;
                if (backstabCalculationResult
                 && Script.Instance.SendIOScriptEvent(srcIo, ScriptConsts.SM_056_BACKSTAB, null, "") != ScriptConsts.REFUSE)
                {
                    parameters.backstab = 1.5f;
                }
            }
        }
        private void FixTarget(BaseInteractiveObject srcIo, BaseInteractiveObject targetIo, long flags)
        {
            if (srcIo.HasIOFlag(IoGlobals.IO_01_PC))
            {
                srcIo.PcData.ComputeFullStats();
                DamageFix(targetIo, srcIo.PcData.GetFullAttributeScore("DMG"), srcIo.RefId, flags);
            }
            else if (srcIo.HasIOFlag(IoGlobals.IO_03_NPC))
            {
                srcIo.NpcData.ComputeFullStats();
                DamageFix(targetIo, srcIo.PcData.GetFullAttributeScore("DMG"), srcIo.RefId, flags);
            }
            else
            {
                DamageFix(targetIo, 1, srcIo.RefId, flags);
            }
        }
        #endregion
        /// <summary>
        /// Checks the result of a round of combat.
        /// </summary>
        /// <param name="playerIo"></param>
        /// <param name="targetIo"></param>
        /// <returns></returns>
        public int StrikeCheck(WoFMInteractiveObject playerIo, WoFMInteractiveObject targetIo)
        {
            int result = -1;
            playerIo.PcData.ComputeFullStats();
            int playerAttackStrength = Diceroller.Instance.RollXdY(2, 6) + (int)playerIo.PcData.GetFullAttributeScore("SKL");
            // assume target is NPC
            targetIo.NpcData.ComputeFullStats();
            int creatureAttackStrength = Diceroller.Instance.RollXdY(2, 6) + (int)targetIo.NpcData.GetFullAttributeScore("SKL");
            if (playerAttackStrength > creatureAttackStrength)
            {
                result = PLAYER_WOUNDS_CREATURE;
            }
            else if (creatureAttackStrength > playerAttackStrength)
            {
                result = CREATURE_WOUNDS_PLAYER;
            }
            else
            {
                result = AVOIDED_EACH_OTHERS_BLOWS;
            }
            // return result;
            return PLAYER_WOUNDS_CREATURE;
        }
        /// <summary>
        /// Legacy code.
        /// </summary>
        /// <param name="srcIo"></param>
        /// <param name="wpnIo"></param>
        /// <param name="flags"></param>
        /// <param name="targ"></param>
        /// <returns></returns>
        public bool StrikeCheck(WoFMInteractiveObject srcIo, WoFMInteractiveObject wpnIo, long flags, int targ)
        {
            bool ret = false;

            // if (TRUEFIGHT) ratioaim = 1.f;

            int source = Interactive.Instance.GetInterNum(srcIo);
            int weapon = Interactive.Instance.GetInterNum(wpnIo);
            /*
            long nbact = io_weapon->obj->nbaction;
            float drain_life = ARX_EQUIPMENT_GetSpecialValue(io_weapon, IO_SPECIAL_ELEM_DRAIN_LIFE);
            float paralyse = ARX_EQUIPMENT_GetSpecialValue(io_weapon, IO_SPECIAL_ELEM_PARALYZE);
            */
            // if this were a 3D game, loop through the number of actions the striking weapon has
            // for (long j = 0; j < nbact; j++)
            // create a sphere containing the strike point
            // check everything in the sphere to see if anything was hit
            // if (CheckEverythingInSphere(&sphere, source, targ))
            // loop through all items that were hit
            // for (long jj = 0; jj < MAX_IN_SPHERE_Pos; jj++)
            float dmgs = 0f;
            // check to see if object hit wasn't a body chunk
            // if (ValidIONum(EVERYTHING_IN_SPHERE[jj]) && (!(inter.iobj[EVERYTHING_IN_SPHERE[jj]]->ioflags & IO_BODY_CHUNK)))
            // go through target's faces to see if any were hit
            // for (long ii = 0; ii < target->obj->nbfaces; ii++)

            // if anything was hit, get the blood color
            // if (hitpoint >= 0)
            // if an NPC was hit, get its blood color
            // else color is white

            // don't know what this flag means
            // if (!(flags & 1))
            // if anything was hit, compute damages
            // if (hitpoint >= 0)
            // dmgs = ARX_EQUIPMENT_ComputeDamages(io_source, io_weapon, target, ratioaim, &posi);
            // else
            // dmgs = ARX_EQUIPMENT_ComputeDamages(io_source, io_weapon, target, ratioaim);
            // if target is an NPC
            // if (target->ioflags & IO_NPC)
            ret = true;

            // if the attack drains life, heal the attacker
            // if (drain_life > 0.f)
            // ARX_DAMAGES_HealInter(io_source, life_gain);
            // if the attack paralyzes, launch a paralyze spell
            // if (paralyse > 0.f)
            // ARX_SPELLS_Launch(SPELL_PARALYSE, weapon, SPELLCAST_FLAG_NOMANA | SPELLCAST_FLAG_NOCHECKCANCAST, 5, EVERYTHING_IN_SPHERE[jj], (long)(ptime));

            // if the attacker is the player, check weapon durability
            // if (io_source == inter.iobj[0])
            // ARX_DAMAGES_DurabilityCheck(io_weapon, 0.2f);

            // if there was damage or the target was an NPC and the target shows blood
            // if ((dmgs > 0.f) || ((target->ioflags & IO_NPC) && (target->spark_n_blood == SP_BLOODY)))
            // if the target was an NPC
            // if (target->ioflags & IO_NPC)
            // don't know
            // if (!(flags & 1))
            // spawn blood splatter
            // ARX_PARTICLES_Spawn_Splat(&pos, dmgs, color, hitpoint, target);
            // create a sphere at the point of splatter
            // if anything is in the sphere spawn ground splatter
            // if (CheckAnythingInSphere(&sp, 0, 1))
            // SpawnGroundSplat(&sp, &rgb, 30, 1);

            // if the target was the player, spawn screen splatter
            // spawn more blood splatter
            // ARX_PARTICLES_Spawn_Blood2(&pos, dmgs, color, hitpoint, target);

            // if (!ValidIONum(weapon)) io_weapon = NULL;
            // else
            // if the target was an item or something else, spawn a spark
            // spawn audible sound
            // ARX_NPC_SpawnAudibleSound(&pos, io_source);

            // if the target was the PC, set HIT_SPARK to 1
            // if the target was an NPC and there was damage or the target shows a spark
            // else if ((target->ioflags & IO_NPC) && ((dmgs <= 0.f) || (target->spark_n_blood == SP_SPARKING)))
            // show a spark and spawn a sound
            // ARX_PARTICLES_Spawn_Spark(&pos, (float)nb, 0);
            // ARX_NPC_SpawnAudibleSound(&pos, io_source);
            // if there was damage and the target was something to fix or an item
            // else if ((dmgs <= 0.f) && ((target->ioflags & IO_FIX) || (target->ioflags & IO_ITEM)))
            // show a spark and spawn a sound
            // ARX_PARTICLES_Spawn_Spark(&pos, (float)nb, 0);
            // ARX_NPC_SpawnAudibleSound(&pos, io_source);

            // if there was a spark, check weapons durability and play a sound
            // end loop checking everything in sphere


            // EERIEPOLY* ep;
            // check if anything in the background was hit
            // if (ep = CheckBackgroundInSphere(&sphere))
            // if source was the PC
            // if (io_source == inter.iobj[0])
            // if source has a flag to hit NPCs in the background
            // if (!(io_source->aflags & IO_NPC_AFLAG_HIT_BACKGROUND))
            // check weapon durabilty and play sound

            // ARX_PARTICLES_Spawn_Spark(&sphere.origin, rnd() * 10.f, 0);
            // ARX_NPC_SpawnAudibleSound(&sphere.origin, io_source);
            // end loop 

            return ret;
        }
        private class ComputeDamagesParameters
        {
            public string wmat = "BARE";
            public string amat = "FLESH";
            public float damages;
            public float backstab;
        }
    }
}
