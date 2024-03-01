using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography.Extensions;

namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public class RsaOptions : AsymmetricAlgorithmOptions<RSA>
	{
		#region Properties

		/// <summary>
		/// Supported values are 512, 1024, 2048, 4096, 8192 etc.
		/// https://www.ibm.com/docs/en/zos/2.2.0?topic=certificates-size-considerations-public-private-keys
		/// </summary>
		public virtual int KeySize { get; set; } = 2048;

		public virtual RSASignaturePaddingMode SignaturePadding { get; set; } = RSASignaturePaddingMode.Pkcs1;

		#endregion

		#region Methods

		public override IAsymmetricAlgorithmOptions Clone()
		{
			return new RsaOptions
			{
				KeySize = this.KeySize,
				SignaturePadding = this.SignaturePadding
			};
		}

		protected internal override X509Certificate2 CopyCertificateWithPrivateKey(RSA asymmetricAlgorithm, X509Certificate2 certificate)
		{
			ArgumentNullException.ThrowIfNull(certificate);

			var certificateWithPrivateKey = certificate.CopyWithPrivateKey(asymmetricAlgorithm);

			certificate.Dispose();

			return certificateWithPrivateKey;
		}

		protected internal override RSA CreateAsymmetricAlgorithm()
		{
			return RSA.Create(this.KeySize);
		}

		protected internal override CertificateRequest CreateCertificateRequest(RSA asymmetricAlgorithm, ICertificateOptions certificateOptions)
		{
			ArgumentNullException.ThrowIfNull(asymmetricAlgorithm);
			ArgumentNullException.ThrowIfNull(certificateOptions);

			return new CertificateRequest(certificateOptions.Subject, asymmetricAlgorithm, certificateOptions.HashAlgorithm.ToHashAlgorithmName(), this.GetRsaSignaturePadding());
		}

		protected internal virtual RSASignaturePadding GetRsaSignaturePadding()
		{
			return this.SignaturePadding switch
			{
				RSASignaturePaddingMode.Pkcs1 => RSASignaturePadding.Pkcs1,
				RSASignaturePaddingMode.Pss => RSASignaturePadding.Pss,
				_ => throw new InvalidOperationException($"Could not get rsa-signature-padding from rsa-signature-padding-mode \"{this.SignaturePadding}\".")
			};
		}

		#endregion
	}
}