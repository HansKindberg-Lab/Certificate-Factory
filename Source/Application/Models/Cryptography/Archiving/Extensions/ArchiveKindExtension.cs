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

		#region Nested types

		extension(ArchiveKind kind)
		{
			#region Methods

			public bool CertificatePemIncluded()
			{
				return kind is not ArchiveKind.P12 and not ArchiveKind.Pfx;
			}

			public string Description()
			{
				return _descriptionRetriever.GetAttribute(kind)?.Description;
			}

			public bool EncryptedPrivateKeyPemIncluded()
			{
				return kind == ArchiveKind.All;
			}

			public string Example()
			{
				return _exampleRetriever.GetAttribute(kind)?.Example;
			}

			public bool P12Included()
			{
				return kind is ArchiveKind.All or ArchiveKind.CrtAndKeyAndP12 or ArchiveKind.CrtAndKeyAndP12AndPfx or ArchiveKind.P12;
			}

			public bool PfxIncluded()
			{
				return kind is ArchiveKind.All or ArchiveKind.CrtAndKeyAndP12AndPfx or ArchiveKind.CrtAndKeyAndPfx or ArchiveKind.Pfx;
			}

			public bool PrivateKeyPemIncluded()
			{
				return kind is not ArchiveKind.P12 and not ArchiveKind.Pfx;
			}

			public bool PublicKeyPemIncluded()
			{
				return kind == ArchiveKind.All;
			}

			#endregion
		}

		#endregion
	}
}