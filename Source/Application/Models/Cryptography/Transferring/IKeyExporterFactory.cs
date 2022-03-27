using System.Security.Cryptography;

namespace Application.Models.Cryptography.Transferring
{
	public interface IKeyExporterFactory
	{
		#region Methods

		IKeyExporter Create(AsymmetricAlgorithm asymmetricAlgorithm);

		#endregion
	}
}