using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WoFM.Constants;

namespace WoFM.Flyweights
{
    public class WoFMNPC : IONpcData
    {
        /// <summary>
        /// the list of attributes and their matching names and modifiers.
        /// </summary>
        private static object[][] attributeMap = new object[][] {
            new object[] { "SKL", "Skill", WoFMGlobals.EQUIP_ELEMENT_SKILL },
            new object[] { "MSK", "Max Skill", WoFMGlobals.EQUIP_ELEMENT_MAX_SKILL },
            new object[] { "STM", "Stamina", WoFMGlobals.EQUIP_ELEMENT_STAMINA },
            new object[] { "MSTM", "Max Stamina", WoFMGlobals.EQUIP_ELEMENT_MAX_STAMINA },
            new object[] { "DMG", "Damage", WoFMGlobals.EQUIP_ELEMENT_DAMAGE },
        };
        public override bool CalculateBackstab()
        {
            throw new NotImplementedException();
        }

        public override bool CalculateCriticalHit()
        {
            throw new NotImplementedException();
        }

        public override float GetFullDamage()
        {
            throw new NotImplementedException();
        }

        public override BaseInteractiveObject GetIo()
        {
            throw new NotImplementedException();
        }

        public override float GetMaxLife()
        {
            throw new NotImplementedException();
        }

        public override int getPoisonned()
        {
            throw new NotImplementedException();
        }

        public override void ManagePoison()
        {
            throw new NotImplementedException();
        }

        public override void RecreatePlayerMesh()
        {
            throw new NotImplementedException();
        }

        protected override void AdjustMana(float dmg)
        {
            throw new NotImplementedException();
        }

        protected override void ApplyRulesModifiers()
        {
        }

        protected override void ApplyRulesPercentModifiers()
        {
        }

        protected override void AwardXpForNpcDeath(int xp, BaseInteractiveObject killerIO)
        {
            throw new NotImplementedException();
        }

        protected override void DamageNonLivingNPC(float dmg, int srcIoid, bool isSpellDamage)
        {
            throw new NotImplementedException();
        }

        protected override object[][] GetAttributeMap()
        {
            return attributeMap;
        }
        protected override float GetBaseMana()
        {
            throw new NotImplementedException();
        }

        protected override string GetLifeAttribute()
        {
            return "STM";
        }
        protected override bool HasLifeRemaining()
        {
            throw new NotImplementedException();
        }

        protected override void MoveToInitialPosition()
        {
            throw new NotImplementedException();
        }

        protected override void RestoreLifeToMax()
        {
            throw new NotImplementedException();
        }

        protected override void StopActiveAnimation()
        {
            throw new NotImplementedException();
        }

        protected override void StopIdleAnimation()
        {
            throw new NotImplementedException();
        }
    }
}
