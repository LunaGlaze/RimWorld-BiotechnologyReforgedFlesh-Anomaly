using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
    public class HediffComp_CompTipStringExtra : HediffComp
    {
        public HediffCompProperties_CompTipStringExtra Props => (HediffCompProperties_CompTipStringExtra)props;
        public override string CompTipStringExtra => Props.compTipStringExtra;
    }
}
