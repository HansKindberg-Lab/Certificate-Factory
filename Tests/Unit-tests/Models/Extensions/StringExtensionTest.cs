using Application.Models.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Models.Extensions
{
	[TestClass]
	public class StringExtensionTest
	{
		#region Methods

		[TestMethod]
		public async Task SplitInParts_Test()
		{
			await Task.CompletedTask;

			const string value = "123456789";

			Assert.AreEqual(9, value.SplitInParts(1).Count());
			Assert.AreEqual(5, value.SplitInParts(2).Count());
			Assert.AreEqual(3, value.SplitInParts(3).Count());
			Assert.AreEqual(3, value.SplitInParts(4).Count());
			Assert.AreEqual(2, value.SplitInParts(5).Count());
			Assert.AreEqual(2, value.SplitInParts(6).Count());
			Assert.AreEqual(2, value.SplitInParts(7).Count());
			Assert.AreEqual(2, value.SplitInParts(8).Count());
			Assert.AreEqual(1, value.SplitInParts(9).Count());
			Assert.AreEqual(1, value.SplitInParts(10).Count());
		}

		#endregion
	}
}