namespace Application.Models.Collections
{
	public class UriComparer : IComparer<Uri>
	{
		#region Methods

		public virtual int Compare(Uri x, Uri y)
		{
			if(ReferenceEquals(x, y))
				return 0;

			if(x == null)
				return -1;

			if(y == null)
				return 1;

			return StringComparer.OrdinalIgnoreCase.Compare(x.OriginalString, y.OriginalString);
		}

		#endregion
	}
}