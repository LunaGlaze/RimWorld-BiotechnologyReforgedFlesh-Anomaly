using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;


namespace Luna_BRF
{
    public class Luna_DestroyHeartBomb : Projectile_Explosive
    {
        public ExtensionDef_LunaDestroyHeartBomb Props => def.GetModExtension<ExtensionDef_LunaDestroyHeartBomb>();

        protected override void Explode()
        {
            float rand = Rand.Value;
            Map map = base.Map;
            float explosionRadius = def.projectile.explosionRadius;
            if (rand <= Props.destroyHeartChance)
            {
                List<Thing> affectedThings = new List<Thing>();
                foreach (IntVec3 c in GenRadial.RadialCellsAround(base.Position, explosionRadius, true))
                {
                    if (c.InBounds(map))
                    {
                        List<Thing> thingsInCell = c.GetThingList(map);
                        foreach (Thing target in thingsInCell)
                        {
                            if (target != this && target is Building_FleshmassHeart Heart)
                            {
                                affectedThings.Add(Heart);
                            }
                        }
                    }
                }
                foreach (Building_FleshmassHeart Heart in affectedThings)
                {
                    Heart?.StartTachycardiacOverload();
                }
            }
            base.Explode();
            if (Props.extraEMP)
            {
                Thing instigator = launcher;
                GenExplosion.DoExplosion(
                    center: base.Position,
                    map: map,
                    radius: explosionRadius,
                    damType: DamageDefOf.EMP,
                    instigator: instigator,
                    damAmount: Props.extraEMPDamage,
                    armorPenetration: 1f);
            }

        }
    }
}
