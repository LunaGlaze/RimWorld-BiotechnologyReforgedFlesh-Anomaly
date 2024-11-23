using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class CompAbilityEffect_EntityNecrophagia : CompAbilityEffect
    {
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (parent.pawn.TryGetComp<CompNecrophagistEntity>(out var comp))
			{
				comp.StartDigesting(parent.pawn.Position, target);
			}
		}
	}
}
