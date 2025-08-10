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
    public class QuestNode_BRF_RebirthPsycorpseReplace : QuestNode
    {
        [NoTranslate]
        public SlateRef<string> inSignalEnable;
        public SlateRef<Thing> shuttle;

        protected override void RunInt()
        {
            Slate slate = QuestGen.slate;
            string inSignal = QuestGenUtility.HardcodedSignalWithQuestID(inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal");
            QuestGen.quest.BRF_RebirthPsycorpseReplace(null, shuttle.GetValue(slate), inSignal);
        }
        protected override bool TestRunInt(Slate slate)
        {
            return true;
        }
    }
}
