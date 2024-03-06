namespace Application.Models.Cryptography
{
	/// <inheritdoc cref="ICertificateAuthorityOptions" />
	public class CertificateAuthorityOptions : ICertificateAuthorityOptions, ICloneable<CertificateAuthorityOptions>
	{
		#region Properties

		public virtual bool CertificateAuthority { get; set; } = true;
		public virtual bool HasPathLengthConstraint { get; set; }
		public virtual int PathLengthConstraint { get; set; }

		#endregion

		#region Methods

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		ICertificateAuthorityOptions ICloneable<ICertificateAuthorityOptions>.Clone()
		{
			return this.Clone();
		}

		public virtual CertificateAuthorityOptions Clone()
		{
			var memberwiseClone = (CertificateAuthorityOptions)this.MemberwiseClone();

			return new CertificateAuthorityOptions
			{
				CertificateAuthority = memberwiseClone.CertificateAuthority,
				HasPathLengthConstraint = memberwiseClone.HasPathLengthConstraint,
				PathLengthConstraint = memberwiseClone.PathLengthConstraint
			};
		}

		#endregion
	}
}