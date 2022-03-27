namespace Application.Models.Cryptography.Archiving
{
	public interface IArchiveFactory
	{
		#region Methods

		IArchive Create(ICertificate certificate, string password);

		#endregion
	}
}