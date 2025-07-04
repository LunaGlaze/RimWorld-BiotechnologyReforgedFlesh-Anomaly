using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
    public class CompLuna_WeaponDropManage : ThingComp
    {
        public CompProperties_WeaponDropManage Props
        {
            get
            {
                return (CompProperties_WeaponDropManage)this.props;
            }
        }
        public ThingWithComps weapon;
        public ThingDef weaponDef;
        public ThingDef stuff;
        public QualityCategory quality;
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Deep.Look(ref weapon, "BRFWeaponDropManage_weapon", this);
            Scribe_Deep.Look(ref weaponDef, "BRFWeaponDropManage_weaponDef", this);
            Scribe_Deep.Look(ref stuff, "BRFWeaponDropManage_stuff", this);
            Scribe_Deep.Look(ref quality, "BRFWeaponDropManage_quality", this);
        }
        public override void CompTick()
        {
            if (parent.IsHashIntervalTick(250))
            {
                if(weaponDef != null && parent is Pawn pawn && !pawn.Dead && pawn.equipment != null)
                {
                    if (pawn.equipment.Primary == null && weaponDef.destroyOnDrop)
                    {
                        pawn.equipment.AddEquipment(tryMakeNewWeapon());
                    }
                }
                getWeapon(parent as Pawn);
            }
        }
        public void getWeapon(Pawn pawn)
        {
            if(pawn != null)
            {
                Pawn_EquipmentTracker equipment = pawn.equipment;
                if (equipment != null)
                {
                    ThingWithComps weapon1 = equipment.Primary;
                    if (weapon1 != null && !pawn.Downed && pawn.Awake())
                    {
                        weapon = weapon1;
                        weaponDef = weapon1.def;
                        stuff = weapon1.Stuff;
                        QualityUtility.TryGetQuality(weapon1,out quality);
                    }
                }
            }
        }
        public ThingWithComps tryMakeNewWeapon()
        {
            if(weaponDef != null)
            {
                ThingWithComps newWeapon = ThingMaker.MakeThing(weaponDef, stuff) as ThingWithComps;
                CompQuality compQuality = ((newWeapon is MinifiedThing minifiedThing) ? minifiedThing.InnerThing.TryGetComp<CompQuality>() : newWeapon.TryGetComp<CompQuality>());
                if (compQuality != null)
                {
                    compQuality.SetQuality(quality, null);
                }
                return newWeapon;
            }
            else
            {
                return null;
            }
        }
        public override void Notify_Downed()
        {
            base.Notify_Downed();
            /*
            if(weapon != null && parent is Pawn pawn && !pawn.Dead && pawn.equipment != null)
            {
                pawn.equipment.Remove(pawn.equipment.Primary);
                pawn.equipment.AddEquipment(weapon);
            }
            */
        }
        public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
        {
            base.Notify_Killed(prevMap, dinfo);
        }
    }
}
