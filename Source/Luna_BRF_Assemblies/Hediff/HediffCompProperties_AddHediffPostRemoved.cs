using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
    public class HediffCompProperties_AddHediffPostRemoved : HediffCompProperties
    {
        public HediffDef hediffDef;
        public bool allowDeath = false;
        public bool allowDown = true;
        public HediffCompProperties_AddHediffPostRemoved()
        {
            compClass = typeof(HediffComp_AddHediffPostRemoved);
        }
    }
}
