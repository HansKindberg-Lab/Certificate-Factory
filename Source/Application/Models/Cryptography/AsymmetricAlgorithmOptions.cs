using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
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
		#region Fields

		// ReSharper disable StaticMemberInGenericType
		private static readonly ConcurrentDictionary<EnhancedKeyUsage, string> _enhancedKeyUsageCache = new();
		// ReSharper restore StaticMemberInGenericType

		#endregion

		#region Properties

		protected internal virtual ConcurrentDictionary<EnhancedKeyUsage, string> EnhancedKeyUsageCache => _enhancedKeyUsageCache;

		#endregion

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

		public virtual ICertificate CreateCertificate(IOptionsMonitor<CertificateFactoryOptions> certificateFactoryOptionsMonitor, ICertificateOptions certificateOptions, ISystemClock systemClock)
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

				if(certificateOptions.Issuer == null)
				{
					certificate = certificateRequest.CreateSelfSigned(notBefore, notAfter);
				}
				else
				{
					var wrappedIssuerCertificate = certificateOptions.Issuer.Unwrap();

					var serialNumberSize = certificateOptions.SerialNumberSize ?? certificateFactoryOptions.DefaultSerialNumberSize;
					var serialNumber = new byte[serialNumberSize];
					RandomNumberGenerator.Fill(serialNumber);

					certificate = this.CopyCertificateWithPrivateKey(asymmetricAlgorithm, certificateRequest.Create(wrappedIssuerCertificate, notBefore, notAfter, serialNumber));
				}

				return new X509Certificate2Wrapper(certificate);
			}
		}

		protected internal abstract CertificateRequest CreateCertificateRequest(T asymmetricAlgorithm, ICertificateOptions certificateOptions);

		protected internal virtual ISet<string> GetEnhancedKeyUsageDescriptions(EnhancedKeyUsage enhancedKeyUsage)
		{
			var enhancedKeyUsageDescriptions = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

			foreach(var value in Enum.GetValues<EnhancedKeyUsage>())
			{
				if(!enhancedKeyUsage.HasFlag(value))
					continue;

				var description = this.EnhancedKeyUsageCache.GetOrAdd(value, (key) =>
				{
					var field = value.GetType().GetField(value.ToString());
					// ReSharper disable AssignNullToNotNullAttribute
					var descriptionAttribute = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute), false);
					// ReSharper restore AssignNullToNotNullAttribute
					return descriptionAttribute?.Description;
				});

				if(string.IsNullOrWhiteSpace(description))
					continue;

				enhancedKeyUsageDescriptions.Add(description);
			}

			return enhancedKeyUsageDescriptions;
		}

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

		protected internal virtual void PopulateCertificateAuthority(ICertificateAuthorityOptions certificateAuthorityOptions, CertificateRequest certificateRequest)
		{
			ArgumentNullException.ThrowIfNull(certificateRequest);

			if(certificateAuthorityOptions == null)
				return;

			certificateRequest.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, certificateAuthorityOptions.PathLengthConstraint != null, certificateAuthorityOptions.PathLengthConstraint ?? -1, true));
		}

		protected internal virtual void PopulateCertificateRequest(ICertificateOptions certificateOptions, CertificateRequest certificateRequest)
		{
			ArgumentNullException.ThrowIfNull(certificateOptions);
			ArgumentNullException.ThrowIfNull(certificateRequest);

			this.PopulateSubjectAlternativeName(certificateRequest, certificateOptions.SubjectAlternativeName);

			this.PopulateCertificateAuthority(certificateOptions.CertificateAuthority, certificateRequest);

			this.PopulateEnhancedKeyUsage(certificateRequest, certificateOptions.EnhancedKeyUsage);

			certificateRequest.CertificateExtensions.Add(new X509KeyUsageExtension(certificateOptions.KeyUsage, false));
		}

		protected internal virtual void PopulateEnhancedKeyUsage(CertificateRequest certificateRequest, EnhancedKeyUsage enhancedKeyUsage)
		{
			ArgumentNullException.ThrowIfNull(certificateRequest);

			var enhancedKeyUsageDescriptions = this.GetEnhancedKeyUsageDescriptions(enhancedKeyUsage);

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