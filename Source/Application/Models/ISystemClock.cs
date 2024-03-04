namespace Application.Models
{
	public interface ISystemClock
	{
		#region Properties

		DateTimeOffset UtcNow { get; }

		#endregion
	}
}