using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Luna_BRF
{
    public class IncidentWorker_ScarletCerebralJellyfishAssault : IncidentWorker
    {
        private static readonly LargeBuildingSpawnParms BurrowLikeSpawnParms = new LargeBuildingSpawnParms
        {
            maxDistanceToColonyBuilding = -1f,  // 不限制到建筑的最大距离
            minDistToEdge = 10,  // 距离地图边缘最小10格
            attemptNotUnderBuildings = true,  // 尝试不在建筑物下生成
            canSpawnOnImpassable = false,  // 不能生成在不可通行的地方
            attemptSpawnLocationType = SpawnLocationType.Outdoors  // 尝试在户外生成
        };
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!ModsConfig.AnomalyActive)
            {
                return false;
            }
            _ = (Map)parms.target;
            return base.CanFireNowSub(parms);
        }

        public LocalTargetInfo GetTargetAAA(Pawn pawn, Ability ability)
        {
            List<Pawn> pawns = new List<Pawn>();
            foreach (Pawn item in pawn.Map.mapPawns.AllPawnsSpawned)
            {
                if (item.health?.hediffSet?.GetFirstHediffOfDef(HediffDef.Named("BRF_ScarletCerebralJellyfishBrainInsertion")) == null && !item.AnimalOrWildMan() && item.health.hediffSet.GetBrain() != null && item.Faction != pawn.Faction && item.health.State != PawnHealthState.Down)
                {
                    pawns.Add(item);
                }
            }
            if (!pawns.NullOrEmpty())
            {
                float num = float.MaxValue;
				Pawn result = null;
				foreach (Pawn item in pawns)
				{
					if (pawn.Position.InHorDistOf(item.Position, 25f) && (float)item.Position.DistanceToSquared(pawn.Position) < num && GenSight.LineOfSightToThing(pawn.Position, item, pawn.Map))
					{
						num = item.Position.DistanceToSquared(pawn.Position);
						result = item;
					}
				}
				if(result != null)
				{
					return new LocalTargetInfo(result);
				}
                Random rnd = new Random();
                int randIndex = rnd.Next(pawns.Count);
                Pawn target = pawns[randIndex];
                return new LocalTargetInfo(target);
            }
            return null;
        }
        protected Job TryGiveJob(Pawn pawn)
        {
            Ability ability = pawn.abilities?.GetAbility(LunaDefOf.BRF_ScarletCerebralJellyfishBrainInsertion);
            if (ability == null || !ability.CanCast)
            {
                return null;
            }
            LocalTargetInfo target = GetTargetAAA(pawn, ability);
            if (!target.IsValid)
            {
                return null;
            }
            return ability.GetJob(target, target);
        }
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            float num = parms.points/2.5f;
            Pawn scarletCerebralJellyfish = PawnGenerator.GeneratePawn(PawnKindDef.Named("BRF_ScarletCerebralJellyfish"), Faction.OfEntities);
            //根据袭击点数与异象等级给予额外生命值
            Hediff_RapidRegeneration hediff = (Hediff_RapidRegeneration)HediffMaker.MakeHediff(HediffDefOf.RapidRegeneration, scarletCerebralJellyfish);
            if (ModsConfig.AnomalyActive)
            {
                if (Find.Anomaly.LevelDef.anomalyThreatTier > 1)
                {
                    num *= Find.Anomaly.LevelDef.anomalyThreatTier * 0.6f;
                }
            }
            hediff.SetHpCapacity(Math.Max(num, 100));
            scarletCerebralJellyfish.health.AddHediff(hediff);
            // 使用BurrowLikeSpawnParms寻找合适的生成位置
            if (!LargeBuildingCellFinder.TryFindCell(out IntVec3 spawnCell, map, BurrowLikeSpawnParms.ForThing(ThingDefOf.PitBurrow) ))
            {
                return false;  // 如果找不到合适的位置，返回false
            }
            // 在找到的有效位置生成BRF_ScarletCerebralJellyfish并执行效果与工作
            if (spawnCell.IsValid && spawnCell.Standable(map))
            {
                Effecter eff = EffecterDefOf.Skip_ExitNoDelay.Spawn(spawnCell, map);
                eff.ticksLeft = 60;
                TargetInfo targetInfo = new TargetInfo(spawnCell, map);
                eff.Trigger(targetInfo, targetInfo,eff.ticksLeft);
                //eff.Cleanup();
                GenSpawn.Spawn(scarletCerebralJellyfish, spawnCell, map);
                //执行脑姦能力工作
                Job job = TryGiveJob(scarletCerebralJellyfish);
                if(job != null)
                {
                    scarletCerebralJellyfish.jobs.StartJob(job);
                }
            }
            else
            {
                return false;
            }
            SendStandardLetter(def.letterLabel, def.letterText, def.letterDef, parms, scarletCerebralJellyfish);
            return true;
        }
    }

    internal struct NewStruct
    {
        public int Item1;
        public int Item2;

        public NewStruct(int item1, int item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public override bool Equals(object obj)
        {
            return obj is NewStruct other &&
                   Item1 == other.Item1 &&
                   Item2 == other.Item2;
        }

        public override int GetHashCode()
        {
            int hashCode = -1030903623;
            hashCode = hashCode * -1521134295 + Item1.GetHashCode();
            hashCode = hashCode * -1521134295 + Item2.GetHashCode();
            return hashCode;
        }

        public void Deconstruct(out int item1, out int item2)
        {
            item1 = Item1;
            item2 = Item2;
        }

        public static implicit operator (int, int)(NewStruct value)
        {
            return (value.Item1, value.Item2);
        }

        public static implicit operator NewStruct((int, int) value)
        {
            return new NewStruct(value.Item1, value.Item2);
        }
    }
}
