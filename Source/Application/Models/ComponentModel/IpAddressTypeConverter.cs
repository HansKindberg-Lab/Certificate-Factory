using System.ComponentModel;
using System.Globalization;
using System.Net;

namespace Application.Models.ComponentModel
{
	public class IpAddressTypeConverter : TypeConverter
	{
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
						return IPAddress.Parse(text);
					}
					catch(Exception exception)
					{
						throw new InvalidOperationException($"Can not convert from {typeof(string)} \"{text}\" to {typeof(IPAddress)}.", exception);
					}
				default:
					return base.ConvertFrom(context, culture, value);
			}
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if(destinationType == null)
				throw new ArgumentNullException(nameof(destinationType));

			if(destinationType == typeof(string) && value is IPAddress ipAddress)
				return ipAddress.ToString();

			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion
	}
}