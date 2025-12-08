namespace Application.Models.Text.Json
{
	public class JsonIndentationOptions
	{
		#region Properties

		public virtual char Character { get; set; } = '\t';
		public static JsonIndentationOptions Default { get; } = new();
		public virtual byte Size { get; set; } = 1;

		#endregion
	}
}