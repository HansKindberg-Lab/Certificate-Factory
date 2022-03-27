namespace Application.Models.Cryptography.Archiving
{
	public interface IArchive
	{
		#region Properties

		IEnumerable<byte> Bytes { get; }
		string ContentType { get; }

		#endregion
	}
}