namespace Application.Models.Cryptography.Archiving
{
	public interface IArchiveOptions
	{
		#region Properties

		/// <summary>
		/// If the archive will have a flat structure or not. If false the archive will contain a sub-directory with files for each certificate, if true all certificate-files will be in the same directory.
		/// </summary>
		bool Flat { get; }

		/// <summary>
		/// Decides what file-types the archive will contain.
		/// </summary>
		ArchiveKind Kind { get; }

		#endregion
	}
}