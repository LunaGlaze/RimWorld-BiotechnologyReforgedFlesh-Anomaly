using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class ThoughtWorker_HasFleshyBodyPart : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			int num = LunaBRFBaseUtility.AddedAndImplantedPartsFlesh(p);
			if (num > 0)
			{
				return ThoughtState.ActiveAtStage(num - 1);
			}
			return false;
		}
	}

}
