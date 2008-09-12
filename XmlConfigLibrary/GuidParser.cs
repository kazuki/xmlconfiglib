using System;
using System.Xml;

namespace XmlConfigLibrary
{
	public class GuidParser : IParser<Guid>
	{
		static GuidParser _instance = new GuidParser ();
		GuidParser() {}
		public static GuidParser Instance {
			get { return _instance; }
		}

		public bool TryParse (XmlConfigNode node, out Guid value)
		{
			value = Guid.Empty;
			if (node.NodeValue == null)
				return false;
			try {
				value = new Guid (node.NodeValue);
				return true;
			} catch {
				return false;
			}
		}

		public void Write (Guid value, XmlWriter writer)
		{
			writer.WriteString (value.ToString ());
		}
	}
}
