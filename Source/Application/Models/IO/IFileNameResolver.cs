namespace Application.Models.IO
{
	public interface IFileNameResolver
	{
		#region Properties

		char InvalidCharacterReplacement { get; }
		IEnumerable<char> InvalidCharacters { get; }

		#endregion

		#region Methods

		string Resolve(string fileName);

		#endregion
	}
}