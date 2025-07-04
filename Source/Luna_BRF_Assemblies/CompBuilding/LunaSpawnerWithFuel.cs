using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Sound;

namespace Luna_BRF
{
	public class LunaSpawnerWithFuel : ThingComp
	{
		public CompProperties_LunaSpawnerWithFuel PropsSpawner
		{
			get
			{
				return (CompProperties_LunaSpawnerWithFuel)this.props;
			}
		}

		private bool PowerOn
		{
			get
			{
				CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
				return comp != null && comp.PowerOn;
			}
		}

		public bool HasFuel
		{
			get
			{
				CompRefuelable comp = this.parent.GetComp<CompRefuelable>();
				return comp != null && comp.HasFuel;
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.ResetCountdown();
			}
		}

		public override void CompTick()
		{
			int i = Math.Min(this.ticksUntilSpawn, 50);
			bool flag = parent.IsHashIntervalTick(i);
			if (flag)
			{
				this.TickInterval(i);
			}
		}

		public override void CompTickRare()
		{
			this.TickInterval(250);
		}

		private void TickInterval(int interval)
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
			if (comp != null)
			{
				if (!comp.Awake)
				{
					return;
				}
			}
			else if (this.parent.Position.Fogged(this.parent.Map))
			{
				return;
			}
			if (this.PropsSpawner.requiresPower && !this.PowerOn)
			{
				return;
			}
			if (this.PropsSpawner.requiresFuel && !this.HasFuel)
			{
				return;
			}
			this.ticksUntilSpawn -= interval;
			this.CheckShouldSpawn();
		}

		private void CheckShouldSpawn()
		{
			if (this.ticksUntilSpawn <= 0)
			{
				this.ResetCountdown();
				this.TryDoSpawn();
			}
		}

		public bool TryDoSpawn()
		{
			if (!this.parent.Spawned)
			{
				return false;
			}
			if (this.PropsSpawner.spawnMaxAdjacent >= 0)
			{
				int num = 0;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 c = this.parent.Position + GenAdj.AdjacentCellsAndInside[i];
					if (c.InBounds(this.parent.Map))
					{
						List<Thing> thingList = c.GetThingList(this.parent.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j].def == this.PropsSpawner.thingToSpawn)
							{
								num += thingList[j].stackCount;
								if (num >= this.PropsSpawner.spawnMaxAdjacent)
								{
									return false;
								}
							}
						}
					}
				}
			}
			IntVec3 center;
			if (CompSpawner.TryFindSpawnCell(this.parent, this.PropsSpawner.thingToSpawn, this.PropsSpawner.spawnCount, out center))
			{
				Thing thing = ThingMaker.MakeThing(this.PropsSpawner.thingToSpawn, null);
				thing.stackCount = this.PropsSpawner.spawnCount;
				if (thing == null)
				{
					Log.Error("Could not spawn anything for " + this.parent);
				}
				if (this.PropsSpawner.inheritFaction && thing.Faction != this.parent.Faction)
				{
					thing.SetFaction(this.parent.Faction, null);
				}
				Thing t;
				GenPlace.TryPlaceThing(thing, center, this.parent.Map, ThingPlaceMode.Direct, out t, null, null, default(Rot4));
				if (this.PropsSpawner.spawnForbidden)
				{
					t.SetForbidden(true, true);
				}
				if (this.PropsSpawner.showMessageIfOwned && this.parent.Faction == Faction.OfPlayer)
				{
					Messages.Message("MessageCompSpawnerSpawnedItem".Translate(this.PropsSpawner.thingToSpawn.LabelCap), thing, MessageTypeDefOf.PositiveEvent, true);
				}
				if (this.PropsSpawner.playSound && this.PropsSpawner.sound != null)
                {
					this.PropsSpawner.sound.PlayOneShotOnCamera(this.parent.Map);
				}
				return true;
			}
			return false;
		}

		public static bool TryFindSpawnCell(Thing parent, ThingDef thingToSpawn, int spawnCount, out IntVec3 result)
		{
			foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(parent).InRandomOrder(null))
			{
				if (intVec.Walkable(parent.Map))
				{
					Building edifice = intVec.GetEdifice(parent.Map);
					if (edifice == null || !thingToSpawn.IsEdifice())
					{
						Building_Door building_Door = edifice as Building_Door;
						if ((building_Door == null || building_Door.FreePassage) && (parent.def.passability == Traversability.Impassable || GenSight.LineOfSight(parent.Position, intVec, parent.Map)))
						{
							bool flag = false;
							List<Thing> thingList = intVec.GetThingList(parent.Map);
							for (int i = 0; i < thingList.Count; i++)
							{
								Thing thing = thingList[i];
								if (thing.def.category == ThingCategory.Item && (thing.def != thingToSpawn || thing.stackCount > thingToSpawn.stackLimit - spawnCount))
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								result = intVec;
								return true;
							}
						}
					}
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		private void ResetCountdown()
		{
			this.ticksUntilSpawn = this.PropsSpawner.spawnIntervalRange.RandomInRange;
		}

		public override void PostExposeData()
		{
			string str = this.PropsSpawner.saveKeysPrefix.NullOrEmpty() ? null : (this.PropsSpawner.saveKeysPrefix + "_");
			Scribe_Values.Look<int>(ref this.ticksUntilSpawn, str + "ticksUntilSpawn", 0, false);
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Spawn " + this.PropsSpawner.thingToSpawn.label,
					icon = TexCommand.DesirePower,
					action = delegate ()
					{
						this.ResetCountdown();
						this.TryDoSpawn();
					}
				};
			}
			yield break;
		}

		public override string CompInspectStringExtra()
		{
			if (this.PropsSpawner.writeTimeLeftToSpawn && (!this.PropsSpawner.requiresPower || this.PowerOn) && (!this.PropsSpawner.requiresFuel || this.HasFuel))
			{
				return "NextSpawnedItemIn".Translate(GenLabel.ThingLabel(this.PropsSpawner.thingToSpawn, null, this.PropsSpawner.spawnCount)).Resolve() + ": " + this.ticksUntilSpawn.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor);
			}
			return null;
		}

		private int ticksUntilSpawn;
	}
}
