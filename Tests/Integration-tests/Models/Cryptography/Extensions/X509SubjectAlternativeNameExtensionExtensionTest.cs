using System.Security.Cryptography.X509Certificates;
using Application.Models.Cryptography.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Models.Cryptography.Extensions
{
	[TestClass]
	public class X509SubjectAlternativeNameExtensionExtensionTest
	{
		#region Methods

		protected internal virtual async Task<X509Certificate2> CreateCertificate(string crtFileName)
		{
			await Task.CompletedTask;

			var crtFilePath = Path.Combine(Global.ProjectDirectoryPath, "Models", "Cryptography", "Extensions", "Resources", "X509SubjectAlternativeNameExtensionExtension", crtFileName);
			var certificate = X509CertificateLoader.LoadCertificateFromFile(crtFilePath);

			return certificate;
		}

		protected internal virtual async Task<X509SubjectAlternativeNameExtension> CreateSubjectAlternativeNameExtension(string crtFileName)
		{
			var certificate = await this.CreateCertificate(crtFileName);

			return certificate.Extensions.OfType<X509SubjectAlternativeNameExtension>().FirstOrDefault();
		}

		[TestMethod]
		public async Task EnumerateEmailAddresses_Test()
		{
			var subjectAlternativeNameExtension = await this.CreateSubjectAlternativeNameExtension("subject-alternative-name-certificate.crt");

			var emailAddresses = subjectAlternativeNameExtension.EnumerateEmailAddresses().ToList();

			Assert.AreEqual(3, emailAddresses.Count);
			Assert.AreEqual("email-address-1@example.org", emailAddresses[0]);
			Assert.AreEqual("email-address-2@example.org", emailAddresses[1]);
			Assert.AreEqual("email-address-3@example.org", emailAddresses[2]);
		}

		[TestMethod]
		public async Task EnumerateUris_Test()
		{
			var subjectAlternativeNameExtension = await this.CreateSubjectAlternativeNameExtension("subject-alternative-name-certificate.crt");

			var uris = subjectAlternativeNameExtension.EnumerateUris().ToList();

			Assert.AreEqual(3, uris.Count);
			Assert.AreEqual(new Uri("https://uri-1.example.org"), uris[0]);
			Assert.AreEqual(new Uri("https://uri-2.example.org"), uris[1]);
			Assert.AreEqual(new Uri("https://uri-3.example.org"), uris[2]);
		}

		[TestMethod]
		public async Task EnumerateUserPrincipalNames_Test()
		{
			var subjectAlternativeNameExtension = await this.CreateSubjectAlternativeNameExtension("subject-alternative-name-certificate.crt");

			var userPrincipalNames = subjectAlternativeNameExtension.EnumerateUserPrincipalNames().ToList();

			Assert.AreEqual(3, userPrincipalNames.Count);
			Assert.AreEqual("user-principal-name-1@example.org", userPrincipalNames[0]);
			Assert.AreEqual("user-principal-name-2@example.org", userPrincipalNames[1]);
			Assert.AreEqual("user-principal-name-3@example.org", userPrincipalNames[2]);
		}

		#endregion
	}
}