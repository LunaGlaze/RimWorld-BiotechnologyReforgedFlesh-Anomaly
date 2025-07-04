using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
    public class HediffCompProperties_DeathRefusalGiver : HediffCompProperties
    {
        public HediffCompProperties_DeathRefusalGiver()
        {
            compClass = typeof(HediffComp_DeathRefusalGiver);
        }
        public IntRange checkTickRate;
    }
}
