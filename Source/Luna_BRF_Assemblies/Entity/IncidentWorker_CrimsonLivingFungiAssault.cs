using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class IncidentWorker_CrimsonLivingFungiAssault : IncidentWorker
	{
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			parms.faction = Faction.OfEntities;
			parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			int tier = 1;
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(LunaBRFDefOf.BRF_CrimsonLivingFungi, parms);
			float num = Faction.OfEntities.def.MinPointsToGeneratePawnGroup(LunaBRFDefOf.BRF_CrimsonLivingFungi);
			if (parms.points < num)
			{
				parms.points = (defaultPawnGroupMakerParms.points = num);
			}
			if (ModsConfig.AnomalyActive)
			{
				if (Find.Anomaly.LevelDef.anomalyThreatTier > 1)
				{
					tier = Find.Anomaly.LevelDef.anomalyThreatTier;
					parms.points = (defaultPawnGroupMakerParms.points *= (0.75f + Find.Anomaly.LevelDef.anomalyThreatTier * 0.25f));
				}
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms).ToList();
			float hpPoint = Math.Max(Math.Min(parms.points / 16f, 500 * Find.Storyteller.difficulty.threatScale) * (1 + (tier / 10)), 25);
			if (list.Count >= 3)
            {
				list.Clear();
				int rand = Rand.Range(3, 4);
				for(int i=0; i < rand; i++)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDef.Named("BRF_CrimsonLivingFungi"), Faction.OfEntities);
					list.Add(pawn);
				}
			}
			foreach(Pawn item in list)
			{
				LunaBRFHediffUtility.AddPointRapidRegeneration(item, hpPoint);
			}
			if (!parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms))
			{
				return false;
			}
			parms.raidArrivalMode.Worker.Arrive(list, parms);
			if (AnomalyIncidentUtility.IncidentShardChance(parms.points))
			{
				AnomalyIncidentUtility.PawnShardOnDeath(list.RandomElement());
			}
			//LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_GorehulkAssault(), parms.target as Map, list);
			SendStandardLetter(def.letterLabel, def.letterText, def.letterDef, parms, list);
			Find.EntityCodex.SetDiscovered(LunaEntityCodexEntryDefOf.BRF_CrimsonLivingFungi, ThingDef.Named("BRF_CrimsonLivingFungi"));
			return true;
		}
	}
}
