namespace Application.Models.Cryptography.Archiving.Extensions
{
	public static class ArchiveKindExtension
	{
		#region Methods

		public static bool CertificatePemIncluded(this ArchiveKind kind)
		{
			return kind is not ArchiveKind.P12 and not ArchiveKind.Pfx;
		}

		public static bool EncryptedPrivateKeyPemIncluded(this ArchiveKind kind)
		{
			return kind == ArchiveKind.All;
		}

		public static bool P12Included(this ArchiveKind kind)
		{
			return kind is ArchiveKind.All or ArchiveKind.CrtAndKeyAndP12 or ArchiveKind.CrtAndKeyAndP12AndPfx or ArchiveKind.P12;
		}

		public static bool PfxIncluded(this ArchiveKind kind)
		{
			return kind is ArchiveKind.All or ArchiveKind.CrtAndKeyAndP12AndPfx or ArchiveKind.CrtAndKeyAndPfx or ArchiveKind.Pfx;
		}

		public static bool PrivateKeyPemIncluded(this ArchiveKind kind)
		{
			return kind is not ArchiveKind.P12 and not ArchiveKind.Pfx;
		}

		public static bool PublicKeyPemIncluded(this ArchiveKind kind)
		{
			return kind == ArchiveKind.All;
		}

		#endregion
	}
}