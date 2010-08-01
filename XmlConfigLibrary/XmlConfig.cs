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
		Dictionary<string, IDefineValue> _defines = new Dictionary<string, IDefineValue> ();

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
			lock (_defines) {
				Nodes.Clear ();
				if (Attributes != null)
					Attributes.Clear ();
				_cache.Clear ();

				using (XmlTextReader reader = new XmlTextReader (strm)) {
					while (reader.Read ()) {
						if (reader.IsStartElement () && reader.Name == "config") {
							Load (reader.ReadSubtree ());
						}
					}
				}

				object[] invokeParams = new object[4];
				foreach (KeyValuePair<string, IDefineValue> entry in _defines) {
					XmlConfigNode node = Lookup (entry.Key, true);
					Type tmp = typeof (XmlConfigNode<>);
					tmp = tmp.MakeGenericType (entry.Value.GenericsType);
					invokeParams[0] = node;
					invokeParams[1] = entry.Value.ConstructorParams[0];
					invokeParams[2] = entry.Value.ConstructorParams[1];
					invokeParams[3] = entry.Value.ConstructorParams[2];
					XmlConfigNode newNode = (XmlConfigNode)tmp.GetConstructors ()[0].Invoke (invokeParams);
					_cache[entry.Key] = newNode;
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
			lock (_defines) {
				_cache[id] = newNode;
				_defines.Add (id, new DefineValue<T> (parser, validator, defaultValue));
			}
		}
		#endregion

		#region Internal Class
		interface IDefineValue
		{
			Type GenericsType { get; }
			object[] ConstructorParams { get; }
		}
		class DefineValue<T> : IDefineValue
		{
			object[] _params;

			public DefineValue (IParser<T> parser, IValidator<T> validator, T defaultValue)
			{
				_params = new object[] {parser, validator, defaultValue};
			}

			public Type GenericsType {
				get { return typeof (T); }
			}

			public object[] ConstructorParams {
				get { return _params; }
			}
		}
		#endregion
	}
}
