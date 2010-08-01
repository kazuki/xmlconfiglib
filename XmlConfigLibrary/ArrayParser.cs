using System;
using System.Collections.Generic;
using System.Xml;

namespace XmlConfigLibrary
{
	public class ArrayParser<T> : IParser<T[]>
	{
		IParser<T> _elementParser;
		string _itemElementName;
		const string DefaultItemElementName = "item";

		public ArrayParser (IParser<T> elementParser) : this (elementParser, DefaultItemElementName)
		{
		}

		public ArrayParser (IParser<T> elementParser, string itemElementName)
		{
			_elementParser = elementParser;
			_itemElementName = itemElementName;
		}

		public bool TryParse (XmlConfigNode node, out T[] value)
		{
			if (node.NodeValue == null) {
				value = null;
				return false;
			}

			List<T> list = new List<T> ();
			T tmp;
			for (int i = 0; i < node.Nodes.Count; i ++) {
				XmlConfigNode item = node.Nodes[i];
				if (item.Name != _itemElementName)
					continue;
				if (_elementParser.TryParse (item, out tmp))
					list.Add (tmp);
			}
			value = list.ToArray ();
			return true;
		}

		public void Write (T[] value, XmlWriter writer)
		{
			if (value == null || value.Length == 0)
				return;
			for (int i = 0; i < value.Length; i ++) {
				writer.WriteStartElement (_itemElementName);
				_elementParser.Write (value[i], writer);
				writer.WriteEndElement ();
			}
		}
	}
}
