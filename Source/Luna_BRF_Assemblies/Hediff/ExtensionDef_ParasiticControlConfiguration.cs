using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class ExtensionDef_ParasiticControlConfiguration : DefModExtension
    {
        public bool parasitismEndsWhenDowned = false;
        public bool damageBrain = false;
        public EntityCodexEntryDef codexEntry;
        public List<StartingHediff> giveHediffDefsAlso = new List<StartingHediff>();
        public List<HediffDef> removeHediffDefsAlso = new List<HediffDef>();
        public List<StartingHediff> endGiveHediffList = new List<StartingHediff>();
        public List<HediffDef> endRemoveHediffList = new List<HediffDef>();
        public List<StartingHediff> endGiveHediffListMalignant = new List<StartingHediff>();
    }
}
