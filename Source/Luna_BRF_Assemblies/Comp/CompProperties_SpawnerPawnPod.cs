using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class CompProperties_SpawnerPawnPod : CompProperties
    {
        public float hatcherDaystoSpawn = 1f;
        public bool primordialFleshSpawer;
        public PawnKindDef spawnedPawn;
        public CompProperties_SpawnerPawnPod()
        {
            compClass = typeof(CompSpawnerPawnPod);
        }
    }
}
