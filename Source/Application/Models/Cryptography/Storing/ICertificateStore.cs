namespace Application.Models.Cryptography.Storing
{
	public interface ICertificateStore
	{
		#region Methods

		IEnumerable<ICertificate> Certificates();
		IReadOnlyDictionary<string, ICertificateOptions> Templates();

		#endregion
	}
}