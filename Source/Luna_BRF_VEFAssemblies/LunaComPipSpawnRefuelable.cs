using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PipeSystem;
using RimWorld;
using UnityEngine;
using Verse;
using System.Threading.Tasks;

namespace Luna_BRF_VEFAssemblies
{
    [StaticConstructorOnStartup]
    public class LunaComPipSpawnRefuelable : CompRefuelable
	{
		public CompProperties_PipSpawnRefuelable Props
		{
			get
			{
				return (CompProperties_PipSpawnRefuelable)this.props;
			}
		}
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.flickComp = this.parent.GetComp<CompFlickable>();
			foreach (CompResource compResource in this.parent.GetComps<CompResource>())
			{
				string a;
				if (compResource == null)
				{
					a = null;
				}
				else
				{
					CompProperties_Resource props2 = compResource.Props;
					if (props2 == null)
					{
						a = null;
					}
					else
					{
						PipeNetDef pipeNet = props2.pipeNet;
						a = ((pipeNet != null) ? pipeNet.defName : null);
					}
				}
				bool flag = a == this.Props.PipeNet;
				if (flag)
				{
					this.compResource = compResource;
				}
			}
		}
		public override void CompTick()
		{
			if(this.compResource != null){
				if(this.compResource.PipeNet.AvailableCapacity > 0)
                {
					base.CompTick();
					bool flag = parent.IsHashIntervalTick(this.Props.spawnInterval);
					if (flag)
					{
						bool flag2 = base.Fuel > 0f && this.compResource != null;
						if (flag2)
						{
							int count = this.Props.spawnCount;
							this.compResource.PipeNet.DistributeAmongStorage(count);
						}
					}
				}
			}
		}
		public CompResource compResource;
		public CompFlickable flickComp;
	}
}
