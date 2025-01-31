using RimWorld;
using Verse;
using Verse.AI;

namespace Luna_BRF
{
	public class MentalState_EntityCuseBerserk : MentalState_Berserk
	{
		public override bool ForceHostileTo(Thing t)
		{
			if(t.Faction == Faction.OfEntities)
			{
				return false;
			}
			return true;
		}

		public override bool ForceHostileTo(Faction f)
		{
			if (f == Faction.OfEntities)
			{
				return false;
			}
			return true;
		}
	}
}
