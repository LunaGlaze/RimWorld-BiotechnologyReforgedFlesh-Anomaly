using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Luna_BRF
{
    [StaticConstructorOnStartup]
    public class Building_SmallFleshBattery : Building
    {
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			CompPowerBattery comp = base.GetComp<CompPowerBattery>();
			GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
			r.center = this.DrawPos + Vector3.up * 0.1f;
			r.size = Building_SmallFleshBattery.BarSize;
			r.fillPercent = comp.StoredEnergy / comp.Props.storedEnergyMax;
			r.filledMat = Building_SmallFleshBattery.BatteryBarFilledMat;
			r.unfilledMat = Building_SmallFleshBattery.BatteryBarUnfilledMat;
			r.margin = 0.05f;
			Rot4 rotation = base.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
		}
		private static readonly Vector2 BarSize = new Vector2(0.3f, 0.1f);
		private static readonly Material BatteryBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.66f, 0.3f, 0.2f), false);
		private static readonly Material BatteryBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}
