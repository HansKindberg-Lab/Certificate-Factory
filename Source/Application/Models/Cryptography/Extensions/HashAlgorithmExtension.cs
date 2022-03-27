using System.Security.Cryptography;

namespace Application.Models.Cryptography.Extensions
{
	public static class HashAlgorithmExtension
	{
		#region Methods

		public static HashAlgorithmName ToHashAlgorithmName(this HashAlgorithm hashAlgorithm)
		{
			return hashAlgorithm switch
			{
				HashAlgorithm.Md5 => HashAlgorithmName.MD5,
				HashAlgorithm.Sha1 => HashAlgorithmName.SHA1,
				HashAlgorithm.Sha256 => HashAlgorithmName.SHA256,
				HashAlgorithm.Sha384 => HashAlgorithmName.SHA384,
				HashAlgorithm.Sha512 => HashAlgorithmName.SHA512,
				_ => throw new InvalidOperationException($"Could not get hash-algorithm-name from hash-algorithm \"{hashAlgorithm}\".")
			};
		}

		#endregion
	}
}