using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace XmlConfigLibrary
{
	public class XmlConfig : XmlConfigNode
	{
		public const string ConfigVersion = "1.0";
		Dictionary<string, XmlConfigNode> _cache = new Dictionary<string, XmlConfigNode> ();

		public XmlConfig () : base (null, "config")
		{
		}

		#region Load
		public void Load (string path)
		{
			using (FileStream strm = new FileStream (path, FileMode.Open, FileAccess.Read, FileShare.Read, 8, FileOptions.SequentialScan)) {
				Load (strm);
			}
		}

		public void Load (Stream strm)
		{
			using (XmlTextReader reader = new XmlTextReader (strm)) {
				while (reader.Read ()) {
					if (reader.IsStartElement () && reader.Name == "config") {
						Load (reader.ReadSubtree ());
					}
				}
			}
		}
		#endregion

		#region Save
		public void Save (string path)
		{
			using (FileStream strm = new FileStream (path, FileMode.Create, FileAccess.Write, FileShare.None)) {
				Save (strm);
			}
		}

		public void Save (Stream strm)
		{
			SetAttribute ("version", ConfigVersion);
			using (XmlTextWriter writer = new XmlTextWriter (strm, null)) {
				writer.Formatting = Formatting.Indented;
				writer.Indentation = 1;
				writer.IndentChar = '\t';
				Write (writer);
			}
		}
		#endregion

		#region Methods
		public T GetValue<T> (string id)
		{
			XmlConfigNode node;
			if (_cache.TryGetValue (id, out node)) {
				XmlConfigNode<T> node2 = (XmlConfigNode<T>)node;
				return node2.Value;
			}
			throw new KeyNotFoundException ();
		}

		public void SetValue<T> (string id, T value, bool raiseException)
		{
			XmlConfigNode node;
			if (_cache.TryGetValue (id, out node)) {
				XmlConfigNode<T> node2 = (XmlConfigNode<T>)node;
				node2.SetValue (value, raiseException);
				return;
			}
			throw new KeyNotFoundException ();
		}

		public void Define<T> (string id, IParser<T> parser, IValidator<T> validator, T defaultValue)
		{
			XmlConfigNode node = Lookup (id, true);
			XmlConfigNode<T> newNode = new XmlConfigNode<T> (node, parser, validator, defaultValue);
			_cache[id] = newNode;
		}
		#endregion
	}
}
