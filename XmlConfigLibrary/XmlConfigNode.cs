using System;
using System.Collections.Generic;
using System.Xml;

namespace XmlConfigLibrary
{
	public class XmlConfigNode
	{
		public const char PathSeparatorChar = '/';

		string _name;
		string _fullpath;
		List<XmlConfigNode> _nodes = new List<XmlConfigNode> ();
		XmlConfig _doc;
		XmlConfigNode _parent;
		string _nodeValue = null;
		Dictionary<string, string> _atts = null;

		public XmlConfigNode (XmlConfigNode parent, string name)
		{
			_parent = parent;
			_name = name;
			if (parent == null) {
				_fullpath = string.Empty;
				_doc = (this as XmlConfig);
			} else {
				_doc = parent._doc;
				parent._nodes.Add (this);
				if (_parent._fullpath.Length > 0)
					_fullpath = _parent._fullpath + PathSeparatorChar + name;
				else
					_fullpath = name;
			}
		}

		protected void Load (XmlReader reader)
		{
			reader.Read ();
			if (reader.HasAttributes) {
				_atts = new Dictionary<string, string> ();
				while (reader.MoveToNextAttribute ())
					_atts[reader.Name] = reader.Value;
			}

			while (reader.Read ()) {
				switch (reader.NodeType) {
					case XmlNodeType.Text:
						if (_nodeValue == null)
							_nodeValue = reader.Value;
						else
							_nodeValue += reader.Value;
						break;
					case XmlNodeType.Element:
						XmlConfigNode node = new XmlConfigNode (this, reader.Name);
						node.Load (reader.ReadSubtree ());
						break;
				}
			}
			if (_nodeValue == null)
				_nodeValue = string.Empty;
		}

		protected virtual void Write (XmlWriter writer)
		{
			writer.WriteStartElement (_name);
			if (_atts != null) {
				foreach (KeyValuePair<string, string> pair in _atts) {
					writer.WriteAttributeString (pair.Key, pair.Value);
				}
			}
			if (_nodeValue != null) {
				writer.WriteString (_nodeValue);
			}
			for (int i = 0; i < _nodes.Count; i ++)
				_nodes[i].Write (writer);
			writer.WriteEndElement ();
		}

		protected void ClearRawData ()
		{
			_atts = null;
			_nodeValue = null;
			_nodes.Clear ();
			_nodes = null;
		}

		public void SetAttribute (string key, string value)
		{
			if (_atts == null)
				_atts = new Dictionary<string,string> ();
			_atts[key] = value;
		}

		public XmlConfigNode Lookup (string path, bool createIfNotFound)
		{
			if (path == null) throw new ArgumentNullException ();
			if (path.Length == 0) throw new ArgumentException ();
			if (path[0] == PathSeparatorChar) return _doc.Lookup (path.Substring (1), createIfNotFound);

			string[] names = path.Split (PathSeparatorChar);
			XmlConfigNode node = this;
			for (int i = 0; i < names.Length; i++) {
				bool found = false;
				for (int q = 0; q < node.Nodes.Count; q++) {
					if (node.Nodes[q].Name == names[i]) {
						node = node.Nodes[q];
						found = true;
						break;
					}
				}
				if (!found) {
					if (createIfNotFound)
						node = new XmlConfigNode (node, names[i]);
					else
						return null;
				}
			}
			return node;
		}

		#region Properties
		public XmlConfig Document {
			get { return _doc; }
		}
		public string Name {
			get { return _name; }
		}
		public string FullName {
			get { return _fullpath; }
		}
		public XmlConfigNode ParentNode {
			get { return _parent; }
		}
		public IList<XmlConfigNode> Nodes {
			get { return _nodes; }
		}
		public string NodeValue {
			get { return _nodeValue; }
		}
		public IDictionary<string, string> Attributes {
			get { return _atts; }
		}
		#endregion
	}
}
