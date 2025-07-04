using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
    public class HediffCompProperties_RebirthPsycorpse : HediffCompProperties
    {
        public int malnutritionTrigger = 30;

        public PawnKindDef turnPawnKind;

        public EntityCodexEntryDef codexEntry;

        public int minToGetHungry = 60000;

        public int maxToGetHungry = 1800000;

        public string letterLabel;

        public string letterText;

        public LetterDef letterDef;

        public FactionDef factionDef;

        public HediffCompProperties_RebirthPsycorpse()
        {
            compClass = typeof(HediffComp_RebirthPsycorpse);
        }
    }
}
