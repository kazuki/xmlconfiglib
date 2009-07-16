using System;
using System.Xml;

namespace XmlConfigLibrary
{
	public class StringParser : IParser<string>
	{
		static StringParser _instance = new StringParser ();
		StringParser () {}
		public static StringParser Instance {
			get { return _instance; }
		}

		public bool TryParse (XmlConfigNode node, out string value)
		{
			if (node.NodeValue == null) {
				value = string.Empty;
				return false;
			}

			value = node.NodeValue;
			return true;
		}

		public void Write (string value, XmlWriter writer)
		{
			writer.WriteString (value);
		}
	}
}
