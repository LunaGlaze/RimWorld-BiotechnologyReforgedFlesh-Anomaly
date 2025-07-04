using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
    public class CompLuna_DamageLimitation : ThingComp
    {
        public CompProperties_DamageLimitation Props
        {
            get
            {
                return (CompProperties_DamageLimitation)this.props;
            }
        }
        public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            absorbed = false;
            int maxDamage = Props.maxSingleDamage;
            float mult = Props.maxEffeMultiplier;
            float damage = dinfo.Amount;
            if (Props.effectiveUpperLimit && mult > 1 && damage > maxDamage * mult) 
            {
                dinfo.SetAmount( damage / mult );
            }
            else
            {
                dinfo.SetAmount(maxDamage);
            }
        }
    }
}
