using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Luna_BRF
{
    public class QuestPart_TransporterPawns_BRF_RebirthPsycorpseReplace : QuestPart_TransporterPawns
    {
        public override void Process(Pawn pawn)
        {
            if (!ModsConfig.AnomalyActive || !Find.EntityCodex.Discovered(LunaEntityCodexEntryDefOf.VoidMonolith))
            {
                return;
            }
            if (!pawn.health.hediffSet.HasHediff(LunaBRFDefOf.BRF_RebirthPsycorpse_AngelsDescend))
            {
                if(Find.Anomaly.Level < 2 || Find.EntityCodex.Discovered(LunaEntityCodexEntryDefOf.BRF_RebirthPsycorpse))
                {
                    if (Rand.Chance(0.005f))
                    {
                        pawn.health.AddHediff(HediffMaker.MakeHediff(LunaBRFDefOf.BRF_RebirthPsycorpse_AngelsDescend, pawn));
                    }
                }
                else
                {
                    if (Rand.Chance(0.025f))
                    {
                        pawn.health.AddHediff(HediffMaker.MakeHediff(LunaBRFDefOf.BRF_RebirthPsycorpse_AngelsDescend, pawn));
                    }
                }
            }
        }
    }
}
