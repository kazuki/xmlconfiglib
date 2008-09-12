using System;
using System.Xml;

namespace XmlConfigLibrary
{
	public class DateTimeParser : IParser<DateTime>
	{
		static DateTimeParser _instance = new DateTimeParser ();
		DateTimeParser () {}
		public static DateTimeParser Instance {
			get { return _instance; }
		}

		public bool TryParse (XmlConfigNode node, out DateTime value)
		{
			value = DateTime.MinValue;
			if (node.NodeValue == null)
				return false;
			return DateTime.TryParse (node.NodeValue, out value);
		}

		public void Write (DateTime value, XmlWriter writer)
		{
			writer.WriteString (value.ToString ("o"));
		}
	}
}
