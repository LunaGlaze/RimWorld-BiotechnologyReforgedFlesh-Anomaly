using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class IncidentWorker_BloodstainedTickParasiticableAssault : IncidentWorker
	{
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			parms.faction = Faction.OfEntities;
			parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(LunaBRFDefOf.BRF_BloodstainedTickParasiticable, parms);
			float num = Faction.OfEntities.def.MinPointsToGeneratePawnGroup(LunaBRFDefOf.BRF_BloodstainedTickParasiticable);
			if (parms.points < num)
			{
				parms.points = (defaultPawnGroupMakerParms.points = num * 2f);
			}
			if (ModsConfig.AnomalyActive)
			{
				if (Find.Anomaly.LevelDef.anomalyThreatTier > 1)
				{
					if (Find.Anomaly.LevelDef.anomalyThreatTier <= 2)
					{
						parms.points = (defaultPawnGroupMakerParms.points += 250 + num);

					}
					else if (Find.Anomaly.LevelDef.anomalyThreatTier <= 3)
					{
						parms.points = (defaultPawnGroupMakerParms.points += (250 + num) * 2 );

					}
					else if (Find.Anomaly.LevelDef.anomalyThreatTier <= 4)
					{
						parms.points = (defaultPawnGroupMakerParms.points += (250 + num) * 4);

					}
					else
					{
						parms.points = (defaultPawnGroupMakerParms.points += (500 + num) * 4);
					}
				}
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms).ToList();
			if (ModsConfig.AnomalyActive)
			{
				Pawn basePawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Megascarab, Faction.OfEntities);
				if (Find.Anomaly.LevelDef.anomalyThreatTier < 1 && Find.Anomaly.LevelDef.anomalyThreatTier >= 0)
				{
					basePawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Spelopede, Faction.OfEntities);
				}else{
					basePawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Megaspider, Faction.OfEntities);
					if (Find.Anomaly.LevelDef.anomalyThreatTier > 2)
					{
						list.Add(basePawn);
					}
				}
				list.Add(basePawn);
			}
			if (!parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms))
			{
				return false;
			}
			parms.raidArrivalMode.Worker.Arrive(list, parms);
			foreach (Pawn pawn in list)
            {
				Hediff hediff = HediffMaker.MakeHediff(LunaBRFDefOf.BRF_BloodstainedTickParasiticed, pawn);
				pawn.health.AddHediff(hediff);
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
			}
			SendStandardLetter(def.letterLabel, def.letterText, def.letterDef, parms, list);
			return true;
		}
	}
}
