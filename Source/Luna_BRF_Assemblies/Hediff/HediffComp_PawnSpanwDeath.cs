using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class HediffComp_PawnSpanwDeath : HediffComp
	{
		public HediffCompProperties_PawnSpanwDeath Props => (HediffCompProperties_PawnSpanwDeath)props;
        public override void Notify_PawnKilled()
		{
			if (parent.Severity >= Props.minActSeverity)
			{
				spawnPawn();
				if (ModsConfig.AnomalyActive && Props.codexEntry != null)
				{
					Find.EntityCodex.SetDiscovered(Props.codexEntry);
				}
				if (Props.filth != null)
				{
					FilthMaker.TryMakeFilth(base.Pawn.Position, base.Pawn.Map, Props.filth, Props.filthCount);
				}
				parent.Severity -= Props.severityFeduce;
			}
		}
		public Faction getFaction()
        {
            if (Props.sameFaction)
            {
				return parent.pawn.Faction;
            }
            else
            {
				Faction faction = Find.FactionManager.FirstFactionOfDef(Props.factionDef);
				return faction;
			}
        }
		private void spawnPawn()
		{
			Pawn pawn = parent.pawn;
			//Pawn pawn = base.Pawn
			Pawn pawn2 = PawnGenerator.GeneratePawn(new PawnGenerationRequest(Props.spanwPawnKind, getFaction(), PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, 0f, 0f));
			//GenSpawn.Spawn(pawn2, CellFinder.StandableCellNear(pawn.Position, pawn.Map, 2f), pawn.Map);
			GenSpawn.Spawn(pawn2, pawn.Position, pawn.Map, WipeMode.VanishOrMoveAside);
			CompInspectStringEmergence compInspectStringEmergence = pawn2.TryGetComp<CompInspectStringEmergence>();
			if (compInspectStringEmergence != null)
			{
				compInspectStringEmergence.sourcePawn = pawn;
			}
            if (Props.giveLetter)
			{
				TaggedString label = Props.letterLabel.Formatted(pawn.Named("PAWN"));
				TaggedString text = Props.letterText.Formatted(pawn.Named("PAWN"));
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatBig, pawn2);
			}
		}
	}
}