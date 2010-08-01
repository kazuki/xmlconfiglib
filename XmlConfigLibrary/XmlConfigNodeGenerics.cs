using System;
using System.Xml;

namespace XmlConfigLibrary
{
	class XmlConfigNode<T> : XmlConfigNode
	{
		IParser<T> _parser;
		IValidator<T> _validator;
		T _value;
		T _defaultValue;

		public XmlConfigNode (XmlConfigNode node, IParser<T> parser, IValidator<T> validator, T defaultValue) : base (node.ParentNode, node.Name)
		{
			if (parser == null)
				throw new ArgumentNullException ("parser");
			_parser = parser;
			_validator = validator;
			_defaultValue = defaultValue;

			if (!parser.TryParse (node, out _value) || (validator != null && !validator.Validate (_value)))
				_value = defaultValue;
			ClearRawData ();
			if (node.ParentNode != null)
				node.ParentNode.Nodes.Remove (node);
		}
		
		public bool SetValue (T value, bool raiseException)
		{
			if (_validator == null || _validator.Validate (value)) {
				_value = value;
				return true;
			}

			if (raiseException)
				throw new ArgumentException ();
			return false;
		}

		protected override void Write (XmlWriter writer)
		{
			writer.WriteStartElement (Name);
			_parser.Write (_value, writer);
			writer.WriteEndElement ();
		}

		public T Value {
			get { return _value; }
		}
	}
}
