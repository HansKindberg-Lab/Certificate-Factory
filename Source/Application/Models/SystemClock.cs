namespace Application.Models
{
	public class SystemClock(TimeProvider timeProvider) : ISystemClock
	{
		#region Constructors

		public SystemClock() : this(TimeProvider.System) { }

		#endregion

		#region Properties

		protected internal virtual TimeProvider TimeProvider { get; } = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
		public virtual DateTimeOffset UtcNow => this.TimeProvider.GetUtcNow();

		#endregion
	}
}