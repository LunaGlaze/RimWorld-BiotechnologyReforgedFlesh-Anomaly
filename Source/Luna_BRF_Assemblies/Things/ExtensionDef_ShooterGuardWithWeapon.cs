using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Luna_BRF
{
    public class ExtensionDef_ShooterGuardWithWeapon : DefModExtension
    {
        public float idleTurnAnglePerTick = 0.1f;

        public Vector3 turretTopBaseOffset = Vector3.zero;

        public float turretTopBaseAngle;

        public bool turretTopBaseFlippable;
    }
}
