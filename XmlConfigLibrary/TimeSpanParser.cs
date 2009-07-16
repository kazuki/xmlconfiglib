using System;
using System.Xml;

namespace XmlConfigLibrary
{
	public class TimeSpanParser : IParser<TimeSpan>
	{
		static TimeSpanParser _instance = new TimeSpanParser ();
		TimeSpanParser () {}
		public static TimeSpanParser Instance {
			get { return _instance; }
		}

		public bool TryParse (XmlConfigNode node, out TimeSpan value)
		{
			if (node.NodeValue == null) {
				value = TimeSpan.MinValue;
				return false;
			}
			return TimeSpan.TryParse (node.NodeValue, out value);
		}

		public void Write (TimeSpan value, XmlWriter writer)
		{
			writer.WriteString (value.ToString ());
		}
	}
}
