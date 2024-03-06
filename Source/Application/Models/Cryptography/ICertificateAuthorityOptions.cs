namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public interface ICertificateAuthorityOptions : ICloneable<ICertificateAuthorityOptions>
	{
		#region Properties

		bool CertificateAuthority { get; set; }
		bool HasPathLengthConstraint { get; set; }
		int PathLengthConstraint { get; set; }

		#endregion
	}
}