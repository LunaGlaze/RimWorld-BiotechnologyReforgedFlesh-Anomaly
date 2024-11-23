using System;
using RimWorld;
using Verse;


namespace Luna_BRF
{
    public class ExtensionDef_LunaHediffAddBullet : DefModExtension
    {
        public float addHediffChance = 1f;
        public HediffDef HediffDefToAdd;
        public HediffDef NeedHediffDef;
        public HediffDef HediffDefTransform;
    }
}
