using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography.Extensions;

namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public class EcdsaOptions : AsymmetricAlgorithmOptions<ECDsa>
	{
		#region Properties

		public virtual EllipticCurve? EllipticCurve { get; set; }

		#endregion

		#region Methods

		public override IAsymmetricAlgorithmOptions Clone()
		{
			return new EcdsaOptions
			{
				EllipticCurve = this.EllipticCurve
			};
		}

		protected internal override X509Certificate2 CopyCertificateWithPrivateKey(ECDsa asymmetricAlgorithm, X509Certificate2 certificate)
		{
			ArgumentNullException.ThrowIfNull(certificate);

			var certificateWithPrivateKey = certificate.CopyWithPrivateKey(asymmetricAlgorithm);

			certificate.Dispose();

			return certificateWithPrivateKey;
		}

		protected internal override ECDsa CreateAsymmetricAlgorithm()
		{
			if(this.EllipticCurve == null)
				return ECDsa.Create();

			return ECDsa.Create(this.CreateCurve(this.EllipticCurve.Value));
		}

		protected internal override CertificateRequest CreateCertificateRequest(ECDsa asymmetricAlgorithm, ICertificateOptions certificateOptions)
		{
			ArgumentNullException.ThrowIfNull(asymmetricAlgorithm);
			ArgumentNullException.ThrowIfNull(certificateOptions);

			return new CertificateRequest(certificateOptions.Subject, asymmetricAlgorithm, certificateOptions.HashAlgorithm.ToHashAlgorithmName());
		}

		protected internal virtual ECCurve CreateCurve(EllipticCurve ellipticCurve)
		{
			return ellipticCurve switch
			{
				Cryptography.EllipticCurve.BrainpoolP160R1 => ECCurve.NamedCurves.brainpoolP160r1,
				Cryptography.EllipticCurve.BrainpoolP160T1 => ECCurve.NamedCurves.brainpoolP160t1,
				Cryptography.EllipticCurve.BrainpoolP192R1 => ECCurve.NamedCurves.brainpoolP192r1,
				Cryptography.EllipticCurve.BrainpoolP192T1 => ECCurve.NamedCurves.brainpoolP192t1,
				Cryptography.EllipticCurve.BrainpoolP224R1 => ECCurve.NamedCurves.brainpoolP224r1,
				Cryptography.EllipticCurve.BrainpoolP224T1 => ECCurve.NamedCurves.brainpoolP224t1,
				Cryptography.EllipticCurve.BrainpoolP256R1 => ECCurve.NamedCurves.brainpoolP256r1,
				Cryptography.EllipticCurve.BrainpoolP256T1 => ECCurve.NamedCurves.brainpoolP256t1,
				Cryptography.EllipticCurve.BrainpoolP320R1 => ECCurve.NamedCurves.brainpoolP320r1,
				Cryptography.EllipticCurve.BrainpoolP320T1 => ECCurve.NamedCurves.brainpoolP320t1,
				Cryptography.EllipticCurve.BrainpoolP384R1 => ECCurve.NamedCurves.brainpoolP384r1,
				Cryptography.EllipticCurve.BrainpoolP384T1 => ECCurve.NamedCurves.brainpoolP384t1,
				Cryptography.EllipticCurve.BrainpoolP512R1 => ECCurve.NamedCurves.brainpoolP512r1,
				Cryptography.EllipticCurve.BrainpoolP512T1 => ECCurve.NamedCurves.brainpoolP512t1,
				Cryptography.EllipticCurve.NistP256 => ECCurve.NamedCurves.nistP256,
				Cryptography.EllipticCurve.NistP384 => ECCurve.NamedCurves.nistP384,
				Cryptography.EllipticCurve.NistP521 => ECCurve.NamedCurves.nistP521,
				_ => throw new InvalidOperationException($"Could not create named curve from elliptic curve \"{ellipticCurve}\".")
			};
		}

		#endregion
	}
}