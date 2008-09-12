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
			try {
				value = (T)Enum.Parse (_type, node.NodeValue);
				return true;
			} catch {
				value = (T)Enum.GetValues (_type).GetValue (0);
				return false;
			}
		}

		public void Write (T value, XmlWriter writer)
		{
			writer.WriteString (Enum.GetName (_type, value));
		}
	}
}
