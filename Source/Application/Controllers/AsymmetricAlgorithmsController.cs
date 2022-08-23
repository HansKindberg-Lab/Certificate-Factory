using Application.Models;
using Application.Models.Views.AsymmetricAlgorithms;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class AsymmetricAlgorithmsController : SiteController
	{
		#region Constructors

		public AsymmetricAlgorithmsController(IFacade facade) : base(facade) { }

		#endregion

		#region Methods

		public virtual async Task<IActionResult> Index()
		{
			var model = new AsymmetricAlgorithmsViewModel();

			foreach(var item in this.Facade.AsymmetricAlgorithmRepository.Dictionary)
			{
				model.Dictionary.Add(item);
			}

			return await Task.FromResult(this.View(model));
		}

		#endregion
	}
}