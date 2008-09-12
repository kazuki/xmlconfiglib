using System;
using System.IO;
using XmlConfigLibrary;

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

#if true
			config.SetValue<int> ("type/int", 1, false);
			config.SetValue<string> ("type/string", "hoge", false);
			byte[] temp = new byte[32];
			new Random().NextBytes (temp);
			config.SetValue<byte[]> ("type/binary", temp, false);
			config.SetValue<DateTime> ("type/datetime", DateTime.Now, false);
			config.SetValue<TimeSpan> ("type/timespan", new TimeSpan (DateTime.Now.Ticks), false);
			config.SetValue<Guid> ("type/guid", Guid.NewGuid (), false);
			config.SetValue<TestEnum> ("type/enum", TestEnum.foo, false);
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
