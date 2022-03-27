namespace Application.Models.Cryptography
{
	public class AsymmetricAlgorithmInformation
	{
		#region Properties

		public virtual IAsymmetricAlgorithmOptions Options { get; set; }
		public virtual string Text { get; set; }

		#endregion
	}
}