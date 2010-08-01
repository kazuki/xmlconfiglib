namespace XmlConfigLibrary
{
	public interface IValidator<T>
	{
		bool Validate (T value);
	}
}
