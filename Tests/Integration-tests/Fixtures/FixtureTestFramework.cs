using System.ComponentModel;
using System.Net;
using Application.Models.ComponentModel;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: TestFramework("IntegrationTests.Fixtures.FixtureTestFramework", "Integration-tests")]

namespace IntegrationTests.Fixtures
{
	public class FixtureTestFramework : XunitTestFramework
	{
		#region Constructors

		public FixtureTestFramework(IMessageSink messageSink) : base(messageSink)
		{
			TypeDescriptor.AddAttributes(typeof(IPAddress), new TypeConverterAttribute(typeof(IpAddressTypeConverter)));
			TypeDescriptor.AddAttributes(typeof(IPNetwork), new TypeConverterAttribute(typeof(IpNetworkTypeConverter)));
		}

		#endregion
	}
}