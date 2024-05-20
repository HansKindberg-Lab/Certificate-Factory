using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography.Configuration;
using Application.Models.Cryptography.Extensions;
using Microsoft.Extensions.Options;

namespace Application.Models.Cryptography
{
	/// <inheritdoc cref="IAsymmetricAlgorithmOptions" />
	public abstract class AsymmetricAlgorithmOptions<T> : IAsymmetricAlgorithmOptions, ICloneable<AsymmetricAlgorithmOptions<T>> where T : AsymmetricAlgorithm
	{
		#region Methods

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		IAsymmetricAlgorithmOptions ICloneable<IAsymmetricAlgorithmOptions>.Clone()
		{
			return this.Clone();
		}

		public abstract AsymmetricAlgorithmOptions<T> Clone();
		protected internal abstract X509Certificate2 CopyCertificateWithPrivateKey(T asymmetricAlgorithm, X509Certificate2 certificate);
		protected internal abstract T CreateAsymmetricAlgorithm();

		public virtual ICertificate CreateCertificate(IOptionsMonitor<CertificateFactoryOptions> certificateFactoryOptionsMonitor, ICertificateOptions certificateOptions, ILoggerFactory loggerFactory, ISystemClock systemClock)
		{
			ArgumentNullException.ThrowIfNull(certificateFactoryOptionsMonitor);
			ArgumentNullException.ThrowIfNull(certificateOptions);
			ArgumentNullException.ThrowIfNull(systemClock);

			var certificateFactoryOptions = certificateFactoryOptionsMonitor.CurrentValue;

			using(var asymmetricAlgorithm = this.CreateAsymmetricAlgorithm())
			{
				var certificateRequest = this.CreateCertificateRequest(asymmetricAlgorithm, certificateOptions);

				this.PopulateCertificateRequest(certificateOptions, certificateRequest);

				X509Certificate2 certificate;
				var notBefore = this.GetNotBefore(certificateOptions, systemClock);
				var notAfter = this.GetNotAfter(certificateFactoryOptions, certificateOptions, notBefore);
				var serialNumber = this.GetSerialNumber(certificateFactoryOptions, certificateOptions);

				if(certificateOptions.Issuer == null)
				{
					// We could have used "certificateRequest.CreateSelfSigned(notBefore, notAfter)" here but then we can not set the serial number explicit.
					certificate = certificateRequest.Create(new X500DistinguishedName(certificateOptions.Subject), this.CreateSignatureGenerator(asymmetricAlgorithm), notBefore, notAfter, serialNumber);
				}
				else
				{
					var wrappedIssuerCertificate = certificateOptions.Issuer.Unwrap();

					certificate = certificateRequest.Create(wrappedIssuerCertificate, notBefore, notAfter, serialNumber);
				}

				certificate = this.CopyCertificateWithPrivateKey(asymmetricAlgorithm, certificate);

				return new X509Certificate2Wrapper(certificate, loggerFactory);
			}
		}

		protected internal abstract CertificateRequest CreateCertificateRequest(T asymmetricAlgorithm, ICertificateOptions certificateOptions);
		protected internal abstract X509SignatureGenerator CreateSignatureGenerator(T asymmetricAlgorithm);

		protected internal virtual DateTimeOffset GetNotAfter(CertificateFactoryOptions certificateFactoryOptions, ICertificateOptions certificateOptions, DateTimeOffset notBefore)
		{
			ArgumentNullException.ThrowIfNull(certificateFactoryOptions);
			ArgumentNullException.ThrowIfNull(certificateOptions);

			if(certificateOptions.NotAfter != null)
				return certificateOptions.NotAfter.Value;

			var notAfter = notBefore.Add(certificateFactoryOptions.DefaultCertificateLifetime);

			// ReSharper disable InvertIf
			if(certificateOptions.Issuer != null)
			{
				var issuerNotAfter = new DateTimeOffset(certificateOptions.Issuer.NotAfter);

				if(notAfter > issuerNotAfter)
					notAfter = issuerNotAfter;
			}
			// ReSharper restore InvertIf

			return notAfter;
		}

		protected internal virtual DateTimeOffset GetNotBefore(ICertificateOptions certificateOptions, ISystemClock systemClock)
		{
			ArgumentNullException.ThrowIfNull(certificateOptions);
			ArgumentNullException.ThrowIfNull(systemClock);

			if(certificateOptions.NotBefore != null)
				return certificateOptions.NotBefore.Value;

			var notBefore = systemClock.UtcNow;

			// ReSharper disable InvertIf
			if(certificateOptions.Issuer != null)
			{
				var issuerNotBefore = new DateTimeOffset(certificateOptions.Issuer.NotBefore);

				if(notBefore < issuerNotBefore)
					notBefore = issuerNotBefore;
			}
			// ReSharper restore InvertIf

			return notBefore;
		}

