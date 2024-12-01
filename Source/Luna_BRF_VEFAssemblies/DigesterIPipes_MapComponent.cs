using System.Collections.Generic;
using Verse;

namespace Luna_BRF_VEFAssemblies
{
	public class DigesterIPipes_MapComponent : MapComponent
	{
		public List<LunaComPipSpawnRefuelable> comps = new List<LunaComPipSpawnRefuelable>();

		public DigesterIPipes_MapComponent(Map map)
			: base(map)
		{
		}
	}

}