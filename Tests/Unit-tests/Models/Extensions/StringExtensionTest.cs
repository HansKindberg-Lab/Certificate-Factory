using Application.Models.Extensions;

namespace UnitTests.Models.Extensions
{
	public class StringExtensionTest
	{
		#region Methods

		[Fact]
		public async Task ReplaceStartOfEachLine_Test()
		{
			await Task.CompletedTask;

			Assert.Equal($"\t\t {Environment.NewLine}\t\tRow 2     {Environment.NewLine}\tRow 3        {Environment.NewLine}Row 4   ", $"     {Environment.NewLine}    Row 2     {Environment.NewLine}  Row 3        {Environment.NewLine}Row 4   ".ReplaceStartOfEachLine("  ", "\t"));
		}

		[Fact]
		public async Task SplitInParts_Test()
		{
			await Task.CompletedTask;

			const string value = "123456789";

			Assert.Equal(9, value.SplitInParts(1).Count());
			Assert.Equal(5, value.SplitInParts(2).Count());
			Assert.Equal(3, value.SplitInParts(3).Count());
			Assert.Equal(3, value.SplitInParts(4).Count());
			Assert.Equal(2, value.SplitInParts(5).Count());
			Assert.Equal(2, value.SplitInParts(6).Count());
			Assert.Equal(2, value.SplitInParts(7).Count());
			Assert.Equal(2, value.SplitInParts(8).Count());
			Assert.Single(value.SplitInParts(9));
			Assert.Single(value.SplitInParts(10));
		}

		#endregion
	}
}