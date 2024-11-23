using System;
using RimWorld;
using Verse;


namespace Luna_BRF
{
    public class ExtensionDef_LunaDestroyHeartBomb : DefModExtension
    {
        public float destroyHeartChance = 1f;
        public bool extraEMP;
        public int extraEMPDamage = 1;
    }
}
