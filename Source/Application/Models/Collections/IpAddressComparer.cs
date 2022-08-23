using System.Net;

namespace Application.Models.Collections
{
	public class IpAddressComparer : IComparer<IPAddress>
	{
		#region Methods

		public virtual int Compare(IPAddress x, IPAddress y)
		{
			if(ReferenceEquals(x, y))
				return 0;

			if(x == null)
				return -1;

			if(y == null)
				return 1;

			return StringComparer.OrdinalIgnoreCase.Compare(x.ToString(), y.ToString());
		}

		#endregion
	}
}