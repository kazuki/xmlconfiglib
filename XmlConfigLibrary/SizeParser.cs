using System;
using System.Drawing;
using System.Xml;

namespace XmlConfigLibrary
{
	public class SizeParser : IParser<Size>
	{
		static SizeParser _instance = new SizeParser ();
		SizeParser() {}
		public static SizeParser Instance {
			get { return _instance; }
		}

		public bool TryParse (XmlConfigNode node, out Size value)
		{
			value = Size.Empty;
			string sw, sh;
			if (node.Attributes == null ||
				!node.Attributes.TryGetValue ("width", out sw) ||
				!node.Attributes.TryGetValue ("height", out sh)) {
				return false;
			}
			int width, height;
			if (int.TryParse (sw, out width) && int.TryParse (sh, out height)) {
				value = new Size (width, height);
				return true;
			}
			return false;
		}

		public void Write (Size value, XmlWriter writer)
		{
			writer.WriteAttributeString ("width", value.Width.ToString ());
			writer.WriteAttributeString ("height", value.Height.ToString ());
		}
	}
}
