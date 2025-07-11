﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI.Group;
using Verse;
using Verse.Noise;

namespace Luna_BRF
{
    public class IncidentWorker_BloodvineZombieSwarm : IncidentWorker_EntitySwarm
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            base.TryExecuteWorker(parms);
            Find.EntityCodex.SetDiscovered(LunaEntityCodexEntryDefOf.BRF_BloodvineZombie, ThingDef.Named("BRF_BloodvineZombie"));
            return true;
        }
        protected override PawnGroupKindDef GroupKindDef => LunaDefOf.BRF_BloodvineZombieSwarmer;

        protected override LordJob GenerateLordJob(IntVec3 entry, IntVec3 dest)
        {
            return new LordJob_AssaultColony(Faction.OfEntities, canKidnap: false, canTimeoutOrFlee: false, sappers: false, useAvoidGridSmart: false, canSteal: false);
        }
        protected override void SendLetter(IncidentParms parms, List<Pawn> entities)
        {
            SendStandardLetter(def.letterLabel, def.letterText, def.letterDef, parms, entities);
        }
    }
}