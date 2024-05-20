namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public interface IAuthorityInformationAccessOptions : ICloneable<IAuthorityInformationAccessOptions>
	{
		#region Properties

		ICollection<Uri> CaIssuerUris { get; }
		ICollection<Uri> OcspUris { get; }

		#endregion
	}
}