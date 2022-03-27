namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public interface ICertificateAuthorityOptions : ICloneable<ICertificateAuthorityOptions>
	{
		#region Properties

		int? PathLengthConstraint { get; set; }

		#endregion
	}
}