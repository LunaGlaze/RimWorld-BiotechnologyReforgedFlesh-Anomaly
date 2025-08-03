using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Luna_BRF
{
    public class LunaInfectedTerrainSpread : ThingComp
    {
		public CompProperties_LunaInfectedTerrainSpread Props
		{
			get
			{
				return this.props as CompProperties_LunaInfectedTerrainSpread;
			}
		}
		public float workingSpeedMultiplier;
		protected bool Active
		{
			get
			{
				return this.parent.Spawned && (this.compFuel == null || this.compFuel.HasFuel);
			}
		}
		public override void CompTick()
		{
			base.CompTick();
			if (this.Active)
			{
				if (this.nextTickEffect == 0)
				{
					this.nextTickEffect = this.NextTickEffect;
				}
				if (Find.TickManager.TicksGame >= this.nextTickEffect)
                {
                    this.nextTickEffect = this.NextTickEffect;
                    Convert();
                    return;
				}
			}
			else
			{
				this.nextTickEffect++;
			}
		}
		public int NextTickEffect
		{
			get
			{
				return Find.TickManager.TicksGame + (int)(this.Props.spawnTickRate.RandomInRange / workingSpeedMultiplier);
			}
		}
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && TerrainValidator(parent.Position.GetTerrain(parent.Map)))
			{
				parent.Map.terrainGrid.SetTerrain(parent.Position, ConvertTerrainSet(parent.Position.GetTerrain(parent.Map)));
			}
			compFuel = parent.GetComp<CompRefuelable>();
        }
        private bool TryGetCellToCovert(out IntVec3 cell)
        {
			List<IntVec3> cells = (from allowcell in GenRadial.RadialCellsAround(parent.Position, Props.radius, true)
								   where allowcell.InBounds(parent.Map) && CanEverConvertCell(allowcell, parent.Map)
								   select allowcell).ToList();
            cell = cells.OrderBy((IntVec3 x) => x.DistanceTo(parent.Position)).FirstOrDefault();
            return cell != default(IntVec3);
        }
        public bool CanEverConvertCell(IntVec3 cell, Map map, TerrainDef skip = null)
        {
			if (!CellValidator(cell, map))
			{
				return false;
			}
            TerrainDef terrain = cell.GetTerrain(map);
            return TerrainValidator(terrain,skip);
        }
        protected bool CellValidator(IntVec3 cell, Map map)
        {
            if (!cell.InBounds(map))
            {
                return false;
            }
            Building edifice = cell.GetEdifice(map);
            if (edifice != null && edifice.def.passability == Traversability.Impassable)
            {
                return false;
            }
			return true;
        }
		public bool TerrainValidator(TerrainDef terrain, TerrainDef skip = null)
		{
			if (AllowTranTerrain(terrain))
			{
				return true;
			}
			if(Props.specialConvertTerrainDefs != null)
            {
                CompProperties_LunaInfectedTerrainSpread.SpecialConvertTerrainDefSet specialConvertTerrainDefSet;
                if (SpecialConvertTerrainValidator(terrain, out specialConvertTerrainDefSet))
				{
					return true;
				}
            }
            if (terrain.IsWater && terrain.layerable)
            {
                return false;
            }
            if (skip != null && terrain == skip)
            {
                return false;
            }
            if (terrain.passability == Traversability.Impassable)
            {
                return false;
            }
            if (terrain.isFoundation || terrain.IsRoad || terrain.IsSubstructure)
            {
                return false;
            }
            if ((terrain.natural || (terrain.affordances != null && (terrain.affordances.Contains(TerrainAffordanceDefOf.SmoothableStone) || terrain.affordances.Contains(LunaDefOf.Diggable)))) )
            {
                return true;
            }
            if (!terrain.canEverTerraform)
            {
                return false;
            }
            if (!terrain.affordances.Contains(TerrainAffordanceDefOf.Light))
            {
                return false;
            }
			return true;
        }
        public bool AllowTranTerrain(TerrainDef terrain)
		{
			List<TerrainDef> terrainDefs = new List<TerrainDef>();
			if (Props.allowTerrains.NullOrEmpty())
            {
				return false;
            }
            else
            {
				terrainDefs = Props.allowTerrains;
				return terrainDefs.Contains(terrain);
			}
        }
        public TerrainDef ConvertTerrainSet(TerrainDef terrain)
        {
			CompProperties_LunaInfectedTerrainSpread.SpecialConvertTerrainDefSet specialConvertTerrainDefSet;
            if (SpecialConvertTerrainValidator(terrain, out specialConvertTerrainDefSet))
			{
				return specialConvertTerrainDefSet.convertTerrainDef;
            }
            return Props.setConvertTerrainDef;
        }
        public bool SpecialConvertTerrainValidator(TerrainDef terrain, out CompProperties_LunaInfectedTerrainSpread.SpecialConvertTerrainDefSet specialConvertTerrainDefSet)
        {
            specialConvertTerrainDefSet = null;
            if (Props.specialConvertTerrainDefs != null)
            {
                specialConvertTerrainDefSet = Props.specialConvertTerrainDefs.FirstOrDefault((CompProperties_LunaInfectedTerrainSpread.SpecialConvertTerrainDefSet x) => x.requiredTerrainDef == terrain);
            }
			return specialConvertTerrainDefSet != null;
        }
        private void Convert()
        {
            if (TryGetCellToCovert(out var cell))
            {
                parent.Map.terrainGrid.SetTerrain(cell, ConvertTerrainSet(cell.GetTerrain(parent.Map)));
            }
        }
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo item in base.CompGetGizmosExtra())
			{
				yield return item;
			}
			yield return new Command_Action
			{
				defaultLabel = "BRF_WorkingSpeedMultiplierReduced".Translate(),
				defaultDesc = "BRF_WorkingSpeedMultiplierReduced_Desc".Translate(),
				action = delegate
				{
					float oldSpeed = workingSpeedMultiplier;

					if (oldSpeed > 1)
					{
						workingSpeedMultiplier -= 1f;
                    }
                    else
					{
						workingSpeedMultiplier -= 0.25f;
					}
					workingSpeedMultiplier = Mathf.Clamp(workingSpeedMultiplier, 0.01f, 10f);
					this.nextTickEffect = (int)( (this.nextTickEffect - Find.TickManager.TicksGame) * oldSpeed / workingSpeedMultiplier +  Find.TickManager.TicksGame);
					LunaSpawnerPawnWithFuel compSpawn = parent.GetComp<LunaSpawnerPawnWithFuel>();
					if (compSpawn != null)
                    {
						compSpawn.nextPawnSpawnTick = (int)((compSpawn.nextPawnSpawnTick - Find.TickManager.TicksGame) * oldSpeed / workingSpeedMultiplier + Find.TickManager.TicksGame);
					}
				},
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower")
			};
			yield return new Command_Action
			{
				defaultLabel = "BRF_WorkingSpeedMultiplierReset".Translate(),
				defaultDesc = "BRF_WorkingSpeedMultiplierReset_Desc".Translate(),
				action = delegate
				{
					float oldSpeed = workingSpeedMultiplier;
					workingSpeedMultiplier = 1f;
					this.nextTickEffect = (int)((this.nextTickEffect - Find.TickManager.TicksGame) * oldSpeed / workingSpeedMultiplier + Find.TickManager.TicksGame);
					LunaSpawnerPawnWithFuel compSpawn = parent.GetComp<LunaSpawnerPawnWithFuel>();
					if (compSpawn != null)
					{
						compSpawn.nextPawnSpawnTick = (int)((compSpawn.nextPawnSpawnTick - Find.TickManager.TicksGame) * oldSpeed / workingSpeedMultiplier + Find.TickManager.TicksGame);
					}
				},
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempReset")
			};
			yield return new Command_Action
			{
				defaultLabel = "BRF_WorkingSpeedMultiplierAdded".Translate(),
				defaultDesc = "BRF_WorkingSpeedMultiplierAdded_Desc".Translate(),
				action = delegate
				{
					float oldSpeed = workingSpeedMultiplier;

					if (oldSpeed >= 1)
					{
						workingSpeedMultiplier += 1f;
					}
					else
					{
                        switch (workingSpeedMultiplier)
                        {
							default:
								workingSpeedMultiplier = 1f;
								break;
							case 0.01f:
								workingSpeedMultiplier = 0.25f;
								break;
							case 0.25f:
								workingSpeedMultiplier = 0.5f;
								break;
							case 0.5f:
								workingSpeedMultiplier = 0.75f;
								break;
						}
					}
					workingSpeedMultiplier = Mathf.Clamp(workingSpeedMultiplier, 0.01f, 10f);
					this.nextTickEffect = (int)((this.nextTickEffect - Find.TickManager.TicksGame) * oldSpeed / workingSpeedMultiplier + Find.TickManager.TicksGame);
					LunaSpawnerPawnWithFuel compSpawn = parent.GetComp<LunaSpawnerPawnWithFuel>();
					if (compSpawn != null)
					{
						compSpawn.nextPawnSpawnTick = (int)((compSpawn.nextPawnSpawnTick - Find.TickManager.TicksGame) * oldSpeed / workingSpeedMultiplier + Find.TickManager.TicksGame);
					}
				},
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise")
			};
            if (DebugSettings.ShowDevGizmos)
            {
                yield return new Command_Action
                {
                    defaultLabel = "DEV: Convert",
                    action = Convert,
                    Disabled = !Active
                };
            }
        }
        public override void PostPostMake()
		{
			workingSpeedMultiplier = Props.baseWorkingSpeedMultiplier;
		}
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look(ref workingSpeedMultiplier, "BRF_CompWorkingSpeedMultiplier", 1f);
		}
		public override string CompInspectStringExtra()
        {
			string text = "BRF_CompWorkingSpeedMultiplier".Translate() + workingSpeedMultiplier;
			if(this.compFuel != null && this.compFuel.HasFuel)
            {
                if (!compFuel.Props.consumeFuelOnlyWhenUsed)
				{
					int numTicks = (int)((compFuel.Fuel / compFuel.Props.fuelConsumptionRate * 60000f) / workingSpeedMultiplier);
					text = text + "\n(" + "BRF_CompWorkingSpeedTrulyFuelConsume".Translate() + numTicks.ToStringTicksToPeriod() + ")";
				}
			}
			return text;

		}
		public int nextTickEffect;
		public CompRefuelable compFuel;
	}
}