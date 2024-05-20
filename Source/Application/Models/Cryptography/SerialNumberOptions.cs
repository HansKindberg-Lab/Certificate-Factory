using System.Text;

namespace Application.Models.Cryptography
{
	/// <inheritdoc cref="ISubjectAlternativeNameOptions" />
	public class SerialNumberOptions : ISerialNumberOptions, ICloneable<SerialNumberOptions>
	{
		#region Properties

		public virtual byte? Size { get; set; }
		public virtual string Value { get; set; }

		#endregion

		#region Methods

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		ISerialNumberOptions ICloneable<ISerialNumberOptions>.Clone()
		{
			return this.Clone();
		}

		public virtual SerialNumberOptions Clone()
		{
			var memberwiseClone = (SerialNumberOptions)this.MemberwiseClone();

			return new SerialNumberOptions
			{
				Size = memberwiseClone.Size,
				Value = this.Value == null ? null : new StringBuilder(this.Value).ToString()
			};
		}

		#endregion
	}
}