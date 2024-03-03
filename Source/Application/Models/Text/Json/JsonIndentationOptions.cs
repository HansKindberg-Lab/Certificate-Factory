namespace Application.Models.Text.Json
{
	public class JsonIndentationOptions
	{
		#region Fields

		private static JsonIndentationOptions _default;

		#endregion

		#region Properties

		public virtual char Character { get; set; } = '\t';
		public static JsonIndentationOptions Default => _default ??= new JsonIndentationOptions();
		public virtual byte Size { get; set; } = 1;

		#endregion
	}
}