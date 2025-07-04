using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Grammar;
using Verse.Sound;

namespace Luna_BRF
{
	[StaticConstructorOnStartup]
	public class LunaBRFBaseUtility
    {
		public static int AddedAndImplantedPartsFlesh(Pawn pawn)
		{
			int num = pawn.health.hediffSet.CountAddedAndImplantedPartsFlesh();
			if (ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.Xenogenes.Any())
			{
				num++;
			}
			return num;
		}
	}
}
