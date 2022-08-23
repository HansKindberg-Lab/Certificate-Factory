using Application.Models.Cryptography;

namespace Application.Models.Views.AsymmetricAlgorithms
{
	public class AsymmetricAlgorithmsViewModel
	{
		#region Properties

		public virtual IDictionary<string, AsymmetricAlgorithmInformation> Dictionary { get; } = new Dictionary<string, AsymmetricAlgorithmInformation>(StringComparer.OrdinalIgnoreCase);

		#endregion
	}
}