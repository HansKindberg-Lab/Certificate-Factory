namespace Application.Models.Cryptography
{
	/// <inheritdoc />
	public interface ISerialNumberOptions : ICloneable<ISerialNumberOptions>
	{
		#region Properties

		/// <summary>
		/// The serial-number size. The number of bytes a generated serial-number will have if the Value is not set. If null and also Value is null the configured serial-number size is used.
		/// </summary>
		byte? Size { get; set; }

		/// <summary>
		/// Explicit value for the serial-number.
		/// </summary>
		string Value { get; set; }

		#endregion
	}
}