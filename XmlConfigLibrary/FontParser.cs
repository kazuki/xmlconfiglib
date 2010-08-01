using System;
using System.Xml;
using System.Drawing;

namespace XmlConfigLibrary
{
	public class FontParser : IParser<Font>
	{
		static FontParser _instance = new FontParser ();
		FontParser() {}
		public static FontParser Instance {
			get { return _instance; }
		}

		public bool TryParse (XmlConfigNode node, out Font value)
		{
			string family, strSize, strStyle, strUnit;
			float size;
			FontStyle style;
			GraphicsUnit unit;
			value = null;
			if (node.Attributes == null ||
				!node.Attributes.TryGetValue ("fontname", out family) ||
				!node.Attributes.TryGetValue ("size", out strSize)) {
				return false;
			}
			
			if (!node.Attributes.TryGetValue ("style", out strStyle))
				strStyle = FontStyle.Regular.ToString ();
			if (!node.Attributes.TryGetValue ("unit", out strUnit))
				strUnit = GraphicsUnit.Point.ToString ();
			if (!float.TryParse (strSize, out size))
				return false;
			try {
				style = (FontStyle)Enum.Parse (typeof (FontStyle), strStyle);
			} catch {
				return false;
			}
			try {
				unit = (GraphicsUnit)Enum.Parse (typeof (GraphicsUnit), strUnit);
			} catch {
				return false;
			}
			try {
				value = new Font (family, size, style, unit);
				return true;
			} catch {
				return false;
			}
		}

		public void Write (Font value, XmlWriter writer)
		{
			writer.WriteAttributeString ("fontname", value.FontFamily.Name);
			writer.WriteAttributeString ("size", value.Size.ToString ());
			writer.WriteAttributeString ("style", value.Style.ToString ());
			writer.WriteAttributeString ("unit", value.Unit.ToString ());
		}
	}
}
