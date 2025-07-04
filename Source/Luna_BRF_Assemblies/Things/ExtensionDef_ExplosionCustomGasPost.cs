using System.Collections.Generic;
using RimWorld;
using Verse;


namespace Luna_BRF
{
    public class ExtensionDef_ExplosionCustomGasPost : DefModExtension
	{
		public ThingDef postExplosionGasType;
		public int postExplosionGasRang;
        public bool extraEMP;
        public int extraEMPDamage = 1;
    }
}
