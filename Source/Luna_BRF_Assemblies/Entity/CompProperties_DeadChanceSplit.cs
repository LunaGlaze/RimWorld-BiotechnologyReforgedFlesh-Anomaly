using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class CompProperties_DeadChanceSplit : CompProperties
    {
        public CompProperties_DeadChanceSplit()
        {
            this.compClass = typeof(LunaDeadChanceSplit);
        }
        public float DeadSplitChance = 0.5f;
        public int dividePawnCount;
        public List<PawnKindDef> dividePawnKindOptions = new List<PawnKindDef>();

    }
}
