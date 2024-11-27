using System.Security.Cryptography;

namespace Application.Models.Cryptography
{
	public class AsymmetricAlgorithmRepository : IAsymmetricAlgorithmRepository
	{
		#region Fields

		private static string _defaultKey;
		private static IDictionary<string, AsymmetricAlgorithmInformation> _dictionary;
		private static readonly Lock _dictionaryLock = new();

		#endregion

		#region Properties

		public virtual string DefaultKey
		{
			get
			{
				// ReSharper disable InvertIf
				if(_defaultKey == null)
				{
					var defaultEntry = this.Dictionary.FirstOrDefault(item => item.Value.Options is RsaOptions { KeySize: 2048, SignaturePadding: RSASignaturePaddingMode.Pkcs1 });

					if(defaultEntry.Equals(default(KeyValuePair<string, AsymmetricAlgorithmInformation>)))
						throw new InvalidOperationException("Invalid asymmetric algorithm setup.");

					_defaultKey = defaultEntry.Key;
				}
				// ReSharper restore InvertIf

				return _defaultKey;
			}
		}

		public virtual IDictionary<string, AsymmetricAlgorithmInformation> Dictionary
		{
			get
			{
				// ReSharper disable InvertIf
				if(_dictionary == null)
				{
					lock(_dictionaryLock)
					{
						if(_dictionary == null)
						{
							var dictionary = new Dictionary<string, AsymmetricAlgorithmInformation>(StringComparer.OrdinalIgnoreCase);

							const string ecdsaName = nameof(ECDsa);

							dictionary.Add(ecdsaName, new AsymmetricAlgorithmInformation
							{
								Options = new EcdsaOptions(),
								Text = $"{ecdsaName} (default)"
							});

							foreach(var ellipticCurve in Enum.GetValues<EllipticCurve>())
							{
								dictionary.Add($"{ecdsaName}:{ellipticCurve}", new AsymmetricAlgorithmInformation
								{
									Options = new EcdsaOptions { EllipticCurve = ellipticCurve },
									Text = $"{ecdsaName} - {ellipticCurve}"
								});
							}

							const string rsaName = nameof(RSA);

							foreach(var keySize in new[] { 512, 1024, 2048, 4096 })
							{
								foreach(var signaturePadding in Enum.GetValues<RSASignaturePaddingMode>())
								{
									dictionary.Add($"{rsaName}:{keySize}:{signaturePadding}", new AsymmetricAlgorithmInformation
									{
										Options = new RsaOptions { KeySize = keySize, SignaturePadding = signaturePadding },
										Text = $"{rsaName} - {keySize} - {signaturePadding}"
									});
								}
							}

							_dictionary = dictionary;
						}
					}
				}
				// ReSharper restore InvertIf

				return _dictionary;
			}
		}

		#endregion
	}
}