using UnityEngine;
using Verse;

namespace Luna_BRF
{
    public class BRFTurretTop
    {
        private const int IdleTurnDuration = 40;

        private const int IdleTurnIntervalMin = 200;

        private const int IdleTurnIntervalMax = 400;

        private ShooterGuardWithWeapon parent;

        private float curRotationInt = Rand.Range(0f, 360f);

        private int ticksUntilIdleTurn;

        private int idleTurnTicksLeft;

        private bool idleTurnClockwise = Rand.Bool;

        public float CurRotation
        {
            get
            {
                return curRotationInt;
            }
            set
            {
                float num = curRotationInt;
                curRotationInt = value;
                if (curRotationInt > 360f)
                {
                    curRotationInt -= 360f;
                }
                if (curRotationInt < 0f)
                {
                    curRotationInt += 360f;
                }
                if ((num < 180f && curRotationInt >= 180f) || (num >= 180f && curRotationInt < 180f))
                {
                    parent.DirtyMapMesh(parent.Map);
                }
            }
        }

        public void SetRotationFromOrientation()
        {
            CurRotation = parent.Rotation.AsAngle;
        }

        public BRFTurretTop(ShooterGuardWithWeapon parent)
        {
            this.parent = parent;
        }

        public void ForceFaceTarget(LocalTargetInfo targ)
        {
            if (parent.Gun != null && targ.IsValid)
            {
                CurRotation = (targ.Cell.ToVector3Shifted() - parent.DrawPos).AngleFlat();
            }
        }

        public void TurretTopTick()
        {
            if (parent.Gun == null)
            {
                return;
            }
            LocalTargetInfo currentTarget = parent.CurrentTarget;
            if (currentTarget.IsValid)
            {
                CurRotation = (currentTarget.Cell.ToVector3Shifted() - parent.DrawPos).AngleFlat();
                ticksUntilIdleTurn = Rand.RangeInclusive(200, 400);
            }
            else if (ticksUntilIdleTurn > 0)
            {
                ticksUntilIdleTurn--;
                if (ticksUntilIdleTurn == 0)
                {
                    if (Rand.Value < 0.5f)
                    {
                        idleTurnClockwise = true;
                    }
                    else
                    {
                        idleTurnClockwise = false;
                    }
                    idleTurnTicksLeft = 40;
                }
            }
            else
            {
                if (idleTurnClockwise)
                {
                    CurRotation += parent.ShooterGuardWithWeaponExtension.idleTurnAnglePerTick;
                }
                else
                {
                    CurRotation -= parent.ShooterGuardWithWeaponExtension.idleTurnAnglePerTick;
                }
                idleTurnTicksLeft--;
                if (idleTurnTicksLeft <= 0)
                {
                    ticksUntilIdleTurn = Rand.RangeInclusive(200, 400);
                }
            }
        }

        public void DrawTurret(Vector3 recoilDrawOffset, float recoilAngleOffset)
        {
            Thing gun = parent.Gun;
            if (gun != null)
            {
                float num = (parent.AttackVerb?.AimAngleOverride ?? CurRotation) + recoilAngleOffset;
                Vector3 vector = new Vector3(parent.def.building.turretTopOffset.x, 0f, parent.def.building.turretTopOffset.y).RotatedBy(num);
                float x = gun.DrawSize.x;
                vector += recoilDrawOffset;
                Matrix4x4 matrix = default(Matrix4x4);
                if (num < 180f)
                {
                    matrix.SetTRS(parent.DrawPos + Altitudes.AltIncVect * 2f + vector, (num - 90f).ToQuat(), new Vector3(x, 1f, x));
                    Graphics.DrawMesh(MeshPool.plane10, matrix, gun.Graphic.MatSingleFor(gun), 0);
                }
                else
                {
                    matrix.SetTRS(parent.DrawPos + Altitudes.AltIncVect * 2f + vector, (num + 90f).ToQuat(), new Vector3(x, 1f, x));
                    Graphics.DrawMesh(MeshPool.GridPlaneFlip(new Vector2(1f, 1f)), matrix, gun.Graphic.MatSingleFor(gun), 0);
                }
            }
        }
    }
}
