using System.ComponentModel;
using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.HttpOverrides;

namespace Application.Models.ComponentModel
{
	public class IpNetworkTypeConverter : TypeConverter
	{
		#region Properties

		public virtual char Delimiter { get; set; } = '/';

		#endregion

		#region Methods

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			switch(value)
			{
				case null:
				case string { Length: 0 }:
					return null;
				case string text:
					try
					{
						var parts = text.Split(this.Delimiter, 2);

						var ipAddress = IPAddress.Parse(parts[0]);

						var prefixLength = 0;

						if(parts.Length > 1)
							prefixLength = int.Parse(parts[1], null);

						return new IPNetwork(ipAddress, prefixLength);
					}
					catch(Exception exception)
					{
						throw new InvalidOperationException($"Can not convert from {typeof(string)} \"{text}\" to {typeof(IPNetwork)}.", exception);
					}
				default:
					return base.ConvertFrom(context, culture, value);
			}
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if(destinationType == null)
				throw new ArgumentNullException(nameof(destinationType));

			if(destinationType == typeof(string) && value is IPNetwork ipNetwork)
				return $"{ipNetwork.Prefix}{this.Delimiter}{ipNetwork.PrefixLength}";

			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion
	}
}