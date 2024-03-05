namespace Application.Models.Views.HostCertificate
{
	public class CertificateInformation
	{
		#region Properties

		public virtual bool Archived { get; set; }
		public virtual string FriendlyName { get; set; }
		public virtual bool HasPrivateKey { get; set; }
		public virtual string Issuer { get; set; }
		public virtual string KeyAlgorithm { get; set; }
		public virtual string KeyAlgorithmName { get; set; }
		public virtual DateTime NotAfter { get; set; }
		public virtual DateTime NotBefore { get; set; }
		public virtual string SerialNumber { get; set; }
		public virtual string SignatureAlgorithm { get; set; }
		public virtual string Store { get; set; }
		public virtual string StringRepresentation { get; set; }
		public virtual string Subject { get; set; }
		public virtual string Thumbprint { get; set; }
		public virtual int Version { get; set; }

		#endregion
	}
}