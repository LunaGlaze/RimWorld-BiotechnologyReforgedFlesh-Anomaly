using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Luna_BRF
{
    public class CreepJoinerWorker_RebirthPsycorpseDownside : BaseCreepJoinerWorker
    {
        public override void OnCreated()
        {
            base.Pawn.health.AddHediff(LunaBRFDefOf.BRF_RebirthPsycorpse_AngelsDescend);
        }

        public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
        {
        }
    }
}
