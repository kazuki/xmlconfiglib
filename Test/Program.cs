using System;
using System.IO;
using XmlConfigLibrary;
using System.Drawing;

namespace Test
{
	class Program
	{
		static void Main (string[] args)
		{
			XmlConfig config = new XmlConfig ();
			if (File.Exists ("test.xml"))
				config.Load ("test.xml");
			config.Define<int> ("type/int", IntParser.Instance, new IntRangeValidator (-10, 10), 0);
			config.Define<string> ("type/string", StringParser.Instance, null, "");
			config.Define<byte[]> ("type/binary", BinaryParser.Instance, null, new byte[0]);
			config.Define<DateTime> ("type/datetime", DateTimeParser.Instance, null, DateTime.Now);
			config.Define<TimeSpan> ("type/timespan", TimeSpanParser.Instance, null, new TimeSpan (DateTime.Now.Ticks));
			config.Define<Guid> ("type/guid", GuidParser.Instance, null, Guid.NewGuid ());
			config.Define<TestEnum> ("type/enum", new EnumParser<TestEnum> (), null, TestEnum.hoge);
			config.Define<Rectangle> ("type/rectangle", RectangleParser.Instance, null, Rectangle.Empty);
			config.Define<bool> ("type/bool", BooleanParser.Instance, null, false);
			config.Define<string[]> ("type/array/string", new ArrayParser<string> (StringParser.Instance), null, new string[0]);
			config.Define<TestEnum[]> ("type/array/enum", new ArrayParser<TestEnum> (new EnumParser<TestEnum> (), "i"), null, new TestEnum[0]);

#if false
			config.SetValue<int> ("type/int", 1, false);
			config.SetValue<string> ("type/string", "hoge", false);
			byte[] temp = new byte[32];
			new Random().NextBytes (temp);
			config.SetValue<byte[]> ("type/binary", temp, false);
			config.SetValue<DateTime> ("type/datetime", DateTime.Now, false);
			config.SetValue<TimeSpan> ("type/timespan", new TimeSpan (DateTime.Now.Ticks), false);
			config.SetValue<Guid> ("type/guid", Guid.NewGuid (), false);
			config.SetValue<TestEnum> ("type/enum", TestEnum.foo, false);
			config.SetValue<Rectangle> ("type/rectangle", new Rectangle (10, 20, 30, 40), false);
			config.SetValue<bool> ("type/bool", true, false);
			config.SetValue<string[]> ("type/array/string", new string[]{"item0", "item1", "item2", "item3"}, false);
			config.SetValue<TestEnum[]> ("type/array/enum", new TestEnum[]{TestEnum.hoge, TestEnum.piyo}, false);
#endif

			config.Save ("test2.xml");
		}
	}

	enum TestEnum
	{
		hoge,
		foo,
		bar,
		piyo
	}
}
