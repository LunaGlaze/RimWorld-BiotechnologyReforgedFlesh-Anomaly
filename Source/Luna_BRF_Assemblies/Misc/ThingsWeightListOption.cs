using System.Xml;
using Verse;


namespace Luna_BRF
{
	public class ThingsWeightListOption
	{
		public ThingDef thing;

		public float selectionWeight;
		public override string ToString()
		{
			return "(" + ((thing != null) ? thing.ToString() : "null") + " w=" + selectionWeight.ToString("F2") + ")";
		}
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thing", xmlRoot.Name);
			selectionWeight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}
	}
}
