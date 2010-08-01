using System;
using System.Xml;

namespace XmlConfigLibrary
{
	public class EnumParser<T> : IParser<T>
	{
		Type _type;

		public EnumParser ()
		{
			_type = typeof (T);
			if (!_type.IsEnum)
				throw new ArgumentException ();
		}

		public bool TryParse (XmlConfigNode node, out T value)
		{
			if (node.NodeValue == null) {
				value = default (T);
				return false;
			}
			try {
				value = (T)Enum.Parse (_type, node.NodeValue);
				return true;
			} catch {
				value = default (T);
				return false;
			}
		}

		public void Write (T value, XmlWriter writer)
		{
			writer.WriteString (Enum.GetName (_type, value));
		}
	}
}
