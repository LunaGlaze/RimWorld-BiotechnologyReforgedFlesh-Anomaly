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
    public class LunaComPipSpawnRefuelable : CompRefuelable, IThingHolder
	{
		public CompProperties_PipSpawnRefuelable iProps
		{
			get
			{
				return (CompProperties_PipSpawnRefuelable)Props;
			}
		}
		public LunaComPipSpawnRefuelable()
		{
			innerContainer = new ThingOwner<Thing>(this);
		}
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			flickComp = parent.GetComp<CompFlickable>();
			foreach (CompResource comp in parent.GetComps<CompResource>())
			{
				if (((comp == null) ? null : ((Def)(object)comp.Props?.pipeNet)?.defName) == "BRF_NutritionalPasteNet")
				{
					compResource = comp;
				}
				if (ModLister.HasActiveModWithName("Vanilla Nutrient Paste Expanded") && ((comp == null) ? null : ((Def)(object)comp.Props?.pipeNet)?.defName) == "VNPE_NutrientPasteNet")
				{
					compResourceNutrientPaste = comp;
				}
			}
		}
		public override void CompTick()
		{
			if(compResource != null){
				if(compResource.PipeNet.AvailableCapacity > 0)
                {
					base.CompTick();
					bool flag = parent.IsHashIntervalTick(iProps.spawnInterval);
					if (flag)
					{
						bool flag2 = base.Fuel > 0f && compResource != null;
						if (flag2)
						{
							int count = iProps.spawnCount;
							compResource.PipeNet.DistributeAmongStorage(count);
						}
					}
				}
				if (parent.IsHashIntervalTick(600) && compResourceNutrientPaste != null && compResourceNutrientPaste.PipeNet.Stored > 1f && base.FuelPercentOfMax < 0.9f)
				{
					compResourceNutrientPaste.PipeNet.DrawAmongStorage(1f, compResourceNutrientPaste.PipeNet.storages);
					Refuel(18f);
				}
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

        public CompResource compResource;
		public CompResource compResourceNutrientPaste;
		public CompFlickable flickComp;
		public ThingOwner innerContainer;
	}
}
