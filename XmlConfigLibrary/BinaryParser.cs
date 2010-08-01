using System;
using System.Xml;

namespace XmlConfigLibrary
{
	public class BinaryParser : IParser<byte[]>
	{
		static BinaryParser _instance = new BinaryParser ();
		BinaryParser () {}
		public static BinaryParser Instance {
			get { return _instance; }
		}

		public bool TryParse (XmlConfigNode node, out byte[] value)
		{
			if (node.NodeValue == null) {
				value = null;
				return false;
			}
			if (node.NodeValue.Length == 0) {
				value = new byte[0];
				return true;
			}
			try {
				value = Convert.FromBase64String (node.NodeValue);
				return true;
			} catch {
				value = null;
				return false;
			}
		}

		public void Write (byte[] value, XmlWriter writer)
		{
			if (value != null && value.Length > 0)
				writer.WriteBase64 (value, 0, value.Length);
		}
	}
}
