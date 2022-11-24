namespace Application.Models.Cryptography.Archiving
{
	public interface IArchiveFactory
	{
		#region Methods

		IArchive Create(ICertificate certificate, ArchiveKind kind, string password);
		IArchive Create(IDictionary<string, ICertificate> certificates, IArchiveOptions options, string password);

		#endregion
	}
}