using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class ExtensionDef_ClentaminatorList : DefModExtension
    {
        public List<ThingDef> blackListThingsBase = new List<ThingDef>();
        public List<ThingsWeightListOption> isTreeLists = new List<ThingsWeightListOption>();
        public List<ThingsWeightListOption> notTreeLists = new List<ThingsWeightListOption>();
        public List<PawnKindDef> whiteListPawn = new List<PawnKindDef>();
        public List<PawnGenOption> pawnLists = new List<PawnGenOption>();
        public FactionDef defaultFaction = null;
        public List<TerrainDef> whiteListTerrain = new List<TerrainDef>();
        public TerrainDef terrainToSet = null;
        public List<ThingDef> destroyGasTypes = new List<ThingDef>();
        public DamageDef takeDamageFleshWithoutEntity;
        public float damageFlesh = 2f;
        public HediffDef hediffDefGiveFlesh;
        public DamageDef takeDamageMechanoid;
        public float damageMechanoid = 2f;
        public HediffDef hediffDefGiveMechanoid;
        public bool fleshTypeWithoutBRFLung = false;
        public bool outFire = false;
    }
}