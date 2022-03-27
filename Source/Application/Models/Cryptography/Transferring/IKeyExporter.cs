namespace Application.Models.Cryptography.Transferring
{
	public interface IKeyExporter
	{
		#region Methods

		string GetEncryptedPrivateKeyPem(string password);
		string GetPrivateKeyPem();
		string GetPublicKeyPem();

		#endregion
	}
}