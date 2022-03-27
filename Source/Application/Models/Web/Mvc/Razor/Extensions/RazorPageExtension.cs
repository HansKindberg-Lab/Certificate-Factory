using Microsoft.AspNetCore.Mvc.Razor;

namespace Application.Models.Web.Mvc.Razor.Extensions
{
	public static class HttpContextExtension
	{
		#region Methods

		public static bool IsActive(this RazorPageBase razorPage, string controller)
		{
			var routeValueDictionary = razorPage?.ViewContext.RouteData.Values;

			// ReSharper disable InvertIf
			if(routeValueDictionary != null)
			{
				if(routeValueDictionary.TryGetValue("controller", out var value))
				{
					if(value is string stringValue)
						return string.Equals(controller, stringValue, StringComparison.OrdinalIgnoreCase);
				}
			}
			// ReSharper restore InvertIf

			return false;
		}

		#endregion
	}
}