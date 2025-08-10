using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace Luna_BRF
{
    public class CompSpawnerPawnPod : ThingComp
	{
		public CompProperties_SpawnerPawnPod Props => (CompProperties_SpawnerPawnPod)props;
		private float gestateProgress;
		public Faction hatcheeFaction;
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look(ref gestateProgress, "gestateProgress", 0f);
			Scribe_References.Look(ref hatcheeFaction, "hatcheeFaction");
		}
		public override void CompTick()
		{
			float num = 1f / (Props.hatcherDaystoSpawn * 60000f);
			gestateProgress += num;
			if (gestateProgress > 1f)
			{
				PodSpanwer();
			}
		}
		public void PodSpanwer()
		{
			try
			{
				hatcheeFaction = parent.Faction;
				PawnGenerationRequest request = new PawnGenerationRequest(Props.spawnedPawn, hatcheeFaction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn);
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, parent))
				{
					if (parent.Spawned)
					{
                        if (pawn.RaceProps.IsFlesh)
                        {
							FilthMaker.TryMakeFilth(parent.Position, parent.Map, ThingDefOf.Filth_AmnioticFluid);
						}
						if (Props.primordialFleshSpawer)
						{
							FilthMaker.TryMakeFilth(parent.Position, parent.Map, LunaBRFDefOf.Filth_BRF_PrimordialTwistedFlesh);
						}
					}
				}
				else
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
			}
			finally
			{
				parent.Destroy();
			}
		}
		public override string CompInspectStringExtra()
		{
			return "EggProgress".Translate() + ": " + gestateProgress.ToStringPercent() + "\n" + "HatchesIn".Translate() + ": " + "PeriodDays".Translate((Props.hatcherDaystoSpawn * (1f - gestateProgress)).ToString("F1"));
		}
	}
}
