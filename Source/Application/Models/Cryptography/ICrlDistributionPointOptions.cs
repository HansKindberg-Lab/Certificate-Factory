namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public interface ICrlDistributionPointOptions : ICloneable<ICrlDistributionPointOptions>
	{
		#region Properties

		ICollection<Uri> Uris { get; }

		#endregion
	}
}