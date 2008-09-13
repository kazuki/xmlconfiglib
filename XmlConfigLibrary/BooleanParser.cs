using System;
using System.Xml;

namespace XmlConfigLibrary
{
	public class BooleanParser : IParser<bool>
	{
		static BooleanParser _instance = new BooleanParser ();
		BooleanParser() {}
		public static BooleanParser Instance {
			get { return _instance; }
		}

		public bool TryParse (XmlConfigNode node, out bool value)
		{
			value = true;
			if (node.NodeValue == null || !bool.TryParse (node.NodeValue, out value))
				return false;
			return true;
		}

		public void Write (bool value, XmlWriter writer)
		{
			writer.WriteString (value.ToString ());
		}
	}
}
