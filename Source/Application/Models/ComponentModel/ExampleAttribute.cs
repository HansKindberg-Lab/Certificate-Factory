namespace Application.Models.ComponentModel
{
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="value">A value that can result in multiple lines if it contains the character '\a'. The character '\a' is used to separate the value in multiple lines.</param>
	[AttributeUsage(AttributeTargets.All)]
	public sealed class ExampleAttribute(string value) : Attribute
	{
		#region Fields

		private const char _delimiter = '\a';

		#endregion

		#region Properties

		public string Example => this.Value != null ? string.Join(Environment.NewLine, this.Value.Split(_delimiter)) : null;
		public string Value { get; } = value;

		#endregion
	}
}