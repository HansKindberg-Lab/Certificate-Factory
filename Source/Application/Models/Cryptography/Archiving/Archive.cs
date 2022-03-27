namespace Application.Models.Cryptography.Archiving
{
	/// <inheritdoc />
	public class Archive : IArchive
	{
		#region Properties

		public virtual IEnumerable<byte> Bytes { get; set; }
		public virtual string ContentType { get; set; }

		#endregion
	}
}