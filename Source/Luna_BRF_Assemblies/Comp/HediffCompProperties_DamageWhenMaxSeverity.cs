using System;
using System.Collections.Generic;
using Verse;

namespace Luna_BRF
{
    public class HediffCompProperties_DamageWhenMaxSeverity : HediffCompProperties
    {
        public HediffCompProperties_DamageWhenMaxSeverity()
        {
            compClass = typeof(HediffComp_DamageWhenMaxSeverity);
        }
        public List<DamageDef> damageDefsList;
        public int damAmount;
        public bool surgeryInjuries;
        public bool isFleshPulse;
        public bool customDamage;
        public bool withoutMechanoid = true;
        public bool addHediff;
        public HediffDef hediffDefAdd;
    }
}