using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Luna_BRF
{
    [StaticConstructorOnStartup]
    public class ShooterGuardWithWeapon : Building, IThingHolder, INotifyHauledTo, IAttackTarget, IAttackTargetSearcher
    {
        protected BRFTurretTop turretTop;
        private ThingOwner innerContainer;
        private Thing reservedWeapon;
        private static readonly Texture2D EquipCommandTex = ContentFinder<Texture2D>.Get("UI/Abilities/BRF_EquipCommand");
        private static readonly Texture2D CancelCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel");
        protected LocalTargetInfo currentTarget;
        protected LocalTargetInfo lastAttackedTarget;
        protected int lastAttackTargetTick;
        protected bool burstActivated;
        protected int burstCooldownTicksLeft;
        protected int burstWarmupTicksLeft;
        public BRFTurretTop TurretTop => turretTop;

        public Thing Gun
        {
            get
            {
                if (innerContainer.Count <= 0)
                {
                    return null;
                }
                return innerContainer[0];
            }
        }
        Thing IAttackTarget.Thing => this;
        Thing IAttackTargetSearcher.Thing => this;
        float IAttackTarget.TargetPriorityFactor => 1f;
        public LocalTargetInfo CurrentTarget => currentTarget;
        public LocalTargetInfo TargetCurrentlyAimingAt => currentTarget;
        LocalTargetInfo IAttackTargetSearcher.LastAttackedTarget => lastAttackedTarget;
        int IAttackTargetSearcher.LastAttackTargetTick => lastAttackTargetTick;
        Verb IAttackTargetSearcher.CurrentEffectiveVerb => AttackVerb;
        public Verb AttackVerb => GunCompEq.PrimaryVerb;
        protected CompEquippable GunCompEq => Gun.TryGetComp<CompEquippable>();
        public bool WarmingUp => burstWarmupTicksLeft > 0;
        public Thing ReservedWeapon => reservedWeapon;
        public int GraphicIndex => (innerContainer.Count > 0) ? 1 : 0;

        private ExtensionDef_ShooterGuardWithWeapon shooterGuardWithWeaponExtension;
        public ExtensionDef_ShooterGuardWithWeapon ShooterGuardWithWeaponExtension
        {
            get
            {
                if (shooterGuardWithWeaponExtension == null)
                {
                    shooterGuardWithWeaponExtension = def.GetModExtension<ExtensionDef_ShooterGuardWithWeapon>();
                }
                return shooterGuardWithWeaponExtension;
            }
        }

        public bool Active
        {
            get
            {
                if (PowerCanWork && FuelCanWork)
                {
                    return innerContainer.Count > 0;
                }
                return false;
            }
        }
        public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
        {
            return !Active;
        }
        public ShooterGuardWithWeapon()
        {
            innerContainer = new ThingOwner<Thing>(this, oneStackOnly: true);
            turretTop = new BRFTurretTop(this);
        }

        private bool PowerCanWork
        {
            get
            {
                CompPowerTrader comp = this.GetComp<CompPowerTrader>();
                return (comp != null && comp.PowerOn) || comp == null;
            }
        }

        public bool FuelCanWork
        {
            get
            {
                CompRefuelable comp = this.GetComp<CompRefuelable>();
                return (comp != null && comp.HasFuel) || comp == null;
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
            Scribe_References.Look(ref reservedWeapon, "reservedWeapon");
            Scribe_TargetInfo.Look(ref currentTarget, "currentTarget");
            Scribe_TargetInfo.Look(ref lastAttackedTarget, "lastAttackedTarget");
            Scribe_Values.Look(ref lastAttackTargetTick, "lastAttackTargetTick", 0);
            Scribe_Values.Look(ref burstActivated, "burstActivated", defaultValue: false);
            Scribe_Values.Look(ref burstCooldownTicksLeft, "burstCooldownTicksLeft", 0);
            Scribe_Values.Look(ref burstWarmupTicksLeft, "burstWarmupTicksLeft", 0);
            if (Scribe.mode == LoadSaveMode.PostLoadInit && Gun != null)
            {
                UpdateGunVerbs();
            }
        }

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return innerContainer;
        }

        public void Notify_HauledTo(Pawn hauler, Thing thing, int count)
        {
            EquipGun(thing);
        }
        public override void PostMake()
        {
            base.PostMake();
            burstCooldownTicksLeft = def.building.turretInitialCooldownTime.SecondsToTicks();
        }
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (!respawningAfterLoad)
            {
                turretTop.SetRotationFromOrientation();
            }
        }
        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            reservedWeapon = null;
            if (innerContainer.Count > 0)
            {
                innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near);
                innerContainer.ClearAndDestroyContents(mode);
            }
            base.DeSpawn(mode);
            ResetCurrentTarget();
        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            if (Gun != null)
            {
                Vector3 turretTopBaseOffset = ShooterGuardWithWeaponExtension.turretTopBaseOffset;
                if (ShooterGuardWithWeaponExtension.turretTopBaseFlippable && TurretTop.CurRotation >= 180f)
                {
                    turretTopBaseOffset.x = 0f - turretTopBaseOffset.x;
                }
                if (AttackVerb is Verb_LaunchProjectile shootVerb)
                {
                    EquipmentUtility.Recoil(Gun.def, shootVerb, out var drawOffset, out var angleOffset, turretTop.CurRotation);
                    turretTop.DrawTurret(turretTopBaseOffset + drawOffset, ShooterGuardWithWeaponExtension.turretTopBaseAngle + angleOffset);
                }
                else
                {
                    turretTop.DrawTurret(turretTopBaseOffset, ShooterGuardWithWeaponExtension.turretTopBaseAngle);
                }
            }
            base.DrawAt(drawLoc, flip);
        }

        protected override void Tick()
        {
            base.Tick();
            if (Active && base.Spawned && !base.Destroyed)
            {
                GunCompEq.verbTracker.VerbsTick();
                if (AttackVerb.state == VerbState.Bursting)
                {
                    return;
                }
                burstActivated = false;
                if (WarmingUp)
                {
                    burstWarmupTicksLeft--;
                    if (burstWarmupTicksLeft == 0)
                    {
                        BeginBurst();
                    }
                }
                else
                {
                    if (burstCooldownTicksLeft > 0)
                    {
                        burstCooldownTicksLeft--;
                    }
                    if (burstCooldownTicksLeft <= 0 && this.IsHashIntervalTick(30))
                    {
                        TryStartShootSomething(canBeginBurstImmediately: true);
                    }
                }
                turretTop.TurretTopTick();
            }
            else
            {
                ResetCurrentTarget();
            }
        }

        public override void DrawExtraSelectionOverlays()
        {
            base.DrawExtraSelectionOverlays();
            if (base.Spawned && Active)
            {
                float range = AttackVerb.verbProps.range;
                if (range < 90f)
                {
                    GenDraw.DrawRadiusRing(base.Position, range);
                }
                float num = AttackVerb.verbProps.EffectiveMinRange(allowAdjacentShot: true);
                if (num < 90f && num > 0.1f)
                {
                    GenDraw.DrawRadiusRing(base.Position, num);
                }
                if (WarmingUp)
                {
                    int degreesWide = (int)((float)burstWarmupTicksLeft * 0.5f);
                    GenDraw.DrawAimPie(this, currentTarget, degreesWide, (float)def.size.x * 0.5f);
                }
            }
        }

        protected void TryActivateBurst()
        {
            burstActivated = true;
            TryStartShootSomething(canBeginBurstImmediately: true);
        }

        protected void TryStartShootSomething(bool canBeginBurstImmediately)
        {
            if (!base.Spawned || (AttackVerb.ProjectileFliesOverhead() && base.Map.roofGrid.Roofed(base.Position)) || !AttackVerb.Available())
            {
                ResetCurrentTarget();
                return;
            }
            currentTarget = TryFindNewTarget();
            if (currentTarget.IsValid)
            {
                float randomInRange = def.building.turretBurstWarmupTime.RandomInRange;
                if (randomInRange > 0f)
                {
                    burstWarmupTicksLeft = randomInRange.SecondsToTicks();
                }
                else if (canBeginBurstImmediately)
                {
                    BeginBurst();
                }
                else
                {
                    burstWarmupTicksLeft = 1;
                }
            }
            else
            {
                ResetCurrentTarget();
            }
        }

        protected LocalTargetInfo TryFindNewTarget()
        {
            Faction faction = base.Faction;
            float range = AttackVerb.verbProps.range;
            if (Rand.Value < 0.5f && AttackVerb.ProjectileFliesOverhead() && faction.HostileTo(Faction.OfPlayer) && base.Map.listerBuildings.allBuildingsColonist.Where(delegate (Building building)
            {
                float num = AttackVerb.verbProps.EffectiveMinRange(building, this);
                int num2 = building.Position.DistanceToSquared(base.Position);
                return num * num < (float)num2 && (float)num2 < range * range;
            }).TryRandomElement(out var result))
            {
                return result;
            }
            TargetScanFlags targetScanFlags = TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
            if (!AttackVerb.ProjectileFliesOverhead())
            {
                targetScanFlags |= TargetScanFlags.NeedLOSToAll;
                targetScanFlags |= TargetScanFlags.LOSBlockableByGas;
            }
            if (AttackVerb.IsIncendiary_Ranged())
            {
                targetScanFlags |= TargetScanFlags.NeedNonBurning;
            }
            return (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(this, targetScanFlags, IsValidTarget);
        }

        protected virtual bool IsValidTarget(Thing thing)
        {
            if (thing is Pawn pawn)
            {
                if (base.Faction == Faction.OfPlayer && pawn.IsPrisoner)
                {
                    return false;
                }
                if (AttackVerb.ProjectileFliesOverhead())
                {
                    RoofDef roofDef = base.Map.roofGrid.RoofAt(thing.Position);
                    if (roofDef != null && roofDef.isThickRoof)
                    {
                        return false;
                    }
                }
                return !GenAI.MachinesLike(base.Faction, pawn);
            }
            return true;
        }

        public void ReserveGun(Thing gun)
        {
            reservedWeapon = gun;
        }

        public void EquipGun(Thing gun)
        {
            reservedWeapon = null;
            if (gun == null)
            {
                if (innerContainer.Count > 0)
                {
                    innerContainer.TryDropAll(base.PositionHeld, base.MapHeld, ThingPlaceMode.Near);
                    innerContainer.Clear();
                }
            }
            else
            {
                if (CanEquipWeapon(gun))
                {
                    UpdateGunVerbs();
                }
                DirtyMapMesh(base.Map);
            }
        }


        protected bool CanEquipWeapon(Thing gun)
        {
            if (gun == null)
            {
                return false;
            }
            CompEquippable compEquippable = gun.TryGetComp<CompEquippable>();
            if (compEquippable == null || compEquippable.PrimaryVerb == null || compEquippable.PrimaryVerb.IsMeleeAttack)
            {
                return false;
            }
            CompBiocodable compBiocodable = gun.TryGetComp<CompBiocodable>();
            if (compBiocodable != null && (compBiocodable.Biocoded || compBiocodable.Props.biocodeOnEquip))
            {
                return false;
            }
            return true;
        }

        protected virtual void BeginBurst()
        {
            if (Active)
            {
                AttackVerb.TryStartCastOn(currentTarget);
                lastAttackTargetTick = Find.TickManager.TicksGame;
                lastAttackedTarget = currentTarget;
            }
        }

        protected virtual void BurstComplete()
        {
            burstCooldownTicksLeft = (Gun.GetStatValue(StatDefOf.RangedWeapon_Cooldown) * 2.5f).SecondsToTicks();
        }
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }
            if (Gun != null)
            {
                ThingDef thingDef = Gun.def;
                Command_Action command_Action = new Command_Action();
                command_Action.defaultLabel = "BRF_Command_UnequipWeapon".Translate();
                command_Action.defaultDesc = new StringBuilder().Append(Gun.LabelCap).Append(": ").Append(thingDef.description.CapitalizeFirst())
                    .ToString();
                command_Action.icon = (Texture)(object)thingDef.uiIcon;
                command_Action.iconAngle = thingDef.uiIconAngle;
                command_Action.iconOffset = thingDef.uiIconOffset;
                command_Action.action = delegate
                {
                    EquipGun(null);
                };
                if (base.Faction != Faction.OfPlayer)
                {
                    command_Action.Disable("CannotOrderNonControlled".Translate());
                }
                yield return command_Action;
            }
            else if (ReservedWeapon != null)
            {
                Command_Action command_Action2 = new Command_Action();
                command_Action2.icon = (Texture)(object)CancelCommandTex;
                command_Action2.defaultLabel = "BRF_Command_CancelReserveWeapon".Translate();
                command_Action2.defaultDesc = "BRF_Command_CancelReserveWeaponDesc".Translate();
                command_Action2.action = delegate
                {
                    reservedWeapon = null;
                };
                yield return command_Action2;
            }
            else
            {
                Command_Action command_Action3 = new Command_Action();
                command_Action3.icon = (Texture)(object)EquipCommandTex;
                command_Action3.iconDrawScale = 1.25f;
                command_Action3.defaultLabel = "BRF_Command_EquipWeapon".Translate();
                command_Action3.defaultDesc = "BRF_Command_EquipWeaponDesc".Translate();
                command_Action3.action = delegate
                {
                    Find.Targeter.BeginTargeting(new TargetingParameters
                    {
                        canTargetPawns = false,
                        canTargetBuildings = false,
                        canTargetItems = true,
                        canTargetAnimals = false,
                        canTargetHumans = false,
                        canTargetMechs = false,
                        canTargetSubhumans = false,
                        canTargetBloodfeeders = false,
                        mapObjectTargetsMustBeAutoAttackable = false,
                        validator = delegate (TargetInfo target)
                        {
                            if (!target.HasThing || !CanEquipWeapon(target.Thing) || target.Thing.Map.reservationManager.IsReserved(target.Thing))
                            {
                                return false;
                            }
                            foreach (Building item in base.Map.listerBuildings.AllBuildingsColonistOfDef(LunaBRFDefOf.BRF_PsycorpseGuard))
                            {
                                if (item is ShooterGuardWithWeapon arcanePlant_Shootus && arcanePlant_Shootus.ReservedWeapon == target.Thing)
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                    }, delegate (LocalTargetInfo target)
                    {
                        if (target.HasThing)
                        {
                            reservedWeapon = target.Thing;
                        }
                    }, delegate (LocalTargetInfo target)
                    {
                        if (target.HasThing)
                        {
                            Widgets.MouseAttachedLabel(target.Label);
                        }
                    });
                };
                yield return command_Action3;
            }
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
            if (base.Spawned && ReservedWeapon != null && Gun == null)
            {
                stringBuilder.AppendInNewLine("Queued".Translate());
                stringBuilder.Append(": ");
                stringBuilder.Append(ReservedWeapon.LabelCap);
            }
            return stringBuilder.ToString();
        }

        protected virtual void ResetCurrentTarget()
        {
            currentTarget = LocalTargetInfo.Invalid;
            burstWarmupTicksLeft = 0;
        }

        protected void UpdateGunVerbs()
        {
            List<Verb> allVerbs = Gun.TryGetComp<CompEquippable>().AllVerbs;
            for (int i = 0; i < allVerbs.Count; i++)
            {
                Verb verb = allVerbs[i];
                verb.caster = this;
                verb.castCompleteCallback = BurstComplete;
            }
        }
    }
}
