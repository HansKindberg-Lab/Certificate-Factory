namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public class CertificateAuthorityOptions : ICertificateAuthorityOptions
	{
		#region Properties

		public virtual int? PathLengthConstraint { get; set; }

		#endregion

		#region Methods

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		public ICertificateAuthorityOptions Clone()
		{
			return (ICertificateAuthorityOptions)this.MemberwiseClone();
		}

		#endregion
	}
}