using System;
using System.Collections.Generic;
using Verse;

namespace Luna_BRF
{
	public class CompProperties_AnimalProduct : CompProperties
	{
		public bool hideDisplayOnWildAnimals = false;

		public int gatheringIntervalDays = 1;

		public int resourceAmount = 1;

		public ThingDef resourceDef = null;

		public string customResourceString = "";

		public bool isRandom = false;

		public List<string> randomItems = null;

		public List<string> seasonalItems = null;

		public bool hasAditional = false;

		public int additionalItemsProb = 1;

		public int additionalItemsNumber = 1;

		public List<string> additionalItems = null;

		public bool goInOrder = false;

		public CompProperties_AnimalProduct()
		{
			compClass = typeof(CompAnimalProduct);
		}
	}
}
