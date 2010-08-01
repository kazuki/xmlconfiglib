namespace XmlConfigLibrary
{
	public class IntRangeValidator : IValidator<int>
	{
		int _min, _max;

		public IntRangeValidator (int max) : this (0, max)
		{
		}
		public IntRangeValidator (int min, int max)
		{
			_min = min;
			_max = max;
		}

		public bool Validate (int value)
		{
			return _min <= value && value <= _max;
		}
	}
}
