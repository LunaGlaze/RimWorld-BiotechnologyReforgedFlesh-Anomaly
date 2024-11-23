using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Luna_BRF
{
	public class JobDriver_CorpseNecrophagistDigest : JobDriver
	{
		private CompNecrophagistEntity comp;

		private CompNecrophagistEntity Comp
		{
			get
			{
				CompNecrophagistEntity result;
				if ((result = comp) == null)
				{
					result = (comp = pawn.GetComp<CompNecrophagistEntity>());
				}
				return result;
			}
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		public override string GetReport()
		{
			return null;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			int digestionTicks = Comp.GetDigestionTicks();
			Toil toil = Toils_General.Wait(digestionTicks).WithProgressBarToilDelay(TargetIndex.None, digestionTicks);
			toil.FailOn(() => !Comp.Digesting);
			toil.PlaySustainerOrSound(SoundDefOf.Pawn_Devourer_Digesting);
			toil.AddFinishAction(Comp.CompleteDigestion);
			yield return toil;
		}
	}
}
