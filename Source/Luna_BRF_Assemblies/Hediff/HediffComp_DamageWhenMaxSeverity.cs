using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Luna_BRF
{
    public class HediffComp_DamageWhenMaxSeverity : HediffComp
    {
        public HediffCompProperties_DamageWhenMaxSeverity Props => (HediffCompProperties_DamageWhenMaxSeverity)props;
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            float severity = parent.Severity;
            float maxSeverity = Def.maxSeverity;
            if(severity >= maxSeverity)
            {
                if (!Props.withoutMechanoid || (Props.withoutMechanoid && !base.Pawn.RaceProps.IsMechanoid) )
                {
                    ApplyDamage(base.Pawn, Props.damAmount);
                    if (Props.addHediff && Props.hediffDefAdd != null)
                    {
                        AddHediff(base.Pawn, Props.hediffDefAdd);
                    }
                }
                base.Pawn.health.RemoveHediff(parent);
            }
        }
        public void AddHediff(Pawn Pawn , HediffDef hediffDef)
        {
            Hediff TargetHediffOnPawn = Pawn.health?.hediffSet?.GetFirstHediffOfDef(hediffDef);
            float randomSeverity = Rand.Range(0.15f, 0.30f);
            if (TargetHediffOnPawn != null)
            {
                TargetHediffOnPawn.Severity += randomSeverity;
            }
            else
            {
                Hediff hediff = HediffMaker.MakeHediff(hediffDef, Pawn);
                hediff.Severity = randomSeverity;
                Pawn.health.AddHediff(hediff);
            }
        }
        public void ApplyDamage(Pawn panw,int totalDamage)
        {
            if (totalDamage > 0)
            {
                if (Props.surgeryInjuries)
                {
                    HealthUtility.GiveRandomSurgeryInjuries(panw, totalDamage, null);
                }
                else if (Props.customDamage || Props.isFleshPulse)
                {
                    List<DamageDef> damageDefs = null;
                    if (Props.customDamage)
                    {
                        if (Props.damageDefsList != null) { Debug.LogError($"Hediff {parent}'s xml file is missing the damageDefsList in HediffComp_DamageWhenMaxSeverity."); }
                        else { damageDefs = Props.damageDefsList; }
                    }
                    if (Props.isFleshPulse)
                    {
                        damageDefs.Add(LunaDefOf.BRF_FleshPulse);
                    }
                    GiveRandomDamage(panw, totalDamage, damageDefs);
                }
            }
        }
        public static void GiveRandomDamage(Pawn p, int totalDamage, List<DamageDef> damageDefs)
        {
            if (p != null)
            {
                // 获取角色所有非概念性且不缺失的身体部位
                IEnumerable<BodyPartRecord> source = from x in p.health.hediffSet.GetNotMissingParts()
                                                     where !x.def.conceptual
                                                     select x;

                source = source.Where((BodyPartRecord x) => GetMinHealthOfPartsWeWantToAvoidDestroying(x, p) >= 1f);

                // 确保大脑不因伤害过高而直接损毁
                BodyPartRecord brain = p.health.hediffSet.GetBrain();
                if (brain != null)
                {
                    float maxBrainHealth = brain.def.GetMaxHealth(p);
                    source = source.Where((BodyPartRecord x) => x != brain || p.health.hediffSet.GetPartHealth(x) >= maxBrainHealth * 0.5f + 1f);
                }

                // 循环施加伤害，直到达到总伤害值或无可用部位
                while (totalDamage > 0 && source.Any())
                {
                    BodyPartRecord bodyPartRecord = source.RandomElement();
                    float partHealth = p.health.hediffSet.GetPartHealth(bodyPartRecord);

                    // 计算伤害值并调整，确保伤害不会在特定部位过度溢出
                    int num = Mathf.Max(3, GenMath.RoundRandom(partHealth * Rand.Range(0.5f, 1f)));
                    float minHealthOfPartsWeWantToAvoidDestroying = GetMinHealthOfPartsWeWantToAvoidDestroying(bodyPartRecord, p);
                    if (minHealthOfPartsWeWantToAvoidDestroying - (float)num < -0.5f)
                    {
                        num = Mathf.RoundToInt(minHealthOfPartsWeWantToAvoidDestroying - 0.5f);
                    }
                    if (bodyPartRecord == brain && partHealth - (float)num < brain.def.GetMaxHealth(p) * 0.5f)
                    {
                        num = Mathf.Max(Mathf.RoundToInt(partHealth - brain.def.GetMaxHealth(p) * 0.5f), 1);
                    }

                    if (num > 0)
                    {
                        // 从传入的伤害类型列表中随机选择一个
                        DamageDef chosenDamageDef = damageDefs.RandomElement();
                        DamageInfo dinfo = new DamageInfo(chosenDamageDef, num, 0f, -1f, null, bodyPartRecord);
                        dinfo.SetIgnoreArmor(ignoreArmor: true);
                        dinfo.SetIgnoreInstantKillProtection(ignore: true);

                        // 应用伤害并减少总伤害值
                        p.TakeDamage(dinfo);
                        totalDamage -= num;
                        continue;
                    }
                    break;
                }
            }
        }
        private static float GetMinHealthOfPartsWeWantToAvoidDestroying(BodyPartRecord part, Pawn pawn)
        {
            float num = 999999f;
            while (part != null)
            {
                if (ShouldRandomDamageAvoidDestroying(part, pawn))
                {
                    num = Mathf.Min(num, pawn.health.hediffSet.GetPartHealth(part));
                }
                part = part.parent;
            }
            return num;
        }
        private static bool ShouldRandomDamageAvoidDestroying(BodyPartRecord part, Pawn pawn)
        {
            if (part == pawn.RaceProps.body.corePart)
            {
                return true;
            }
            if (part.def.tags.Any((BodyPartTagDef x) => x.vital))
            {
                return true;
            }
            for (int i = 0; i < part.parts.Count; i++)
            {
                if (ShouldRandomDamageAvoidDestroying(part.parts[i], pawn))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
