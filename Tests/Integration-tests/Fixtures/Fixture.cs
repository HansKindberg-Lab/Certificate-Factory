using System.ComponentModel;
using System.Net;
using Application.Models.ComponentModel;

namespace IntegrationTests.Fixtures
{
	public class Fixture : IDisposable
	{
		#region Constructors

		public Fixture()
		{
			TypeDescriptor.AddAttributes(typeof(IPAddress), new TypeConverterAttribute(typeof(IpAddressTypeConverter)));
			TypeDescriptor.AddAttributes(typeof(IPNetwork), new TypeConverterAttribute(typeof(IpNetworkTypeConverter)));
		}

		#endregion

		#region Methods

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}