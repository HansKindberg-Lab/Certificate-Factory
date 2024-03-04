namespace Application.Models.Cryptography.Storing
{
	public interface IApplicationCertificateStore
	{
		#region Methods

		IEnumerable<ICertificate> Certificates();
		IReadOnlyDictionary<string, ICertificateOptions> Templates();

		#endregion
	}
}