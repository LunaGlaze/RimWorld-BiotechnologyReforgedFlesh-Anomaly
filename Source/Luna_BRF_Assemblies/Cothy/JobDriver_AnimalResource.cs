using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Luna_BRF
{
	public class JobDriver_AnimalResource : JobDriver_GatherAnimalBodyResources
	{
		private float gatherProgress;

		protected override float WorkTotal => 1700f;

		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompAnimalProduct>();
		}

		public CompAnimalProduct GetSpecificComp(Pawn animal)
		{
			return animal.TryGetComp<CompAnimalProduct>();
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil wait = new Toil();
			wait.initAction = delegate
			{
				Pawn actor2 = wait.actor;
				Pawn pawn2 = (Pawn)job.GetTarget(TargetIndex.A).Thing;
				actor2.pather.StopDead();
				PawnUtility.ForceWait(pawn2, 15000, null, maintainPosture: true);
			};
			wait.tickAction = delegate
			{
				Pawn actor = wait.actor;
				actor.skills.Learn(SkillDefOf.Animals, 0.13f);
				gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed);
				if (gatherProgress >= WorkTotal)
				{
					GetSpecificComp((Pawn)(Thing)job.GetTarget(TargetIndex.A)).InformGathered(pawn);
					actor.jobs.EndCurrentJob(JobCondition.Succeeded);
					if (ModLister.HasActiveModWithName("Alpha Animals"))
					{
						actor.health.AddHediff(HediffDef.Named("AA_GatheredResource"));
					}
				}
			};
			wait.AddFinishAction(delegate
			{
				Pawn pawn = (Pawn)job.GetTarget(TargetIndex.A).Thing;
				if (pawn != null && pawn.CurJobDef == JobDefOf.Wait_MaintainPosture)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
				}
			});
			wait.FailOnDespawnedOrNull(TargetIndex.A);
			wait.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			wait.AddEndCondition(() => GetComp((Pawn)(Thing)job.GetTarget(TargetIndex.A)).ActiveAndFull ? JobCondition.Ongoing : JobCondition.Incompletable);
			wait.defaultCompleteMode = ToilCompleteMode.Never;
			wait.WithProgressBar(TargetIndex.A, () => gatherProgress / WorkTotal);
			wait.activeSkill = () => SkillDefOf.Animals;
			yield return wait;
		}
	}
}
