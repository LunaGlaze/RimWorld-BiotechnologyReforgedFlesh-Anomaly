using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
    public class HediffCompProperties_CompTipStringExtra : HediffCompProperties
    {
        public HediffCompProperties_CompTipStringExtra()
        {
            compClass = typeof(HediffComp_CompTipStringExtra);
        }
        public string compTipStringExtra;
    }
}
