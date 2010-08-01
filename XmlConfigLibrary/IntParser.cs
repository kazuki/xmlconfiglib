using System;
using System.Xml;

namespace XmlConfigLibrary
{
	public class IntParser : IParser<int>
	{
		static IntParser _instance = new IntParser ();
		IntParser() {}
		public static IntParser Instance {
			get { return _instance; }
		}

		public bool TryParse (XmlConfigNode node, out int value)
		{
			if (node.NodeValue == null || !int.TryParse (node.NodeValue, out value)) {
				value = 0;
				return false;
			}
			return true;
		}

		public void Write (int value, XmlWriter writer)
		{
			writer.WriteString (value.ToString ());
		}
	}
}
