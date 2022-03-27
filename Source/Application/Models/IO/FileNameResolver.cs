namespace Application.Models.IO
{
	public class FileNameResolver : IFileNameResolver
	{
		#region Fields

		private const char _invalidCharacterReplacement = '_';

		#endregion

		#region Properties

		public virtual char InvalidCharacterReplacement => _invalidCharacterReplacement;
		public virtual IEnumerable<char> InvalidCharacters => Path.GetInvalidFileNameChars();

		#endregion

		#region Methods

		public virtual string Resolve(string fileName)
		{
			if(fileName == null)
				return null;

			// ReSharper disable LoopCanBeConvertedToQuery
			foreach(var invalidFileNameCharacter in this.InvalidCharacters)
			{
				fileName = fileName.Replace(invalidFileNameCharacter, this.InvalidCharacterReplacement);
			}
			// ReSharper restore LoopCanBeConvertedToQuery

			return fileName;
		}

		#endregion
	}
}