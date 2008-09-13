using System;
using System.Drawing;
using System.Xml;

namespace XmlConfigLibrary
{
	public class RectangleParser : IParser<Rectangle>
	{
		static RectangleParser _instance = new RectangleParser ();
		RectangleParser() {}
		public static RectangleParser Instance {
			get { return _instance; }
		}

		public bool TryParse (XmlConfigNode node, out Rectangle value)
		{
			value = Rectangle.Empty;
			XmlConfigNode locNode = node.Lookup ("location", false);
			XmlConfigNode sizeNode = node.Lookup ("size", false);
			if (locNode == null || sizeNode == null)
				return false;
			Size size;
			Point location;
			if (!SizeParser.Instance.TryParse (sizeNode, out size))
				return false;
			if (!PointParser.Instance.TryParse (locNode, out location))
				return false;
			value = new Rectangle (location, size);
			return true;
		}

		public void Write (Rectangle value, XmlWriter writer)
		{
			writer.WriteStartElement ("location");
			PointParser.Instance.Write (value.Location, writer);
			writer.WriteEndElement ();
			
			writer.WriteStartElement ("size");
			SizeParser.Instance.Write (value.Size, writer);
			writer.WriteEndElement ();
		}
	}
}
