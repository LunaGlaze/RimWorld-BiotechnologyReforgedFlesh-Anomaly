using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class IncidentWorker_BloodstainedTickParasiticableAssaultLittle : IncidentWorker
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
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms).ToList();
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
