using System.Text;
using System.Text.Json.Serialization;
using Application.Models.Text.Json;
using Application.Models.Text.Json.Extensions;

namespace Application.Models.Cryptography
{
	public class CertificateConstructionTreeOptions : ICloneable<CertificateConstructionTreeOptions>
	{
		#region Properties

		/// <summary>
		/// Default values for all certificate-properties if a property is not set explicitly.
		/// </summary>
		public virtual CertificateConstructionOptions Defaults { get; set; } = new()
		{
			AsymmetricAlgorithm = "RSA:2048:Pkcs1"
		};

		/// <summary>
		/// The name for the zip-file that is created.
		/// </summary>
		[JsonIgnore]
		public virtual string Name { get; set; } = "Certificates";

		/// <summary>
		/// The root-certificates to create.
		/// </summary>
		public virtual IDictionary<string, CertificateConstructionNodeOptions> Roots { get; } = new Dictionary<string, CertificateConstructionNodeOptions>(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// Default values for all root-certificate-properties if a property is not set explicitly.
		/// </summary>
		public virtual CertificateConstructionOptions RootsDefaults { get; set; }

		#endregion

		#region Methods

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		public virtual CertificateConstructionTreeOptions Clone()
		{
			var clone = new CertificateConstructionTreeOptions
			{
				Defaults = this.Defaults?.Clone(),
				Name = this.Name == null ? null : new StringBuilder(this.Name).ToString(),
				RootsDefaults = this.RootsDefaults?.Clone()
			};

			foreach(var (key, value) in this.Roots)
			{
				clone.Roots.Add(new StringBuilder(key).ToString(), value?.Clone());
			}

			return clone;
		}

		public virtual string ToJson()
		{
			return JsonSerializerExtension.Serialize(this, JsonSerializerOptionsExtension.Default, JsonIndentationOptions.Default);
		}

		#endregion
	}
}