using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class Gas_Clentaminator : Gas
	{
        private int tickerInterval = 0;

        private int tickerMax = 256;
		public ExtensionDef_ClentaminatorList defExtension => this.def.GetModExtension<ExtensionDef_ClentaminatorList>();
		protected override void Tick()
		{
			base.Tick();
			if(defExtension != null)
			{
				try
				{
					if (tickerInterval >= tickerMax)
					{
						HashSet<Thing> hashSet = new HashSet<Thing>(base.Position.GetThingList(base.Map));
						if (hashSet != null)
						{
							foreach (Thing item in hashSet)
							{
								if(item is Gas && defExtension.destroyGasTypes.Contains(item.def))
                                {
									item.Destroy();
                                }
								PlantProperties plant = item.def.plant;
								PawnKindDef pawnKind = null;
								if (item is Pawn pawn0)
								{
									pawnKind = pawn0.kindDef;
								}
								if (plant == null && pawnKind == null)
								{
									continue;
								}
								if (plant != null && !defExtension.blackListThingsBase.Contains(item.def))
								{
									if (plant.IsTree && !defExtension.isTreeLists.NullOrEmpty() )
									{
										ThingDef target1 = defExtension.isTreeLists.RandomElementByWeight((ThingsWeightListOption x) => x.selectionWeight).thing;
										Plant plant2 = (Plant)GenSpawn.Spawn(target1, base.Position, base.Map);
										Plant plant3 = (Plant)item;
										plant2.Growth = plant3.Growth;
										item.Destroy();
									}
									else if (!plant.IsTree && !defExtension.notTreeLists.NullOrEmpty() )
									{
										ThingDef target2 = defExtension.notTreeLists.RandomElementByWeight((ThingsWeightListOption x) => x.selectionWeight).thing;
										Plant plant4 = (Plant)GenSpawn.Spawn(target2, base.Position, base.Map);
										Plant plant5 = (Plant)item;
										plant4.Growth = plant5.Growth;
										item.Destroy();
									}
								}
								else if (item is Pawn pawn1)
                                {
                                    if (pawn1.Spawned)
									{
										bool malignantFleshActivation = pawn1.health?.hediffSet?.GetFirstHediffOfDef(HediffDef.Named("BRF_MalignantFleshActivation")) != null;
										if(pawnKind != null && defExtension.whiteListPawn.Contains(pawnKind) && !defExtension.pawnLists.NullOrEmpty() )
										{
											Gender gender = pawn1.gender;
											Faction faction = pawn1.Faction;
											if (defExtension.defaultFaction != null)
											{
												faction = Find.FactionManager.FirstFactionOfDef(defExtension.defaultFaction);
											}
											PawnKindDef newKind = defExtension.pawnLists.RandomElementByWeight((PawnGenOption x) => x.selectionWeight).kind;
											PawnGenerationRequest request = new PawnGenerationRequest(newKind, faction, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true, 1f, false, true, true, false, false);
											Pawn pawn2 = PawnGenerator.GeneratePawn(request);
											PawnUtility.TrySpawnHatchedOrBornPawn(pawn2, pawn1);
											if (pawn2.def.race.hasGenders)
											{
												pawn2.gender = gender;
											}
											pawn1.Strip(false);
											pawn1.Destroy();
										}
										else if (pawn1.RaceProps.IsFlesh && !pawn1.kindDef.IsFleshBeast() && !pawn1.RaceProps.IsAnomalyEntity && !pawn1.IsShambler && (pawn1.mutant != null && !pawn1.mutant.Def.isImmuneToInfections)) {
											IEnumerable<BodyPartRecord> lungList = LunaBRFHediffUtility.GetLungWithoutFleshReforgeBodyParts(pawn1);
											if (!defExtension.fleshTypeWithoutBRFLung || (defExtension.fleshTypeWithoutBRFLung && !lungList.EnumerableNullOrEmpty()))
                                            {
												if(defExtension.hediffDefGiveFlesh != null)
                                                {
													HealthUtility.AdjustSeverity(pawn1, defExtension.hediffDefGiveFlesh, 0.0075f);
												}
												if(defExtension.takeDamageFleshWithoutEntity != null && !malignantFleshActivation)
												{
													pawn1.TakeDamage(new DamageInfo(defExtension.takeDamageFleshWithoutEntity, defExtension.damageFlesh));
												}
                                            }
										}
										else if(pawn1.RaceProps.IsMechanoid && !pawn1.RaceProps.IsAnomalyEntity)
										{
											if (defExtension.hediffDefGiveMechanoid != null)
											{
												HealthUtility.AdjustSeverity(pawn1, defExtension.hediffDefGiveMechanoid, 0.01f);
											}
											if (defExtension.takeDamageFleshWithoutEntity != null)
											{
												pawn1.TakeDamage(new DamageInfo(defExtension.takeDamageMechanoid, defExtension.damageMechanoid));
											}
										}
                                        if (malignantFleshActivation)
										{
											if (defExtension.takeDamageFleshWithoutEntity != null)
											{
												pawn1.TakeDamage(new DamageInfo(defExtension.takeDamageFleshWithoutEntity, defExtension.damageFlesh * 2.5f));
											}
											if (defExtension.takeDamageFleshWithoutEntity != null)
											{
												pawn1.TakeDamage(new DamageInfo(defExtension.takeDamageMechanoid, defExtension.damageMechanoid * 2.5f));
											}
										}
									}
								}else if(item is Fire)
                                {
									if (defExtension.outFire) { item.Destroy(); }
                                }
							}
						}
                        if (defExtension.whiteListTerrain != null && defExtension.terrainToSet != null) {
                            if (defExtension.whiteListTerrain.Contains(base.Position.GetTerrain(base.Map)))
                            {
								base.Map.terrainGrid.SetTerrain(base.Position, defExtension.terrainToSet);
							}
						}
						tickerInterval = 0;
					}
					tickerInterval++;
				}
				catch (NullReferenceException)
				{
				}
			}
		}
	}
}
