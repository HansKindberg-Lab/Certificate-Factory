namespace Application.Models
{
	public interface ICloneable<out T> : ICloneable
	{
		#region Methods

		new T Clone();

		#endregion
	}
}