using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Luna_BRF
{
    public class CompLuna_DropButcherProductsDead : ThingComp
	{
		public CompProperties_DropButcherProductsDead Props
		{
			get
			{
				return (CompProperties_DropButcherProductsDead)this.props;
			}
		}

		public override IEnumerable<ThingDefCountClass> GetAdditionalLeavings(Map map, DestroyMode mode)
		{
			List<ThingDefCountClass> baseButcherProducts = new List<ThingDefCountClass>();
			Pawn pawn = (Pawn)this.parent;
			baseButcherProducts.AddRange(LunaBRFDeathUtility.GetPawnBaseButcherProductsThingDefCountClass(pawn));
			foreach (ThingDefCountClass additionalLeaving in baseButcherProducts)
			{
				yield return additionalLeaving;
			}
			if (Props.specialDropWithSize != null && Props.perBodySize > 0)
            {
				int per = ((int)Math.Floor(((float)pawn.BodySize - Props.minBodySize) / Props.perBodySize));
				yield return new ThingDefCountClass(Props.specialDropWithSize, per*Props.perCount);
			}
		}
	}
}
