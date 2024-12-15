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
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(LunaDefOf.BRF_BloodstainedTickParasiticable, parms);
			float num = Faction.OfEntities.def.MinPointsToGeneratePawnGroup(LunaDefOf.BRF_BloodstainedTickParasiticable);
			if (parms.points < num)
			{
				parms.points = (defaultPawnGroupMakerParms.points = num * 2f);
			}
			if (ModsConfig.AnomalyActive)
			{
				switch (Find.Anomaly.LevelDef.anomalyThreatTier)
				{
					case 1:
						parms.points = (defaultPawnGroupMakerParms.points += 250 + num);
						break;
					case 2:
						parms.points = (defaultPawnGroupMakerParms.points += (250 + num) * 2 );
						break;
				}
                if (Find.Anomaly.LevelDef.anomalyThreatTier > 2)
                {
					parms.points = (defaultPawnGroupMakerParms.points += (250 + num) * 4);
				}
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms).ToList();
			Pawn basePawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Spelopede, Faction.OfEntities);
			list.Add(basePawn);
			if (!parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms))
			{
				return false;
			}
			parms.raidArrivalMode.Worker.Arrive(list, parms);
			foreach (Pawn pawn in list)
            {
				Hediff hediff = HediffMaker.MakeHediff(LunaDefOf.BRF_BloodstainedTickParasiticed, pawn);
				pawn.health.AddHediff(hediff);
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
			}
			SendStandardLetter(def.letterLabel, def.letterText, def.letterDef, parms, list);
			return true;
		}
	}
}
