using System.ComponentModel;
using Application.Models.ComponentModel;

namespace Application.Models.Cryptography.Archiving.Extensions
{
	public static class ArchiveKindExtension
	{
		#region Fields

		private static readonly EnumerationAttributeRetriever<DescriptionAttribute, ArchiveKind> _descriptionRetriever = new();
		private static readonly EnumerationAttributeRetriever<ExampleAttribute, ArchiveKind> _exampleRetriever = new();

		#endregion

		#region Methods

		public static bool CertificatePemIncluded(this ArchiveKind kind)
		{
			return kind is not ArchiveKind.P12 and not ArchiveKind.Pfx;
		}

		public static string Description(this ArchiveKind kind)
		{
			return _descriptionRetriever.GetAttribute(kind)?.Description;
		}

		public static bool EncryptedPrivateKeyPemIncluded(this ArchiveKind kind)
		{
			return kind == ArchiveKind.All;
		}

		public static string Example(this ArchiveKind kind)
		{
			return _exampleRetriever.GetAttribute(kind)?.Example;
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