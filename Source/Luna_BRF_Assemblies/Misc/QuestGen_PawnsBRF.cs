using RimWorld;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
    public static class QuestGen_PawnsBRF
    {
        public struct GetPawnParms
        {
            public bool mustBeFactionLeader;

            public bool mustBeWorldPawn;

            public bool ifWorldPawnThenMustBeFree;

            public bool ifWorldPawnThenMustBeFreeOrLeader;

            public bool mustHaveNoFaction;

            public bool mustBeFreeColonist;

            public bool mustBePlayerPrisoner;

            public bool mustBeNotSuspended;

            public bool mustHaveRoyalTitleInCurrentFaction;

            public bool mustBeNonHostileToPlayer;

            public bool? allowPermanentEnemyFaction;

            public bool canGeneratePawn;

            public bool requireResearchedBedroomFurnitureIfRoyal;

            public PawnKindDef mustBeOfKind;

            public Faction mustBeOfFaction;

            public FloatRange seniorityRange;

            public TechLevel minTechLevel;

            public List<FactionDef> excludeFactionDefs;

            public bool allowTemporaryFactions;

            public bool allowHidden;

            public bool mustBeCapableOfViolence;

            public bool redressPawn;

            public PawnGenerationRequest GenerationRequest(PawnKindDef pawnKind, Faction faction, RoyalTitleDef fixedTitle)
            {
                bool flag = mustBeCapableOfViolence;
                return new PawnGenerationRequest(pawnKind, faction, PawnGenerationContext.NonPlayer, null, true, false, false, true, flag, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, fixedTitle);
            }
        }

        public static QuestPart_TransporterPawns_BRF_RebirthPsycorpseReplace BRF_RebirthPsycorpseReplace(this Quest quest, IEnumerable<Pawn> pawns = null, Thing pawnsInTransporter = null, string inSignal = null)
        {
            QuestPart_TransporterPawns_BRF_RebirthPsycorpseReplace questPart_TransporterPawns_BRF_RebirthPsycorpseReplace = new QuestPart_TransporterPawns_BRF_RebirthPsycorpseReplace();
            questPart_TransporterPawns_BRF_RebirthPsycorpseReplace.pawnsInTransporter = pawnsInTransporter;
            questPart_TransporterPawns_BRF_RebirthPsycorpseReplace.pawns = pawns?.ToList();
            questPart_TransporterPawns_BRF_RebirthPsycorpseReplace.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal");
            quest.AddPart(questPart_TransporterPawns_BRF_RebirthPsycorpseReplace);
            return questPart_TransporterPawns_BRF_RebirthPsycorpseReplace;
        }
    }
}
