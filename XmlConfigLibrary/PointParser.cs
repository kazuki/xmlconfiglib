using System;
using System.Drawing;
using System.Xml;

namespace XmlConfigLibrary
{
	public class PointParser : IParser<Point>
	{
		static PointParser _instance = new PointParser ();
		PointParser() {}
		public static PointParser Instance {
			get { return _instance; }
		}

		public bool TryParse (XmlConfigNode node, out Point value)
		{
			value = Point.Empty;
			string sx, sy;
			if (node.Attributes == null ||
				!node.Attributes.TryGetValue ("x", out sx) ||
				!node.Attributes.TryGetValue ("y", out sy)) {
				return false;
			}
			int x, y;
			if (int.TryParse (sx, out x) && int.TryParse (sy, out y)) {
				value = new Point (x, y);
				return true;
			}
			return false;
		}

		public void Write (Point value, XmlWriter writer)
		{
			writer.WriteAttributeString ("x", value.X.ToString ());
			writer.WriteAttributeString ("y", value.Y.ToString ());
		}
	}
}
