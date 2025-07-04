using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class IncidentWorker_ClumpBaseAssault : IncidentWorker
	{
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			parms.faction = Faction.OfEntities;
			parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(LunaDefOf.BRF_ClumpGroup, parms);
			float num = Faction.OfEntities.def.MinPointsToGeneratePawnGroup(LunaDefOf.BRF_ClumpGroup);
			if (parms.points < num)
			{
				parms.points = (defaultPawnGroupMakerParms.points = num * 2f);
			}
			if (ModsConfig.AnomalyActive)
			{
				if(Find.Anomaly.LevelDef.anomalyThreatTier > 1)
				{
					if (Find.Anomaly.LevelDef.anomalyThreatTier <= 2)
					{
						parms.points = (defaultPawnGroupMakerParms.points += num);

					}else if (Find.Anomaly.LevelDef.anomalyThreatTier <= 3)
					{
						parms.points = (defaultPawnGroupMakerParms.points += 500 + num);

					}
					else if (Find.Anomaly.LevelDef.anomalyThreatTier <= 4)
					{
						parms.points = (defaultPawnGroupMakerParms.points += (500 + num) * 2);

                    }
                    else
					{
						parms.points = (defaultPawnGroupMakerParms.points += (500 + num) * 4);
					}
				}
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms).ToList();
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
			Find.EntityCodex.SetDiscovered(LunaEntityCodexEntryDefOf.BRF_Clump, ThingDef.Named("BRF_Clump"));
			return true;
		}
	}
}
