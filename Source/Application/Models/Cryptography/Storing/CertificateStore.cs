using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography.Storing.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Models.Cryptography.Storing
{
	/// <inheritdoc />
	public class CertificateStore : ICertificateStore
	{
		#region Constructors

		public CertificateStore(ILoggerFactory loggerFactory, IOptionsMonitor<CertificateStoreOptions> optionsMonitor)
		{
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
			this.OptionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
		}

		#endregion

		#region Properties

		protected internal virtual ILogger Logger { get; }
		protected internal virtual IOptionsMonitor<CertificateStoreOptions> OptionsMonitor { get; }

		#endregion

		#region Methods

		public virtual IEnumerable<ICertificate> Certificates()
		{
			var certificates = new List<ICertificate>();
			var options = this.OptionsMonitor.CurrentValue;

			foreach(var certificateOptions in options.Certificates)
			{
				try
				{
					var x509Certificate = X509Certificate2.CreateFromPem(certificateOptions.CertificatePem, certificateOptions.PrivateKeyPem);

					certificates.Add(new X509Certificate2Wrapper(x509Certificate));
				}
				catch(Exception exception)
				{
					if(this.Logger.IsEnabled(LogLevel.Error))
						this.Logger.LogError(exception, $"Could not create certificate from pem. CertificatePem: \"{certificateOptions.CertificatePem}\", PrivateKeyPem: \"{certificateOptions.PrivateKeyPem}\"");
				}
			}

			return certificates;
		}

		public virtual IReadOnlyDictionary<string, ICertificateOptions> Templates()
		{
			var templates = new Dictionary<string, ICertificateOptions>();

			foreach(var (name, certificateOptions) in this.OptionsMonitor.CurrentValue.Templates)
			{
				templates.Add(name, certificateOptions.Clone());
			}

			return new ReadOnlyDictionary<string, ICertificateOptions>(templates);
		}

		#endregion
	}
}