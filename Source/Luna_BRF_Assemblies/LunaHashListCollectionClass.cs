using System.Collections.Generic;
using Verse;

namespace Luna_BRF
{
    [StaticConstructorOnStartup]
    public static class LunaHashListCollectionClass
    {
		public static bool VEFloaded => ModLister.GetActiveModWithIdentifier("oskarpotocki.vanillafactionsexpanded.core", true) != null;
		//public static bool CEloaded => ModLister.GetActiveModWithIdentifier("CETeam.CombatExtended", true) != null;
		public static HashSet<Thing> floating_pawns;
		public static HashSet<Thing> waterstriding_pawns;
		public static HashSet<Thing> draftable_pawns;
		public static HashSet<Thing> nofleeing_pawns;
		//漂浮pawns列表操作
		public static void AddFloatingPawnToList(Thing thing)
		{
			if (!floating_pawns.Contains(thing))
			{
				floating_pawns.Add(thing);
			}
		}
		public static void RemoveFloatingPawnFromList(Thing thing)
		{
			if (floating_pawns.Contains(thing))
			{
				floating_pawns.Remove(thing);
			}
		}
		//水上行走pawns列表操作
		public static void AddWaterstridingPawnToList(Thing thing)
		{
			if (!waterstriding_pawns.Contains(thing))
			{
				waterstriding_pawns.Add(thing);
			}
		}
		public static void RemoveWaterstridingPawnFromList(Thing thing)
		{
			if (waterstriding_pawns.Contains(thing))
			{
				waterstriding_pawns.Remove(thing);
			}
		}
		//可征召生物
		public static bool IsDraftablePawn(this Pawn pawn)
		{
			return draftable_pawns.Contains(pawn);
		}
		public static bool IsDraftableControllablePawn(this Pawn pawn)
		{
			return pawn.IsDraftablePawn() && pawn.Faction != null && pawn.Faction.IsPlayer && pawn.MentalState == null;
		}
		public static void AddDraftablePawnToList(Thing thing)
		{
			if (!draftable_pawns.Contains(thing))
			{
				draftable_pawns.Add(thing);
			}
		}
		public static void RemoveDraftablePawnFromList(Thing thing)
		{
			if (draftable_pawns.Contains(thing))
			{
				draftable_pawns.Remove(thing);
			}
		}
		//nofleeing
		public static void AddNotFleeingPawnToList(Thing thing)
		{
			if (!nofleeing_pawns.Contains(thing))
			{
				nofleeing_pawns.Add(thing);
			}
		}
		public static void RemoveNotFleeingPawnFromList(Thing thing)
		{
			if (nofleeing_pawns.Contains(thing))
			{
				nofleeing_pawns.Remove(thing);
			}
		}
		static LunaHashListCollectionClass()
		{
			floating_pawns = new HashSet<Thing>();
			waterstriding_pawns = new HashSet<Thing>();
			draftable_pawns = new HashSet<Thing>();
			nofleeing_pawns = new HashSet<Thing>();
		}
	}
}