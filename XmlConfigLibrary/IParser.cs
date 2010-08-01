using System.Xml;

namespace XmlConfigLibrary
{
	public interface IParser<T>
	{
		bool TryParse (XmlConfigNode node, out T value);

		void Write (T value, XmlWriter writer);
	}
}
