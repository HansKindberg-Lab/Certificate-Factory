using System.Collections;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Application.Models.Cryptography.Extensions
{
	public static class X509SubjectAlternativeNameExtensionExtension
	{
		#region Fields

		private static readonly FieldInfo _decodedField = typeof(X509SubjectAlternativeNameExtension).GetField("_decoded", BindingFlags.Instance | BindingFlags.NonPublic);
		private static readonly Type _generalNameAsnType = Type.GetType("System.Security.Cryptography.Asn1.GeneralNameAsn, System.Security.Cryptography");
		private static readonly FieldInfo _generalNameAsnTypeOtherNameField = _generalNameAsnType.GetField("OtherName", BindingFlags.Instance | BindingFlags.NonPublic);
		private static readonly FieldInfo _generalNameAsnTypeRfc822NameField = _generalNameAsnType.GetField("Rfc822Name", BindingFlags.Instance | BindingFlags.NonPublic);
		private static readonly FieldInfo _generalNameAsnTypeUriField = _generalNameAsnType.GetField("Uri", BindingFlags.Instance | BindingFlags.NonPublic);
		private static readonly Type _otherNameAsnType = Type.GetType("System.Security.Cryptography.Asn1.OtherNameAsn, System.Security.Cryptography");
		private static readonly FieldInfo _otherNameAsnTypeTypeIdField = _otherNameAsnType.GetField("TypeId", BindingFlags.Instance | BindingFlags.NonPublic);
		private static readonly FieldInfo _otherNameAsnTypeValueField = _otherNameAsnType.GetField("Value", BindingFlags.Instance | BindingFlags.NonPublic);

		#endregion

		#region Methods

		private static void EnsureEnumeration(this X509SubjectAlternativeNameExtension subjectAlternativeNameExtension)
		{
			ArgumentNullException.ThrowIfNull(subjectAlternativeNameExtension);

			_ = subjectAlternativeNameExtension.EnumerateDnsNames().ToList();
		}

		public static IEnumerable<string> EnumerateEmailAddresses(this X509SubjectAlternativeNameExtension subjectAlternativeNameExtension)
		{
			var list = new List<string>();

			foreach(var item in subjectAlternativeNameExtension.GetInternalList())
			{
				if(_generalNameAsnTypeRfc822NameField.GetValue(item) is string email)
					list.Add(email);
			}

			return list;
		}

		public static IEnumerable<Uri> EnumerateUris(this X509SubjectAlternativeNameExtension subjectAlternativeNameExtension)
		{
			var list = new List<Uri>();

			foreach(var item in subjectAlternativeNameExtension.GetInternalList())
			{
				if(_generalNameAsnTypeUriField.GetValue(item) is string uri)
					list.Add(new Uri(uri));
			}

			return list;
		}

		public static IEnumerable<string> EnumerateUserPrincipalNames(this X509SubjectAlternativeNameExtension subjectAlternativeNameExtension)
		{
			var list = new List<string>();

			foreach(var item in subjectAlternativeNameExtension.GetInternalList())
			{
				var otherName = _generalNameAsnTypeOtherNameField.GetValue(item);

				if(otherName != null && otherName.GetType() == _otherNameAsnType)
				{
					if(_otherNameAsnTypeTypeIdField.GetValue(otherName) is "1.3.6.1.4.1.311.20.2.3")
					{
						if(_otherNameAsnTypeValueField.GetValue(otherName) is ReadOnlyMemory<byte> bytes)
						{
							var value = Encoding.UTF8.GetString(bytes.ToArray()).TrimStart('\f');

							if(value.StartsWith("!", StringComparison.OrdinalIgnoreCase))
								value = value[1..];

							list.Add(value);
						}
					}
				}
			}

			return list;
		}

		private static IEnumerable<object> GetInternalList(this X509SubjectAlternativeNameExtension subjectAlternativeNameExtension)
		{
			ArgumentNullException.ThrowIfNull(subjectAlternativeNameExtension);

			subjectAlternativeNameExtension.EnsureEnumeration();

			return (_decodedField.GetValue(subjectAlternativeNameExtension) as IEnumerable)!.OfType<object>();
		}

		#endregion
	}
}