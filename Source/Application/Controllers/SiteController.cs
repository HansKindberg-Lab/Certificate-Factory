using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public abstract class SiteController : Controller
	{
		#region Constructors

		protected SiteController(IFacade facade)
		{
			this.Facade = facade ?? throw new ArgumentNullException(nameof(facade));
			this.Logger = (facade.LoggerFactory ?? throw new ArgumentException("The logger-factory property can not be null.", nameof(facade))).CreateLogger(this.GetType());
		}

		#endregion

		#region Properties

		protected internal virtual IFacade Facade { get; }
		protected internal virtual ILogger Logger { get; }

		#endregion
	}
}