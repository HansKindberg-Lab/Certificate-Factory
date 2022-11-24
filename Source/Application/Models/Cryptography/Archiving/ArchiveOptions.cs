namespace Application.Models.Cryptography.Archiving
{
	/// <inheritdoc />
	public class ArchiveOptions : IArchiveOptions
	{
		#region Properties

		public virtual bool Flat { get; set; }
		public virtual ArchiveKind Kind { get; set; } = ArchiveKind.All;

		#endregion
	}
}