using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace Luna_BRF
{
    public class Projectile_ExplosionCustomGasPost : Projectile_Explosive
    {
        public ExtensionDef_ExplosionCustomGasPost Props => def.GetModExtension<ExtensionDef_ExplosionCustomGasPost>();
        protected override void Explode()
        {
            if (base.Map != null && Props != null)
            {
                //获取方形区域为 GenAdj.OccupiedRect(base.Position, base.Rotation, IntVec2.One).ExpandedBy(Props.postExplosionGasRang).Cells
                foreach (IntVec3 cell in GenRadial.RadialCellsAround(base.Position, Props.postExplosionGasRang, true))
                {
                    if (cell.InBounds(base.Map))
                    {
                        Thing thing = ThingMaker.MakeThing(Props.postExplosionGasType);
                        thing.Rotation = Rot4.North;
                        thing.Position = cell;
                        thing.SpawnSetup(base.Map, false);
                    }
                }
            }
            base.Explode();
            if (Props.extraEMP)
            {
                Thing instigator = launcher;
                GenExplosion.DoExplosion(
                    center: base.Position,
                    map: base.Map,
                    radius: def.projectile.explosionRadius,
                    damType: DamageDefOf.EMP,
                    instigator: instigator,
                    damAmount: Props.extraEMPDamage,
                    armorPenetration: 1f);
            }
        }
    }
}
