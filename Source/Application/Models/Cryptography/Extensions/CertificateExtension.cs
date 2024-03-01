using System.Security.Cryptography.X509Certificates;

namespace Application.Models.Cryptography.Extensions
{
	public static class CertificateExtension
	{
		#region Methods

		public static X509Certificate2 Unwrap(this ICertificate certificate)
		{
			ArgumentNullException.ThrowIfNull(certificate);

			if(certificate is not X509Certificate2Wrapper certificateWrapper)
				throw new InvalidOperationException($"Could not unwrap certificate of type \"{certificate.GetType()}\". For the moment only certificates of type \"{typeof(X509Certificate2Wrapper)}\" can be unwrapped.");

			return certificateWrapper.WrappedCertificate;
		}

		#endregion
	}
}