		protected internal virtual byte[] GetSerialNumber(CertificateFactoryOptions certificateFactoryOptions, ICertificateOptions certificateOptions)
		{
			ArgumentNullException.ThrowIfNull(certificateFactoryOptions);
			ArgumentNullException.ThrowIfNull(certificateOptions);

			if(!string.IsNullOrEmpty(certificateOptions.SerialNumber?.Value))
				return Convert.FromHexString(certificateOptions.SerialNumber.Value);

			var serialNumberSize = certificateOptions.SerialNumber?.Size ?? certificateFactoryOptions.DefaultSerialNumberSize;
			var serialNumber = new byte[serialNumberSize];
			RandomNumberGenerator.Fill(serialNumber);

			return serialNumber;
		}

		protected internal virtual void PopulateAuthorityInformationAccess(CertificateRequest certificateRequest, IAuthorityInformationAccessOptions authorityInformationAccess)
		{
			ArgumentNullException.ThrowIfNull(certificateRequest);

			if(authorityInformationAccess == null)
				return;

			var authorityInformationAccessExtension = new X509AuthorityInformationAccessExtension(authorityInformationAccess.OcspUris.Select(uri => uri.ToString()), authorityInformationAccess.CaIssuerUris.Select(uri => uri.ToString()));

			certificateRequest.CertificateExtensions.Add(authorityInformationAccessExtension);
		}

		protected internal virtual void PopulateCertificateAuthority(ICertificateAuthorityOptions certificateAuthorityOptions, CertificateRequest certificateRequest)
		{
			ArgumentNullException.ThrowIfNull(certificateRequest);

			if(certificateAuthorityOptions == null)
				return;

			certificateRequest.CertificateExtensions.Add(new X509BasicConstraintsExtension(certificateAuthorityOptions.CertificateAuthority, certificateAuthorityOptions.HasPathLengthConstraint, certificateAuthorityOptions.PathLengthConstraint, true));
		}

		protected internal virtual void PopulateCertificateRequest(ICertificateOptions certificateOptions, CertificateRequest certificateRequest)
		{
			ArgumentNullException.ThrowIfNull(certificateOptions);
			ArgumentNullException.ThrowIfNull(certificateRequest);

			this.PopulateSubjectAlternativeName(certificateRequest, certificateOptions.SubjectAlternativeName);

			this.PopulateCrlDistributionPoint(certificateRequest, certificateOptions.CrlDistributionPoint);

			this.PopulateAuthorityInformationAccess(certificateRequest, certificateOptions.AuthorityInformationAccess);

			this.PopulateCertificateAuthority(certificateOptions.CertificateAuthority, certificateRequest);

			this.PopulateEnhancedKeyUsage(certificateRequest, certificateOptions.EnhancedKeyUsage);

			certificateRequest.CertificateExtensions.Add(new X509KeyUsageExtension(certificateOptions.KeyUsage, false));
		}

		protected internal virtual void PopulateCrlDistributionPoint(CertificateRequest certificateRequest, ICrlDistributionPointOptions crlDistributionPoint)
		{
			ArgumentNullException.ThrowIfNull(certificateRequest);

			if(crlDistributionPoint == null)
				return;

			var crlDistributionPointExtension = CertificateRevocationListBuilder.BuildCrlDistributionPointExtension(crlDistributionPoint.Uris.Select(uri => uri.ToString()));

			certificateRequest.CertificateExtensions.Add(crlDistributionPointExtension);
		}

		protected internal virtual void PopulateEnhancedKeyUsage(CertificateRequest certificateRequest, EnhancedKeyUsage enhancedKeyUsage)
		{
			ArgumentNullException.ThrowIfNull(certificateRequest);

			var enhancedKeyUsageDescriptions = enhancedKeyUsage.Descriptions();

			if(enhancedKeyUsageDescriptions == null || !enhancedKeyUsageDescriptions.Any())
				return;

			var oidCollection = new OidCollection();

			foreach(var enhancedKeyUsageDescription in enhancedKeyUsageDescriptions)
			{
				oidCollection.Add(new Oid(enhancedKeyUsageDescription));
			}

			certificateRequest.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(oidCollection, true));
		}

		protected internal virtual void PopulateSubjectAlternativeName(CertificateRequest certificateRequest, ISubjectAlternativeNameOptions subjectAlternativeName)
		{
			ArgumentNullException.ThrowIfNull(certificateRequest);

			if(subjectAlternativeName == null)
				return;

			var subjectAlternativeNameBuilder = new SubjectAlternativeNameBuilder();

			foreach(var dnsName in subjectAlternativeName.DnsNames)
			{
				subjectAlternativeNameBuilder.AddDnsName(dnsName);
			}

			foreach(var emailAddress in subjectAlternativeName.EmailAddresses)
			{
				subjectAlternativeNameBuilder.AddEmailAddress(emailAddress);
			}

			foreach(var ipAddress in subjectAlternativeName.IpAddresses)
			{
				subjectAlternativeNameBuilder.AddIpAddress(ipAddress);
			}

			foreach(var uri in subjectAlternativeName.Uris)
			{
				subjectAlternativeNameBuilder.AddUri(uri);
			}

			foreach(var userPrincipalName in subjectAlternativeName.UserPrincipalNames)
			{
				subjectAlternativeNameBuilder.AddUserPrincipalName(userPrincipalName);
			}

			certificateRequest.CertificateExtensions.Add(subjectAlternativeNameBuilder.Build());
		}

		#endregion
	}
}