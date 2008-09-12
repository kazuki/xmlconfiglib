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
			value = node.NodeValue;
			if (value == null)
				value = string.Empty;
			return true;
		}

		public void Write (string value, XmlWriter writer)
		{
			writer.WriteString (value);
		}
	}
}
