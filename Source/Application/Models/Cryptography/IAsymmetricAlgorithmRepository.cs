namespace Application.Models.Cryptography
{
	public interface IAsymmetricAlgorithmRepository
	{
		#region Properties

		string DefaultKey { get; }
		IDictionary<string, AsymmetricAlgorithmInformation> Dictionary { get; }

		#endregion
	}
}